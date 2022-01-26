using ImMicro.Common.Data;

namespace ImMicro.Model.AuditLog
{
    public class AuditLog : SoftDeleteEntity
    {
        public string KeyPropertyValue  { get; set; }
        public string EntityName { get; set; }
        public string OperationType { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public string EnvironmentData { get; set; }
    }
}