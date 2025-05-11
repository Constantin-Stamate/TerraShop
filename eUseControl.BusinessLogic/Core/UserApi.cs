using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
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
using System.Text.RegularExpressions;
using eUseControl.Domain.Entities.Review;

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
                    return new URegisterResp 
                    { 
                        Status = false, 
                        StatusMsg = "Minimum 8 characters required!" 
                    };
                }

                if (!Regex.IsMatch(data.Password, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).+$"))
                {
                    return new URegisterResp
                    {
                        Status = false,
                        StatusMsg = "Password must meet complexity requirements!"
                    };
                }

                using (var db = new UserContext())
                {
                    if (db.Users.Any(u => u.Email == data.Email))
                    {
                        return new URegisterResp 
                        { 
                            Status = false, 
                            StatusMsg = "Email has already been used!" 
                        };
                    }

                    if (db.Users.Any(u => u.Username == data.Username))
                    {
                        return new URegisterResp 
                        { 
                            Status = false, 
                            StatusMsg = "Username has already been used!" 
                        };
                    }

                    var hashedPassword = LoginHelper.HashGen(data.Password);

                    var newUser = new UDbTable
                    {
                        Username = data.Username,
                        Email = data.Email,
                        Password = hashedPassword,
                        RegistrationDateTime = data.RegistrationDateTime,
                        RegistrationIp = data.RegistrationIp,
                        Level = data.Level
                    };

                    db.Users.Add(newUser);
                    db.SaveChanges();
                }

                return new URegisterResp 
                { 
                    Status = true,
                    StatusMsg = "You have successfully registered!"
                };
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return new URegisterResp 
                { 
                    Status = false, 
                    StatusMsg = "Hmm, something went wrong!" 
                };
            }
        }

        internal ULoginResp UserLoginAction(ULoginData data)
        {
            try
            {
                UDbTable result;
                var validate = new EmailAddressAttribute();
                var pass = LoginHelper.HashGen(data.Password);

                using (var db = new UserContext())
                {
                    if (validate.IsValid(data.Username))
                    {
                        result = db.Users
                            .FirstOrDefault(u => u.Email == data.Username && u.Password == pass);
                    }
                    else
                    {
                        result = db.Users
                            .FirstOrDefault(u => u.Username == data.Username && u.Password == pass);
                    }

                    if (result == null)
                    {
                        return new ULoginResp
                        {
                            Status = false,
                            StatusMsg = "The username or password is incorrect!"
                        };
                    }

                    result.LastIp = data.LastIp;
                    result.LastLogin = data.LastLogin;

                    db.Entry(result).State = EntityState.Modified;
                    db.SaveChanges();

                    return new ULoginResp
                    {
                        Status = true,
                        UserMinimal = new UserMinimal
                        {
                            Id = result.Id,
                            Username = result.Username,
                            Email = result.Email,
                            LastLogin = result.LastLogin ?? DateTime.Now,
                            LastIp = result.LastIp,
                            Level = result.Level ?? URole.User
                        }
                    };
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return new ULoginResp
                {
                    Status = false,
                    StatusMsg = "An unexpected error occurred during login!"
                };
            }
        }

        internal HttpCookie Cookie(string loginCredential)
        {
            try
            {
                var apiCookie = new HttpCookie("X-KEY")
                {
                    Value = CookieGenerator.Create(loginCredential)
                };

                using (var db = new SessionContext())
                {
                    Session curent;
                    var validate = new EmailAddressAttribute();

                    curent = db.Sessions
                        .FirstOrDefault(e => e.Username == loginCredential);

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
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return null;
            }
        }

        internal UserMinimal UserCookie(string cookie)
        {
            try
            {
                Session session;
                UDbTable curentUser;

                using (var db = new SessionContext())
                {
                    session = db.Sessions
                        .FirstOrDefault(s => s.CookieString == cookie && s.ExpireTime > DateTime.Now);
                }

                if (session == null) return null;

                using (var db = new UserContext())
                {
                    var validate = new EmailAddressAttribute();

                    if (validate.IsValid(session.Username))
                    {
                        curentUser = db.Users
                            .FirstOrDefault(u => u.Email == session.Username);
                    }
                    else
                    {
                        curentUser = db.Users
                            .FirstOrDefault(u => u.Username == session.Username);
                    }
                }

                if (curentUser == null) return null;

                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<UDbTable, UserMinimal>();
                });

                var mapper = config.CreateMapper();
                UserMinimal userMinimal = mapper.Map<UserMinimal>(curentUser);

                return userMinimal;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return null;
            }
        }

        internal ProductResp CreateProductAction(ProductData productData, int userId)
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
                    return new ProductResp 
                    { 
                        Status = false, 
                        StatusMsg = "All fields are required!" 
                    };
                }

                if (productData.ProductQuantity <= 0)
                {
                    return new ProductResp 
                    { 
                        Status = false, 
                        StatusMsg = "Quantity must be a positive number!" 
                    };
                }

                if (productData.ProductPrice <= 0)
                {
                    return new ProductResp 
                    { 
                        Status = false, 
                        StatusMsg = "Price must be greater than zero!" 
                    };
                }

                Category category;
                using (var db = new CategoryContext())
                {
                    category = db.Categories
                        .FirstOrDefault(c => c.CategoryName == productData.ProductCategory);

                    if (category == null)
                    {
                        return new ProductResp 
                        { 
                            Status = false, 
                            StatusMsg = "Invalid category!" 
                        };
                    }
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
                        UserId = userId,
                        CategoryId = category.Id,
                        ProductStatus = ProductStatus.Available,
                        ProductRating = 5
                    };

                    db.Products.Add(product);
                    db.SaveChanges();
                }

                return new ProductResp 
                { 
                    Status = true, 
                    StatusMsg = "The product has been successfully created!"
                };
            }
            catch (Exception ex) 
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return new ProductResp 
                { 
                    Status = false, 
                    StatusMsg = "An error occurred while saving the product!" 
                };
            }
        }

        internal List<ProductMinimal> GetProductsByUserIdAction(int userId)
        {
            try
            {
                using (var db = new ProductContext())
                {
                    var products = db.Products
                        .Where(p => p.UserId == userId)
                        .ToList();

                    var config = new MapperConfiguration(cfg =>
                    {
                        cfg.CreateMap<ProductDbTable, ProductMinimal>();
                    });

                    var mapper = config.CreateMapper();
                    var productMinimals = mapper.Map<List<ProductMinimal>>(products);

                    return productMinimals;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return new List<ProductMinimal>();
            }
        }

        internal ProductData GetProductByIdAction(int productId)
        {
            try
            {
                using (var db = new ProductContext()) 
                {
                    var product = db.Products
                                    .Where(p => p.Id == productId)
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
                            ProductPostDate = product.ProductPostDate,
                            ProductRating = product.ProductRating,
                            UserId = product.UserId
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

        internal ProductResp UpdateProductAction(ProductData productData)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(productData.ProductName) ||
                    string.IsNullOrWhiteSpace(productData.ProductAddress) ||
                    string.IsNullOrWhiteSpace(productData.ProductQuality) ||
                    string.IsNullOrWhiteSpace(productData.ProductRegion) ||
                    string.IsNullOrWhiteSpace(productData.ProductDescription) ||
                    string.IsNullOrWhiteSpace(productData.ProductCategory))
                {
                    return new ProductResp 
                    { 
                        Status = false, 
                        StatusMsg = "All fields are required!" 
                    };
                }

                if (productData.ProductQuantity <= 0)
                {
                    return new ProductResp 
                    { 
                        Status = false, 
                        StatusMsg = "Quantity must be a positive number!" 
                    };
                }

                if (productData.ProductPrice <= 0)
                {
                    return new ProductResp 
                    { 
                        Status = false, 
                        StatusMsg = "Price must be greater than zero!" 
                    };
                }

                Category category;
                using (var db = new CategoryContext())
                {
                    category = db.Categories
                        .FirstOrDefault(c => c.CategoryName == productData.ProductCategory);

                    if (category == null)
                    {
                        return new ProductResp 
                        { 
                            Status = false, 
                            StatusMsg = "Invalid category!" 
                        };
                    }
                }

                ProductDbTable product;
                using (var db = new ProductContext())
                {
                    product = db.Products
                        .FirstOrDefault(p => p.Id == productData.Id);

                    if (product == null)
                    {
                        return new ProductResp 
                        { 
                            Status = false, 
                            StatusMsg = "Product not found!" 
                        };
                    }

                    product.ProductName = productData.ProductName;
                    product.ProductAddress = productData.ProductAddress;
                    product.ProductQuantity = productData.ProductQuantity;
                    product.ProductQuality = productData.ProductQuality;
                    product.ProductPrice = productData.ProductPrice;
                    product.ProductRegion = productData.ProductRegion;
                    if (!string.IsNullOrEmpty(productData.ProductImageUrl))
                        product.ProductImageUrl = productData.ProductImageUrl;
                    product.ProductDescription = productData.ProductDescription;
                    product.CategoryId = category.Id;

                    db.Entry(product).State = EntityState.Modified;
                    db.SaveChanges();
                }

                return new ProductResp 
                { 
                    Status = true, 
                    StatusMsg = "Product updated successfully!" 
                };
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return new ProductResp 
                { 
                    Status = false, 
                    StatusMsg = "An error occurred while updating the product!" 
                };
            }
        }

        internal ProfileData GetProfileByUserIdAction(int userId)
        {
            try
            {
                UDbTable user;
                using (var userDb = new UserContext())
                {
                    user = userDb.Users
                        .FirstOrDefault(u => u.Id == userId);

                    if (user == null)
                    {
                        return null;
                    }
                }

                ProfileData profile = null;
                using (var profileDb = new ProfileContext())
                {
                    var userProfile = profileDb.UserProfiles
                        .FirstOrDefault(p => p.UserId == userId);

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

        internal ProfileResp UpdateProfileAction(ProfileData profileData)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(profileData.FirstName) || profileData.FirstName.Length < 5)
                { 
                    return new ProfileResp 
                    { 
                        Status = false, 
                        StatusMsg = "First name must be at least 5 characters!" 
                    };
                }

                if (string.IsNullOrWhiteSpace(profileData.LastName) || profileData.LastName.Length < 5)
                {
                    return new ProfileResp
                    {
                        Status = false,
                        StatusMsg = "Last name must be at least 5 characters!"
                    };
                }

                if (string.IsNullOrWhiteSpace(profileData.Email) || !Regex.IsMatch(profileData.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                {
                    return new ProfileResp
                    {
                        Status = false,
                        StatusMsg = "Please enter a valid email address!"
                    };
                }

                if (string.IsNullOrWhiteSpace(profileData.PhoneNumber) || !Regex.IsMatch(profileData.PhoneNumber, @"^\+?\d{7,15}$"))
                {
                    return new ProfileResp
                    {
                        Status = false,
                        StatusMsg = "Please enter a valid phone number!"
                    };
                }

                if (string.IsNullOrWhiteSpace(profileData.Address) || profileData.Address.Length < 5)
                {
                    return new ProfileResp
                    {
                        Status = false,
                        StatusMsg = "Address must be at least 5 characters!"
                    };
                }

                using (var userDb = new UserContext())
                {
                    var existingEmailUser = userDb.Users
                        .FirstOrDefault(u => u.Email == profileData.Email && u.Id != profileData.UserId);

                    if (existingEmailUser != null)
                    {
                        return new ProfileResp
                        {
                            Status = false,
                            StatusMsg = "This email is already in use by another user!"
                        };
                    }

                    using (var profileDb = new ProfileContext())
                    {
                        var userProfile = profileDb.UserProfiles
                            .FirstOrDefault(p => p.UserId == profileData.UserId);

                        if (userProfile == null)
                        {
                            return new ProfileResp
                            {
                                Status = false,
                                StatusMsg = "We couldn't find your profile!"
                            };
                        }

                        userProfile.FirstName = profileData.FirstName;
                        userProfile.LastName = profileData.LastName;
                        userProfile.Email = profileData.Email;
                        userProfile.Address = profileData.Address;
                        userProfile.PhoneNumber = profileData.PhoneNumber;
                        if (!string.IsNullOrEmpty(profileData.ProfileImageUrl))
                            userProfile.ProfileImageUrl = profileData.ProfileImageUrl;
                        userProfile.LastProfileUpdate = DateTime.Now;
                        profileDb.Entry(userProfile).State = EntityState.Modified;
                        profileDb.SaveChanges();

                        var user = userDb.Users
                            .FirstOrDefault(u => u.Id == profileData.UserId);

                        if (user != null)
                        {
                            user.Email = profileData.Email;
                            userDb.Entry(user).State = EntityState.Modified;
                            userDb.SaveChanges();
                        }
                    }

                    return new ProfileResp
                    {
                        Status = true,
                        StatusMsg = "Your profile has been updated!"
                    };
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return new ProfileResp 
                { 
                    Status = false, 
                    StatusMsg = "Oops! There was an error saving your changes!" 
                };
            }
        }

        internal ProfileResp ChangePasswordAction(string currentPassword, string newPassword, int userId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(currentPassword) || string.IsNullOrWhiteSpace(newPassword))
                {
                    return new ProfileResp 
                    { 
                        Status = false, 
                        StatusMsg = "Passwords cannot be empty!" 
                    };
                }

                if (newPassword.Length < 8)
                {
                    return new ProfileResp 
                    { 
                        Status = false, 
                        StatusMsg = "New password must be at least 8 characters long!" 
                    };
                }

                if (currentPassword == newPassword)
                {
                    return new ProfileResp 
                    { 
                        Status = false, 
                        StatusMsg = "New password must be different from the current one!" 
                    };
                }

                if (!Regex.IsMatch(newPassword, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).+$"))
                {
                    return new ProfileResp
                    {
                        Status = false,
                        StatusMsg = "Password must meet complexity requirements!"
                    };
                }

                var hashedCurrent = LoginHelper.HashGen(currentPassword);
                var hashedNew = LoginHelper.HashGen(newPassword);

                using (var db = new UserContext())
                {
                    var user = db.Users
                        .FirstOrDefault(u => u.Id == userId && u.Password == hashedCurrent);

                    if (user == null)
                    {
                        return new ProfileResp 
                        { 
                            Status = false, 
                            StatusMsg = "Incorrect current password!" 
                        };
                    }

                    user.Password = hashedNew;
                    db.Entry(user).State = EntityState.Modified;
                    db.SaveChanges();

                    return new ProfileResp 
                    { 
                        Status = true, 
                        StatusMsg = "Password changed successfully!" 
                    };
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return new ProfileResp 
                { 
                    Status = false, 
                    StatusMsg = "An error occurred while updating the password!" 
                };
            }
        }

        internal List<ProductSummary> GetAvailableProductsAction()
        {
            try
            {
                using (var db = new ProductContext())
                {
                    var products = db.Products
                        .Where(p => p.ProductStatus == ProductStatus.Available)
                        .ToList();

                    var productsList = new List<ProductSummary>();

                    using (var categoryDb = new CategoryContext())
                    {
                        foreach (var product in products)
                        {
                            var category = categoryDb.Categories
                                .FirstOrDefault(c => c.Id == product.CategoryId);

                            if (category != null)
                            {
                                var productSummary = new ProductSummary
                                {
                                    Id = product.Id,
                                    ProductCategory = category.CategoryName,
                                    ProductName = product.ProductName,
                                    ProductDescription = product.ProductDescription,
                                    ProductPrice = product.ProductPrice,
                                    ProductImageUrl = product.ProductImageUrl,
                                    ProductPostDate = product.ProductPostDate,
                                    ProductRegion = product.ProductRegion
                                };

                                productsList.Add(productSummary);
                            }
                        }
                    }

                    return productsList;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return new List<ProductSummary>();
            }
        }

        internal void UpdateProductStatusAction(int productId)
        {
            try
            {
                using (var db = new ProductContext())
                {
                    var product = db.Products
                        .FirstOrDefault(p => p.Id == productId);

                    if (product != null)
                    {
                        product.ProductStatus = product.ProductStatus == ProductStatus.Available ? ProductStatus.Unavailable : ProductStatus.Available;

                        db.Entry(product).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        internal Dictionary<Category, int> GetCategoryProductCountsAction()
        {
            try
            {
                using (var categoryDb = new CategoryContext())
                {
                    var categories = categoryDb.Categories
                        .ToList();

                    using (var productDb = new ProductContext())
                    {
                        var products = productDb.Products
                            .Where(p => p.ProductStatus == ProductStatus.Available)
                            .ToList();

                        var result = new Dictionary<Category, int>();

                        foreach (var category in categories)
                        {
                            int count = products
                                .Count(p => p.CategoryId == category.Id);

                            result[category] = count;
                        }

                        return result;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return new Dictionary<Category, int>();
            }
        }

        internal List<ProductSummary> GetAvailableProductsByCategoryIdAction(int? categoryId)
        {
            try
            {
                using (var db = new ProductContext())
                {
                    var products = db.Products
                        .Where(p => p.CategoryId == categoryId && p.ProductStatus == ProductStatus.Available)
                        .ToList();

                    var productsList = new List<ProductSummary>();

                    using (var categoryDb = new CategoryContext())
                    {
                        var category = categoryDb.Categories
                            .FirstOrDefault(c => c.Id == categoryId);

                        if (category != null)
                        {
                            foreach (var product in products)
                            {
                                var productSummary = new ProductSummary
                                {
                                    Id = product.Id,
                                    ProductCategory = category.CategoryName,
                                    ProductName = product.ProductName,
                                    ProductDescription = product.ProductDescription,
                                    ProductPrice = product.ProductPrice,
                                    ProductImageUrl = product.ProductImageUrl,
                                    ProductPostDate = product.ProductPostDate,
                                    ProductRegion = product.ProductRegion
                                };

                                productsList.Add(productSummary);
                            }
                        }
                    }

                    return productsList;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return new List<ProductSummary>();
            }
        }

        internal ReviewResp CreateReviewAction(ReviewData data, int userId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(data.Review))
                {
                    return new ReviewResp
                    {
                        Status = false,
                        StatusMsg = "Please enter a review before submitting!"
                    };
                }

                if (data.Rating <= 0)
                {
                    return new ReviewResp
                    {
                        Status = false,
                        StatusMsg = "Please select a rating for the product before submitting your review!"
                    };
                }

                using (var db = new ReviewContext())
                {
                    var review = new ReviewDbTable
                    {
                        UserId = userId,
                        ProductId = data.ProductId,
                        ReviewPostDate = DateTime.Now,
                        Review = data.Review,
                        Rating = data.Rating
                    };

                    db.Reviews.Add(review);
                    db.SaveChanges();
                }

                return new ReviewResp
                {
                    Status = true,
                    StatusMsg = "Your review has been successfully created!"
                };
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return new ReviewResp
                {
                    Status = false,
                    StatusMsg = "An error occurred while submitting your review!"
                };
            }
        }

        internal List<ReviewData> GetReviewsByProductIdAction(int productId)
        {
            try
            {
                using (var db = new ReviewContext())
                {
                    var allReviews = db.Reviews
                        .Where(r => r.ProductId == productId)
                        .ToList();

                    var config = new MapperConfiguration(cfg =>
                    {
                        cfg.CreateMap<ReviewDbTable, ReviewData>();
                    });

                    var mapper = config.CreateMapper();
                    var reviews = mapper.Map<List<ReviewData>>(allReviews);

                    return reviews;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return new List<ReviewData>();
            }
        }

        internal ReviewResp DeleteReviewAction(int reviewId)
        {
            try
            {
                using (var db = new ReviewContext())
                {
                    var review = db.Reviews
                        .FirstOrDefault(r => r.Id == reviewId);

                    if (review == null)
                    {
                        return new ReviewResp
                        {
                            Status = false,
                            StatusMsg = "Hmm... we couldn't find the review you were trying to delete!"
                        };
                    }

                    db.Reviews.Remove(review);
                    db.SaveChanges();

                    return new ReviewResp
                    {
                        Status = true,
                        StatusMsg = "Your review has been successfully deleted!"
                    };
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return new ReviewResp
                {
                    Status = false,
                    StatusMsg = "Oops! Something went wrong while trying to delete the review!"
                };
            }
        }

        internal ReviewData GetReviewByIdAction(int? reviewId)
        {
            try
            {
                using (var db = new ReviewContext())
                {
                    var reviewData = db.Reviews
                        .FirstOrDefault(r => r.Id == reviewId);

                    if (reviewData == null)
                    {
                        return null;
                    }

                    var config = new MapperConfiguration(cfg =>
                    {
                        cfg.CreateMap<ReviewDbTable, ReviewData>();
                    });

                    var mapper = config.CreateMapper();
                    var review = mapper.Map<ReviewData>(reviewData);

                    return review;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return null;
            }
        }

        internal ReviewResp UpdateReviewAction(ReviewData data)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(data.Review))
                {
                    return new ReviewResp
                    {
                        Status = false,
                        StatusMsg = "Please enter a review before submitting!"
                    };
                }

                if (data.Rating <= 0)
                {
                    return new ReviewResp
                    {
                        Status = false,
                        StatusMsg = "Please select a rating for the product before submitting your review!"
                    };
                }

                using (var db = new ReviewContext())
                {
                    var reviewData = db.Reviews
                        .FirstOrDefault(r => r.Id == data.Id);

                    if (reviewData == null)
                    {
                        return new ReviewResp
                        {
                            Status = false,
                            StatusMsg = "Hmm... we couldn't find the review you were trying to update!"
                        };
                    }

                    reviewData.Review = data.Review;
                    reviewData.Rating = data.Rating;
                    reviewData.ReviewPostDate = DateTime.Now;

                    db.Entry(reviewData).State = EntityState.Modified;
                    db.SaveChanges();

                    return new ReviewResp
                    {
                        Status = true,
                        StatusMsg = "Your review has been successfully updated!"
                    };
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return new ReviewResp
                {
                    Status = false,
                    StatusMsg = "An error occurred while updating your review!"
                };
            }
        }

        internal void UpdateProductRatingAction(int productId)
        {
            try
            {
                using (var db = new ReviewContext())
                {
                    var ratings = db.Reviews
                        .Where(r => r.ProductId == productId)
                        .Select(r => r.Rating)
                        .ToList();

                    double average = ratings.Any() ? ratings.Average() : 5;
                    int rating = (int)Math.Ceiling(average);

                    using (var productDb = new ProductContext())
                    {
                        var product = productDb.Products
                            .FirstOrDefault(p => p.Id == productId);

                        if (product != null)
                        {
                            product.ProductRating = rating;
                            productDb.Entry(product).State = EntityState.Modified;
                            productDb.SaveChanges();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        internal UserSummary GetUserByIdAction(int userId)
        {
            try
            {
                using (var db = new UserContext())
                {
                    var userData = db.Users
                        .FirstOrDefault(u => u.Id == userId);

                    if (userData == null) return null;

                    var config = new MapperConfiguration(cfg =>
                    {
                        cfg.CreateMap<UDbTable, UserSummary>();
                    });

                    var mapper = config.CreateMapper();
                    return mapper.Map<UserSummary>(userData);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return null;
            }
        }

        internal List<ProductSummary> SortProductsAction(string sortOption, List<ProductSummary> products)
        {
            try
            {
                switch (sortOption)
                {
                    case "lowToHigh":
                        return products
                            .OrderBy(p => p.ProductPrice)
                            .ToList();

                    case "highToLow":
                        return products
                            .OrderByDescending(p => p.ProductPrice)
                            .ToList();

                    case "oldest":
                        return products
                            .OrderBy(p => p.ProductPostDate)
                            .ToList();

                    case "newest":
                        return products
                            .OrderByDescending(p => p.ProductPostDate)
                            .ToList();

                    default:
                        return products;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return products ?? new List<ProductSummary>();
            }
        }

        internal List<ProductSummary> GetProductsByMaxPriceAction(int maxPrice, List<ProductSummary> products)
        {
            try
            {
                var filteredProducts = products
                    .Where(p => p.ProductPrice <= maxPrice)
                    .ToList();

                return filteredProducts;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return new List<ProductSummary>();
            }
        }

        internal List<ProductSummary> GetProductsBySearchQueryAction(string searchQuery, List<ProductSummary> products)
        {
            try
            {
                if (string.IsNullOrEmpty(searchQuery))
                {
                    return products;
                }

                var searchWords = searchQuery
                    .Split(new[] { ' ', ',', '.', ';', '!', '?' }, StringSplitOptions.RemoveEmptyEntries);

                var filteredProducts = products
                    .Where(p => searchWords.Any(word =>
                        (!string.IsNullOrEmpty(p.ProductName) && p.ProductName.IndexOf(word, StringComparison.OrdinalIgnoreCase) >= 0) ||
                        (!string.IsNullOrEmpty(p.ProductDescription) && p.ProductDescription.IndexOf(word, StringComparison.OrdinalIgnoreCase) >= 0)
                    ))
                    .ToList();

                return filteredProducts;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return new List<ProductSummary>();
            }
        }

        internal List<ProductSummary> GetProductsByCountryAction(string country, List<ProductSummary> products)
        {
            try
            {
                if (string.IsNullOrEmpty(country))
                {
                    return products;
                }

                var filteredProducts = products
                    .Where(p => string.Equals(p.ProductRegion, country, StringComparison.OrdinalIgnoreCase))
                    .ToList();

                return filteredProducts;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return new List<ProductSummary>();
            }
        }
    }
}
