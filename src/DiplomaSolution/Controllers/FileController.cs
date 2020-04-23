using System;
using System.IO;
using DiplomaSolution.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DiplomaSolution.Controllers
{
    public class FileController : Controller
    {
        private IFileManagerService fileManagerService { get; set; }

        public FileController(IFileManagerService fileManager)
        {
            fileManagerService = fileManager;
        }

        [HttpPost]
        public string LoadFile(IFormFile file)
        {
            fileManagerService.LoadFileToTheServer(file);

            return "Done";
        }
    }
}
