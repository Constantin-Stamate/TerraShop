using eUseControl.BusinessLogic;
using eUseControl.BusinessLogic.Interfaces;
using eUseControl.Domain.Entities.Profile;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace eUseControl.Tests.ServicesTests
{
    [TestClass]
    public class ProfileTests
    {
        private readonly IProfile _profile;

        public ProfileTests()
        {
            var bl = new BusinessLogicManager();
            _profile = bl.GetProfileBL();
        }

        [TestMethod]
        public void InvalidUser()
        {
            int userId = -1;

            var profile = _profile.GetProfileByUserId(userId);

            Assert.IsNull(profile);
        }

        [TestMethod]
        public void DefaultProfile()
        {
            int userId = 4;

            var profile = _profile.GetProfileByUserId(userId);

            Assert.IsNotNull(profile);
            Assert.AreEqual("User", profile.FirstName);
            Assert.AreEqual("User", profile.LastName);
            Assert.AreEqual("000-000-0000", profile.PhoneNumber);
            Assert.AreEqual("/Assets/img/user.jpg", profile.ProfileImageUrl);
        }

        [TestMethod]
        public void ExistingProfile()
        {
            int userId = 1;

            var profile = _profile.GetProfileByUserId(userId);

            Assert.IsNotNull(profile);
            Assert.AreEqual(1, profile.UserId);
            Assert.IsFalse(string.IsNullOrWhiteSpace(profile.FirstName));
            Assert.IsFalse(string.IsNullOrWhiteSpace(profile.LastName));
        }

        [TestMethod]
        public void InvalidFirstName()
        {
            var data = new ProfileData
            {
                UserId = 1,
                FirstName = "Ana",
                LastName = "Popescu",
                Email = "ana@example.com",
                PhoneNumber = "+37360000000",
                Address = "Street 1"
            };

            var result = _profile.UpdateProfile(data);

            Assert.IsFalse(result.Status);
            Assert.AreEqual("First name must be at least 5 characters!", result.StatusMsg);
        }

        [TestMethod]
        public void InvalidLastName()
        {
            var data = new ProfileData
            {
                UserId = 1,
                FirstName = "Elena",
                LastName = "Pop",
                Email = "elena@example.com",
                PhoneNumber = "+37360000000",
                Address = "Street 1"
            };

            var result = _profile.UpdateProfile(data);

            Assert.IsFalse(result.Status);
            Assert.AreEqual("Last name must be at least 5 characters!", result.StatusMsg);
        }

        [TestMethod]
        public void InvalidEmail()
        {
            var data = new ProfileData
            {
                UserId = 1,
                FirstName = "Marian",
                LastName = "Ionel",
                Email = "invalidemail",
                PhoneNumber = "+37360000000",
                Address = "Street 1"
            };

            var result = _profile.UpdateProfile(data);

            Assert.IsFalse(result.Status);
            Assert.AreEqual("Please enter a valid email address!", result.StatusMsg);
        }

        [TestMethod]
        public void InvalidPhone()
        {
            var data = new ProfileData
            {
                UserId = 1,
                FirstName = "Vasile",
                LastName = "Ionescu",
                Email = "vasile@example.com",
                PhoneNumber = "123",
                Address = "Street 1"
            };

            var result = _profile.UpdateProfile(data);

            Assert.IsFalse(result.Status);
            Assert.AreEqual("Please enter a valid phone number!", result.StatusMsg);
        }

        [TestMethod]
        public void InvalidAddress()
        {
            var data = new ProfileData
            {
                UserId = 1,
                FirstName = "Maria",
                LastName = "Popescu",
                Email = "maria@example.com",
                PhoneNumber = "+37360000000",
                Address = "Str"
            };

            var result = _profile.UpdateProfile(data);

            Assert.IsFalse(result.Status);
            Assert.AreEqual("Address must be at least 5 characters!", result.StatusMsg);
        }

        [TestMethod]
        public void DuplicateEmail()
        {
            var data = new ProfileData
            {
                UserId = 1,
                FirstName = "Marinela",
                LastName = "Popescu",
                Email = "maria@email.com",
                PhoneNumber = "060686171",
                Address = "Main Street 1"
            };

            var result = _profile.UpdateProfile(data);

            Assert.IsFalse(result.Status);
            Assert.AreEqual("This email is already in use by another user!", result.StatusMsg);
        }

        [TestMethod]
        public void ProfileNotFound()
        {
            var data = new ProfileData
            {
                UserId = -1,
                FirstName = "Ioana",
                LastName = "Marin",
                Email = "ioana@example.com",
                PhoneNumber = "+37362222222",
                Address = "Main Street 1"
            };

            var result = _profile.UpdateProfile(data);

            Assert.IsFalse(result.Status);
            Assert.AreEqual("We couldn't find your profile!", result.StatusMsg);
        }

        [TestMethod]
        public void SuccessUpdate()
        {
            var data = new ProfileData
            {
                UserId = 4,
                FirstName = "Andrei",
                LastName = "Popescu",
                Email = "andrei@example.com",
                PhoneNumber = "+37369999999",
                Address = "Strada Noua",
                ProfileImageUrl = "/Assets/img/andrei.jpg"
            };

            var result = _profile.UpdateProfile(data);

            Assert.IsTrue(result.Status);
            Assert.AreEqual("Your profile has been updated!", result.StatusMsg);
        }

        [TestMethod]
        public void EmptyPasswordFields()
        {
            string currentPassword = "";
            string newPassword = "";
            int userId = 1;

            var result = _profile.ChangePassword(currentPassword, newPassword, userId);

            Assert.IsFalse(result.Status);
            Assert.AreEqual("Passwords cannot be empty!", result.StatusMsg);
        }

        [TestMethod]
        public void TooShortNewPassword()
        {
            string currentPassword = "CurrentPass1!";
            string newPassword = "Short1!";
            int userId = 1;

            var result = _profile.ChangePassword(currentPassword, newPassword, userId);

            Assert.IsFalse(result.Status);
            Assert.AreEqual("New password must be at least 8 characters long!", result.StatusMsg);
        }

        [TestMethod]
        public void SamePasswords()
        {
            string currentPassword = "SamePass1!";
            string newPassword = "SamePass1!";
            int userId = 1;

            var result = _profile.ChangePassword(currentPassword, newPassword, userId);

            Assert.IsFalse(result.Status);
            Assert.AreEqual("New password must be different from the current one!", result.StatusMsg);
        }

        [TestMethod]
        public void WeakNewPassword()
        {
            string currentPassword = "StrongPass1!";
            string newPassword = "weakpass";
            int userId = 1;

            var result = _profile.ChangePassword(currentPassword, newPassword, userId);

            Assert.IsFalse(result.Status);
            Assert.AreEqual("Password must meet complexity requirements!", result.StatusMsg);
        }

        [TestMethod]
        public void IncorrectCurrentPassword()
        {
            string currentPassword = "StrongPass123!";
            string newPassword = "NewStrong1!";
            int userId = 1;

            var result = _profile.ChangePassword(currentPassword, newPassword, userId);

            Assert.IsFalse(result.Status);
            Assert.AreEqual("Incorrect current password!", result.StatusMsg);
        }

        [TestMethod]
        public void SuccessPasswordChange()
        {
            string currentPassword = "StrongPass123!";
            string newPassword = "NewStrong1!";
            int userId = 4;

            var result = _profile.ChangePassword(currentPassword, newPassword, userId);

            Assert.IsTrue(result.Status);
            Assert.AreEqual("Password changed successfully!", result.StatusMsg);
        }
    }
}
