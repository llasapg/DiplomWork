using System.Threading.Tasks;
using DiplomaSolution.Models;
using DiplomaSolution.Services.Models;
using Microsoft.AspNetCore.Http;

namespace DiplomaSolution.Services.Interfaces
{
    public interface IFileManagerService
    {
        /// <summary>
        /// So if we have large file we need to store it on our hardware
        /// </summary>
        /// <param name="file"></param>
        Task<DefaultServiceResponse> LoadFileToTheServer(IFormFile file, string customerId);

        /// <summary>
        /// In case of small file ( user profile image ) we can store it in the DB
        /// </summary>
        /// <param name="file"></param>
        Task<DefaultServiceResponse> LoadFileToTheDB(IFormFile file, string customerId);

        /// <summary>
        /// Method to perform file modification using Magick.Net
        /// </summary>
        /// <returns></returns>
        Task<DefaultServiceResponse> ModifyFile(ModifyModel modifyModel);
    }
}
