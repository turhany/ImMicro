using System;
using Exporty.Models;

namespace ImMicro.Contract.Audit
{
    public class AuditLogView
    {
        public Guid Id { get; set; }
        public string KeyPropertyValue  { get; set; }
        [Export(ColumnName = "Entity Name")]
        public string EntityName { get; set; }
        [Export(ColumnName = "Operation Type")]
        public string OperationType { get; set; }
        [Export(ColumnName = "Old Value")]
        public string OldValue { get; set; }
        [Export(ColumnName = "New Value")]
        public string NewValue { get; set; }
        public string EnvironmentData { get; set; }
        [Export(ColumnName = "Created Date")]
        public DateTime CreatedOn { get; set; }
    }
}