using System;
using System.Web;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using DiplomaSolution.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
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
        public void LoadFileToTheServer(IFormFile file)
        {
            using (var fileStream = file.OpenReadStream())
            {
                using(BinaryReader br = new BinaryReader(fileStream))
                {
                    var byteFileData = br.ReadBytes((Int32)fileStream.Length);

                    CustomerContext.CustomerFiles.Add(new FormFile {
                    FileData = byteFileData,
                    FullName = Path.GetFileName(file.FileName)
                    });  
                }

                CustomerContext.SaveChanges();
            }
        }
    }
}
