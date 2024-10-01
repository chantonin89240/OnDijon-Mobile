using NUnit.Framework;
using OnDijon.CG.Entities.Dto;
using OnDijon.CG.Entities.Request;
using OnDijon.CG.Services.Interfaces;
using OnDijon.UnitTest.Utils.Helpers;
using System;
using System.Threading.Tasks;

namespace OnDijon.UnitTest.CG.Services.Tests
{
    class AccountServiceTests
    {
        private const string testMail = "adrien.liotard@gmail.com";
        private const string testPassword = "Password1";
        private const string testPassword2 = "Password2";

        private IAccountService _accountService;
        private ISession _session;

        [SetUp]
        public void Setup()
        {
            //init IOC container
            var locator = new Locator();

            //get services
            _accountService = locator.GetInstance<IAccountService>();
            _session = locator.GetInstance<ISession>();
        }

        [Test]
        public async Task AuthenticateTest()
        {
            var authenticationRequest = new AuthenticationRequest
            {
                Mail = testMail,
                Password = testPassword
            };
            var response = await _accountService.Authenticate(authenticationRequest);

            Assert.IsTrue(response.IsSuccessful());
            AssertHelper.IsNotNullAndNotEmpty(_session.Profile.Guid);
        }

        [Test]
        public async Task GetProfileTest()
        {
            var response = await _accountService.Get();

            Assert.IsTrue(response.IsSuccessful());
            AssertHelper.IsNotNullAndNotEmpty(_session.Profile.Guid);
            AssertHelper.IsNotNullAndNotEmpty(_session.Profile.FirstName);
            AssertHelper.IsNotNullAndNotEmpty(_session.Profile.Name);
            AssertHelper.IsNotNullAndNotEmpty(_session.Profile.Mail);
        }

        [TestCase(testPassword, testPassword2)]
        [TestCase(testPassword2, testPassword)]
        public async Task ChangePasswordTest(string oldPassword, string newPassword)
        {
            var changePasswordRequest = new ChangePasswordRequest
            {
                OldPassword = oldPassword,
                NewPassword = newPassword
            };
            var response = await _accountService.ChangePassword(changePasswordRequest);

            Assert.IsTrue(response.IsSuccessful());
        }

        [Test]
        public async Task UpdateProfileTest()
        {
            var name = StringHelper.RandomString();
            var firstName = StringHelper.RandomString();

            var updateRequest = new UpdateRequest()
            {
                Profile = new ProfileDto
                {
                    Guid = _session.Profile.Guid,
                    Name = name,
                    FirstName = firstName,
                    Mail = _session.Profile.Mail,
                    Gender = _session.Profile.Gender,
                    Birthday = DateTime.Today
                }
            };
            Console.WriteLine(updateRequest.ToString());

            var updateResponse = await _accountService.Update(updateRequest);
            Console.WriteLine(updateResponse.ToString());

            Assert.IsTrue(updateResponse.IsSuccessful());

            //get profile to check values
            var getResponse = await _accountService.Get();
            Console.WriteLine(getResponse.ToString());

            Assert.IsTrue(getResponse.IsSuccessful());
            Assert.AreEqual(name, _session.Profile.Name);
            Assert.AreEqual(firstName, _session.Profile.FirstName);
        }
    }
}
