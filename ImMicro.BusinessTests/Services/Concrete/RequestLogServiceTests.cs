using Autofac;
using ImMicro.Business.RequestLog.Abstract;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ImMicro.BusinessTests;
using System.Threading;

// ReSharper disable CheckNamespace

namespace ImMicro.Business.Services.Concrete.Tests
{
    [TestClass]
    public class RequestLogServiceTests : TestBase
    {
        private readonly IRequestLogService _requestLogService;

        public RequestLogServiceTests()
        {
            _requestLogService = Container.Resolve<IRequestLogService>();
        }

        [TestMethod]
        public void SaveTest()
        {
            //arrange
            var entity = new Model.RequestLog.RequestLog
            {
                Request = "Request",
                Response = "Response",
                StatusCode = "200",
                RequestPath = "www.unittest.com"
            };

            //act
            var response = _requestLogService.SaveAsync(entity, CancellationToken.None).Result;

            //assert
            Assert.IsTrue(response);
        }
    }
}