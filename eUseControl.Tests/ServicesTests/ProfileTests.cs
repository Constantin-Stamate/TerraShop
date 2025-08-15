using System.Threading.Tasks;
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
        public async Task InvalidUser()
        {
            int userId = -1;

            var profile = await _profile.GetProfileByUserId(userId);

            Assert.IsNull(profile, "Expected null profile for invalid user ID!");
        }

        [TestMethod]
        public async Task DefaultProfile()
        {
            int userId = 4;

            var profile = await _profile.GetProfileByUserId(userId);

            Assert.IsNotNull(profile, "Expected a profile object for user ID 4!");
            Assert.AreEqual("User", profile.FirstName, "Expected default first name 'User'!");
            Assert.AreEqual("User", profile.LastName, "Expected default last name 'User'!");
            Assert.AreEqual("000-000-0000", profile.PhoneNumber, "Expected default phone number!");
            Assert.AreEqual("/Assets/img/user.jpg", profile.ProfileImageUrl, "Expected default profile image URL!");
        }

        [TestMethod]
        public async Task ExistingProfile()
        {
            int userId = 1;

            var profile = await _profile.GetProfileByUserId(userId);

            Assert.IsNotNull(profile, "Expected a profile object for existing user ID!");
            Assert.AreEqual(userId, profile.UserId, "UserId should match!");
            Assert.IsFalse(string.IsNullOrWhiteSpace(profile.FirstName), "First name should not be empty or whitespace!");
            Assert.IsFalse(string.IsNullOrWhiteSpace(profile.LastName), "Last name should not be empty or whitespace!");
        }

        [TestMethod]
        public async Task InvalidFirstName()
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

            var result = await _profile.UpdateProfile(data);

            Assert.IsFalse(result.Status, "Expected failure when first name is shorter than 5 characters!");
            Assert.AreEqual("First name must be at least 5 characters!", result.StatusMsg);
        }

        [TestMethod]
        public async Task InvalidLastName()
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

            var result = await _profile.UpdateProfile(data);

            Assert.IsFalse(result.Status, "Expected failure when last name is shorter than 5 characters!");
            Assert.AreEqual("Last name must be at least 5 characters!", result.StatusMsg);
        }

        [TestMethod]
        public async Task InvalidEmail()
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

            var result = await _profile.UpdateProfile(data);

            Assert.IsFalse(result.Status, "Expected failure with invalid email format!");
            Assert.AreEqual("Please enter a valid email address!", result.StatusMsg);
        }

        [TestMethod]
        public async Task InvalidPhone()
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

            var result = await _profile.UpdateProfile(data);

            Assert.IsFalse(result.Status, "Expected failure with invalid phone number!");
            Assert.AreEqual("Please enter a valid phone number!", result.StatusMsg);
        }

        [TestMethod]
        public async Task InvalidAddress()
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

            var result = await _profile.UpdateProfile(data);

            Assert.IsFalse(result.Status, "Expected failure when address is shorter than 5 characters!");
            Assert.AreEqual("Address must be at least 5 characters!", result.StatusMsg);
        }

        [TestMethod]
        public async Task DuplicateEmail()
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

            var result = await _profile.UpdateProfile(data);

            Assert.IsFalse(result.Status, "Expected failure when email is already used by another user!");
            Assert.AreEqual("This email is already in use by another user!", result.StatusMsg);
        }

        [TestMethod]
        public async Task ProfileNotFound()
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

            var result = await _profile.UpdateProfile(data);

            Assert.IsFalse(result.Status, "Expected failure when profile not found!");
            Assert.AreEqual("We couldn't find your profile!", result.StatusMsg);
        }

        [TestMethod]
        public async Task SuccessUpdate()
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

            var result = await _profile.UpdateProfile(data);

            Assert.IsTrue(result.Status, "Expected success on valid profile update!");
            Assert.AreEqual("Your profile has been updated!", result.StatusMsg);
        }

        [TestMethod]
        public async Task EmptyPasswordFields()
        {
            string currentPassword = "";
            string newPassword = "";
            int userId = 1;

            var result = await _profile.ChangePassword(currentPassword, newPassword, userId);

            Assert.IsFalse(result.Status, "Expected failure when passwords are empty!");
            Assert.AreEqual("Passwords cannot be empty!", result.StatusMsg);
        }

        [TestMethod]
        public async Task TooShortNewPassword()
        {
            string currentPassword = "CurrentPass1!";
            string newPassword = "Short1!";
            int userId = 1;

            var result = await _profile.ChangePassword(currentPassword, newPassword, userId);

            Assert.IsFalse(result.Status, "Expected failure when new password is too short!");
            Assert.AreEqual("New password must be at least 8 characters long!", result.StatusMsg);
        }

        [TestMethod]
        public async Task SamePasswords()
        {
            string currentPassword = "SamePass1!";
            string newPassword = "SamePass1!";
            int userId = 1;

            var result = await _profile.ChangePassword(currentPassword, newPassword, userId);

            Assert.IsFalse(result.Status, "Expected failure when new password matches current password!");
            Assert.AreEqual("New password must be different from the current one!", result.StatusMsg);
        }

        [TestMethod]
        public async Task WeakNewPassword()
        {
            string currentPassword = "StrongPass1!";
            string newPassword = "weakpass";
            int userId = 1;

            var result = await _profile.ChangePassword(currentPassword, newPassword, userId);

            Assert.IsFalse(result.Status, "Expected failure when new password does not meet complexity requirements!");
            Assert.AreEqual("Password must meet complexity requirements!", result.StatusMsg);
        }

        [TestMethod]
        public async Task IncorrectCurrentPassword()
        {
            string currentPassword = "StrongPass123!";
            string newPassword = "NewStrong1!";
            int userId = 1;

            var result = await _profile.ChangePassword(currentPassword, newPassword, userId);

            Assert.IsFalse(result.Status, "Expected failure when current password is incorrect!");
            Assert.AreEqual("Incorrect current password!", result.StatusMsg);
        }

        [TestMethod]
        public async Task SuccessPasswordChange()
        {
            string currentPassword = "StrongPass123!";
            string newPassword = "NewStrong1!";
            int userId = 4;

            var result = await _profile.ChangePassword(currentPassword, newPassword, userId);

            Assert.IsTrue(result.Status, "Expected success when password is changed correctly!");
            Assert.AreEqual("Password changed successfully!", result.StatusMsg);
        }
    }
}
