using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using AutoMapper;
using eUseControl.BusinessLogic.DBModel;
using eUseControl.Domain.Entities;
using eUseControl.Domain.Entities.User;
using eUseControl.Domain.Enums;
using eUseControl.Helpers;

namespace eUseControl.BusinessLogic.Core
{
    public class UserApi
    {
        internal URegisterResp UserRegisterAction(URegisterData data)
        {
            try
            {
                if (data.Password.Length < 8)
                {
                    return new URegisterResp { Status = false, StatusMsg = "Minimum 8 characters required!" };
                }

                using (var db = new UserContext())
                {
                    if (db.Users.Any(u => u.Email == data.Email))
                    {
                        return new URegisterResp { Status = false, StatusMsg = "Email has already been used!" };
                    }

                    if (db.Users.Any(u => u.Username == data.Username))
                    {
                        return new URegisterResp { Status = false, StatusMsg = "Username has already been used!" };
                    }

                    var hashedPassword = LoginHelper.HashGen(data.Password);

                    var newUser = new UDbTable
                    {
                        Username = data.Username,
                        Email = data.Email,
                        Password = hashedPassword,
                        RegistrationDateTime = data.RegistrationDateTime,
                        RegistrationIp = data.RegistrationIp
                    };

                    db.Users.Add(newUser);
                    db.SaveChanges();
                }

                return new URegisterResp { Status = true };
            }
            catch (DbUpdateException e)
            {
                var innerException = e.InnerException;
                while (innerException != null)
                {
                    System.Diagnostics.Debug.WriteLine(innerException.Message);
                    innerException = innerException.InnerException;
                }

                return new URegisterResp { Status = false, StatusMsg = "We couldn't save your account!" };
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                return new URegisterResp { Status = false, StatusMsg = "Hmm, something went wrong!" };
            }
        }

        internal ULoginResp UserLoginAction(ULoginData data)
        {
            UDbTable result;
            var validate = new EmailAddressAttribute();

            if (validate.IsValid(data.Username))
            {
                var pass = LoginHelper.HashGen(data.Password);

                using (var db = new UserContext())
                {
                    result = db.Users.FirstOrDefault(u => u.Email == data.Username && u.Password == pass);
                }

                if (result == null)
                {
                    return new ULoginResp { Status = false, StatusMsg = "The username or password is incorrect!" };
                }

                using (var todo = new UserContext())
                {
                    result.LastIp = data.LastIp;
                    result.Level = data.Level;
                    result.LastLogin = data.LastLogin;
                    todo.Entry(result).State = EntityState.Modified;
                    todo.SaveChanges();
                }

                var userMinimal = new UserMinimal
                {
                    Id = result.Id,
                    Username = result.Username,
                    Email = result.Email,
                    LastLogin = result.LastLogin ?? DateTime.Now,
                    LastIp = result.LastIp,
                    Level = result.Level ?? URole.User
                };
                return new ULoginResp { Status = true, UserMinimal = userMinimal };
            }
            else
            {
                var pass = LoginHelper.HashGen(data.Password);
                using (var db = new UserContext())
                {
                    result = db.Users.FirstOrDefault(u => u.Username == data.Username && u.Password == pass);
                }

                if (result == null)
                {
                    return new ULoginResp { Status = false, StatusMsg = "The username or password is incorrect!" };
                }

                using (var todo = new UserContext())
                {
                    result.LastIp = data.LastIp;
                    result.Level = data.Level;
                    result.LastLogin = data.LastLogin;
                    todo.Entry(result).State = EntityState.Modified;
                    todo.SaveChanges();
                }

                var userMinimal = new UserMinimal
                {
                    Id = result.Id,
                    Username = result.Username,
                    Email = result.Email,
                    LastLogin = result.LastLogin ?? DateTime.Now,
                    LastIp = result.LastIp,
                    Level = result.Level ?? URole.User
                };
                return new ULoginResp { Status = true, UserMinimal = userMinimal };
            }
        }

        internal HttpCookie Cookie(string loginCredential)
        {
            var apiCookie = new HttpCookie("X-KEY")
            {
                Value = CookieGenerator.Create(loginCredential)
            };

            using (var db = new SessionContext())
            {
                Session curent;
                var validate = new EmailAddressAttribute();

                if (validate.IsValid(loginCredential))
                {
                    curent = (from e in db.Sessions where e.Username == loginCredential select e).FirstOrDefault();
                }
                else
                {
                    curent = (from e in db.Sessions where e.Username == loginCredential select e).FirstOrDefault();
                }

                if (curent != null)
                {
                    curent.CookieString = apiCookie.Value;
                    curent.ExpireTime = DateTime.Now.AddMinutes(60);

                    using (var todo = new SessionContext())
                    {
                        todo.Entry(curent).State = EntityState.Modified;
                        todo.SaveChanges();
                    }
                }
                else
                {
                    db.Sessions.Add(new Session
                    {
                        Username = loginCredential,
                        CookieString = apiCookie.Value,
                        ExpireTime = DateTime.Now.AddMinutes(60)
                    });
                    db.SaveChanges();
                }
            }

            return apiCookie;
        }

        internal UserMinimal UserCookie(string cookie)
        {
            Session session;
            UDbTable curentUser;

            using (var db = new SessionContext())
            {
                session = db.Sessions.FirstOrDefault(s => s.CookieString == cookie && s.ExpireTime > DateTime.Now);
            }

            if (session == null) return null;

            using (var db = new UserContext())
            {
                var validate = new EmailAddressAttribute();

                if (validate.IsValid(session.Username))
                {
                    curentUser = db.Users.FirstOrDefault(u => u.Email == session.Username);
                }
                else
                {
                    curentUser = db.Users.FirstOrDefault(u => u.Username == session.Username);
                }
            }

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<UDbTable, UserMinimal>();
            });

            var mapper = config.CreateMapper();
            UserMinimal userMinimal = mapper.Map<UserMinimal>(curentUser);

            return userMinimal;
        }
    }
}
