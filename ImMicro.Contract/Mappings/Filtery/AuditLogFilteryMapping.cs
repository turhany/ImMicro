using Filtery.Configuration.Filtery;
using Filtery.Constants;
using Filtery.Models.Filter;
using ImMicro.Model.AuditLog;

namespace ImMicro.Contract.Mappings.Filtery
{
    public class AuditLogFilteryMapping: AbstractFilteryMapping<AuditLog>
    {
        public AuditLogFilteryMapping()
        {
            mapper.Name("keypropertyvalue")
                .OrderProperty(p => p.KeyPropertyValue)
                .Filter(p => p.KeyPropertyValue.ToLower().Equals(FilteryQueryValueMarker.FilterStringValue.ToLower()), FilterOperation.Equal)
                .Filter(p => !p.KeyPropertyValue.ToLower().Equals(FilteryQueryValueMarker.FilterStringValue.ToLower()), FilterOperation.NotEqual)
                .Filter(p => p.KeyPropertyValue.ToLower().Contains(FilteryQueryValueMarker.FilterStringValue.ToLower()), FilterOperation.Contains)
                .Filter(p => p.KeyPropertyValue.ToLower().StartsWith(FilteryQueryValueMarker.FilterStringValue.ToLower()), FilterOperation.StartsWith)
                .Filter(p => p.KeyPropertyValue.ToLower().EndsWith(FilteryQueryValueMarker.FilterStringValue.ToLower()), FilterOperation.EndsWith);
            
            mapper.Name("entityname")
                .OrderProperty(p => p.EntityName)
                .Filter(p => p.EntityName.ToLower().Equals(FilteryQueryValueMarker.FilterStringValue.ToLower()), FilterOperation.Equal)
                .Filter(p => !p.EntityName.ToLower().Equals(FilteryQueryValueMarker.FilterStringValue.ToLower()), FilterOperation.NotEqual)
                .Filter(p => p.EntityName.ToLower().Contains(FilteryQueryValueMarker.FilterStringValue.ToLower()), FilterOperation.Contains)
                .Filter(p => p.EntityName.ToLower().StartsWith(FilteryQueryValueMarker.FilterStringValue.ToLower()), FilterOperation.StartsWith)
                .Filter(p => p.EntityName.ToLower().EndsWith(FilteryQueryValueMarker.FilterStringValue.ToLower()), FilterOperation.EndsWith);
            
            mapper.Name("operationtype")
                .OrderProperty(p => p.OperationType)
                .Filter(p => p.OperationType.ToLower().Equals(FilteryQueryValueMarker.FilterStringValue.ToLower()), FilterOperation.Equal)
                .Filter(p => !p.OperationType.ToLower().Equals(FilteryQueryValueMarker.FilterStringValue.ToLower()), FilterOperation.NotEqual)
                .Filter(p => p.OperationType.ToLower().Contains(FilteryQueryValueMarker.FilterStringValue.ToLower()), FilterOperation.Contains)
                .Filter(p => p.OperationType.ToLower().StartsWith(FilteryQueryValueMarker.FilterStringValue.ToLower()), FilterOperation.StartsWith)
                .Filter(p => p.OperationType.ToLower().EndsWith(FilteryQueryValueMarker.FilterStringValue.ToLower()), FilterOperation.EndsWith);
            
            mapper.Name("oldvalue")
                .OrderProperty(p => p.OldValue)
                .Filter(p => p.OldValue.ToLower().Equals(FilteryQueryValueMarker.FilterStringValue.ToLower()), FilterOperation.Equal)
                .Filter(p => !p.OldValue.ToLower().Equals(FilteryQueryValueMarker.FilterStringValue.ToLower()), FilterOperation.NotEqual)
                .Filter(p => p.OldValue.ToLower().Contains(FilteryQueryValueMarker.FilterStringValue.ToLower()), FilterOperation.Contains)
                .Filter(p => p.OldValue.ToLower().StartsWith(FilteryQueryValueMarker.FilterStringValue.ToLower()), FilterOperation.StartsWith)
                .Filter(p => p.OldValue.ToLower().EndsWith(FilteryQueryValueMarker.FilterStringValue.ToLower()), FilterOperation.EndsWith);
            
            mapper.Name("newvalue")
                .OrderProperty(p => p.NewValue)
                .Filter(p => p.NewValue.ToLower().Equals(FilteryQueryValueMarker.FilterStringValue.ToLower()), FilterOperation.Equal)
                .Filter(p => !p.NewValue.ToLower().Equals(FilteryQueryValueMarker.FilterStringValue.ToLower()), FilterOperation.NotEqual)
                .Filter(p => p.NewValue.ToLower().Contains(FilteryQueryValueMarker.FilterStringValue.ToLower()), FilterOperation.Contains)
                .Filter(p => p.NewValue.ToLower().StartsWith(FilteryQueryValueMarker.FilterStringValue.ToLower()), FilterOperation.StartsWith)
                .Filter(p => p.NewValue.ToLower().EndsWith(FilteryQueryValueMarker.FilterStringValue.ToLower()), FilterOperation.EndsWith);
            
            mapper
                .Name("createdon")
                .OrderProperty(p => p.CreatedOn)
                .Filter(p => p.CreatedOn == FilteryQueryValueMarker.FilterDateTimeValue, FilterOperation.Equal)
                .Filter(p => p.CreatedOn != FilteryQueryValueMarker.FilterDateTimeValue, FilterOperation.NotEqual)
                .Filter(p => p.CreatedOn > FilteryQueryValueMarker.FilterDateTimeValue, FilterOperation.GreaterThan)
                .Filter(p => p.CreatedOn < FilteryQueryValueMarker.FilterDateTimeValue, FilterOperation.LessThan)
                .Filter(p => p.CreatedOn >= FilteryQueryValueMarker.FilterDateTimeValue, FilterOperation.GreaterThanOrEqual)
                .Filter(p => p.CreatedOn <= FilteryQueryValueMarker.FilterDateTimeValue, FilterOperation.LessThanOrEqual);
        }
    }
}