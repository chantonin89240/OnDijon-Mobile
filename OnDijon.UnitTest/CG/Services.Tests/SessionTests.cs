using NUnit.Framework;
using OnDijon.CG.Entities.Dto;
using OnDijon.CG.Services;
using OnDijon.CG.Services.Interfaces;
using OnDijon.Common.Utils.Services.Interfaces;
using OnDijon.UnitTest.Common.Services.Mocks;
using System;
using System.Threading.Tasks;

namespace OnDijon.UnitTest.CG.Services.Tests
{
    class SessionTests
    {
        private ICacheService _cacheService;
        private ISession _session;

        [SetUp]
        public void Setup()
        {
            _cacheService = new CacheMockService();
            _session = new Session(_cacheService);
        }

        [Test]
        public async Task SaveProfileTest()
        {
            Assert.IsFalse(_session.IsConnected());

            var name = "Dupont";
            var firstName = "Jean";
            var mail = "jean.dupont@email.com";
            var birthday = DateTime.Today;
            var phoneNumber = "0123456789";
            var gender = "Monsieur";

            _session.Profile = new ProfileDto { Name = name, FirstName = firstName, Mail = mail, Birthday = birthday, PhoneNumber = phoneNumber, Gender = gender };

            Assert.IsTrue(_session.IsConnected());

            //test cached profile properties
            var cachedProfile = await _cacheService.Get<ProfileDto>("Profile");
            Assert.Multiple(() =>
            {
                Assert.AreEqual(name, cachedProfile.Name);
                Assert.AreEqual(firstName, cachedProfile.FirstName);
                Assert.AreEqual(mail, cachedProfile.Mail);
                Assert.AreEqual(birthday, cachedProfile.Birthday);
                Assert.AreEqual(phoneNumber, cachedProfile.PhoneNumber);
                Assert.AreEqual(gender, cachedProfile.Gender);
            });

            //test delete profile
            _session.Profile = null;
            var nullProfile = await _cacheService.Get<ProfileDto>("Profile");
            Assert.IsNull(nullProfile);
        }
    }
}
