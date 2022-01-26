using System;

namespace ImMicro.Contract.App.User
{
    public class UserSearchView
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Type { get; set; }
    }
}