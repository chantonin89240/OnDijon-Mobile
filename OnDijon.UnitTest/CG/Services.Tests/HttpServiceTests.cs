using NUnit.Framework;
using OnDijon.CG.Entities.Dto.Reporting;
using OnDijon.CG.Enums;
using OnDijon.CG.Exceptions;
using OnDijon.CG.Utils.Extensions;
using OnDijon.Common.Utils.Services.Interfaces;
using System;
using System.Collections;
using System.Net;
using System.Threading.Tasks;

namespace OnDijon.UnitTest.CG.Services.Tests
{
    class HttpServiceTests
    {
        public static class TestData
        {
            public static HttpStatusCode[] StatusCodes =
            {
                HttpStatusCode.OK,
                HttpStatusCode.BadRequest,
                HttpStatusCode.Unauthorized,
                HttpStatusCode.Forbidden,
                HttpStatusCode.NotFound,
                HttpStatusCode.InternalServerError,
                HttpStatusCode.ServiceUnavailable
            };

            public static IEnumerable CallStatusTestCases(string successUrl)
            {
                yield return new TestCaseData(successUrl).Returns(CallStatusEnum.Success);
                yield return new TestCaseData(CreateTestUrl(HttpStatusCode.BadRequest)).Returns(CallStatusEnum.InvalidInformations);
                yield return new TestCaseData(CreateTestUrl(HttpStatusCode.Unauthorized)).Returns(CallStatusEnum.InvalidCredentials);
                yield return new TestCaseData(CreateTestUrl(HttpStatusCode.Forbidden)).Returns(CallStatusEnum.InvalidCredentials);
                yield return new TestCaseData(CreateTestUrl(HttpStatusCode.NotFound)).Returns(CallStatusEnum.NotFound);
                yield return new TestCaseData(CreateTestUrl(HttpStatusCode.InternalServerError)).Returns(CallStatusEnum.InternalServerError);
                yield return new TestCaseData(CreateTestUrl(HttpStatusCode.ServiceUnavailable)).Returns(CallStatusEnum.ServiceUnavailable);
            }
        }

        private IHttpService _httpService;

        [SetUp]
        public void Setup()
        {
            //init IOC container
            var locator = new Locator();

            //get services
            _httpService = locator.GetInstance<IHttpService>();
        }

        [TestCaseSource(typeof(TestData), "StatusCodes")]
        public async Task GetAsyncTest(HttpStatusCode expectedStatusCode)
        {
            var testUrl = CreateTestUrl(expectedStatusCode);

            try
            {
                var httpResponse = await _httpService.GetAsync(testUrl);
                Assert.AreEqual(expectedStatusCode, httpResponse.StatusCode);
            }
            catch (HttpStatusCodeException ex)
            {
                Console.WriteLine(ex.ToString());
                Assert.AreEqual(expectedStatusCode, ex.StatusCode);
            }
        }

        [TestCaseSource(typeof(TestData), "StatusCodes")]
        public async Task GetStringAsyncTest(HttpStatusCode expectedStatusCode)
        {
            var testUrl = "https://httpstat.us/";
            if (expectedStatusCode != HttpStatusCode.OK)
            {
                testUrl = CreateTestUrl(expectedStatusCode);
            }

            try
            {
                var stringResponse = await _httpService.GetStringAsync(testUrl);
                Assert.IsNotNull(stringResponse);
                Assert.IsNotEmpty(stringResponse);
            }
            catch (HttpStatusCodeException ex)
            {
                Console.WriteLine(ex.ToString());
                Assert.AreEqual(expectedStatusCode, ex.StatusCode);
            }
        }

        [TestCaseSource(typeof(TestData), "CallStatusTestCases", new object[] { "https://hyperviseurpp.ondijon.fr/mobile-app/1.0.0/types" })]
        public async Task<CallStatusEnum> GetDtoListAsyncTest(string testUrl)
        {
            try
            {
                var dtoResponse = await _httpService.GetDtoListAsync<ReportTypeDto>(testUrl, true);
                if (dtoResponse.State == CallStatusEnum.Success)
                {
                    Assert.IsNotNull(dtoResponse.Data);
                }
                return dtoResponse.State;
            }
            catch (HttpStatusCodeException ex)
            {
                Console.WriteLine(ex.ToString());
                return ex.StatusCode.ToCallStatus();
            }
        }

        private static string CreateTestUrl(HttpStatusCode expectedStatusCode)
        {
            return "https://httpstat.us/" + (int)expectedStatusCode;
        }
    }
}
