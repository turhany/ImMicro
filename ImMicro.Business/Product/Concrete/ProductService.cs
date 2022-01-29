﻿using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Threading.Tasks;
using Filtery.Extensions;
using Filtery.Models;
using ImMicro.Business.Product.Abstract;
using ImMicro.Business.Product.Validator;
using ImMicro.Common.Application;
using ImMicro.Common.BaseModels.Service;
using ImMicro.Common.Cache.Abstract;
using ImMicro.Common.Constans;
using ImMicro.Common.Data.Abstract;
using ImMicro.Common.Lock.Abstract;
using ImMicro.Common.Pager;
using ImMicro.Common.Service;
using ImMicro.Contract.App.Product;
using ImMicro.Contract.Mappings.Filtery;
using ImMicro.Contract.Service.Product;
using ImMicro.Resources.Extensions;
using ImMicro.Resources.Model;
using ImMicro.Resources.Service;
using Microsoft.EntityFrameworkCore;

namespace ImMicro.Business.Product.Concrete;

public class ProductService : BaseApplicationService, IProductService
{
    private readonly IGenericRepository<Model.Product.Product> _productRepository; 
    private readonly IGenericRepository<Model.Category.Category> _categoryRepository; 
    private readonly ICacheService _cacheService;
    private readonly ILockService _lockService;
    
    public ProductService(
        IGenericRepository<Model.Product.Product> productRepository,
        IGenericRepository<Model.Category.Category> categoryRepository,
        ICacheService cacheService, 
        ILockService lockService)
    {
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
        _cacheService = cacheService;
        _lockService = lockService;
    }

    public async Task<ServiceResult<ProductView>> GetAsync(Guid id)
    {
        var cacheKey = string.Format(CacheKeyConstants.ProductCacheKey, id);

        var product = await _cacheService.GetOrSetObjectAsync(cacheKey, async () => await _productRepository.FindOneAsync(p => p.Id == id && p.IsDeleted == false));

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
            Data = Mapper.Map<ProductView>(product)
        };
    }

    public async Task<ServiceResult<ExpandoObject>> CreateAsync(CreateProductRequestServiceRequest request)
    {
        var validationResponse = ValidationService.Validate(typeof(CreateProductRequestValidator), request);

        if (!validationResponse.IsValid)
        {
            return new ServiceResult<ExpandoObject>
            {
                Status = ResultStatus.InvalidInput,
                Message = ServiceResponseMessage.INVALID_INPUT_ERROR,
                ValidationMessages = validationResponse.ErrorMessages
            };
        }

        var category = await _categoryRepository.FindOneWithAsNoTrackingAsync(p => p.Id == request.CategoryId && p.IsDeleted == false);
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

        entity = await _productRepository.InsertAsync(entity);

        dynamic productWrapper = new ExpandoObject();
        productWrapper.Id = entity.Id;

        return new ServiceResult<ExpandoObject>
        {
            Status = ResultStatus.Successful,
            Message = Resource.Created(Entities.Product, entity.Title),
            Data = productWrapper
        };
    }

    public async Task<ServiceResult<ExpandoObject>> UpdateAsync(UpdateProductRequestServiceRequest request)
    {
        var validationResponse = ValidationService.Validate(typeof(UpdateProductRequestValidator), request);

            if (!validationResponse.IsValid)
            {
                return new ServiceResult<ExpandoObject>
                {
                    Status = ResultStatus.InvalidInput,
                    Message = ServiceResponseMessage.INVALID_INPUT_ERROR,
                    ValidationMessages = validationResponse.ErrorMessages
                };
            }

            var entity = await _productRepository.FindOneAsync(p => p.Id == request.Id && p.IsDeleted == false);

            if (entity == null)
            {
                return new ServiceResult<ExpandoObject>
                {
                    Status = ResultStatus.ResourceNotFound,
                    Message = Resource.NotFound(Entities.Product)
                };
            }

            var category = await _categoryRepository.FindOneWithAsNoTrackingAsync(p => p.Id == request.CategoryId && p.IsDeleted == false);
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
            
            using (await _lockService.CreateLockAsync(lockKey))
            {
                entity.Title = request.Title.Trim();
                entity.Description = request.Description?.Trim();
                entity.StockQuantity = request.StockQuantity;
                entity.IsActive = request.StockQuantity > category.MinStockQuantity;
                entity.CategoryId = request.CategoryId;

                entity = await _productRepository.UpdateAsync(entity);

                await _cacheService.RemoveAsync(cacheKey);
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

    public async Task<ServiceResult<ExpandoObject>> DeleteAsync(Guid id)
    {
        var entity = await _productRepository.FindOneAsync(p => p.Id == id && p.IsDeleted == false);

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
            
        using (await _lockService.CreateLockAsync(lockKey))
        {
            await _productRepository.DeleteAsync(entity); 
                
            await _cacheService.RemoveAsync(cacheKey);   
        }

        return new ServiceResult<ExpandoObject>
        {
            Status = ResultStatus.Successful,
            Message = Resource.Deleted(Entities.Product, entity.Title)
        };
    }
    
    public async Task<ServiceResult<PagedList<ProductView>>> Search(FilteryRequest request)
    {
        var filteryResponse = await _productRepository
            .Find(p => p.IsActive == true)
            .Include(p => p.Category)
            .AsNoTracking()
            .BuildFilteryAsync(new ProductFilteryMapping(), request);

        var response = new PagedList<ProductView>
        {
            Data = Mapper.Map<List<ProductView>>(filteryResponse.Data),
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