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
                file.CopyTo(new FileStream($"wwwroot/CustomersImages/{file.FileName}", FileMode.Create));
            }
        }
    }
}
