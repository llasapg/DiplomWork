using Microsoft.AspNetCore.Http;

namespace DiplomaSolution.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class IndexViewData
    {
        /// <summary>
        /// 
        /// </summary>
        public IFormFile FormFileData { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Customer Customer { get; set; }
    }
}
