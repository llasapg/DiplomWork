using System;
using System.IO;
using DiplomaSolution.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using DiplomaSolution.Models;

namespace DiplomaSolution.Services.Classes
{
    public class FileManagerService : IFileManagerService
    {
        private CustomerContext CustomerContext { get; set; }

        public FileManagerService(CustomerContext customerContext)
        {
            CustomerContext = customerContext;
        }

        /// <summary>
        /// Loads customer images to DB to deal with them in future
        /// </summary>
        /// <param name="file"></param>
        public void LoadFileToTheServer(IFormFile file) //todo
        {
            if (file != null)
            {
                using (var fileStream = file.OpenReadStream())
                {
                    using (var br = new BinaryReader(fileStream))
                    {
                        var byteFileData = br.ReadBytes((Int32)fileStream.Length);

                        CustomerContext.CustomerFiles.Add(new Models.FormFile
                        {
                            FileData = byteFileData,
                            FullName = Path.GetFileName(file.FileName)
                        });
                    }

                    CustomerContext.SaveChanges();
                }

                file.CopyTo(new FileStream($"wwwroot/CustomersImages/{file.FileName}", FileMode.Create));
            }
        }
    }
}
