namespace DiplomaSolution.Models
{
    /// <summary>
    /// Enum to describe response from authorization or other services provided to login customer or etc...
    /// </summary>
    public enum StatusCodesEnum
    {
        /// <summary>
        /// Data was handled no more actions needed ( it should be used in case of endpoint )
        /// </summary>
        Ok = 200,
        /// <summary>
        /// Data was handled but redirect needed to next action 
        /// </summary>
        RedirectNeeded = 300,
        /// <summary>
        /// Customer provided bad data --> return again view with validation errors
        /// </summary>
        BadDataProvided = 404
    }
}
