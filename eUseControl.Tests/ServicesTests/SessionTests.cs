using System;
using eUseControl.BusinessLogic;
using eUseControl.BusinessLogic.Interfaces;
using eUseControl.Domain.Entities;
using eUseControl.Domain.Entities.User;
using eUseControl.Domain.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace eUseControl.Tests.ServicesTests
{
    [TestClass]
    public class SessionTests
    {
        private readonly ISession _session;

        public SessionTests()
        {
            var bl = new BusinessLogicManager();
            _session = bl.GetSessionBL();
        }

        [TestMethod]
        public void RegisterPasswordTooShort()
        {
            var data = new URegisterData
            {
                Username = "shortuser",
                Email = "short@domain.com",
                Password = "123",
                RegistrationDateTime = DateTime.Now,
                RegistrationIp = "127.0.0.1",
                Level = URole.User
            };

            var result = _session.UserRegister(data);

            Assert.IsFalse(result.Status, "Expected registration to fail due to password being too short!");
            Assert.AreEqual("Minimum 8 characters required!", result.StatusMsg);
        }

        [TestMethod]
        public void RegisterWeakPassword()
        {
            var data = new URegisterData
            {
                Username = "weakpassuser",
                Email = "weak@domain.com",
                Password = "abcdefgh",
                RegistrationDateTime = DateTime.Now,
                RegistrationIp = "127.0.0.1",
                Level = URole.User
            };

            var result = _session.UserRegister(data);

            Assert.IsFalse(result.Status, "Expected registration to fail due to weak password!");
            Assert.AreEqual("Password must meet complexity requirements!", result.StatusMsg);
        }

        [TestMethod]
        public void RegisterEmailAlreadyUsed()
        {
            var data = new URegisterData
            {
                Username = "anyuser123",
                Email = "maria@email.com",
                Password = "StrongPass123!",
                RegistrationDateTime = DateTime.Now,
                RegistrationIp = "127.0.0.1",
                Level = URole.User
            };

            var result = _session.UserRegister(data);

            Assert.IsFalse(result.Status, "Expected registration to fail because the email is already used!");
            Assert.AreEqual("Email has already been used!", result.StatusMsg);
        }

        [TestMethod]
        public void RegisterUsernameAlreadyUsed()
        {
            var data = new URegisterData
            {
                Username = "andrei22",
                Email = "newemail1234@domain.com",
                Password = "StrongPass123!",
                RegistrationDateTime = DateTime.Now,
                RegistrationIp = "127.0.0.1",
                Level = URole.User
            };

            var result = _session.UserRegister(data);

            Assert.IsFalse(result.Status, "Expected registration to fail because the username is already used!");
            Assert.AreEqual("Username has already been used!", result.StatusMsg);
        }

        [TestMethod]
        public void RegisterSuccess()
        {
            var data = new URegisterData
            {
                Username = "anyuser123",
                Email = "weak@domain.com",
                Password = "StrongPass123!",
                RegistrationDateTime = DateTime.Now,
                RegistrationIp = "127.0.0.1",
                Level = URole.User
            };

            var result = _session.UserRegister(data);

            Assert.IsTrue(result.Status, "Expected registration to succeed with valid data!");
            Assert.AreEqual("You have successfully registered!", result.StatusMsg);
        }

        [TestMethod]
        public void LoginByUsernameSuccess()
        {
            var user = new ULoginData
            {
                Username = "anyuser123",
                Password = "StrongPass123!",
                LastIp = "127.0.0.1",
                LastLogin = DateTime.Now
            };

            var result = _session.UserLogin(user);

            Assert.IsTrue(result.Status, "Expected login to succeed with correct credentials!");
            Assert.IsNotNull(result.UserMinimal, "Expected user data to be returned after successful login!");
            Assert.AreEqual(user.Username, result.UserMinimal.Username, "Expected the returned username to match the input username!");
        }

        [TestMethod]
        public void LoginWrongPassword()
        {
            var user = new ULoginData
            {
                Username = "maria33",
                Password = "MariaSjjecure!",
                LastIp = "127.0.0.1",
                LastLogin = DateTime.Now
            };

            var result = _session.UserLogin(user);

            Assert.IsFalse(result.Status, "Expected login to fail due to wrong password!");
            Assert.AreEqual("The username or password is incorrect!", result.StatusMsg);
        }

        [TestMethod]
        public void LoginUnknownUser()
        {
            var user = new ULoginData
            {
                Username = "ioana33",
                Password = "MariaSecure!",
                LastIp = "127.0.0.1",
                LastLogin = DateTime.Now
            };

            var result = _session.UserLogin(user);

            Assert.IsFalse(result.Status, "Expected login to fail because the user does not exist!");
            Assert.AreEqual("The username or password is incorrect!", result.StatusMsg);
        }

        [TestMethod]
        public void LoginEmptyFields()
        {
            var user = new ULoginData
            {
                Username = "",
                Password = "",
                LastIp = "127.0.0.1",
                LastLogin = DateTime.Now
            };

            var result = _session.UserLogin(user);

            Assert.IsFalse(result.Status, "Expected login to fail due to empty username and password!");
            Assert.AreEqual("The username or password is incorrect!", result.StatusMsg);
        }

        [TestMethod]
        public void GetUserFound()
        {
            int userId = 1;

            var result = _session.GetUserById(userId);

            Assert.IsNotNull(result, "Expected to find a user with the given ID!");
            Assert.AreEqual(userId, result.Id, "Expected the returned user ID to match the requested ID!");
        }

        [TestMethod]
        public void GetUserNotFound()
        {
            int userId = -1;

            var result = _session.GetUserById(userId);

            Assert.IsNull(result, "Expected no user to be found with invalid ID!");
        }
    }
}
