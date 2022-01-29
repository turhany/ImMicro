using Filtery.Configuration.Filtery;
using Filtery.Constants;
using Filtery.Models.Filter;
using ImMicro.Model.Product;

namespace ImMicro.Contract.Mappings.Filtery;

public class ProductFilteryMapping: AbstractFilteryMapping<Product>
{
    public ProductFilteryMapping()
    {
        mapper
            .Name("title")
            .OrderProperty(p =>p.Title)
            .Filter(p => p.Title.ToLower().Equals(FilteryQueryValueMarker.FilterStringValue.ToLower()), FilterOperation.Equal)
            .Filter(p => !p.Title.ToLower().Equals(FilteryQueryValueMarker.FilterStringValue.ToLower()), FilterOperation.NotEqual)
            .Filter(p => p.Title.ToLower().Contains(FilteryQueryValueMarker.FilterStringValue.ToLower()), FilterOperation.Contains)
            .Filter(p => p.Title.ToLower().StartsWith(FilteryQueryValueMarker.FilterStringValue.ToLower()), FilterOperation.StartsWith)
            .Filter(p => p.Title.ToLower().EndsWith(FilteryQueryValueMarker.FilterStringValue.ToLower()), FilterOperation.EndsWith);
            
        mapper
            .Name("description")
            .OrderProperty(p =>p.Description)
            .Filter(p => p.Description.ToLower().Equals(FilteryQueryValueMarker.FilterStringValue.ToLower()), FilterOperation.Equal)
            .Filter(p => !p.Description.ToLower().Equals(FilteryQueryValueMarker.FilterStringValue.ToLower()), FilterOperation.NotEqual)
            .Filter(p => p.Description.ToLower().Contains(FilteryQueryValueMarker.FilterStringValue.ToLower()), FilterOperation.Contains)
            .Filter(p => p.Description.ToLower().StartsWith(FilteryQueryValueMarker.FilterStringValue.ToLower()), FilterOperation.StartsWith)
            .Filter(p => p.Description.ToLower().EndsWith(FilteryQueryValueMarker.FilterStringValue.ToLower()), FilterOperation.EndsWith);
        
        mapper
            .Name("category")
            .OrderProperty(p =>p.Category.Name)
            .Filter(p => p.Category.Name.ToLower().Equals(FilteryQueryValueMarker.FilterStringValue.ToLower()), FilterOperation.Equal)
            .Filter(p => !p.Category.Name.ToLower().Equals(FilteryQueryValueMarker.FilterStringValue.ToLower()), FilterOperation.NotEqual)
            .Filter(p => p.Category.Name.ToLower().Contains(FilteryQueryValueMarker.FilterStringValue.ToLower()), FilterOperation.Contains)
            .Filter(p => p.Category.Name.ToLower().StartsWith(FilteryQueryValueMarker.FilterStringValue.ToLower()), FilterOperation.StartsWith)
            .Filter(p => p.Category.Name.ToLower().EndsWith(FilteryQueryValueMarker.FilterStringValue.ToLower()), FilterOperation.EndsWith);
        
        mapper
            .Name("stock")
            .OrderProperty(p => p.StockQuantity)
            .Filter(p => p.StockQuantity == FilteryQueryValueMarker.FilterIntValue, FilterOperation.Equal)
            .Filter(p => p.StockQuantity != FilteryQueryValueMarker.FilterIntValue, FilterOperation.NotEqual)
            .Filter(p => p.StockQuantity > FilteryQueryValueMarker.FilterIntValue, FilterOperation.GreaterThan)
            .Filter(p => p.StockQuantity < FilteryQueryValueMarker.FilterIntValue, FilterOperation.LessThan)
            .Filter(p => p.StockQuantity >= FilteryQueryValueMarker.FilterIntValue, FilterOperation.GreaterThanOrEqual)
            .Filter(p => p.StockQuantity <= FilteryQueryValueMarker.FilterIntValue, FilterOperation.LessThanOrEqual);
    }
}