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
using eUseControl.Domain.Entities.Wishlist;
using eUseControl.Domain.Entities.Cart;
using eUseControl.Domain.Entities.Order;
using eUseControl.Domain.Entities.Payment;

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

                if (productData.ProductQuantity < 0)
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
                            Id = product.Id,
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

                if (productData.ProductQuantity < 0)
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
                                    ProductRegion = product.ProductRegion,
                                    ProductQuantity = product.ProductQuantity
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

        internal ProductResp UpdateProductStatusAction(int productId)
        {
            try
            {
                using (var db = new ProductContext())
                {
                    var product = db.Products
                        .FirstOrDefault(p => p.Id == productId);

                    if (product != null)
                    {
                        product.ProductStatus = product.ProductStatus == ProductStatus.Available
                            ? ProductStatus.Unavailable
                            : ProductStatus.Available;

                        db.Entry(product).State = EntityState.Modified;
                        db.SaveChanges();

                        return new ProductResp
                        {
                            Status = true,
                            StatusMsg = "Product status updated successfully!"
                        };
                    }
                    else
                    {
                        return new ProductResp
                        {
                            Status = false,
                            StatusMsg = "Product not found!"
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return new ProductResp
                {
                    Status = false,
                    StatusMsg = "An error occurred while updating the product status!"
                };
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
                                    ProductRegion = product.ProductRegion,
                                    ProductQuantity = product.ProductQuantity
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

        internal ProductResp UpdateProductRatingAction(int productId)
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

                            return new ProductResp
                            {
                                Status = true,
                                StatusMsg = "Product rating updated successfully!"
                            };
                        }
                        else
                        {
                            return new ProductResp
                            {
                                Status = false,
                                StatusMsg = "Product not found!"
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return new ProductResp
                {
                    Status = false,
                    StatusMsg = "An error occurred while updating the product rating!"
                };
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

        internal WishlistResp AddProductToWishlistAction(int userId, int productId)
        {
            try
            {
                using (var db = new WishlistContext())
                {
                    var existingProduct = db.WishlistProducts
                        .FirstOrDefault(w => w.UserId == userId && w.ProductId == productId);

                    if (existingProduct == null)
                    {
                        var wishlistItem = new WishlistDbTable
                        {
                            UserId = userId,
                            ProductId = productId,
                            AddedDate = DateTime.Now
                        };

                        db.WishlistProducts.Add(wishlistItem);
                        db.SaveChanges();

                        return new WishlistResp
                        {
                            Status = true,
                            StatusMsg = "Product added to wishlist!"
                        };
                    }
                    else
                    {
                        return new WishlistResp
                        {
                            Status = false,
                            StatusMsg = "Product is already in the wishlist!"
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return new WishlistResp
                {
                    Status = false,
                    StatusMsg = "An error occurred while adding the product to the wishlist!"
                };
            }
        }

        internal List<ProductLite> GetAllWishlistProductsAction(int userId)
        {
            try
            {
                using (var db = new WishlistContext())
                {
                    var productIds = db.WishlistProducts
                        .Where(w => w.UserId == userId)
                        .Select(w => w.ProductId)
                        .ToList();

                    if (!productIds.Any()) return new List<ProductLite>();

                    using (var productsDb = new ProductContext())
                    {
                        var products = productsDb.Products
                            .Where(p => productIds.Contains(p.Id))
                            .ToList();

                        var config = new MapperConfiguration(cfg =>
                        {
                            cfg.CreateMap<ProductDbTable, ProductLite>();
                        });

                        var mapper = config.CreateMapper();
                        return mapper.Map<List<ProductLite>>(products);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return new List<ProductLite>();
            }
        }

        internal List<int> GetWishlistProductIdsAction(int userId)
        {
            try
            {
                using (var db = new WishlistContext())
                {
                    var productIds = db.WishlistProducts
                        .Where(w => w.UserId == userId)
                        .Select(w => w.ProductId)
                        .ToList();

                    return productIds;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return new List<int>();
            }
        }

        internal int GetWishlistCountByUserIdAction(int userId)
        {
            try
            {
                using (var db = new WishlistContext())
                {
                    return db.WishlistProducts
                             .Count(w => w.UserId == userId);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return 0;
            }
        }

        internal WishlistResp RemoveProductFromWishlistAction(int productId, int userId)
        {
            try
            {
                using (var db = new WishlistContext())
                {
                    var wishlistItem = db.WishlistProducts
                        .FirstOrDefault(w => w.UserId == userId && w.ProductId == productId);

                    if (wishlistItem != null)
                    {
                        db.WishlistProducts.Remove(wishlistItem);
                        db.SaveChanges();

                        return new WishlistResp
                        {
                            Status = true,
                            StatusMsg = "Product removed from wishlist!"
                        };
                    }
                    else
                    {
                        return new WishlistResp
                        {
                            Status = false,
                            StatusMsg = "Product not found in wishlist!"
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return new WishlistResp
                {
                    Status = false,
                    StatusMsg = "An error occurred while removing the product from the wishlist!"
                };
            }
        }

        internal CartResp AddItemToCartAction(int productId, int userId)
        {
            try
            {
                using (var db = new CartContext())
                {
                    var existingItem = db.CartItems
                        .FirstOrDefault(c => c.ProductId == productId && c.UserId == userId);

                    decimal itemPrice = 0;
                    using (var productsDb = new ProductContext())
                    {
                        var product = productsDb.Products
                            .FirstOrDefault(p => p.Id == productId);

                        if (product == null)
                        {
                            return new CartResp
                            {
                                Status = false,
                                StatusMsg = "The requested product was not found!"
                            };
                        }

                        itemPrice = product.ProductPrice;
                    }

                    if (existingItem == null)
                    {
                        var cartItem = new CartDbTable
                        {
                            UserId = userId,
                            ProductId = productId,
                            SelectedQuantity = 1,
                            Subtotal = itemPrice,
                            AddedDate = DateTime.Now
                        };

                        db.CartItems.Add(cartItem);
                        db.SaveChanges();

                        return new CartResp
                        {
                            Status = true,
                            StatusMsg = "Product successfully added to the cart!"
                        };
                    }
                    else
                    {
                        return new CartResp
                        {
                            Status = false,
                            StatusMsg = "Product is already in the cart!"
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return new CartResp
                {
                    Status = false,
                    StatusMsg = "An error occurred while adding the product to the cart!"
                };
            }
        }

        internal List<CartData> GetCartItemsByUserIdAction(int userId)
        {
            var cartDataList = new List<CartData>();

            try
            {
                using (var db = new CartContext())
                {
                    var allCartItems = db.CartItems
                        .Where(c => c.UserId == userId)
                        .ToList();

                    using (var productsDb = new ProductContext())
                    {
                        foreach (var cartItem in allCartItems)
                        {
                            var product = productsDb.Products
                                    .FirstOrDefault(p => p.Id == cartItem.ProductId);

                            if (product != null)
                            {
                                var cartData = new CartData
                                {
                                    ProductId = cartItem.ProductId,
                                    ProductName = product.ProductName,
                                    ProductImageUrl = product.ProductImageUrl,
                                    ProductPrice = product.ProductPrice,
                                    ProductQuantity = product.ProductQuantity,
                                    SelectedQuantity = cartItem.SelectedQuantity,
                                    Subtotal = cartItem.Subtotal
                                };

                                cartDataList.Add(cartData);
                            }
                        }
                    }

                    return cartDataList;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return new List<CartData>();
            }
        }

        internal CartResp RemoveItemFromCartAction(int productId, int userId)
        {
            try
            {
                using (var db = new CartContext())
                {
                    var cartItem = db.CartItems
                        .FirstOrDefault(p => p.ProductId == productId && p.UserId == userId);

                    if (cartItem != null)
                    {
                        db.CartItems.Remove(cartItem);
                        db.SaveChanges();

                        return new CartResp
                        {
                            Status = true,
                            StatusMsg = "The item has been successfully removed from the cart!"
                        };
                    }
                    else
                    {
                        return new CartResp
                        {
                            Status = false,
                            StatusMsg = "This item is not in your cart!"
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return new CartResp
                {
                    Status = false,
                    StatusMsg = "An error occurred while removing the item from the cart!"
                };
            }
        }

        internal int GetCartCountByUserIdAction(int userId)
        {
            try
            {
                using (var db = new CartContext())
                {
                    return db.CartItems
                        .Count(c => c.UserId == userId);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return 0;
            }
        }

        internal CartResp ChangeProductQuantityAction(int productId, int userId, int newQuantity)
        {
            try
            {
                decimal currentProductPrice;
                using (var productsDb = new ProductContext())
                {
                    var product = productsDb.Products
                        .FirstOrDefault(p => p.Id == productId);

                    if (product == null)
                    {
                        return new CartResp
                        {
                            Status = false,
                            StatusMsg = "The requested product was not found!"
                        };
                    }

                    currentProductPrice = product.ProductPrice;
                }

                using (var db = new CartContext())
                {
                    var cartItem = db.CartItems
                        .FirstOrDefault(c => c.ProductId == productId && c.UserId == userId);

                    if (cartItem != null)
                    {
                        cartItem.SelectedQuantity = newQuantity;
                        cartItem.Subtotal = newQuantity * currentProductPrice;

                        db.Entry(cartItem).State = EntityState.Modified;
                        db.SaveChanges();

                        return new CartResp
                        {
                            Status = true,
                            StatusMsg = "The quantity has been successfully updated!"
                        };
                    }
                    else
                    {
                        return new CartResp
                        {
                            Status = false,
                            StatusMsg = "The requested item is not in your cart!"
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return new CartResp
                {
                    Status = false,
                    StatusMsg = "An error occurred while updating the quantity!"
                };
            }
        }

        internal (decimal totalPrice, decimal shippingCost) CalculateCartTotalAction(List<CartData> cartItems)
        {
            try
            {
                decimal totalPrice = 0;
                int totalQuantity = 0;

                foreach (var item in cartItems)
                {
                    totalPrice += item.Subtotal;
                    totalQuantity += item.SelectedQuantity;
                }

                decimal costPerItem = 1m;
                decimal maxShipping = 8m;

                decimal shippingCost = totalQuantity * costPerItem;
                shippingCost = shippingCost > maxShipping ? maxShipping : shippingCost;

                return (totalPrice, shippingCost);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return (0m, 0m);
            }
        }

        internal decimal ApplyCouponDiscountAction(decimal totalPrice, string couponCode)
        {
            try
            {
                using (var db = new CouponContext())
                {
                    var coupon = db.DiscountCoupons
                        .FirstOrDefault(c => c.Code == couponCode);

                    if (coupon != null && coupon.IsActive && coupon.ExpirationDate >= DateTime.Now)
                    {
                        decimal discountRate = coupon.DiscountPercent * 0.01m;
                        decimal discountAmount = totalPrice * discountRate;
                        decimal newPrice = totalPrice - discountAmount;

                        return newPrice;
                    }

                    return totalPrice;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return totalPrice;
            }
        }

        internal CartResp ClearCartItemsAfterOrderAction(int userId)
        {
            try
            {
                using (var db = new CartContext())
                {
                    var userCartItems = db.CartItems
                        .Where(c => c.UserId == userId)
                        .ToList();

                    if (userCartItems.Any())
                    {
                        db.CartItems.RemoveRange(userCartItems);
                        db.SaveChanges();
                    }

                    return new CartResp
                    {
                        Status = true,
                        StatusMsg = "Cart items cleared successfully!"
                    };
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return new CartResp
                {
                    Status = false,
                    StatusMsg = "Failed to clear cart items!"
                };
            }
        }

        internal decimal ComputeOrderTotalAction(decimal finalPrice, decimal shippingCost)
        {
            try
            {
                return finalPrice + shippingCost;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return -1; 
            }
        }

        internal decimal ComputeDiscountAmountAction(decimal initialPrice, decimal finalPrice) 
        {
            try
            {
                return initialPrice - finalPrice;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return -1;
            }
        }

        internal OrderResp PlaceOrderAction(OrderData orderData, int userId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(orderData.FirstName) ||
                    string.IsNullOrWhiteSpace(orderData.LastName) ||
                    string.IsNullOrWhiteSpace(orderData.DeliveryAddress) ||
                    string.IsNullOrWhiteSpace(orderData.PhoneNumber) ||
                    string.IsNullOrWhiteSpace(orderData.Email) ||
                    string.IsNullOrWhiteSpace(orderData.PaymentMethod))
                {
                    return new OrderResp
                    {
                        Status = false,
                        StatusMsg = "Please complete all required fields!"
                    };
                }

                if (!new EmailAddressAttribute().IsValid(orderData.Email))
                {
                    return new OrderResp
                    {
                        Status = false,
                        StatusMsg = "Please enter a valid email address!"
                    };
                }

                if (!Regex.IsMatch(orderData.PhoneNumber, @"^\+?\d{7,15}$"))
                {
                    return new OrderResp
                    {
                        Status = false,
                        StatusMsg = "Please enter a valid phone number!"
                    };
                }

                Coupon coupon = null;
                if (!string.IsNullOrWhiteSpace(orderData.CouponCode))
                {
                    using (var couponsDb = new CouponContext())
                    {
                        coupon = couponsDb.DiscountCoupons
                            .FirstOrDefault(c => c.Code ==  orderData.CouponCode);
                    };
                }

                using (var db = new OrderContext())
                {
                    var newOrder = new OrderDbTable
                    {
                        UserId = userId,
                        FirstName = orderData.FirstName,
                        LastName = orderData.LastName,
                        DeliveryAddress = orderData.DeliveryAddress,
                        PhoneNumber = orderData.PhoneNumber,
                        Email = orderData.Email,
                        Notes = orderData.Notes,
                        PaymentMethod = orderData.PaymentMethod,
                        OrderDate = DateTime.Now,
                        OrderStatus = OrderStatus.Pending,
                        TotalPrice = orderData.TotalPrice
                    };

                    if (coupon != null && coupon.IsActive && coupon.ExpirationDate >= DateTime.Now)
                    {
                        newOrder.CouponId = coupon.Id;
                    }

                    db.CustomerOrders.Add(newOrder);
                    db.SaveChanges();

                    return new OrderResp
                    {
                        Status = true,
                        StatusMsg = "Your order has been placed successfully!",
                        Id = newOrder.Id
                    };
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return new OrderResp
                {
                    Status = false,
                    StatusMsg = "An error occurred while placing your order!"
                };
            }
        }

        internal OrderResp CancelUnpaidOrdersAction(int userId)
        {
            try
            {
                using (var db = new OrderContext())
                {
                    var orders = db.CustomerOrders
                        .Where(c => c.UserId == userId && c.OrderStatus == OrderStatus.Pending && c.PaymentMethod == "Card")
                        .ToList();

                    using (var transactionsDb = new TransactionContext())
                    {
                        foreach (var order in orders)
                        {
                            bool hasTransaction = transactionsDb.UserTransactions
                                .Any(t => t.OrderId == order.Id);

                            if (!hasTransaction)
                            {
                                order.OrderStatus = OrderStatus.Cancelled;
                            }
                        }
                    }

                    db.SaveChanges();

                    return new OrderResp
                    {
                        Status = true,
                        StatusMsg = "Unpaid orders have been cancelled successfully!"
                    };
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return new OrderResp
                {
                    Status = false,
                    StatusMsg = "An error occurred while cancelling unpaid orders!"
                };
            }
        }

        internal OrderMinimal GetOrderByIdAction(int orderId)
        {
            try
            {
                using (var db = new OrderContext())
                {
                    var orderData = db.CustomerOrders
                        .FirstOrDefault(c => c.Id == orderId);

                    if (orderData != null)
                    {
                        var order = new OrderMinimal
                        {
                            Id = orderData.Id,
                            TotalPrice = orderData.TotalPrice,
                            OrderDate = orderData.OrderDate
                        };

                        return order;
                    }

                    return null;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return null;
            }
        }

        internal bool IsValidCard(string cardNumber)
        {
            if (string.IsNullOrWhiteSpace(cardNumber))
            {
                return false;
            }

            string cleaned = cardNumber.Replace(" ", "");
            return cleaned.All(char.IsDigit) && cleaned.Length >= 14 && cleaned.Length <= 16;
        }

        internal bool IsValidExpiryDate(string expiryDate)
        {
            var parts = expiryDate.Split('/');

            if (parts.Length != 2)
            {
                return false;
            }

            string yearPart = parts[0];
            string monthPart = parts[1];

            if (yearPart.Length != 2 || monthPart.Length != 2 || !yearPart.All(char.IsDigit) || !monthPart.All(char.IsDigit))
            {
                return false;
            }
                
            int year = Convert.ToInt32(yearPart);
            int month = Convert.ToInt32(monthPart);

            if (month < 1 || month > 12 || year < 0 || year > 99)
            {
                return false;
            }
                
            int fullYear = 2000 + year;
            var lastDay = new DateTime(fullYear, month, DateTime.DaysInMonth(fullYear, month));

            return lastDay >= DateTime.Now.Date;
        }

        internal bool IsValidCVV(string cvv)
        {
            return !string.IsNullOrWhiteSpace(cvv) && cvv.All(char.IsDigit) && (cvv.Length == 3);
        }

        internal bool IsValidFullName(string fullName)
        {
            return !string.IsNullOrWhiteSpace(fullName) && fullName.Length <= 70;
        }

        internal TransactionResp MakePayment(decimal totalPrice)
        {
            try
            {
                bool success = totalPrice <= 1000;

                if (!success)
                {
                    return new TransactionResp
                    {
                        Status = false,
                        StatusMsg = "Transaction failed: insufficient funds!"
                    };
                }

                return new TransactionResp
                {
                    Status = true,
                    StatusMsg = "Transaction successful!"
                };
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return new TransactionResp
                {
                    Status = false,
                    StatusMsg = "An unexpected error occurred during the payment process!"
                };
            }
        }

        internal TransactionResp ProcessPaymentAction(TransactionData transactionData, int userId)
        {
            try
            {
                if (!IsValidCard(transactionData.CardNumber))
                {
                    return new TransactionResp
                    {
                        Status = false,
                        StatusMsg = "The card information you entered appears to be invalid!"
                    };
                }

                if (!IsValidExpiryDate(transactionData.ExpiryDate))
                {
                    return new TransactionResp
                    {
                        Status = false,
                        StatusMsg = "The expiration date you entered is invalid!"
                    };
                }

                if (!IsValidCVV(transactionData.Cvv))
                {
                    return new TransactionResp
                    {
                        Status = false,
                        StatusMsg = "The CVV code entered is invalid!"
                    };
                }

                if (!IsValidFullName(transactionData.FullName))
                {
                    return new TransactionResp
                    {
                        Status = false,
                        StatusMsg = "The full name you entered is invalid!"
                    };
                }

                OrderDbTable order = null;
                using (var orderDb = new OrderContext())
                {
                    order = orderDb.CustomerOrders
                        .FirstOrDefault(c => c.Id == transactionData.OrderId);
                }

                if (order.TotalPrice <= 0)
                {
                    return new TransactionResp
                    {
                        Status = false,
                        StatusMsg = "The amount entered is invalid!"
                    };
                }

                var paymentResult = MakePayment(order.TotalPrice);

                if (paymentResult.Status)
                {
                    using (var db = new TransactionContext())
                    {
                        var transaction = new TransactionDbTable
                        {
                            OrderId = transactionData.OrderId,
                            UserId = userId,
                            Amount = order.TotalPrice,
                            TransactionDate = DateTime.Now,
                            TransactionStatus = TransactionStatus.Successful
                        };

                        db.UserTransactions.Add(transaction);
                        db.SaveChanges();

                        return new TransactionResp
                        {
                            Status = true,
                            StatusMsg = "Payment was successfully completed!"
                        };
                    }
                }
                else
                {
                    return new TransactionResp
                    {
                        Status = false,
                        StatusMsg = "Your payment could not be processed!"
                    };
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return new TransactionResp
                {
                    Status = false,
                    StatusMsg = "An unexpected error occurred while processing your payment!"
                };
            }
        }
    }
}
