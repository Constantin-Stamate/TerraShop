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

            Assert.IsFalse(result.Status);
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

            Assert.IsFalse(result.Status);
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

            Assert.IsFalse(result.Status);
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

            Assert.IsFalse(result.Status);
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

            Assert.IsTrue(result.Status);
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

            Assert.IsTrue(result.Status);
            Assert.IsNotNull(result.UserMinimal);
            Assert.AreEqual(user.Username, result.UserMinimal.Username);
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

            Assert.IsFalse(result.Status);
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

            Assert.IsFalse(result.Status);
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

            Assert.IsFalse(result.Status);
            Assert.AreEqual("The username or password is incorrect!", result.StatusMsg);
        }

        [TestMethod]
        public void GetUserFound()
        {
            int userId = 1;

            var result = _session.GetUserById(userId);

            Assert.IsNotNull(result);
            Assert.AreEqual(userId, result.Id);
        }

        [TestMethod]
        public void GetUserNotFound()
        {
            int userId = -1;

            var result = _session.GetUserById(userId);

            Assert.IsNull(result);
        }
    }
}
