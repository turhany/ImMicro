using ImMicro.Common.Data;

namespace ImMicro.Data
{
    public class User : Entity
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }

        public bool InProcess { get; set; }
        public bool IsNotified { get; set; }
    }
}