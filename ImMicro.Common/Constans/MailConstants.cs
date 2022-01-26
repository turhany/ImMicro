namespace ImMicro.Common.Constans
{
    public class MailConstants
    {
        public const string SendGridApiKey = "SendGrid_Api_Key";
        
        public const string SubjectPrefix = "ImMicro - ";
        public const string FromEmail = "yildirimturhan@gmail.com";
        public const string FromName = "Türhan Yıldırım";

        #region Templates
        public const string ForgotPasswordMailTemplate = "Login/ForgotPassword.cshtml";
        public const string VerifyEmailMailTemplate = "Login/VerifyEmail.cshtml";
        #endregion
    }
}