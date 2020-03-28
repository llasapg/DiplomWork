using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace DiplomaSolution.Services.Interfaces
{
    public interface IFileManagerService
    {
        /// <summary>
        /// So if we have large file we need to store it on our hardware
        /// </summary>
        /// <param name="file"></param>
        Task LoadFileToTheServer(IFormFile file, string customerId);

        /// <summary>
        /// In case of small file ( user profile image ) we can store it in the DB
        /// </summary>
        /// <param name="file"></param>
        Task LoadFileToTheDB(IFormFile file, string customerId);
    }
}
