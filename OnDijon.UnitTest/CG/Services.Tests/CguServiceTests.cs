using HtmlAgilityPack;
using NUnit.Framework;
using OnDijon.CG.Enums;
using OnDijon.CG.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace OnDijon.UnitTest.CG.Services.Tests
{
    class CguServiceTests
    {
        private ICguService _cguService;

        [SetUp]
        public void Setup()
        {
            //init IOC container
            var locator = new Locator();

            //get services
            _cguService = locator.GetInstance<ICguService>();
        }

        [Test]
        public async Task GetCguTestAsync()
        {
            var cguResponse = await _cguService.GetCguAsync();

            Assert.AreEqual(CallStatusEnum.Success, cguResponse.State);
            Assert.IsNotNull(cguResponse.Data.Html);
            Assert.IsNotEmpty(cguResponse.Data.Html);

            //check html validity
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(cguResponse.Data.Html);
            string body = doc.DocumentNode.SelectSingleNode("//body").InnerHtml;
            Console.WriteLine(body);
            Assert.IsNotNull(body);
            Assert.IsNotEmpty(body);
        }
    }
}
