namespace DiplomaSolution.Services.Models
{
    /// <summary>
    /// Model to represent editing file
    /// </summary>
    public class ModifyModel
    {
        public string UserId { get; set; }
        public string OutputFileType { get; set; }
        public string SelectedOperation { get; set; }
        public int Intesivity { get; set; }
        public bool UseFrame { get; set; }
    }
}
