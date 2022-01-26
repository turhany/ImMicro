using System;

namespace ImMicro.Common.Application
{
    public class CurrentUser
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
    }
}