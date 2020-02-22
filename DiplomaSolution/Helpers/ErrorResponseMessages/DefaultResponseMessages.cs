namespace DiplomaSolution.Helpers.ErrorResponseMessages
{
    public class DefaultResponseMessages
    {
        public static string WrongPasswordAndEmailCombination { get; } =  "";
        public static string EmailIsNotVerified { get; } = "";
        public static string CustomerIsNotFoundInDb { get; } = "Some error occured, please contact local admin";
        public static string ExternalLoginFailed { get; } = "Login failed, partner issue";
        public static string EmailIsNotProvided { get; } = "";
    }
}
