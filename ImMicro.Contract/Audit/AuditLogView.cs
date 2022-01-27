using System;

namespace ImMicro.Contract.Audit
{
    public class AuditLogView
    {
        public Guid Id { get; set; } 
        public string KeyPropertyValue  { get; set; }
        public string EntityName { get; set; }
        public string OperationType { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public string EnvironmentData { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}