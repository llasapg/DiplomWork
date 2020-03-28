using System.IO;
using DiplomaSolution.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using DiplomaSolution.Models;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DiplomaSolution.Services.Classes
{
    /// <summary>
    /// Custom service with functional to work with files
    /// </summary>
    public class FileManagerService : IFileManagerService
    {
        /// <summary>
        /// DB representation   
        /// </summary>
        private CustomerContext DataContext { get; set; }
        /// <summary>
        /// Application configuration   
        /// </summary>
        private IConfiguration Configuration { get; set; }

        /// <summary>
        /// DI reciving constractor
        /// </summary>
        /// <param name="customerContext"></param>
        /// <param name="configuration"></param>
        public FileManagerService(CustomerContext customerContext, IConfiguration configuration)
        {
            DataContext = customerContext;
            Configuration = configuration;
        }

        /// <summary>
        /// Loads customer images to HD to deal with them in future
        /// </summary>
        /// <param name="file"></param>
        public async Task LoadFileToTheServer(IFormFile file, string customerId)
        {
            var fileExtension = Path.GetExtension(file.FileName); // or try to use Name

            if (fileExtension != null && (fileExtension == ".jpg" || fileExtension == ".png")) // for now we should work only with this type of files ( can be set-Up in the configuration file )
            {
                var checkResult = await FileExtensionCheck(file, fileExtension);

                if (checkResult) //file type is save, as file extension ( no viruses )
                {
                    var pathToTheFolder = Configuration["CustomerFilesFolder"];

                    var randomFileName = Path.GetRandomFileName().Replace(".", ""); // replace all the dots to be able to use this files later in the server

                    var systemFileName = Path.Combine(pathToTheFolder, randomFileName, fileExtension);

                    using (var stream = File.Create(systemFileName))
                    {
                        await file.CopyToAsync(stream);
                    }
                }
            }
        }

        /// <summary>
        /// To store customer files to the DB and map them with other customer data
        /// </summary>
        /// <param name="file"></param>
        public async Task LoadFileToTheDB(IFormFile file, string customerId) // todo - check that we can return error list in some cases
        {
            var fileExtension = Path.GetExtension(file.FileName); // or try to use Name

            if (fileExtension != null && (fileExtension == ".jpg" || fileExtension == ".png")) // for now we should work only with this type of files ( can be set-Up in the configuration file )
            {
                var checkResult = await FileExtensionCheck(file, fileExtension);

                if (checkResult) //file type is save, as file extension ( no viruses )
                {
                    var pathToTheFolder = Configuration["CustomerFilesFolder"];

                    var randomFileName = Path.GetRandomFileName().Replace(".", ""); // replace all the dots to be able to use this files later in the server

                    var systemFileName = Path.Combine(pathToTheFolder, randomFileName, fileExtension);

                    using (var stream = new MemoryStream())
                    {
                        await file.CopyToAsync(stream);

                        DataContext.CustomerFiles.Add(new Models.FormFile
                        {
                            Id = new Guid(),
                            CustomerId = customerId,
                            FullName = systemFileName,
                            FileData = stream.ToArray()
                        });

                        DataContext.SaveChanges();
                    }
                }
            }
        }

        /// <summary>
        /// Collection with all available file signatures for our web-site ( check if its .jpg or .png ) 
        /// </summary>
        private Dictionary<string, List<byte[]>> FileSignatures = new Dictionary<string, List<byte[]>>
        {
            {
                ".jpg", new List<byte[]>
                {
                    new byte[] { 0xFF, 0xD8, 0xFF, 0xE0 },
                    new byte[] { 0xFF, 0xD8, 0xFF, 0xE2 },
                    new byte[] { 0xFF, 0xD8, 0xFF, 0xE3 }
                }
            },
            {
                ".png", new List<byte[]>
                {
                    new byte[] { 0x89, 0x50, 0x4E, 0x47 },
                    new byte[] { 0x0D, 0x0A, 0x1A, 0x0A },
                }
            }
        };

        /// <summary>
        /// Method to check, that file extension is same, as customer provided
        /// </summary>
        /// <param name="file"></param>
        /// <param name="fileExtension"></param>
        /// <returns></returns>
        private async Task<bool> FileExtensionCheck(IFormFile file, string fileExtension)
        {
            // check that file really have this extensions

            using (var fileData = new MemoryStream())
            {
                await file.CopyToAsync(fileData);

                using (var reader = new BinaryReader(fileData))
                {
                    var signatures = FileSignatures[fileExtension];

                    var countOfByteToRead = signatures.Max(m => m.Length);

                    var firstBytes = reader.ReadBytes(countOfByteToRead); // find why there is no first bytes

                    var result = signatures.Any(signature => firstBytes.Take(signature.Length).SequenceEqual(signature));

                    return result;
                }
            }
        }
    }
}
