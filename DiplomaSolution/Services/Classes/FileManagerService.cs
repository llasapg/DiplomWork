using System;
using System.Web;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using DiplomaSolution.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;

namespace DiplomaSolution.Services.Classes
{
    public class FileManagerService : IFileManagerService
    {
        private IHostingEnvironment HostingEnvironment { get; set; }

        public FileManagerService(IHostingEnvironment hostingEnvironment)
        {
            HostingEnvironment = hostingEnvironment;
        }

        public void LoadFileToTheServer(IFormFile file)
        {
            if (file != null)
            {
                var filePath = Path.Combine(HostingEnvironment.WebRootPath + "/CustomersImages" + "/" + Path.GetFileName(file.FileName));

                file.CopyTo(new FileStream(filePath, FileMode.Create));
            }
        }
    }
}
