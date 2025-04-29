using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using AutoMapper;
using eUseControl.BusinessLogic.DBModel;
using eUseControl.Domain.Entities;
using eUseControl.Domain.Entities.User;
using eUseControl.Domain.Enums;
using eUseControl.Helpers;
using eUseControl.Domain.Entities.Product;
using eUseControl.Domain.Entities.Profile;

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

        internal ProductResp CreateProductAction(ProductData productData, string cookie)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(productData.ProductName) ||
                    string.IsNullOrWhiteSpace(productData.ProductAddress) ||
                    string.IsNullOrWhiteSpace(productData.ProductQuality) ||
                    string.IsNullOrWhiteSpace(productData.ProductRegion) ||
                    string.IsNullOrWhiteSpace(productData.ProductImageUrl) ||
                    string.IsNullOrWhiteSpace(productData.ProductDescription) ||
                    string.IsNullOrWhiteSpace(productData.ProductCategory))
                {
                    return new ProductResp { Status = false, StatusMsg = "All fields are required!" };
                }

                if (productData.ProductQuantity <= 0)
                {
                    return new ProductResp { Status = false, StatusMsg = "Quantity must be a positive number!" };
                }

                if (productData.ProductPrice <= 0)
                {
                    return new ProductResp { Status = false, StatusMsg = "Price must be greater than zero!" };
                }

                Category category;
                using (var db = new CategoryContext())
                {
                    category = db.Categories.FirstOrDefault(c => c.CategoryName == productData.ProductCategory);
                    if (category == null)
                    {
                        return new ProductResp { Status = false, StatusMsg = "Invalid category!" };
                    }
                }

                var userMinimal = UserCookie(cookie);
                if (userMinimal == null)
                {
                    return new ProductResp { Status = false, StatusMsg = "User not authenticated!" };          
                }

                using (var db = new ProductContext())
                {
                    var product = new ProductDbTable
                    {
                        ProductName = productData.ProductName,
                        ProductAddress = productData.ProductAddress,
                        ProductQuantity = productData.ProductQuantity,
                        ProductQuality = productData.ProductQuality,
                        ProductPrice = productData.ProductPrice,
                        ProductRegion = productData.ProductRegion,
                        ProductImageUrl = productData.ProductImageUrl,
                        ProductDescription = productData.ProductDescription,
                        ProductPostDate = DateTime.Now,
                        UserId = userMinimal.Id,
                        CategoryId = category.Id,
                        ProductStatus = ProductStatus.Available
                    };

                    db.Products.Add(product);
                    db.SaveChanges();
                }

                return new ProductResp { Status = true, StatusMsg = "Product created successfully!" };
            }
            catch (Exception ex) 
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return new ProductResp { Status = false, StatusMsg = "An error occurred while saving the product!" };
            }
        }

        internal List<ProductMinimal> GetProducts(string cookie)
        {
            var userMinimal = UserCookie(cookie);
            if (userMinimal == null)
            {
                return new List<ProductMinimal>();
            }

            using (var db = new ProductContext())
            {
                var products = db.Products
                    .Where(p => p.UserId == userMinimal.Id)
                    .ToList();

                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<ProductDbTable, ProductMinimal>()
                       .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                       .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.ProductName))
                       .ForMember(dest => dest.ProductDescription, opt => opt.MapFrom(src => src.ProductDescription))
                       .ForMember(dest => dest.ProductPrice, opt => opt.MapFrom(src => src.ProductPrice))
                       .ForMember(dest => dest.ProductImageUrl, opt => opt.MapFrom(src => src.ProductImageUrl));
                });

                var mapper = config.CreateMapper();
                var productMinimals = mapper.Map<List<ProductMinimal>>(products);

                return productMinimals;
            }
        }

        internal ProductData GetProductByIdAction(int productId, string cookie)
        {
            try
            {
                using (var db = new ProductContext()) 
                {
                    var userMinimal = UserCookie(cookie);

                    var product = db.Products
                                    .Where(p => p.Id == productId && p.UserId == userMinimal.Id)
                                    .FirstOrDefault();

                    if (product == null)
                    {
                        return null; 
                    }

                    using (var categoryDb = new CategoryContext())
                    {
                        var categoryName = categoryDb.Categories
                                                     .Where(c => c.Id == product.CategoryId)
                                                     .Select(c => c.CategoryName)
                                                     .FirstOrDefault();

                        var productData = new ProductData
                        {
                            ProductName = product.ProductName,
                            ProductAddress = product.ProductAddress,
                            ProductQuantity = product.ProductQuantity,
                            ProductQuality = product.ProductQuality,
                            ProductPrice = product.ProductPrice,
                            ProductRegion = product.ProductRegion,
                            ProductImageUrl = product.ProductImageUrl,
                            ProductDescription = product.ProductDescription,
                            ProductCategory = categoryName,
                        };

                        return productData;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return null; 
            }
        }

        internal ProductResp UpdateProductAction(ProductData productData, string cookie, int productId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(productData.ProductName) ||
                    string.IsNullOrWhiteSpace(productData.ProductAddress) ||
                    string.IsNullOrWhiteSpace(productData.ProductQuality) ||
                    string.IsNullOrWhiteSpace(productData.ProductRegion) ||
                    string.IsNullOrWhiteSpace(productData.ProductImageUrl) ||
                    string.IsNullOrWhiteSpace(productData.ProductDescription) ||
                    string.IsNullOrWhiteSpace(productData.ProductCategory))
                {
                    return new ProductResp { Status = false, StatusMsg = "All fields are required!" };
                }

                if (productData.ProductQuantity <= 0)
                {
                    return new ProductResp { Status = false, StatusMsg = "Quantity must be a positive number!" };
                }

                if (productData.ProductPrice <= 0)
                {
                    return new ProductResp { Status = false, StatusMsg = "Price must be greater than zero!" };
                }

                Category category;
                using (var db = new CategoryContext())
                {
                    category = db.Categories.FirstOrDefault(c => c.CategoryName == productData.ProductCategory);
                    if (category == null)
                    {
                        return new ProductResp { Status = false, StatusMsg = "Invalid category!" };
                    }
                }

                var userMinimal = UserCookie(cookie);
                if (userMinimal == null)
                {
                    return new ProductResp { Status = false, StatusMsg = "User not authenticated!" };
                }

                ProductDbTable product;
                using (var db = new ProductContext())
                {
                    product = db.Products.FirstOrDefault(p => p.Id == productId && p.UserId == userMinimal.Id);
                    if (product == null)
                    {
                        return new ProductResp { Status = false, StatusMsg = "Product not found!" };
                    }

                    product.ProductName = productData.ProductName;
                    product.ProductAddress = productData.ProductAddress;
                    product.ProductQuantity = productData.ProductQuantity;
                    product.ProductQuality = productData.ProductQuality;
                    product.ProductPrice = productData.ProductPrice;
                    product.ProductRegion = productData.ProductRegion;
                    product.ProductImageUrl = productData.ProductImageUrl;
                    product.ProductDescription = productData.ProductDescription;
                    product.CategoryId = category.Id;

                    db.Entry(product).State = EntityState.Modified;
                    db.SaveChanges();
                }

                return new ProductResp { Status = true, StatusMsg = "Product updated successfully!" };
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return new ProductResp { Status = false, StatusMsg = "An error occurred while updating the product!" };
            }
        }

        internal ProfileData GetProfileByUserIdAction(int userId)
        {
            try
            {
                UDbTable user;
                using (var userDb = new UserContext())
                {
                    user = userDb.Users.FirstOrDefault(u => u.Id == userId);
                    if (user == null)
                    {
                        return null;
                    }
                }

                ProfileData profile = null;
                using (var profileDb = new ProfileContext())
                {
                    var userProfile = profileDb.UserProfiles.FirstOrDefault(p => p.UserId == userId);

                    if (userProfile == null)
                    {
                        userProfile = new ProfileDbTable
                        {
                            UserId = userId,
                            FirstName = "User", 
                            LastName = "User", 
                            Email = user.Email,
                            Address = "N/A",
                            PhoneNumber = "N/A",
                            LastProfileUpdate = DateTime.Now,
                            ProfileImageUrl = "/Assets/img/user.jpg"
                        };

                        profileDb.UserProfiles.Add(userProfile);
                        profileDb.SaveChanges();
                    }

                    var config = new MapperConfiguration(cfg =>
                    {
                        cfg.CreateMap<ProfileDbTable, ProfileData>();
                    });

                    var mapper = config.CreateMapper();
                    profile = mapper.Map<ProfileData>(userProfile);
                }

                return profile;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return null;  
            }
        }

        internal ProfileResp UpdateProfileAction(int userId, ProfileData profileData)
        {
            try
            {
                using (var db = new ProfileContext())
                {
                    var userProfile = db.UserProfiles.FirstOrDefault(p => p.UserId == userId);

                    if (userProfile == null)
                    {
                        return new ProfileResp { Status = false, StatusMsg = "We couldn't find your profile!" };
                    }

                    userProfile.FirstName = profileData.FirstName;
                    userProfile.LastName = profileData.LastName;
                    userProfile.Email = profileData.Email;
                    userProfile.Address = profileData.Address;
                    userProfile.PhoneNumber = profileData.PhoneNumber;
                    if (!string.IsNullOrEmpty(profileData.ProfileImageUrl))
                    {
                        userProfile.ProfileImageUrl = profileData.ProfileImageUrl;
                    }
                    userProfile.LastProfileUpdate = DateTime.Now;

                    db.Entry(userProfile).State = EntityState.Modified;
                    db.SaveChanges();
                }

                return new ProfileResp { Status = true, StatusMsg = "Your profile has been updated!" };
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return new ProfileResp { Status = false, StatusMsg = "Oops! There was an error saving your changes!" };
            }
        }

        internal ProfileResp ChangePasswordAction(string currentPassword, string newPassword, int userId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(currentPassword) || string.IsNullOrWhiteSpace(newPassword))
                {
                    return new ProfileResp { Status = false, StatusMsg = "Passwords cannot be empty!" };
                }

                if (newPassword.Length < 8)
                {
                    return new ProfileResp { Status = false, StatusMsg = "New password must be at least 8 characters long!" };
                }

                if (currentPassword == newPassword)
                {
                    return new ProfileResp { Status = false, StatusMsg = "New password must be different from the current one!" };
                }

                var hashedCurrent = LoginHelper.HashGen(currentPassword);
                var hashedNew = LoginHelper.HashGen(newPassword);

                using (var db = new UserContext())
                {
                    var user = db.Users.FirstOrDefault(u => u.Id == userId && u.Password == hashedCurrent);

                    if (user == null)
                    {
                        return new ProfileResp { Status = false, StatusMsg = "Incorrect current password!" };
                    }

                    user.Password = hashedNew;
                    db.Entry(user).State = EntityState.Modified;
                    db.SaveChanges();

                    return new ProfileResp { Status = true, StatusMsg = "Password changed successfully!" };
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return new ProfileResp { Status = false, StatusMsg = "An error occurred while updating the password!" };
            }
        }
    }
}
