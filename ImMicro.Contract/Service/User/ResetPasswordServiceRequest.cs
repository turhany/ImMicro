namespace ImMicro.Contract.Service.User
{
    public class ResetPasswordServiceRequest
    {
        public string Password { get; set; }
        public string PasswordConfirm { get; set; }
    }
}
