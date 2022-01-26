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
    public class ObjectExtensionsTests
    {
        [TestMethod()]
        public void GetPropertyInfoTest_OK()
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
            var testData = entity.GetPropertyInfo(nameof(RequestLog.Request));

            //assert
            Assert.IsNotNull(testData);
            Assert.AreEqual(nameof(RequestLog.Request), testData.Name);
        }

        [TestMethod()]
        public void GetPropertyInfoTest_Instance_Null()
        {
            //arrange
            RequestLog entity = null;

            //act - assert
            Assert.ThrowsException<ArgumentNullException>(() => entity.GetPropertyInfo(nameof(RequestLog.Request)));
        }
        
        [TestMethod()]
        public void GetPropertyInfoTest_Property_Null()
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
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => entity.GetPropertyInfo("SomeProperty"));
        }

        [TestMethod()]
        public void SetPropertyValueTest()
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
            var newText = "RequestModify";
            entity.SetPropertyValue(nameof(RequestLog.Request), newText);
            
            //assert
            Assert.AreEqual(newText, entity.Request);
        }
    }
}