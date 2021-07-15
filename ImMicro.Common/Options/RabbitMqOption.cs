namespace ImMicro.Common.Options
{
    public class RabbitMqOption
    {
        public string RabbitMqUri { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        
        public string UserInitQueue { get; set; }
    }
}