using System;

namespace ImMicro.Common.Auth.Concrete
{
    public class JwtContract
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public object UserType { get; set; }
    }
}