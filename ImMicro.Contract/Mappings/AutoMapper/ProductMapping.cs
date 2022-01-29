using AutoMapper;
using ImMicro.Contract.App.Product;
using ImMicro.Contract.Service.Product;
using ImMicro.Model.Product;

namespace ImMicro.Contract.Mappings.AutoMapper;

public class ProductMapping : Profile
{
    public ProductMapping()
    {
        CreateMap<CreateProductRequest, CreateProductRequestServiceRequest>();
        CreateMap<UpdateProductRequest, UpdateProductRequestServiceRequest>();
        CreateMap<Product, ProductView>().ForMember(p => p.CategoryName, b => b.MapFrom(p => p.Category.Name));
    }
}