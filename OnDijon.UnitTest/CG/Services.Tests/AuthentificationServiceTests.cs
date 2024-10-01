using NUnit.Framework;
using OnDijon.CG.Services.Interfaces;
using OnDijon.UnitTest.Utils.Helpers;
using System.Threading.Tasks;

namespace OnDijon.UnitTest.CG.Services.Tests
{
    class AuthentificationServiceTests
    {
        private IAuthentificationService _authService;

        [SetUp]
        public void Setup()
        {
            //init IOC container
            var locator = new Locator();

            //get services
            _authService = locator.GetInstance<IAuthentificationService>();
        }

        [Test]
        public async Task GetAndRefreshTokenTest()
        {
            //test get token
            var token1 = await _authService.GetToken();
            AssertHelper.IsNotNullAndNotEmpty(token1);

            //test refresh token
            var token2 = await _authService.RefreshToken();
            AssertHelper.IsNotNullAndNotEmpty(token2);
        }
    }
}
