using NUnit.Framework;
using OnDijon.CG.Entities;
using OnDijon.CG.Entities.Dto;
using OnDijon.Common.Utils.Validations;
using System;
using System.Linq;

namespace OnDijon.UnitTest.CG.Entities.Tests
{
    class ValidatableProfileTests
    {
        [Test]
        public void DtoToPropertiesTest()
        {
            var dto = new ProfileDto
            {
                Name = "Dupont",
                FirstName = "Jean",
                Mail = "jean.dupont@email.com",
                Birthday = DateTime.Today,
                PhoneNumber = "0123456789",
                Gender = "Monsieur"
            };

            var validatable = new ValidatableProfile { Dto = dto };

            AreDtoAndPropertiesEqual(dto, validatable);
        }

        [Test]
        public void PropertiesToDtoTest()
        {
            var validatable = new ValidatableProfile();
            validatable.Name.Value = "Dupont";
            validatable.FirstName.Value = "Jean";
            validatable.Mail.Value = "jean.dupont@email.com";
            validatable.Birthday.Value = DateTime.Today;
            validatable.PhoneNumber.Value = "0123456789";
            validatable.Gender.Value = "Monsieur";

            AreDtoAndPropertiesEqual(validatable.Dto, validatable);
        }

        private static void AreDtoAndPropertiesEqual(ProfileDto dto, ValidatableProfile validatable)
        {
            Assert.AreEqual(dto.Name, validatable.Name.Value);
            Assert.AreEqual(dto.FirstName, validatable.FirstName.Value);
            Assert.AreEqual(dto.Mail, validatable.Mail.Value);
            Assert.AreEqual(dto.Birthday, validatable.Birthday.Value);
            Assert.AreEqual(dto.PhoneNumber, validatable.PhoneNumber.Value);
            Assert.AreEqual(dto.Gender, validatable.Gender.Value);
        }

        [TestCase("Dupont", "Jean", "jean.dupont@email.com", "0123456789", "Monsieur", ExpectedResult = true)]
        [TestCase("Dupont", "Jean", "jean.dupont@email.com", null, "Monsieur", ExpectedResult = true)]
        [TestCase(null, "Jean", "jean.dupont@email.com", "0123456789", "Monsieur", ExpectedResult = false)]
        [TestCase("Dupont2", "Jean", "jean.dupont@email.com", "0123456789", "Monsieur", ExpectedResult = false)]
        [TestCase("Dupont", null, "jean.dupont@email.com", "0123456789", "Monsieur", ExpectedResult = false)]
        [TestCase("Dupont", "Jean2", "jean.dupont@email.com", "0123456789", "Monsieur", ExpectedResult = false)]
        [TestCase("Dupont", "Jean", null, "0123456789", "Monsieur", ExpectedResult = false)]
        [TestCase("Dupont", "Jean", "invalid", "0123456789", "Monsieur", ExpectedResult = false)]
        [TestCase("Dupont", "Jean", "jean.dupont@email.com", "invalid", "Monsieur", ExpectedResult = false)]
        [TestCase("Dupont", "Jean", "jean.dupont@email.com", "0123456789", null, ExpectedResult = false)]
        public bool ProfileValidationsTest(string name, string firstName, string mail, string phoneNumber, string gender)
        {
            var validatable = new ValidatableProfile();
            validatable.Name.Value = name;
            validatable.FirstName.Value = firstName;
            validatable.Mail.Value = mail;
            validatable.PhoneNumber.Value = phoneNumber;
            validatable.Gender.Value = gender;

            var properties = new ValidatableObject<string>[] {
                validatable.Name, validatable.FirstName, validatable.Mail, validatable.PhoneNumber, validatable.Gender
            };

            validatable.Validate();
            var isValid = validatable.IsValid;

            TestErrors(properties, isValid);

            return isValid;
        }

        [TestCase("Password1", "Password1", ExpectedResult = true)]
        [TestCase("Password1", "Password2", ExpectedResult = false)]
        [TestCase("password1", "password1", ExpectedResult = false)]
        [TestCase("", "", ExpectedResult = false)]
        [TestCase(null, null, ExpectedResult = false)]
        public bool PasswordValidationsTest(string password, string passwordConfirmation)
        {
            var validatable = new ValidatableProfile();
            validatable.Password.Value = password;
            validatable.PasswordConfirmation.Value = passwordConfirmation;

            var properties = new ValidatableObject<string>[] {
                validatable.Password, validatable.PasswordConfirmation
            };

            var isValid = validatable.ValidatePassword();

            TestErrors(properties, isValid);

            return isValid;
        }

        private static void TestErrors(ValidatableObject<string>[] properties, bool isValid)
        {
            if (isValid)
            {
                Assert.IsTrue(properties.All(p => p.IsValid));
                Assert.IsTrue(properties.All(p => p.Errors.Count == 0));
            }
            else
            {
                Assert.IsTrue(properties.Any(p => !p.IsValid));
                Assert.IsTrue(properties.Any(p => p.Errors.Count > 0));
            }

            foreach (var p in properties)
            {
                Assert.AreEqual(p.IsValid, p.Errors.Count == 0);
            }
        }
    }
}
