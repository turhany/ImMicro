using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
// ReSharper disable CheckNamespace

namespace ImMicro.Common.Helpers.Tests
{
    [TestClass]
    public class DynamicRouteParserTests
    {
        [TestMethod()]
        public void ParseRouteTest_BaseRoute_OK()
        {
            //arrange
            var routeParser = new DynamicRouteParser("https://www.ImMicro.com/casio/saat-p-{productId}");

            //act
            var response = routeParser.ParseRoute("https://www.ImMicro.com/casio/saat-p-1925865?boutiqueId=439892&merchantId=105064");

            //assert
            Assert.IsTrue(response.Any(p => p.Key.Equals("productId", StringComparison.InvariantCultureIgnoreCase) && p.Type == RouteParameterType.Route));
            Assert.AreEqual("1925865",response.First(p => p.Key.Equals("productId", StringComparison.InvariantCultureIgnoreCase) && p.Type == RouteParameterType.Route).Value);
        }
        
        [TestMethod()]
        public void ParseRouteTest_QueryString_OK()
        {
            //arrange
            var routeParser = new DynamicRouteParser("https://www.ImMicro.com/casio/saat-p-{productId}");

            //act
            var response = routeParser.ParseRoute("https://www.ImMicro.com/casio/saat-p-1925865?boutiqueId=439892&merchantId=105064");

            //assert
            Assert.IsTrue(response.Any(p => p.Key.Equals("boutiqueId", StringComparison.InvariantCultureIgnoreCase) && p.Type == RouteParameterType.QueryString));
            Assert.AreEqual("439892",response.First(p => p.Key.Equals("boutiqueId", StringComparison.InvariantCultureIgnoreCase) && p.Type == RouteParameterType.QueryString).Value);
        }
        
        [TestMethod()]
        public void ParseRouteTest_BaseRoute_NOK()
        {
            //arrange
            var routeParser = new DynamicRouteParser("https://www.ImMicro.com/casio/saat-p-{pId}");

            //act
            var response = routeParser.ParseRoute("https://www.ImMicro.com/casio/saat-p-1925865?boutiqueId=439892&merchantId=105064");

            //assert
            Assert.IsFalse(response.Any(p => p.Key.Equals("productId", StringComparison.InvariantCultureIgnoreCase) && p.Type == RouteParameterType.Route));
        }
        
        [TestMethod()]
        public void ParseRouteTest_QueryString_NOK()
        {
            //arrange
            var routeParser = new DynamicRouteParser("https://www.ImMicro.com/casio/saat-p-{productId}");

            //act
            var response = routeParser.ParseRoute("https://www.ImMicro.com/casio/saat-p-1925865?bId=439892&merchantId=105064");

            //assert
            Assert.IsFalse(response.Any(p => p.Key.Equals("boutiqueId", StringComparison.InvariantCultureIgnoreCase) && p.Type == RouteParameterType.QueryString)); 
        }
        
        [TestMethod()]
        public void ParseRouteTest_BaseRouteTemplate_NULL()
        {
            //arrange - act - assert
            Assert.ThrowsException<ArgumentNullException>(() => new DynamicRouteParser(null));
        }
        
        [TestMethod()]
        public void ParseRouteTest_QueryString_HasMultiple_QuestionMark()
        {
            //arrange
            var routeParser = new DynamicRouteParser("https://www.ImMicro.com/casio/saat-p-{productId}");

            //act - assert
            Assert.ThrowsException<ArgumentException>(() => routeParser.ParseRoute("https://www.ImMicro.com/casio/saat-p-1925865?bId=439892?merchantId=105064"));
        }
    }
}