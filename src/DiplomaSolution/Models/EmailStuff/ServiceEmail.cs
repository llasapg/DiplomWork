namespace DiplomaSolution.Models
{
    public class ServiceEmail
    {
        public string FromEmail { get; set; }
        public string ToEmail { get; set; }
        public string FromName { get; set; }
        public string ToName { get; set; }
        public string EmailSubject { get; set; }
        public string EmailText { get; set; }
        public string EmailHtmlText { get; set; }
    }
}
