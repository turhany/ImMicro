using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Autofac;
using Filtery.Exceptions;
using Filtery.Models;
using Filtery.Models.Order;
using ImMicro.Business.Audit.Abstract;
using ImMicro.Common.BaseModels.Service;
using ImMicro.Common.Data.Abstract;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ImMicro.BusinessTests.Services.Concrete;

[TestClass]
public class AuditLogServiceTests : TestBase
{
    private readonly IAuditLogService _auditLogService;
    private readonly IGenericRepository<Model.AuditLog.AuditLog> _auditLogRepository;

    public AuditLogServiceTests()
    {
        _auditLogService = Container.Resolve<IAuditLogService>();
        _auditLogRepository = Container.Resolve<IGenericRepository<Model.AuditLog.AuditLog>>();
    }
    
    [TestMethod]
    public async Task GetAsyncTest_NO_DATA()
    {
        //arrange - act
        var response = await _auditLogService.GetAsync(Guid.NewGuid());

        //assert 
        Assert.AreEqual(ResultStatus.ResourceNotFound, response.Status);
    }
    
    [TestMethod]
    public async Task GetAsyncTest_OK()
    {
        //arrange
        var auditLog = new Model.AuditLog.AuditLog { Id = Guid.NewGuid(), EntityName = "TestEntity"};
        await _auditLogRepository.InsertAsync(auditLog);
            
        //act
        var response = await _auditLogService.GetAsync(auditLog.Id);

        //assert 
        Assert.AreEqual(ResultStatus.Successful, response.Status);
    }
    
    [TestMethod]
    public async Task SearchAsync_OK()
    {
        //arrange
        var auditLog = new Model.AuditLog.AuditLog { Id = Guid.NewGuid(), EntityName = "TestEntity"};
        await _auditLogRepository.InsertAsync(auditLog);
        
        //act
        var response = await _auditLogService.SearchAsync(new FilteryRequest()
        {
            PageNumber = 1,
            PageSize = 10
        });

        //assert 
        Assert.AreEqual(ResultStatus.Successful, response.Status);
    }
    
    [TestMethod]
    public async Task SearchAsync_NOK()
    {
        //INFO: I can not use "Assert.ThrowsExceptionAsync" because it's not catch base exception type
        
        //arrange - act - assert 
        try
        {
            await _auditLogService.SearchAsync(new FilteryRequest
            {
                OrderOperations = new Dictionary<string, OrderOperation>()
                {
                    {"errorkey", OrderOperation.Ascending}
                },
                PageNumber = 1,
                PageSize = 10
            });
        }
        catch (FilteryBaseException)
        {
            Assert.IsTrue(true);
        }
        catch(Exception)
        {
            Assert.IsTrue(false);
        }
    }
}