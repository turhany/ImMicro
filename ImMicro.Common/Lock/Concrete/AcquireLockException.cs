using System; 

namespace ImMicro.Common.Lock.Concrete
{
    public class AcquireLockException : Exception
    {
        private static string ExceptionMessage = "The lock wasn't acquired. Key: {0}";
        public AcquireLockException(string key) : base(string.Format(ExceptionMessage, key))
        {

        }
    }
}
