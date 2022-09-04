using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Dapper;
using Filtery.Extensions;
using Filtery.Models;
using ImMicro.Business.Product.Abstract;
using ImMicro.Business.Product.Validator;
using ImMicro.Cache.Abstract;
using ImMicro.Cache.Constants;
using ImMicro.Common.BaseModels.Service; 
using ImMicro.Common.Constans;
using ImMicro.Common.Dapper; 
using ImMicro.Common.Pager;
using ImMicro.Contract.App.Product;
using ImMicro.Contract.Mappings.Filtery;
using ImMicro.Contract.Service.Product;
using ImMicro.Data.BaseRepositories;
using ImMicro.Lock.Abstract; 
using ImMicro.Resources.Extensions;
using ImMicro.Resources.Model;
using ImMicro.Resources.Service;
using ImMicro.Validation.Abstract;
using Microsoft.EntityFrameworkCore;

namespace ImMicro.Business.Product.Concrete;

public class ProductService : IProductService
{
    private readonly IGenericRepository<Model.Product.Product> _productRepository; 
    private readonly IGenericRepository<Model.Category.Category> _categoryRepository; 
    private readonly ICacheService _cacheService;
    private readonly ILockService _lockService;
    private readonly IMapper _mapper;
    private readonly IValidationService _validationService;
    private readonly DapperContext _dapperContext;
    
    public ProductService(
        IGenericRepository<Model.Product.Product> productRepository,
        IGenericRepository<Model.Category.Category> categoryRepository,
        ICacheService cacheService, 
        ILockService lockService, 
        IMapper mapper, 
        IValidationService validationService, 
        DapperContext dapperContext)
    {
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
        _cacheService = cacheService;
        _lockService = lockService;
        _mapper = mapper;
        _validationService = validationService;
        _dapperContext = dapperContext;
    }

    public async Task<ServiceResult<ProductView>> GetAsync(Guid id, CancellationToken cancellationToken)
    {
        var cacheKey = string.Format(CacheKeyConstants.ProductCacheKey, id);

        var product = await _cacheService.GetOrSetObjectAsync(cacheKey, async () => 
        await _productRepository.FindOneAsync(p => p.Id == id && p.IsDeleted == false, cancellationToken),
        CacheConstants.DefaultCacheDuration,
        cancellationToken);

        if (product == null)
        {
            return new ServiceResult<ProductView>
            {
                Status = ResultStatus.ResourceNotFound,
                Message = Resource.NotFound(Entities.Product)
            };
        }
            
        return new ServiceResult<ProductView>
        {
            Status = ResultStatus.Successful,
            Message = Resource.Retrieved(),
            Data = _mapper.Map<ProductView>(product)
        };
    }
    
    public async Task<ServiceResult<ProductView>> GetWithDapperAsync(Guid id, CancellationToken cancellationToken)
    {
        ProductView product;
        using (var connection = _dapperContext.CreateConnection())
        {
            product = await connection.QuerySingleOrDefaultAsync<ProductView>(
                @"SELECT
                       p.""Id"",
                       p.""Title"",
                       p.""Description"",
                       p.""StockQuantity"",
                       c.""Id"" CategoryId,
                       c.""Name"" CategoryName
                    FROM ""Product"" p 
                    INNER JOIN ""Category"" c ON c.""Id"" = p.""CategoryId"" 
                    WHERE p.""Id"" = @Id AND p.""IsDeleted"" = false", 
                new {Id = id}, 
                commandType:CommandType.Text);
        }
        
        if (product == null)
        {
            return new ServiceResult<ProductView>
            {
                Status = ResultStatus.ResourceNotFound,
                Message = Resource.NotFound(Entities.Product)
            };
        }
            
        return new ServiceResult<ProductView>
        {
            Status = ResultStatus.Successful,
            Message = Resource.Retrieved(),
            Data = product
        };
    }

    public async Task<ServiceResult<ExpandoObject>> CreateAsync(CreateProductRequestServiceRequest request, CancellationToken cancellationToken)
    {
        var validationResponse = _validationService.Validate(typeof(CreateProductRequestValidator), request);

        if (!validationResponse.IsValid)
        {
            return new ServiceResult<ExpandoObject>
            {
                Status = ResultStatus.InvalidInput,
                Message = ServiceResponseMessage.INVALID_INPUT_ERROR,
                ValidationMessages = validationResponse.ErrorMessages
            };
        }

        var category = await _categoryRepository.FindOneWithAsNoTrackingAsync(p => p.Id == request.CategoryId && p.IsDeleted == false, cancellationToken);
        if (category == null)
        {
            return new ServiceResult<ExpandoObject>
            {
                Status = ResultStatus.ResourceNotFound,
                Message = Resource.NotFound(Entities.Category)
            };
        }

        var entity = new Model.Product.Product
        {
            Title = request.Title.Trim(),
            Description = request.Description?.Trim(),
            StockQuantity = request.StockQuantity,
            IsActive = request.StockQuantity > category.MinStockQuantity,
            CategoryId = request.CategoryId
        };

        entity = await _productRepository.InsertAsync(entity, cancellationToken);

        dynamic productWrapper = new ExpandoObject();
        productWrapper.Id = entity.Id;

        return new ServiceResult<ExpandoObject>
        {
            Status = ResultStatus.Successful,
            Message = Resource.Created(Entities.Product, entity.Title),
            Data = productWrapper
        };
    }

