using System;
using System.Dynamic;
using System.Threading;
using System.Threading.Tasks;
using Filtery.Models;
using ImMicro.Common.BaseModels.Service;
using ImMicro.Common.Data.Abstract;
using ImMicro.Common.Pager;
using ImMicro.Contract.App.Product;
using ImMicro.Contract.Service.Product;

namespace ImMicro.Business.Product.Abstract;

public interface IProductService: IService
{
    Task<ServiceResult<ProductView>> GetAsync(Guid id, CancellationToken cancellationToken);  
    Task<ServiceResult<PagedList<ProductView>>> SearchAsync(FilteryRequest request, CancellationToken cancellationToken);
    Task<ServiceResult<ExpandoObject>> CreateAsync(CreateProductRequestServiceRequest request, CancellationToken cancellationToken);
    Task<ServiceResult<ExpandoObject>> UpdateAsync(UpdateProductRequestServiceRequest request, CancellationToken cancellationToken);
    Task<ServiceResult<ExpandoObject>> DeleteAsync(Guid id, CancellationToken cancellationToken);
}