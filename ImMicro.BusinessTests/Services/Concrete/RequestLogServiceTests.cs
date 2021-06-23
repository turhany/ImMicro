using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ImMicro.Business.Services.Abstract;
using ImMicro.BusinessTests;
using ImMicro.Model;

// ReSharper disable CheckNamespace

namespace ImMicro.Business.Services.Concrete.Tests
{
    [TestClass()]
    public class RequestLogServiceTests : TestBase
    {
        private readonly IRequestLogService _requestLogService;

        public RequestLogServiceTests()
        {
            _requestLogService = Container.Resolve<IRequestLogService>();
        }

        [TestMethod()]
        public void SaveTest()
        {
            //arrange
            var entity = new RequestLog
            {
                Request = "Request",
                Response = "Response",
                StatusCode = "200",
                RequestPath = "www.unittest.com"
            };
            
            //act
            var response = _requestLogService.SaveAsync(entity).Result;

            //assert
            Assert.IsTrue(response);
        }
    }
}