    public async Task<ServiceResult<ExpandoObject>> UpdateAsync(UpdateProductRequestServiceRequest request, CancellationToken cancellationToken)
    {
        var validationResponse = _validationService.Validate(typeof(UpdateProductRequestValidator), request);

            if (!validationResponse.IsValid)
            {
                return new ServiceResult<ExpandoObject>
                {
                    Status = ResultStatus.InvalidInput,
                    Message = ServiceResponseMessage.INVALID_INPUT_ERROR,
                    ValidationMessages = validationResponse.ErrorMessages
                };
            }

            var entity = await _productRepository.FindOneAsync(p => p.Id == request.Id && p.IsDeleted == false, cancellationToken);

            if (entity == null)
            {
                return new ServiceResult<ExpandoObject>
                {
                    Status = ResultStatus.ResourceNotFound,
                    Message = Resource.NotFound(Entities.Product)
                };
            }

            var category = await _categoryRepository.FindOneWithAsNoTrackingAsync(p => p.Id == request.CategoryId && p.IsDeleted == false, cancellationToken);
            if (category == null)
            {
                return new ServiceResult<ExpandoObject>
                {
                    Status = ResultStatus.ResourceNotFound,
                    Message = Resource.NotFound(Entities.Category)
                };
            }

            var lockKey = string.Format(LockKeyConstants.ProductLockKey, entity.Id);
            var cacheKey = string.Format(CacheKeyConstants.ProductCacheKey, entity.Id);
            
            using (await _lockService.CreateLockAsync(lockKey, cancellationToken))
            {
                entity.Title = request.Title.Trim();
                entity.Description = request.Description?.Trim();
                entity.StockQuantity = request.StockQuantity;
                entity.IsActive = request.StockQuantity > category.MinStockQuantity;
                entity.CategoryId = request.CategoryId;

                entity = await _productRepository.UpdateAsync(entity, cancellationToken);

                await _cacheService.RemoveAsync(cacheKey, cancellationToken);
            }
            

            dynamic productWrapper = new ExpandoObject();
            productWrapper.Id = entity.Id;

            return new ServiceResult<ExpandoObject>
            {
                Status = ResultStatus.Successful,
                Message = Resource.Updated(Entities.Product, entity.Title),
                Data = productWrapper
            };
    }

    public async Task<ServiceResult<ExpandoObject>> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var entity = await _productRepository.FindOneAsync(p => p.Id == id && p.IsDeleted == false, cancellationToken);

        if (entity == null)
        {
            return new ServiceResult<ExpandoObject>
            {
                Status = ResultStatus.ResourceNotFound,
                Message = Resource.NotFound(Entities.Product)
            };
        }

        var lockKey = string.Format(LockKeyConstants.ProductLockKey, entity.Id);
        var cacheKey = string.Format(CacheKeyConstants.ProductCacheKey, entity.Id);
            
        using (await _lockService.CreateLockAsync(lockKey, cancellationToken))
        {
            await _productRepository.DeleteAsync(entity, cancellationToken); 
                
            await _cacheService.RemoveAsync(cacheKey, cancellationToken);   
        }

        return new ServiceResult<ExpandoObject>
        {
            Status = ResultStatus.Successful,
            Message = Resource.Deleted(Entities.Product, entity.Title)
        };
    }
    
    public async Task<ServiceResult<PagedList<ProductView>>> SearchAsync(FilteryRequest request, CancellationToken cancellationToken)
    {
        var filteryResponse = await _productRepository
            .Find(p => p.IsActive == true)
            .Include(p => p.Category)
            .AsNoTracking()
            .BuildFilteryAsync(new ProductFilteryMapping(), request);

        var response = new PagedList<ProductView>
        {
            Data = _mapper.Map<List<ProductView>>(filteryResponse.Data),
            PageInfo = new Page
            {
                PageNumber = filteryResponse.PageNumber,
                PageSize = filteryResponse.PageSize,
                TotalItemCount = filteryResponse.TotalItemCount
            }
        };
            
        return new ServiceResult<PagedList<ProductView>>
        {
            Data = response,
            Status = ResultStatus.Successful
        };
    }
}