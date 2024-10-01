using NUnit.Framework;
using OnDijon.CG.Services;
using OnDijon.UnitTest.CG.Services.Mocks;
using System;
using System.Linq;
using System.Threading;
using Xamarin.Forms;

namespace OnDijon.UnitTest.CG.Services.Tests
{
    class NavigationServiceTests
    {
        private const string Page1Key = "Page1";
        private const string Page2Key = "Page2";
        private const string TestParam = "TEST";

        private INavigation _navigation;
        private NavigationService _navigationService;

        [SetUp]
        public void Setup()
        {
            _navigation = new NavigationMockProxy();
            _navigationService = new NavigationService();
        }

        [Test]
        public void NavigationTest()
        {
            _navigationService.Initialize(_navigation);
            _navigationService.Configure(Page1Key, typeof(MockPage));
            _navigationService.Configure(Page2Key, typeof(MockPageWithParam<string>));

            //navigate to page 1
            _navigationService.NavigateTo(Page1Key);
            Thread.Sleep(100);
            Assert.AreEqual(Page1Key, _navigationService.CurrentPageKey);

            //navigate to page 2 with parameter
            _navigationService.NavigateTo(Page2Key, TestParam);
            Thread.Sleep(100);
            Assert.AreEqual(Page2Key, _navigationService.CurrentPageKey);
            var page2 = _navigation.NavigationStack.LastOrDefault() as MockPageWithParam<string>;
            Assert.AreEqual(TestParam, page2.Parameter);

            //go back to page 1
            _navigationService.GoBack();
            Thread.Sleep(100);
            Assert.AreEqual(Page1Key, _navigationService.CurrentPageKey);
        }

        [Test]
        public void ExceptionsTest()
        {
            _navigationService.Initialize(_navigation);

            //page key not configured
            Assert.Throws<ArgumentException>(() =>
            {
                _navigationService.NavigateTo(Page1Key);
            });

            _navigationService.Configure(Page1Key, typeof(MockPage));
            _navigationService.Configure(Page2Key, typeof(MockPageWithParam<string>));

            //navigate to parameterless page with a parameter
            Assert.Throws<InvalidOperationException>(() =>
            {
                _navigationService.NavigateTo(Page1Key, TestParam);
            });

            //navigate to parameter page without a parameter
            Assert.Throws<InvalidOperationException>(() =>
            {
                _navigationService.NavigateTo(Page2Key);
            });
        }
    }
}
