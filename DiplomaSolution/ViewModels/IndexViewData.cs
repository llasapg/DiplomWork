using Microsoft.AspNetCore.Http;

namespace DiplomaSolution.Models
{
    public class IndexViewData
    {
        public IFormFile FormFileData { get; set; }
        public Customer Customer { get; set; }
    }
}
