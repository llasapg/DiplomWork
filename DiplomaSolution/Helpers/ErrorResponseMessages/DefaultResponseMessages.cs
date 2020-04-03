namespace DiplomaSolution.Helpers.ErrorResponseMessages
{
    public static class DefaultResponseMessages
    {
        public static string WrongPasswordAndEmailCombination { get; } =  "Your email or password are wrong, please try different";
        public static string EmailIsNotVerified { get; } = "Sorry, your current email is not verified";
        public static string CustomerIsNotFoundInDb { get; } = "Some error occured, please contact local admin";
        public static string ExternalLoginFailed { get; } = "Login failed, partner issue";
        public static string EmailIsNotProvided { get; } = "Please, provide email";
        public static string AccountIsLockOut { get; set; } = "Ooops, your login tries ended, please wait 20 min and try again";
        public static string AllreadyHasAccount { get; set; } = "Hey, you already have account, try to login please";
        public static string WrongFileFormatProvided { get; set; } = "Hey, you should try to use another file format ( like jpg or png )";
    }
}
