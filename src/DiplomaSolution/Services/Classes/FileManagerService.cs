using System.IO;
using DiplomaSolution.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using DiplomaSolution.Models;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using DiplomaSolution.Helpers.ErrorResponseMessages;
using DiplomaSolution.ConfigurationModels;
using Microsoft.Extensions.Options;
using DiplomaSolution.Models.FileModels;
using ImageMagick;
using DiplomaSolution.Services.Models;

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
        private IOptionsSnapshot<FileConfiguration> Configuration { get; set; }

        /// <summary>
        /// DI reciving constractor
        /// </summary>
        /// <param name="customerContext"></param>
        /// <param name="configuration"></param>
        public FileManagerService(CustomerContext customerContext, IOptionsSnapshot<FileConfiguration> configuration)
        {
            DataContext = customerContext;
            Configuration = configuration;
        }

        /// <summary>
        /// Loads customer images to HD to deal with them in future
        /// </summary>
        /// <param name="file"></param>
        public async Task<DefaultServiceResponse> LoadFileToTheServer(IFormFile file, string customerId)
        {
            var responseModel = new DefaultServiceResponse() { ValidationErrors = new List<string>() };

            var fileExtension = Path.GetExtension(file.FileName); // or try to use Name

            if (fileExtension != null && (fileExtension == ".jpg" || fileExtension == ".png")) // for now we should work only with this type of files ( can be set-Up in the configuration file )
            {
                var randomFileName = Path.GetRandomFileName().Replace(".", ""); // replace all the dots to be able to use this files later in the server

                var pathToFilesFolder = Configuration.Value.CustomerFilesFolder;

                var systemFileName = Path.Combine(pathToFilesFolder, randomFileName) + fileExtension;

                var checkResult = await FileExtensionCheck(file, fileExtension, systemFileName);

                if (Configuration.Value.SaveFilesWithWrongFormat) //file type is save, as file extension ( no viruses )
                {
                    DataContext.CustomerImageFiles.Add(new ImageFileModel { CustomerId = customerId, FullName = systemFileName, Id = new Guid(), UploadTime = DateTime.Now }); // todo - check file ID!!!! ASAP

                    await DataContext.SaveChangesAsync();

                    using (var stream = File.Create(systemFileName))
                    {
                        await file.CopyToAsync(stream);
                    }

                    responseModel.ResponseData = systemFileName;

                    return responseModel;
                }

                responseModel.ValidationErrors.Add(DefaultResponseMessages.WrongFileFormatProvided);

                return responseModel;
            }
            else
            {
                responseModel.ValidationErrors.Add(DefaultResponseMessages.WrongFileFormatProvided);

                return responseModel;
            }
        }

        /// <summary>
        /// To store customer files to the DB and map them with other customer data
        /// </summary>
        /// <param name="file"></param>
        public async Task<DefaultServiceResponse> LoadFileToTheDB(IFormFile file, string customerId) // todo - check that we can return error list in some cases
        {
            var responseModel = new DefaultServiceResponse() { ValidationErrors = new List<string>() };

            var fileExtension = Path.GetExtension(file.FileName); // or try to use Name

            if (fileExtension != null && (fileExtension == ".jpg" || fileExtension == ".png")) // for now we should work only with this type of files ( can be set-Up in the configuration file )
            {
                var randomFileName = Path.GetRandomFileName().Replace(".", ""); // replace all the dots to be able to use this files later in the server

                var systemFileName = Path.Combine(Configuration.Value.CustomerFilesFolder, randomFileName, fileExtension);

                var checkResult = await FileExtensionCheck(file, fileExtension, systemFileName);

                if (Configuration.Value.SaveFilesWithWrongFormat) //file type is save, as file extension ( no viruses )
                {
                    using (var stream = new MemoryStream())
                    {
                        await file.CopyToAsync(stream);

                        DataContext.AccountLevelFiles.Add(new AccountLevelFile
                        {
                            FileId = new Guid(),
                            CustomerId = customerId,
                            FullName = systemFileName,
                            FileData = stream.ToArray(),
                        });

                        DataContext.SaveChanges();
                    }

                    responseModel.ResponseData = systemFileName;

                    return responseModel;
                }

                responseModel.ValidationErrors.Add(DefaultResponseMessages.WrongFileFormatProvided);

                return responseModel;
            }
            else
            {
                responseModel.ValidationErrors.Add(DefaultResponseMessages.WrongFileFormatProvided);

                return responseModel;
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
        private async Task<bool> FileExtensionCheck(IFormFile file, string fileExtension, string fileName)
        {
            // check that file really have this extensions

            var result = false;

            using (var fileData = File.Create(fileName)) // store file localy before check, and after that delete it
            {
                await file.CopyToAsync(fileData);

                using (var reader = new BinaryReader(fileData))
                {
                    var signatures = FileSignatures[fileExtension];

                    var countOfByteToRead = signatures.Max(m => m.Length);

                    var firstBytes = reader.ReadBytes(countOfByteToRead); // find why there is no first bytes

                    result = signatures.Any(signature => firstBytes.Take(signature.Length).SequenceEqual(signature));
                }

                File.Delete(fileName);

                return result;
            }
        }

        /// <summary>
        /// Method to perform file modification using Magick.Net
        /// </summary>
        /// <returns>Path to the new file ( modified )</returns>
        public async Task<DefaultServiceResponse> ModifyFile(ModifyModel model)
        {
            // Get last uploaded photo by the customer
            var lastUploadedImageName = DataContext.CustomerImageFiles.ToList().Where(imageFile => imageFile.CustomerId == model.UserId).OrderByDescending(orderBy => orderBy.UploadTime).Select(response => response.FullName);

            var intesivity = model.Intesivity;

            var editedFileName = "";

            using (var image = new MagickImage(lastUploadedImageName.First()))
            {
                switch (model.SelectedOperation) // todo - add logic to have normal filters
                {
                    case "CycleColorMap":
                        image.CycleColormap(intesivity);
                        break;
                    case "FloodFill":
                        image.FloodFill(10, 10, 10);
                        break;
                    case "Flop":
                        image.Flop();
                        break;
                    case "GammaCorrect":
                        image.GammaCorrect(intesivity); // gramma
                        break;
                    case "GausiianBlur":
                        image.GaussianBlur(intesivity, intesivity); // double radius + double sigma
                        break;
                    case "MedianFilter":
                        image.MedianFilter(intesivity); // radius
                        break;
                    case "MotionBlur":
                        image.MotionBlur(10, 10, 10);
                        break;
                    case "Negate":
                        image.Negate(); // simple negate 
                        break;

                    default:
                        break;
                }
                if(model.UseFrame)
                {
                    image.Border(7, 7);
                }

                editedFileName = lastUploadedImageName.First().Replace(Path.GetFileNameWithoutExtension(lastUploadedImageName.First()), Path.GetFileNameWithoutExtension(lastUploadedImageName.First()) + "_modified").Replace(Path.GetExtension(lastUploadedImageName.First()), "." + model.OutputFileType.ToLower());
                using (File.Create(editedFileName))
                    image.Write(editedFileName);
            }

            // Save file to the DB

            var originalImageId = DataContext.CustomerImageFiles.Where(imageData => imageData.CustomerId == model.UserId && imageData.FullName.Contains(Path.GetFileNameWithoutExtension(lastUploadedImageName.First()))).Select(result => result.Id).First();

            await DataContext.CustomerEditedImageFiles.AddAsync(new ResultImageFIleModel
            {
                Id = new Guid(),
                CustomerId = model.UserId,
                FullName = editedFileName,
                OriginalImageId = originalImageId.ToString(),
                UploadTime = DateTime.Now
            });

            await DataContext.SaveChangesAsync();

            return new DefaultServiceResponse { ResponseData = "../" + Path.Combine("CustomersImages", Path.GetFileName(editedFileName)) };
        }
    }
}
