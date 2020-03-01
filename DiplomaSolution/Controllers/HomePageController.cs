using DiplomaSolution.Models;
using DiplomaSolution.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;
using Microsoft.Extensions.Logging;
using DiplomaSolution.Helpers.Logging;

namespace DiplomaSolution.Controllers
{
    /// <summary>
    /// Main page controller
    /// </summary>
    public class HomePageController : Controller
    {
        /// <summary>
        /// File manager service ( upload, etc.. )
        /// </summary>
        private IFileManagerService FileManagerService { get; set; }

        private ILoggerFactory ILoggerFactory { get; set; }

        private ILogger<FileLogger> Logger { get; set; }

        /// <summary>
        /// Service resolving
        /// </summary>
        /// <param name="service"></param>
        public HomePageController(IFileManagerService service, ILoggerFactory loggerFactory)
        {
            ILoggerFactory = loggerFactory;
            FileManagerService = service;
            Logger = ILoggerFactory.CreateLogger<FileLogger>();
        }

        /// <summary>
        /// Action to return main page of the application
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Index()
        {
            Logger.LogCritical("Index action hitted ( first log) ");

            return View();
        }

        /// <summary>
        /// Action to upload the photo to web-site ( change flow, so photo will be pinned with selected user )
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public IActionResult Index(IndexViewData data)
        {
            if (data.FormFileData != null)
            {
                FileManagerService.LoadFileToTheServer(data.FormFileData);
            }

            var viewModel = new IndexViewData { FormFileData = data.FormFileData };

            return View(viewModel);
        }
    }
}
