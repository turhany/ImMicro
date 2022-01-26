namespace ImMicro.Contract.App.User
{
    public class ResetPasswordRequest
    {
        public string Password { get; set; }
        public string PasswordConfirm { get; set; }
    }
}
