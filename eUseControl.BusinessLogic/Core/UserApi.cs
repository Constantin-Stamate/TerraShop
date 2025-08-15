using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using eUseControl.BusinessLogic.DBModel;
using eUseControl.Domain.Entities;
using eUseControl.Domain.Entities.Cart;
using eUseControl.Domain.Entities.Chat;
using eUseControl.Domain.Entities.Contact;
using eUseControl.Domain.Entities.Order;
using eUseControl.Domain.Entities.Payment;
using eUseControl.Domain.Entities.Product;
using eUseControl.Domain.Entities.Profile;
using eUseControl.Domain.Entities.Review;
using eUseControl.Domain.Entities.Subscription;
using eUseControl.Domain.Entities.User;
using eUseControl.Domain.Entities.Wishlist;
using eUseControl.Domain.Enums;
using eUseControl.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace eUseControl.BusinessLogic.Core
{
    public class UserApi
    {
        internal async Task<URegisterResp> UserRegisterAction(URegisterData data)
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
                    if (await db.Users
                        .AnyAsync(u => u.Email == data.Email))
                    {
                        return new URegisterResp
                        {
                            Status = false,
                            StatusMsg = "Email has already been used!"
                        };
                    }

                    if (await db.Users
                        .AnyAsync(u => u.Username == data.Username))
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
                    await db.SaveChangesAsync();

                    return new URegisterResp
                    {
                        Status = true,
                        StatusMsg = "You have successfully registered!"
                    };
                }
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

        internal async Task<ULoginResp> UserLoginAction(ULoginData data)
        {
            try
            {
                var isEmail = new EmailAddressAttribute().IsValid(data.Username);
                var hashedPass = LoginHelper.HashGen(data.Password);

                using (var db = new UserContext())
                {
                    var user = await db.Users
                        .FirstOrDefaultAsync(u => (isEmail ? u.Email == data.Username : u.Username == data.Username) && u.Password == hashedPass);

                    if (user == null)
                    {
                        return new ULoginResp
                        {
                            Status = false,
                            StatusMsg = "The username or password is incorrect!"
                        };
                    }

                    user.LastIp = data.LastIp;
                    user.LastLogin = data.LastLogin;

                    db.Entry(user).State = EntityState.Modified;
                    await db.SaveChangesAsync();

                    return new ULoginResp
                    {
                        Status = true,
                        UserMinimal = new UserMinimal
                        {
                            Id = user.Id,
                            Username = user.Username,
                            Email = user.Email,
                            LastLogin = user.LastLogin ?? DateTime.Now,
                            LastIp = user.LastIp,
                            Level = user.Level ?? URole.User
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

        internal async Task<HttpCookie> Cookie(string loginCredential)
        {
            try
            {
                var cookieValue = CookieGenerator.Create(loginCredential);
                var apiCookie = new HttpCookie("X-KEY")
                {
                    Value = cookieValue
                };

                using (var usersDb = new UserContext())
                {
                    var user = await usersDb.Users
                        .FirstOrDefaultAsync(u => u.Username == loginCredential);

                    if (user == null)
                    {
                        System.Diagnostics.Debug.WriteLine("Current user not found!");
                        return null;
                    }

                    using (var sessionDb = new SessionContext())
                    {
                        var current = await sessionDb.UserSessions
                            .FirstOrDefaultAsync(e => e.UserId == user.Id);

                        if (current != null)
                        {
                            current.CookieString = apiCookie.Value;
                            current.ExpireTime = DateTime.Now.AddMinutes(60);

                            sessionDb.Entry(current).State = EntityState.Modified;
                            await sessionDb.SaveChangesAsync();
                        }
                        else
                        {
                            sessionDb.UserSessions.Add(new SessionDbTable
                            {
                                UserId = user.Id,
                                CookieString = apiCookie.Value,
                                ExpireTime = DateTime.Now.AddMinutes(60)
                            });

                            await sessionDb.SaveChangesAsync();
                        }
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
                using (var sessionDb = new SessionContext())
                {
                    var session = sessionDb.UserSessions
                        .FirstOrDefault(s => s.CookieString == cookie && s.ExpireTime > DateTime.Now);

                    if (session == null)
                    {
                        return null;
                    }

                    using (var userDb = new UserContext())
                    {
                        var curentUser = userDb.Users
                            .FirstOrDefault(u => u.Id == session.UserId);

                        if (curentUser == null)
                        {
                            return null;
                        }

                        var user = new UserMinimal
                        {
                            Id = curentUser.Id,
                            Username = curentUser.Username,
                            Email = curentUser.Email,
                            LastIp = curentUser.LastIp,
                            LastLogin = curentUser.LastLogin ?? DateTime.Now,
                            Level = curentUser.Level ?? URole.User
                        };

                        return user;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return null;
            }
        }

        internal async Task<ProductResp> CreateProductAction(ProductData productData, int userId)
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

                using (var db = new CategoryContext())
                {
                    var category = await db.ProductCategories
                        .FirstOrDefaultAsync(c => c.CategoryName == productData.ProductCategory);

                    if (category == null)
                    {
                        return new ProductResp
                        {
                            Status = false,
                            StatusMsg = "Invalid category!"
                        };
                    }

                    using (var productDb = new ProductContext())
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
                            RecommendationStatus = RecommendationStatus.Preferred,
                            ProductRating = 5
                        };

                        productDb.Products.Add(product);
                        await productDb.SaveChangesAsync();

                        return new ProductResp
                        {
                            Status = true,
                            StatusMsg = "The product has been successfully created!"
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
                    StatusMsg = "An error occurred while saving the product!"
                };
            }
        }

        internal async Task<List<ProductMinimal>> GetProductsByUserIdAction(int userId)
        {
            try
            {
                using (var db = new ProductContext())
                {
                    return await db.Products
                        .Where(p => p.UserId == userId)
                        .Select(p => new ProductMinimal
                        {
                            Id = p.Id,
                            ProductName = p.ProductName,
                            ProductDescription = p.ProductDescription,
                            ProductPrice = p.ProductPrice,
                            ProductImageUrl = p.ProductImageUrl,
                            ProductStatus = p.ProductStatus
                        })
                        .ToListAsync();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return new List<ProductMinimal>();
            }
        }

        internal async Task<ProductData> GetProductByIdAction(int productId)
        {
            try
            {
                using (var db = new ProductContext())
                {
                    var product = await db.Products
                        .Where(p => p.Id == productId)
                        .FirstOrDefaultAsync();

                    if (product == null)
                    {
                        return null;
                    }

                    using (var categoryDb = new CategoryContext())
                    {
                        var categoryName = await categoryDb.ProductCategories
                            .Where(c => c.Id == product.CategoryId)
                            .Select(c => c.CategoryName)
                            .FirstOrDefaultAsync();

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

        internal async Task<ProductResp> UpdateProductAction(ProductData productData)
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

                using (var db = new CategoryContext())
                {
                    var category = await db.ProductCategories
                        .FirstOrDefaultAsync(c => c.CategoryName == productData.ProductCategory);

                    if (category == null)
                    {
                        return new ProductResp
                        {
                            Status = false,
                            StatusMsg = "Invalid category!"
                        };
                    }

                    using (var productDb = new ProductContext())
                    {
                        var product = await productDb.Products
                            .FirstOrDefaultAsync(p => p.Id == productData.Id);

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
                        product.ProductDescription = productData.ProductDescription;
                        product.CategoryId = category.Id;
                        if (!string.IsNullOrEmpty(productData.ProductImageUrl))
                        {
                            product.ProductImageUrl = productData.ProductImageUrl;
                        }

                        productDb.Entry(product).State = EntityState.Modified;
                        await productDb.SaveChangesAsync();

                        return new ProductResp
                        {
                            Status = true,
                            StatusMsg = "Product updated successfully!"
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
                    StatusMsg = "An error occurred while updating the product!"
                };
            }
        }

        internal async Task<ProfileData> GetProfileByUserIdAction(int userId)
        {
            try
            {
                using (var userDb = new UserContext())
                {
                    var user = await userDb.Users
                        .FirstOrDefaultAsync(u => u.Id == userId);

                    if (user == null)
                    {
                        return null;
                    }

                    using (var profileDb = new ProfileContext())
                    {
                        var userProfile = await profileDb.UserProfiles
                            .FirstOrDefaultAsync(p => p.UserId == userId);

                        if (userProfile == null)
                        {
                            userProfile = new ProfileDbTable
                            {
                                UserId = userId,
                                FirstName = "User",
                                LastName = "User",
                                Email = user.Email,
                                Address = "N/A",
                                PhoneNumber = "000-000-0000",
                                LastProfileUpdate = DateTime.Now,
                                ProfileImageUrl = "/Assets/img/user.jpg"
                            };

                            profileDb.UserProfiles.Add(userProfile);
                            await profileDb.SaveChangesAsync();
                        }

                        var profile = new ProfileData
                        {
                            Id = userProfile.Id,
                            UserId = userProfile.UserId,
                            FirstName = userProfile.FirstName,
                            LastName = userProfile.LastName,
                            Email = userProfile.Email,
                            Address = userProfile.Address,
                            PhoneNumber = userProfile.PhoneNumber,
                            ProfileImageUrl = userProfile.ProfileImageUrl
                        };

                        return profile;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return null;
            }
        }

        internal async Task<ProfileResp> UpdateProfileAction(ProfileData profileData)
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
                    var existingEmailUser = await userDb.Users
                        .FirstOrDefaultAsync(u => u.Email == profileData.Email && u.Id != profileData.UserId);

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
                        var userProfile = await profileDb.UserProfiles
                            .FirstOrDefaultAsync(p => p.UserId == profileData.UserId);

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
                        userProfile.LastProfileUpdate = DateTime.Now;
                        if (!string.IsNullOrEmpty(profileData.ProfileImageUrl))
                        {
                            userProfile.ProfileImageUrl = profileData.ProfileImageUrl;
                        }

                        profileDb.Entry(userProfile).State = EntityState.Modified;
                        await profileDb.SaveChangesAsync();

                        var user = await userDb.Users
                            .FirstOrDefaultAsync(u => u.Id == profileData.UserId);

                        if (user != null)
                        {
                            user.Email = profileData.Email;

                            userDb.Entry(user).State = EntityState.Modified;
                            await userDb.SaveChangesAsync();
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

        internal async Task<ProfileResp> ChangePasswordAction(string currentPassword, string newPassword, int userId)
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
                    var user = await db.Users
                        .FirstOrDefaultAsync(u => u.Id == userId && u.Password == hashedCurrent);

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
                    await db.SaveChangesAsync();

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

        internal async Task<List<ProductSummary>> GetAvailableProductsAction()
        {
            try
            {
                using (var productDb = new ProductContext())
                {
                    var products = await productDb.Products
                        .Where(p => p.ProductStatus == ProductStatus.Available)
                        .ToListAsync();

                    using (var categoryDb = new CategoryContext())
                    {
                        var productsList = new List<ProductSummary>();

                        foreach (var product in products)
                        {
                            var category = await categoryDb.ProductCategories
                                .FirstOrDefaultAsync(c => c.Id == product.CategoryId);

                            if (category != null)
                            {
                                productsList.Add(new ProductSummary
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
                                });
                            }
                        }

                        return productsList;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return new List<ProductSummary>();
            }
        }

        internal async Task<ProductResp> UpdateProductStatusAction(int productId)
        {
            try
            {
                using (var db = new ProductContext())
                {
                    var product = await db.Products
                        .FirstOrDefaultAsync(p => p.Id == productId);

                    if (product != null)
                    {
                        product.ProductStatus = product.ProductStatus == ProductStatus.Available ? ProductStatus.Unavailable : ProductStatus.Available;

                        db.Entry(product).State = EntityState.Modified;
                        await db.SaveChangesAsync();

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

        internal Dictionary<CategoryData, int> GetCategoryProductCountsAction()
        {
            try
            {
                using (var categoryDb = new CategoryContext())
                {
                    var categories = categoryDb.ProductCategories
                        .ToList();

                    using (var productDb = new ProductContext())
                    {
                        var products = productDb.Products
                            .Where(p => p.ProductStatus == ProductStatus.Available)
                            .ToList();

                        var result = new Dictionary<CategoryData, int>();

                        foreach (var category in categories)
                        {
                            var productCategory = new CategoryData
                            {
                                Id = category.Id,
                                CategoryName = category.CategoryName
                            };

                            int count = products
                                .Count(p => p.CategoryId == category.Id);

                            result[productCategory] = count;
                        }

                        return result;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return new Dictionary<CategoryData, int>();
            }
        }

        internal async Task<List<ProductSummary>> GetAvailableProductsByCategoryIdAction(int? categoryId)
        {
            try
            {
                using (var db = new ProductContext())
                {
                    var products = await db.Products
                        .Where(p => p.CategoryId == categoryId && p.ProductStatus == ProductStatus.Available)
                        .ToListAsync();

                    var productsList = new List<ProductSummary>();

                    using (var categoryDb = new CategoryContext())
                    {
                        var category = await categoryDb.ProductCategories
                            .FirstOrDefaultAsync(c => c.Id == categoryId);

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

        internal async Task<ReviewResp> CreateReviewAction(ReviewData data, int userId)
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

                    db.ProductReviews.Add(review);
                    await db.SaveChangesAsync();
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

        internal async Task<List<ReviewData>> GetReviewsByProductIdAction(int productId)
        {
            try
            {
                using (var db = new ReviewContext())
                {
                    return await db.ProductReviews
                        .Where(r => r.ProductId == productId)
                        .Select(r => new ReviewData
                        {
                            Id = r.Id,
                            UserId = r.UserId,
                            ProductId = r.ProductId,
                            ReviewPostDate = r.ReviewPostDate,
                            Review = r.Review,
                            Rating = r.Rating
                        })
                        .ToListAsync();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return new List<ReviewData>();
            }
        }

        internal async Task<ReviewResp> DeleteReviewAction(int reviewId)
        {
            try
            {
                using (var db = new ReviewContext())
                {
                    var review = await db.ProductReviews
                        .FirstOrDefaultAsync(r => r.Id == reviewId);

                    if (review == null)
                    {
                        return new ReviewResp
                        {
                            Status = false,
                            StatusMsg = "Hmm... we couldn't find the review you were trying to delete!"
                        };
                    }

                    db.ProductReviews.Remove(review);
                    await db.SaveChangesAsync();

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

        internal async Task<ReviewData> GetReviewByIdAction(int? reviewId)
        {
            try
            {
                using (var db = new ReviewContext())
                {
                    return await db.ProductReviews
                        .Where(r => r.Id == reviewId)
                        .Select(r => new ReviewData
                        {
                            Id = r.Id,
                            UserId = r.UserId,
                            ProductId = r.ProductId,
                            ReviewPostDate = r.ReviewPostDate,
                            Review = r.Review,
                            Rating = r.Rating
                        })
                        .FirstOrDefaultAsync();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return null;
            }
        }

        internal async Task<ReviewResp> UpdateReviewAction(ReviewData data)
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
                    var reviewData = await db.ProductReviews
                        .FirstOrDefaultAsync(r => r.Id == data.Id);

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
                    await db.SaveChangesAsync();

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

        internal async Task<ProductResp> UpdateProductRatingAction(int productId)
        {
            try
            {
                using (var db = new ReviewContext())
                {
                    var ratings = await db.ProductReviews
                        .Where(r => r.ProductId == productId)
                        .Select(r => r.Rating)
                        .ToListAsync();

                    double average = ratings.Any() ? ratings.Average() : 5;
                    int rating = (int)Math.Ceiling(average);

                    using (var productDb = new ProductContext())
                    {
                        var product = await productDb.Products
                            .FirstOrDefaultAsync(p => p.Id == productId);

                        if (product != null)
                        {
                            product.ProductRating = rating;

                            productDb.Entry(product).State = EntityState.Modified;
                            await productDb.SaveChangesAsync();

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

        internal async Task<UserSummary> GetUserByIdAction(int userId)
        {
            try
            {
                using (var db = new UserContext())
                {
                    return await db.Users
                        .Where(u => u.Id == userId)
                        .Select(u => new UserSummary
                        {
                            Id = u.Id,
                            Username = u.Username,
                            Email = u.Email
                        })
                        .FirstOrDefaultAsync();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return null;
            }
        }

        internal async Task<List<ProductSummary>> SortProductsAction(string sortOption, List<ProductSummary> products)
        {
            try
            {
                switch (sortOption)
                {
                    case "lowToHigh":
                        return await Task.FromResult(products
                            .OrderBy(p => p.ProductPrice)
                            .ToList());

                    case "highToLow":
                        return await Task.FromResult(products
                            .OrderByDescending(p => p.ProductPrice)
                            .ToList());

                    case "oldest":
                        return await Task.FromResult(products
                            .OrderBy(p => p.ProductPostDate)
                            .ToList());

                    case "newest":
                        return await Task.FromResult(products
                            .OrderByDescending(p => p.ProductPostDate)
                            .ToList());

                    default:
                        return await Task.FromResult(products);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return await Task.FromResult(products ?? new List<ProductSummary>());
            }
        }

        internal async Task<List<ProductSummary>> GetProductsByMaxPriceAction(int maxPrice, List<ProductSummary> products)
        {
            try
            {
                var filteredProducts = products
                    .Where(p => p.ProductPrice <= maxPrice)
                    .ToList();

                return await Task.FromResult(filteredProducts);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return await Task.FromResult(new List<ProductSummary>());
            }
        }

        internal async Task<List<ProductSummary>> GetProductsBySearchQueryAction(string searchQuery, List<ProductSummary> products)
        {
            try
            {
                if (string.IsNullOrEmpty(searchQuery))
                {
                    return await Task.FromResult(products);
                }

                var searchWords = searchQuery
                    .Split(new[] { ' ', ',', '.', ';', '!', '?' }, StringSplitOptions.RemoveEmptyEntries);

                var filteredProducts = products
                    .Where(p => searchWords.Any(word =>
                        (!string.IsNullOrEmpty(p.ProductName) && p.ProductName.IndexOf(word, StringComparison.OrdinalIgnoreCase) >= 0) ||
                        (!string.IsNullOrEmpty(p.ProductDescription) && p.ProductDescription.IndexOf(word, StringComparison.OrdinalIgnoreCase) >= 0)
                    ))
                    .ToList();

                return await Task.FromResult(filteredProducts);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return await Task.FromResult(new List<ProductSummary>());
            }
        }

        internal async Task<List<ProductSummary>> GetProductsByCountryAction(string country, List<ProductSummary> products)
        {
            try
            {
                if (string.IsNullOrEmpty(country))
                {
                    return await Task.FromResult(products);
                }

                var filteredProducts = products
                    .Where(p => string.Equals(p.ProductRegion, country, StringComparison.OrdinalIgnoreCase))
                    .ToList();

                return await Task.FromResult(filteredProducts);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return await Task.FromResult(new List<ProductSummary>());
            }
        }

        internal async Task<WishlistResp> AddProductToWishlistAction(int userId, int productId)
        {
            try
            {
                using (var db = new WishlistContext())
                {
                    var existingProduct = await db.WishlistProducts
                        .FirstOrDefaultAsync(w => w.UserId == userId && w.ProductId == productId);

                    if (existingProduct == null)
                    {
                        var wishlistItem = new WishlistDbTable
                        {
                            UserId = userId,
                            ProductId = productId,
                            AddedDate = DateTime.Now
                        };

                        db.WishlistProducts.Add(wishlistItem);
                        await db.SaveChangesAsync();

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

        internal async Task<List<ProductLite>> GetAllWishlistProductsAction(int userId)
        {
            try
            {
                using (var db = new WishlistContext())
                {
                    var productIds = await db.WishlistProducts
                        .Where(w => w.UserId == userId)
                        .Select(w => w.ProductId)
                        .ToListAsync();

                    if (!productIds.Any())
                    {
                        return new List<ProductLite>();
                    }

                    using (var productsDb = new ProductContext())
                    {
                        var products = await productsDb.Products
                            .Where(p => productIds.Contains(p.Id))
                            .Select(p => new ProductLite
                            {
                                Id = p.Id,
                                ProductName = p.ProductName,
                                ProductPrice = p.ProductPrice,
                                ProductImageUrl = p.ProductImageUrl,
                                ProductQuantity = p.ProductQuantity,
                                ProductQuality = p.ProductQuality,
                            })
                            .ToListAsync();

                        return products;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return new List<ProductLite>();
            }
        }

        internal async Task<List<int>> GetWishlistProductIdsAction(int userId)
        {
            try
            {
                using (var db = new WishlistContext())
                {
                    return await db.WishlistProducts
                        .Where(w => w.UserId == userId)
                        .Select(w => w.ProductId)
                        .ToListAsync();
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

        internal async Task<WishlistResp> RemoveProductFromWishlistAction(int productId, int userId)
        {
            try
            {
                using (var db = new WishlistContext())
                {
                    var wishlistItem = await db.WishlistProducts
                        .FirstOrDefaultAsync(w => w.UserId == userId && w.ProductId == productId);

                    if (wishlistItem == null)
                    {
                        return new WishlistResp
                        {
                            Status = false,
                            StatusMsg = "Product not found in wishlist!"
                        };
                    }

                    db.WishlistProducts.Remove(wishlistItem);
                    await db.SaveChangesAsync();

                    return new WishlistResp
                    {
                        Status = true,
                        StatusMsg = "Product removed from wishlist!"
                    };
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

        internal async Task<CartResp> AddItemToCartAction(int productId, int userId)
        {
            try
            {
                using (var db = new CartContext())
                {
                    var existingItem = await db.CartItems
                        .FirstOrDefaultAsync(c => c.ProductId == productId && c.UserId == userId);

                    if (existingItem != null)
                    {
                        return new CartResp
                        {
                            Status = false,
                            StatusMsg = "Product is already in the cart!"
                        };
                    }

                    using (var productsDb = new ProductContext())
                    {
                        var product = await productsDb.Products
                            .FirstOrDefaultAsync(p => p.Id == productId);

                        if (product == null)
                        {
                            return new CartResp
                            {
                                Status = false,
                                StatusMsg = "The requested product was not found!"
                            };
                        }

                        var cartItem = new CartDbTable
                        {
                            UserId = userId,
                            ProductId = productId,
                            SelectedQuantity = 1,
                            Subtotal = product.ProductPrice,
                            AddedDate = DateTime.Now
                        };

                        db.CartItems.Add(cartItem);
                        await db.SaveChangesAsync();

                        return new CartResp
                        {
                            Status = true,
                            StatusMsg = "Product successfully added to the cart!"
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

        internal async Task<List<CartData>> GetCartItemsByUserIdAction(int userId)
        {
            try
            {
                var cartDataList = new List<CartData>();

                using (var db = new CartContext())
                {
                    var allCartItems = await db.CartItems
                        .Where(c => c.UserId == userId)
                        .ToListAsync();

                    if (allCartItems.Any())
                    {
                        var productIds = allCartItems
                            .Select(c => c.ProductId)
                            .Distinct()
                            .ToList();

                        using (var productsDb = new ProductContext())
                        {
                            var products = await productsDb.Products
                                .Where(p => productIds.Contains(p.Id))
                                .ToListAsync();

                            foreach (var cartItem in allCartItems)
                            {
                                var product = products
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
                    }
                }

                return cartDataList;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return new List<CartData>();
            }
        }

        internal async Task<CartResp> RemoveItemFromCartAction(int productId, int userId)
        {
            try
            {
                using (var db = new CartContext())
                {
                    var cartItem = await db.CartItems
                        .FirstOrDefaultAsync(p => p.ProductId == productId && p.UserId == userId);

                    if (cartItem != null)
                    {
                        db.CartItems.Remove(cartItem);
                        await db.SaveChangesAsync();

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

        internal async Task<CartResp> ChangeProductQuantityAction(int productId, int userId, int newQuantity)
        {
            try
            {
                using (var productsDb = new ProductContext())
                {
                    var product = await productsDb.Products
                        .FirstOrDefaultAsync(p => p.Id == productId);

                    if (product == null)
                    {
                        return new CartResp
                        {
                            Status = false,
                            StatusMsg = "The requested product was not found!"
                        };
                    }

                    using (var db = new CartContext())
                    {
                        var cartItem = await db.CartItems
                            .FirstOrDefaultAsync(c => c.ProductId == productId && c.UserId == userId);

                        if (cartItem != null)
                        {
                            cartItem.SelectedQuantity = newQuantity;
                            cartItem.Subtotal = newQuantity * product.ProductPrice;

                            db.Entry(cartItem).State = EntityState.Modified;
                            await db.SaveChangesAsync();

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

        internal async Task<decimal> ApplyCouponDiscountAction(decimal totalPrice, string couponCode)
        {
            try
            {
                using (var db = new CouponContext())
                {
                    var coupon = await db.DiscountCoupons
                        .FirstOrDefaultAsync(c => c.Code == couponCode);

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

        internal async Task<CartResp> ClearCartItemsAfterOrderAction(int userId)
        {
            try
            {
                using (var db = new CartContext())
                {
                    var userCartItems = await db.CartItems
                        .Where(c => c.UserId == userId)
                        .ToListAsync();

                    if (userCartItems.Any())
                    {
                        db.CartItems.RemoveRange(userCartItems);
                        await db.SaveChangesAsync();
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

        internal async Task<OrderResp> PlaceOrderAction(OrderData orderData, int userId)
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

                    if (!string.IsNullOrWhiteSpace(orderData.CouponCode))
                    {
                        using (var couponsDb = new CouponContext())
                        {
                            var coupon = await couponsDb.DiscountCoupons
                                .FirstOrDefaultAsync(c => c.Code == orderData.CouponCode);

                            if (coupon != null && coupon.IsActive && coupon.ExpirationDate >= DateTime.Now)
                            {
                                newOrder.CouponId = coupon.Id;
                            }
                        }
                    }

                    db.CustomerOrders.Add(newOrder);
                    await db.SaveChangesAsync();

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

        internal async Task<OrderResp> CancelUnpaidOrdersAction(int userId)
        {
            try
            {
                using (var db = new OrderContext())
                {
                    var orders = await db.CustomerOrders
                        .Where(c => c.UserId == userId && c.OrderStatus == OrderStatus.Pending && c.PaymentMethod == "Card")
                        .ToListAsync();

                    using (var transactionsDb = new TransactionContext())
                    {
                        foreach (var order in orders)
                        {
                            bool hasTransaction = await transactionsDb.UserTransactions
                                .AnyAsync(t => t.OrderId == order.Id);

                            if (!hasTransaction)
                            {
                                order.OrderStatus = OrderStatus.Cancelled;
                                db.Entry(order).State = EntityState.Modified;
                            }
                        }
                    }

                    await db.SaveChangesAsync();

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

        internal async Task<OrderMinimal> GetOrderByIdAction(int orderId)
        {
            try
            {
                using (var db = new OrderContext())
                {
                    return await db.CustomerOrders
                        .Where(c => c.Id == orderId)
                        .Select(c => new OrderMinimal
                        {
                            Id = c.Id,
                            TotalPrice = c.TotalPrice,
                            OrderDate = c.OrderDate
                        })
                        .FirstOrDefaultAsync();
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

        internal async Task<TransactionResp> ProcessPaymentAction(TransactionData transactionData, int userId)
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

                using (var orderDb = new OrderContext())
                {
                    var order = await orderDb.CustomerOrders
                        .FirstOrDefaultAsync(c => c.Id == transactionData.OrderId);

                    if (order == null || order.TotalPrice <= 0)
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
                            await db.SaveChangesAsync();

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

        internal async Task<ProductResp> UpdateProductQuantitiesAfterOrderAction(List<CartData> cartItems)
        {
            try
            {
                using (var db = new ProductContext())
                {
                    foreach (var item in cartItems)
                    {
                        var product = await db.Products
                            .FirstOrDefaultAsync(p => p.Id == item.ProductId);

                        if (product != null)
                        {
                            product.ProductQuantity -= item.SelectedQuantity;

                            if (product.ProductQuantity < 0)
                            {
                                product.ProductQuantity = 0;
                            }

                            db.Entry(product).State = EntityState.Modified;
                        }
                    }

                    await db.SaveChangesAsync();

                    return new ProductResp
                    {
                        Status = true,
                        StatusMsg = "Product quantities updated successfully!"
                    };
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return new ProductResp
                {
                    Status = false,
                    StatusMsg = "An unexpected error occurred while updating product stock!"
                };
            }
        }

        internal async Task<Dictionary<ReviewData, ProfileData>> RetrieveAllReviewsAction()
        {
            try
            {
                using (var db = new ReviewContext())
                {
                    var allReviews = await db.ProductReviews
                        .ToListAsync();

                    var reviews = new Dictionary<ReviewData, ProfileData>();

                    foreach (var item in allReviews)
                    {
                        using (var profileDb = new ProfileContext())
                        {
                            var profileData = await profileDb.UserProfiles
                                .FirstOrDefaultAsync(p => p.UserId == item.UserId);

                            if (profileData != null)
                            {
                                var profile = new ProfileData
                                {
                                    FirstName = profileData.FirstName,
                                    LastName = profileData.LastName,
                                    ProfileImageUrl = profileData.ProfileImageUrl
                                };

                                var review = new ReviewData
                                {
                                    Review = item.Review,
                                    Rating = item.Rating,
                                    ReviewPostDate = item.ReviewPostDate,
                                };

                                reviews.Add(review, profile);
                            }
                        }
                    }

                    return reviews;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return new Dictionary<ReviewData, ProfileData>();
            }
        }

        internal async Task<ContactResp> SubmitContactRequestAction(ContactData contactData, int userId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(contactData.Username) ||
                    string.IsNullOrWhiteSpace(contactData.Email) ||
                    string.IsNullOrWhiteSpace(contactData.Message))
                {
                    return new ContactResp
                    {
                        Status = false,
                        StatusMsg = "Please complete all required fields!"
                    };
                }

                using (var db = new ContactContext())
                {
                    var contact = new ContactDbTable
                    {
                        UserId = userId,
                        Username = contactData.Username,
                        Email = contactData.Email,
                        Message = contactData.Message,
                        RequestStatus = RequestStatus.Pending,
                        RequestPostDate = DateTime.Now
                    };

                    db.ContactRequests.Add(contact);
                    await db.SaveChangesAsync();
                }

                return new ContactResp
                {
                    Status = true,
                    StatusMsg = "Contact request submitted successfully!"
                };
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return new ContactResp
                {
                    Status = false,
                    StatusMsg = "An unexpected error occurred while submitting the contact request!"
                };
            }
        }

        internal async Task<List<ProductSummary>> GetRecommendedProductsAction()
        {
            try
            {
                using (var db = new ProductContext())
                {
                    var allProducts = await db.Products
                        .Where(p => p.RecommendationStatus == RecommendationStatus.Preferred && p.ProductStatus == ProductStatus.Available)
                        .ToListAsync();

                    var recommendedProducts = new List<ProductSummary>();

                    using (var categoryDb = new CategoryContext())
                    {
                        foreach (var product in allProducts)
                        {
                            var category = await categoryDb.ProductCategories
                                .FirstOrDefaultAsync(c => c.Id == product.CategoryId);

                            var recommendedProduct = new ProductSummary
                            {
                                Id = product.Id,
                                ProductCategory = category?.CategoryName,
                                ProductDescription = product.ProductDescription,
                                ProductImageUrl = product.ProductImageUrl,
                                ProductName = product.ProductName,
                                ProductPostDate = product.ProductPostDate,
                                ProductPrice = product.ProductPrice,
                                ProductQuantity = product.ProductQuantity,
                                ProductRegion = product.ProductRegion
                            };

                            recommendedProducts.Add(recommendedProduct);
                        }
                    }

                    return recommendedProducts;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return new List<ProductSummary>();
            }
        }

        internal async Task<List<OrderLite>> GetValidOrdersAction(int userId)
        {
            try
            {
                using (var db = new OrderContext())
                {
                    return await db.CustomerOrders
                        .Where(c => c.UserId == userId && (c.OrderStatus == OrderStatus.Pending || c.OrderStatus == OrderStatus.Delivering))
                        .Select(order => new OrderLite
                        {
                            Id = order.Id,
                            TotalPrice = order.TotalPrice,
                            PaymentMethod = order.PaymentMethod,
                            OrderDate = order.OrderDate,
                            OrderStatus = order.OrderStatus,
                            OrderImageUrl = order.PaymentMethod == "Card" ? "~/Assets/img/order/order-icon-1.jpg" : "~/Assets/img/order/order-icon-2.jpg"
                        })
                        .ToListAsync();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return new List<OrderLite>();
            }
        }

        internal async Task<OrderResp> CancelOrderAction(int orderId)
        {
            try
            {
                using (var db = new OrderContext())
                {
                    var currentOrder = await db.CustomerOrders
                        .FirstOrDefaultAsync(c => c.Id == orderId);

                    if (currentOrder != null)
                    {
                        currentOrder.OrderStatus = OrderStatus.Cancelled;

                        db.Entry(currentOrder).State = EntityState.Modified;
                        await db.SaveChangesAsync();

                        return new OrderResp
                        {
                            Status = true,
                            StatusMsg = "Order cancelled successfully!"
                        };
                    }
                    else
                    {
                        return new OrderResp
                        {
                            Status = false,
                            StatusMsg = "Order not found!"
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return new OrderResp
                {
                    Status = false,
                    StatusMsg = "Oops, something went wrong while cancelling your order!"
                };
            }
        }

        internal async Task<ProductResp> RemoveProductAction(int productId)
        {
            try
            {
                using (var db = new ProductContext())
                {
                    var currentProduct = await db.Products
                        .FirstOrDefaultAsync(p => p.Id == productId);

                    if (currentProduct != null)
                    {
                        db.Products.Remove(currentProduct);
                        await db.SaveChangesAsync();

                        return new ProductResp
                        {
                            Status = true,
                            StatusMsg = "Product successfully deleted!"
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
                    StatusMsg = "An error occurred while deleting the product!"
                };
            }
        }

        internal async Task<ChatResp> GetResponseAction(string message, int userId)
        {
            const string _ollamaApiUrl = "http://localhost:11434/api/chat";
            const int maxWords = 500;

            if (string.IsNullOrWhiteSpace(message))
            {
                return new ChatResp
                {
                    Status = false,
                    StatusMsg = "Message cannot be empty!"
                };
            }

            var wordCount = message.Split(new char[] { ' ', '\t', '\n' }, StringSplitOptions.RemoveEmptyEntries).Length;
            if (wordCount > maxWords)
            {
                return new ChatResp
                {
                    Status = false,
                    StatusMsg = $"Message too long. Please limit your question to {maxWords} words!"
                };
            }

            using (var httpClient = new HttpClient())
            {
                string domainContext =
                    "You are an intelligent assistant designed specifically to support users of Terra Shop, an eco-friendly marketplace dedicated to sustainable products and environmental responsibility." +
                    "This advanced chatbot, named TerraAI, provides rapid and precise assistance to customers by answering a wide range of questions typical for an online marketplace." +
                    "You can help with product information such as detailed descriptions, stock availability, product comparisons, and personalized recommendations." +
                    "You assist with order management including order status, delivery modifications, cancellations, and returns. " +
                    "Additionally, you support payment and billing inquiries, explain policies on warranties, delivery, and taxes, and provide quick answers to frequently asked questions." +
                    "TerraAI continuously learns and adapts to improve the Terra Shop customer experience, offering efficient and friendly 24/7 support focused on sustainability goals." +
                    "If a user asks questions outside of Terra Shop’s scope or unrelated topics like sports, celebrities, or unrelated technologies, respond with:" +
                    "'I'm sorry, but I can only assist with topics related to Terra Shop and its eco-friendly marketplace.'" +
                    "Always remain concise, professional, and focused on supporting users within Terra Shop and its sustainability mission.";

                var requestObj = new
                {
                    model = "llama3.2",
                    messages = new[]
                    {
                        new {
                            role = "system",
                            content = domainContext
                        },

                        new {
                            role = "user",
                            content = message
                        }
                    },
                    stream = false
                };

                var jsonRequest = JsonConvert.SerializeObject(requestObj);
                var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

                try
                {
                    var response = await httpClient.PostAsync(_ollamaApiUrl, content);

                    if (!response.IsSuccessStatusCode)
                    {
                        return new ChatResp
                        {
                            Status = false,
                            StatusMsg = $"Error from AI model: {response.StatusCode}"
                        };
                    }

                    var responseString = await response.Content.ReadAsStringAsync();
                    var json = JObject.Parse(responseString);
                    var responseText = json["message"]?["content"]?.ToString();

                    if (string.IsNullOrWhiteSpace(responseText))
                    {
                        return new ChatResp
                        {
                            Status = false,
                            StatusMsg = "Unexpected JSON structure: " + responseString
                        };
                    }

                    var trimmedResponse = responseText.Trim();

                    using (var db = new ChatContext())
                    {
                        var item = new ChatDbTable
                        {
                            UserId = userId,
                            Prompt = message,
                            Message = trimmedResponse,
                            ResponseDate = DateTime.Now
                        };

                        db.ChatMessages.Add(item);
                        await db.SaveChangesAsync();

                        return new ChatResp
                        {
                            Status = true,
                            StatusMsg = trimmedResponse
                        };
                    }
                }
                catch (Exception ex)
                {
                    return new ChatResp
                    {
                        Status = false,
                        StatusMsg = $"Error calling AI model: {ex.Message}"
                    };
                }
            }
        }

        internal async Task<List<ChatData>> RetrieveUserChatsAction(int userId)
        {
            try
            {
                using (var db = new ChatContext())
                {
                    return await db.ChatMessages
                        .Where(c => c.UserId == userId)
                        .Select(chat => new ChatData
                        {
                            Prompt = chat.Prompt,
                            Message = chat.Message
                        })
                        .ToListAsync();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return new List<ChatData>();
            }
        }

        internal async Task<Dictionary<string, List<ProductSummary>>> GetProductsFromTopCategoriesAction()
        {
            try
            {
                using (var db = new ProductContext())
                {
                    var topCategoryIds = await db.Products
                        .GroupBy(p => p.CategoryId)
                        .Select(g => new
                        {
                            CategoryId = g.Key,
                            ProductCount = g.Count()
                        })
                        .OrderByDescending(g => g.ProductCount)
                        .Take(5)
                        .Select(g => g.CategoryId)
                        .ToListAsync();

                    var categoryNames = new Dictionary<int, string>();

                    using (var categoryDb = new CategoryContext())
                    {
                        categoryNames = await categoryDb.ProductCategories
                            .Where(c => topCategoryIds.Contains(c.Id))
                            .ToDictionaryAsync(c => c.Id, c => c.CategoryName);
                    }

                    var result = new Dictionary<string, List<ProductSummary>>();

                    foreach (var categoryId in topCategoryIds)
                    {
                        var categoryName = categoryNames[categoryId];

                        var productsInCategory = await db.Products
                            .Where(p => p.CategoryId == categoryId)
                            .Select(p => new ProductSummary
                            {
                                Id = p.Id,
                                ProductName = p.ProductName,
                                ProductDescription = p.ProductDescription,
                                ProductPrice = p.ProductPrice,
                                ProductImageUrl = p.ProductImageUrl,
                                ProductPostDate = p.ProductPostDate,
                                ProductRegion = p.ProductRegion,
                                ProductQuantity = p.ProductQuantity,
                            })
                            .ToListAsync();

                        result[categoryName] = productsInCategory;
                    }

                    return result;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return new Dictionary<string, List<ProductSummary>>();
            }
        }

        internal async Task<ChatResp> DeleteChatHistoryAction(int userId)
        {
            try
            {
                using (var db = new ChatContext())
                {
                    var messages = db.ChatMessages
                        .Where(c => c.UserId == userId);

                    if (!messages.Any())
                    {
                        return new ChatResp
                        {
                            Status = false,
                            StatusMsg = "No messages found for this user!"
                        };
                    }

                    db.ChatMessages.RemoveRange(messages);
                    await db.SaveChangesAsync();

                    return new ChatResp
                    {
                        Status = true,
                        StatusMsg = "User messages successfully deleted!"
                    };
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return new ChatResp
                {
                    Status = false,
                    StatusMsg = "An error occurred while deleting the user messages!"
                };
            }
        }

        internal async Task<List<string>> ExtractCategoriesAction()
        {
            try
            {
                using (var db = new CategoryContext())
                {
                    return await db.ProductCategories
                                   .Select(c => c.CategoryName)
                                   .ToListAsync();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return new List<string>();
            }
        }

        internal async Task<SubscriptionResp> CreateSubscriptionAction(string email)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(email))
                {
                    return new SubscriptionResp
                    {
                        Status = false,
                        StatusMsg = "Email cannot be empty!"
                    };
                }

                using (var db = new SubscriptionContext())
                {
                    var existingSubscriber = await db.Subscribers
                        .FirstOrDefaultAsync(s => s.Email == email);

                    if (existingSubscriber != null)
                    {
                        return new SubscriptionResp
                        {
                            Status = false,
                            StatusMsg = "Email is already subscribed!"
                        };
                    }

                    var subscription = new SubscriptionDbTable
                    {
                        Email = email,
                        SubscriptionDate = DateTime.Now
                    };

                    db.Subscribers.Add(subscription);
                    await db.SaveChangesAsync();

                    return new SubscriptionResp
                    {
                        Status = true,
                        StatusMsg = "Subscription created successfully!"
                    };
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return new SubscriptionResp
                {
                    Status = false,
                    StatusMsg = "An error occurred while creating subscription!"
                };
            }
        }
    }
}