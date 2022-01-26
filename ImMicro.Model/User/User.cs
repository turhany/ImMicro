using System;
using ImMicro.Common.Data;

namespace ImMicro.Model.User
{
    public class User : SoftDeleteEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }
        public UserType Type { get; set; }
        public string EmailVerificationToken { get; set; }
        public bool EmailVerificationTokenIsUsed { get; set; }
        public string ForgotPasswordToken { get; set; }
        public bool ForgotPasswordTokenIsUsed { get; set; }
        public string ResreshToken { get; set; }
        public DateTime ResreshTokenExpireDate { get; set; }
    }
}