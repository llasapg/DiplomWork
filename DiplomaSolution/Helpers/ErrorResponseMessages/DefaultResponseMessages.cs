namespace DiplomaSolution.Helpers.ErrorResponseMessages
{
    public class DefaultResponseMessages
    {
        public static string WrongPasswordAndEmailCombination { get; } =  "";
        public static string EmailIsNotVerified { get; } = "";
        public static string CustomerIsNotFoundInDb { get; } = "Some error occured, please contact local admin";
        public static string ExternalLoginFailed { get; } = "Login failed, partner issue";
        public static string EmailIsNotProvided { get; } = "";
        public static string AccountIsLockOut { get; set; } = "Ooops, your login tries ended, please wait 20 min and try again";
        public static string AllreadyHasAccount { get; set; } = "Hey, you already have account, try to login please";
    }
}
