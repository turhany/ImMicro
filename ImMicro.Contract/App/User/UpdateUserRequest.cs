using ImMicro.Model.User;

namespace ImMicro.Contract.App.User
{
    public class UpdateUserRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public UserType Type { get; set; }
        public bool IsActive { get; set; }
    }
}