using Filtery.Configuration.Filtery;
using Filtery.Constants;
using Filtery.Models.Filter;
using ImMicro.Model.User;

namespace ImMicro.Contract.Mappings.Filtery
{
    public class UserFilteryMapping : AbstractFilteryMapping<User>
    {
        public UserFilteryMapping()
        {
            mapper
                .Name("firstname")
                .OrderProperty(p =>p.FirstName)
                .Filter(p => p.FirstName.ToLower().Equals(FilteryQueryValueMarker.FilterStringValue.ToLower()), FilterOperation.Equal)
                .Filter(p => !p.FirstName.ToLower().Equals(FilteryQueryValueMarker.FilterStringValue.ToLower()), FilterOperation.NotEqual)
                .Filter(p => p.FirstName.ToLower().Contains(FilteryQueryValueMarker.FilterStringValue.ToLower()), FilterOperation.Contains)
                .Filter(p => p.FirstName.ToLower().StartsWith(FilteryQueryValueMarker.FilterStringValue.ToLower()), FilterOperation.StartsWith)
                .Filter(p => p.FirstName.ToLower().EndsWith(FilteryQueryValueMarker.FilterStringValue.ToLower()), FilterOperation.EndsWith);
            
            mapper
                .Name("lastname")
                .OrderProperty(p =>p.LastName)
                .Filter(p => p.LastName.ToLower().Equals(FilteryQueryValueMarker.FilterStringValue.ToLower()), FilterOperation.Equal)
                .Filter(p => !p.LastName.ToLower().Equals(FilteryQueryValueMarker.FilterStringValue.ToLower()), FilterOperation.NotEqual)
                .Filter(p => p.LastName.ToLower().Contains(FilteryQueryValueMarker.FilterStringValue.ToLower()), FilterOperation.Contains)
                .Filter(p => p.LastName.ToLower().StartsWith(FilteryQueryValueMarker.FilterStringValue.ToLower()), FilterOperation.StartsWith)
                .Filter(p => p.LastName.ToLower().EndsWith(FilteryQueryValueMarker.FilterStringValue.ToLower()), FilterOperation.EndsWith);
        }
    }
}