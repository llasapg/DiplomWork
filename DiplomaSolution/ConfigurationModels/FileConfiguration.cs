using System.Collections.Generic;

namespace DiplomaSolution.ConfigurationModels
{
    /// <summary>
    /// Configuration model to represent app.json configurations 
    /// </summary>
    public class FileConfiguration
    {
        /// <summary>
        /// Path to customer files folder
        /// </summary>
        public string CustomerFilesFolder { get; set; }
        /// <summary>
        /// List of available formats
        /// </summary>
        public List<string> AvailableFileFormats { get; set; }
        /// <summary>
        /// Flag ot delete customer image in case, when validation is failed
        /// </summary>
        public bool SaveFilesWithWrongFormat { get; set; }
    }
}
