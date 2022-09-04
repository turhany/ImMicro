namespace ImMicro.Lock.Redis.Options
{
    public class RedLockOption
    {
        public string HostAddress { get; set; }
        public string HostPort { get; set; }
        public string HostPassword { get; set; }
        public bool HostSsl { get; set; }
        public int ExpireTimeAsSecond { get; set; }
        public int WaitTimeAsSecond { get; set; }
        public int RetryTimeAsSecond { get; set; }
    }
}