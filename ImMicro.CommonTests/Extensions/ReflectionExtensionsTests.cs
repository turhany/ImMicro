using Microsoft.VisualStudio.TestTools.UnitTesting;
using ImMicro.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HelpersToolbox.Extensions;
using ImMicro.Model;
using ImMicro.Model.RequestLog;

namespace ImMicro.Common.Extensions.Tests
{
    [TestClass()]
    public class ReflectionExtensionsTests
    {
        [TestMethod()]
        public void HasPropertyTest_OK()
        {
            //arrange 
            var entity = new RequestLog
            {
                Request = "Request",
                Response = "Response",
                StatusCode = "200",
                RequestPath = "www.unittest.com"
            };
            
            //act - assert
            Assert.IsTrue(entity.HasProperty(nameof(RequestLog.Request)));
        }
        
        [TestMethod()]
        public void HasPropertyTest_NOK()
        {
            //arrange 
            var entity = new RequestLog
            {
                Request = "Request",
                Response = "Response",
                StatusCode = "200",
                RequestPath = "www.unittest.com"
            };
            
            //act - assert
            Assert.IsFalse(entity.HasProperty("SameProperty"));
        }
    }
}