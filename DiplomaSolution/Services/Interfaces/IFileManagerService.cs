using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace DiplomaSolution.Services.Interfaces
{
    public interface IFileManagerService
    {
        void LoadFileToTheServer(IFormFile file);
    }
}
