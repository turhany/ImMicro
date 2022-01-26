namespace ImMicro.Contract.Service.User
{
    public class ChangePasswordServiceRequest
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string NewPasswordConfirm { get; set; }
    }
}
