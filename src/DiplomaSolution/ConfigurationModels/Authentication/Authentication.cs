namespace DiplomaSolution.ConfigurationModels
{
    /// <summary>
    /// Base model to hold configurations connected to external providers
    /// </summary>
    public class Authentication
    {
        /// <summary>
        /// Ptop to hold Facebook configurations
        /// </summary>
        public FacebookAuthentication FacebookAuthentication { get; set; }
        /// <summary>
        /// Prop to hold Google configurations
        /// </summary>
        public GoogleAuthentication GoogleAuthentication { get; set; }
        /// <summary>
        /// Prop to hold SendGrid configurations
        /// </summary>
        public SendGridAuthentication SendGridAuthentication { get; set; }
    }
}
