using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using eUseControl.BusinessLogic.DBModel;
using eUseControl.Domain.Entities;
using eUseControl.Domain.Entities.Cart;
using eUseControl.Domain.Entities.Contact;
using eUseControl.Domain.Entities.Order;
using eUseControl.Domain.Entities.Product;
using eUseControl.Domain.Entities.Profile;
using eUseControl.Domain.Entities.Review;
using eUseControl.Domain.Entities.User;
using eUseControl.Domain.Enums;

namespace eUseControl.BusinessLogic.Core
{
    public class AdminApi
    {
        internal async Task<List<UserLite>> GetAllUsersAction()
        {
            try
            {
                List<UDbTable> users;
                List<ProfileDbTable> profiles;
                List<OrderDbTable> orders;

                using (var userDb = new UserContext())
                {
                    users = await userDb.Users
                        .ToListAsync();
                }

                using (var profileDb = new ProfileContext())
                {
                    profiles = await profileDb.UserProfiles
                        .ToListAsync();
                }

                using (var orderDb = new OrderContext())
                {
                    orders = await orderDb.CustomerOrders
                        .Where(c => c.OrderStatus == OrderStatus.Pending || c.OrderStatus == OrderStatus.Delivering)
                        .ToListAsync();
                }

                var usersList = new List<UserLite>();

                foreach (var user in users)
                {
                    var profile = profiles
                        .FirstOrDefault(p => p.UserId == user.Id);

                    var userOrders = orders
                        .Where(c => c.UserId == user.Id);

                    var client = new UserLite
                    {
                        Id = user.Id,
                        Username = user.Username,
                        Email = user.Email,
                        ProfileImageUrl = profile?.ProfileImageUrl,
                        PhoneNumber = profile?.PhoneNumber,
                        Address = profile?.Address,
                        OrderCount = userOrders.Count(),
                        TotalSpent = userOrders.Sum(o => o.TotalPrice)
                    };

                    usersList.Add(client);
                }

                return usersList;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return new List<UserLite>();
            }
        }

        internal async Task<CouponResp> AddDiscountCouponAction(CouponData couponData)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(couponData.Code))
                {
                    return new CouponResp
                    {
                        Status = false,
                        StatusMsg = "Coupon code must not be empty!"
                    };
                }

                if (couponData.DiscountPercent < 1 || couponData.DiscountPercent > 100)
                {
                    return new CouponResp
                    {
                        Status = false,
                        StatusMsg = "Discount percent must be between 1 and 100!"
                    };
                }

                if (couponData.ExpirationDate <= DateTime.Now)
                {
                    return new CouponResp
                    {
                        Status = false,
                        StatusMsg = "Expiration date must be in the future!"
                    };
                }

                using (var db = new CouponContext())
                {
                    var existing = await db.DiscountCoupons
                        .FirstOrDefaultAsync(c => c.Code == couponData.Code);

                    if (existing != null)
                    {
                        return new CouponResp
                        {
                            Status = false,
                            StatusMsg = "A coupon with this code already exists!"
                        };
                    }

                    var newCoupon = new CouponDbTable
                    {
                        Code = couponData.Code,
                        DiscountPercent = couponData.DiscountPercent,
                        ExpirationDate = couponData.ExpirationDate,
                        IsActive = couponData.IsActive
                    };

                    db.DiscountCoupons.Add(newCoupon);
                    await db.SaveChangesAsync();

                    return new CouponResp
                    {
                        Status = true,
                        StatusMsg = "Coupon successfully added!"
                    };
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return new CouponResp
                {
                    Status = false,
                    StatusMsg = "An error occurred while adding the coupon!"
                };
            }
        }

        internal async Task<List<CategoryData>> GetAllCategoriesAction()
        {
            try
            {
                using (var db = new CategoryContext())
                {
                    return await db.ProductCategories
                        .Select(c => new CategoryData
                        {
                            Id = c.Id,
                            CategoryName = c.CategoryName
                        })
                        .ToListAsync();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return new List<CategoryData>();
            }
        }

        internal async Task<CategoryResp> RemoveCategoryAction(int categoryId)
        {
            try
            {
                using (var db = new CategoryContext())
                {
                    var currentCategory = await db.ProductCategories
                        .FirstOrDefaultAsync(c => c.Id == categoryId);

                    if (currentCategory != null)
                    {
                        db.ProductCategories.Remove(currentCategory);
                        await db.SaveChangesAsync();

                        return new CategoryResp
                        {
                            Status = true,
                            StatusMsg = "Category successfully deleted!"
                        };
                    }
                    else
                    {
                        return new CategoryResp
                        {
                            Status = false,
                            StatusMsg = "Category not found!"
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return new CategoryResp
                {
                    Status = false,
                    StatusMsg = "An error occured while deleting the category!"
                };
            }
        }

        internal async Task<CategoryResp> CreateCategoryAction(string categoryName)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(categoryName))
                {
                    return new CategoryResp
                    {
                        Status = false,
                        StatusMsg = "Category name cannot be empty!"
                    };
                }

                using (var db = new CategoryContext())
                {
                    var existingCategory = await db.ProductCategories
                        .FirstOrDefaultAsync(c => c.CategoryName.ToLower() == categoryName.ToLower());

                    if (existingCategory != null)
                    {
                        return new CategoryResp
                        {
                            Status = false,
                            StatusMsg = "A category with this name already exists!"
                        };
                    }

                    var newCategory = new CategoryDbTable
                    {
                        CategoryName = categoryName
                    };

                    db.ProductCategories.Add(newCategory);
                    await db.SaveChangesAsync();

                    return new CategoryResp
                    {
                        Status = true,
                        StatusMsg = "Category added successfully!"
                    };
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return new CategoryResp
                {
                    Status = false,
                    StatusMsg = "An error occurred while adding the category!"
                };
            }
        }

        internal async Task<List<CouponData>> GetAllDiscountCouponsAction()
        {
            try
            {
                using (var db = new CouponContext())
                {
                    return await db.DiscountCoupons
                        .Select(discount => new CouponData
                        {
                            Id = discount.Id,
                            ExpirationDate = discount.ExpirationDate,
                            Code = discount.Code,
                            IsActive = discount.IsActive,
                            DiscountPercent = discount.DiscountPercent
                        })
                        .ToListAsync();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return new List<CouponData>();
            }
        }

        internal async Task<CouponResp> RemoveDiscountCouponAction(int couponId)
        {
            try
            {
                using (var db = new CouponContext())
                {
                    var existingDiscountCoupon = await db.DiscountCoupons
                        .FirstOrDefaultAsync(c => c.Id == couponId);

                    if (existingDiscountCoupon != null)
                    {
                        db.DiscountCoupons.Remove(existingDiscountCoupon);
                        await db.SaveChangesAsync();

                        return new CouponResp
                        {
                            Status = true,
                            StatusMsg = "Discount coupon successfully deleted!"
                        };
                    }
                    else
                    {
                        return new CouponResp
                        {
                            Status = false,
                            StatusMsg = "Discount coupon not found!"
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return new CouponResp
                {
                    Status = false,
                    StatusMsg = "An error occurred while deleting the discount coupon!"
                };
            }
        }

        internal async Task<List<ReviewSummary>> RetrieveAllReviewsAction()
        {
            try
            {
                List<ProfileDbTable> profiles;
                List<ReviewDbTable> reviews;

                using (var profilesDb = new ProfileContext())
                {
                    profiles = await profilesDb.UserProfiles
                        .ToListAsync();
                }

                using (var reviewsDb = new ReviewContext())
                {
                    reviews = await reviewsDb.ProductReviews
                        .ToListAsync();
                }

                var reviewsList = new List<ReviewSummary>();

                foreach (var review in reviews)
                {
                    var profile = profiles
                        .FirstOrDefault(p => p.UserId == review.UserId);

                    var item = new ReviewSummary
                    {
                        Id = review.Id,
                        ProfileImageUrl = profile?.ProfileImageUrl,
                        FirstName = profile?.FirstName,
                        LastName = profile?.LastName,
                        Review = review.Review,
                        ReviewPostDate = review.ReviewPostDate,
                        Rating = review.Rating
                    };

                    reviewsList.Add(item);
                }

                return reviewsList;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return new List<ReviewSummary>();
            }
        }

        internal async Task<ReviewResp> RemoveReviewAction(int reviewId)
        {
            try
            {
                using (var db = new ReviewContext())
                {
                    var existingReview = await db.ProductReviews
                        .FirstOrDefaultAsync(r => r.Id == reviewId);

                    if (existingReview != null)
                    {
                        db.ProductReviews.Remove(existingReview);
                        await db.SaveChangesAsync();

                        return new ReviewResp
                        {
                            Status = true,
                            StatusMsg = "Review successfully deleted!"
                        };
                    }
                    else
                    {
                        return new ReviewResp
                        {
                            Status = false,
                            StatusMsg = "Review not found!"
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return new ReviewResp
                {
                    Status = false,
                    StatusMsg = "An error occured while deleting the review!"
                };
            }
        }

        internal async Task<List<ProductLite>> RetrieveAllProductsAction()
        {
            try
            {
                List<CategoryDbTable> categories;

                using (var categoriesDb = new CategoryContext())
                {
                    categories = await categoriesDb.ProductCategories
                        .ToListAsync();
                }

                using (var db = new ProductContext())
                {
                    var products = await db.Products
                        .ToListAsync();

                    var allProducts = new List<ProductLite>();

                    foreach (var product in products)
                    {
                        var item = new ProductLite
                        {
                            Id = product.Id,
                            ProductImageUrl = product.ProductImageUrl,
                            ProductName = product.ProductName,
                            ProductPrice = product.ProductPrice,
                            ProductQuantity = product.ProductQuantity,
                            RecommendationStatus = product.RecommendationStatus,
                        };

                        var category = categories
                            .FirstOrDefault(c => c.Id == product.CategoryId);

                        item.ProductCategory = category?.CategoryName ?? "Unknown";
                        allProducts.Add(item);
                    }

                    return allProducts;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return new List<ProductLite>();
            }
        }

        internal async Task<ProductResp> RemoveProductAction(int productId)
        {
            try
            {
                using (var db = new ProductContext())
                {
                    var existingProduct = await db.Products
                        .FirstOrDefaultAsync(p => p.Id == productId);

                    if (existingProduct != null)
                    {
                        db.Products.Remove(existingProduct);
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

        internal async Task<ProductResp> ChangeRecommendationStatusAction(int productId)
        {
            try
            {
                using (var db = new ProductContext())
                {
                    var product = await db.Products
                        .FirstOrDefaultAsync(p => p.Id == productId);

                    if (product == null)
                    {
                        return new ProductResp
                        {
                            Status = false,
                            StatusMsg = "Product not found!"
                        };
                    }

                    product.RecommendationStatus = product.RecommendationStatus == RecommendationStatus.Preferred ? RecommendationStatus.Ignored : RecommendationStatus.Preferred;

                    db.Entry(product).State = EntityState.Modified;
                    await db.SaveChangesAsync();

                    return new ProductResp
                    {
                        Status = true,
                        StatusMsg = "Recommendation status updated successfully!"
                    };
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return new ProductResp
                {
                    Status = false,
                    StatusMsg = "An error occurred while updating the recommendation status!"
                };
            }
        }

        internal async Task<List<OrderLite>> RetrieveAllOrdersAction()
        {
            try
            {
                using (var db = new OrderContext())
                {
                    return await db.CustomerOrders
                        .Select(order => new OrderLite
                        {
                            Id = order.Id,
                            OrderDate = order.OrderDate,
                            OrderStatus = order.OrderStatus,
                            DeliveryAddress = order.DeliveryAddress,
                            PaymentMethod = order.PaymentMethod,
                            PhoneNumber = order.PhoneNumber,
                            TotalPrice = order.TotalPrice,
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

        internal async Task<OrderResp> RemoveOrderAction(int orderId)
        {
            try
            {
                using (var db = new OrderContext())
                {
                    var existingOrder = await db.CustomerOrders
                        .FirstOrDefaultAsync(o => o.Id == orderId);

                    if (existingOrder != null)
                    {
                        db.CustomerOrders.Remove(existingOrder);
                        await db.SaveChangesAsync();

                        return new OrderResp
                        {
                            Status = true,
                            StatusMsg = "Order successfully deleted!"
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
                    StatusMsg = "An error occurred while deleting the order!"
                };
            }
        }

        internal async Task<OrderResp> ChangeOrderStatusAction(int orderId)
        {
            try
            {
                using (var db = new OrderContext())
                {
                    var existingOrder = await db.CustomerOrders
                        .FirstOrDefaultAsync(o => o.Id == orderId);

                    if (existingOrder == null)
                    {
                        return new OrderResp
                        {
                            Status = false,
                            StatusMsg = "Order not found!"
                        };
                    }

                    if (existingOrder.OrderStatus == OrderStatus.Cancelled)
                    {
                        return new OrderResp
                        {
                            Status = false,
                            StatusMsg = "Cannot change status of a cancelled order!"
                        };
                    }

                    existingOrder.OrderStatus = existingOrder.OrderStatus == OrderStatus.Pending ? OrderStatus.Delivering : OrderStatus.Pending;

                    db.Entry(existingOrder).State = EntityState.Modified;
                    await db.SaveChangesAsync();

                    return new OrderResp
                    {
                        Status = true,
                        StatusMsg = "Order status updated successfully!"
                    };
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return new OrderResp
                {
                    Status = false,
                    StatusMsg = "An error occurred while updating the order status!"
                };
            }
        }

        internal async Task<List<ContactSummary>> RetrieveAllRequestsAction()
        {
            try
            {
                using (var db = new ContactContext())
                {
                    return await db.ContactRequests
                        .Select(request => new ContactSummary
                        {
                            Id = request.Id,
                            Username = request.Username,
                            Email = request.Email,
                            Message = request.Message,
                            RequestPostDate = request.RequestPostDate,
                            RequestStatus = request.RequestStatus
                        })
                        .ToListAsync();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return new List<ContactSummary>();
            }
        }

        internal async Task<ContactResp> RemoveRequestAction(int requestId)
        {
            try
            {
                using (var db = new ContactContext())
                {
                    var existingContact = await db.ContactRequests
                        .FirstOrDefaultAsync(r => r.Id == requestId);

                    if (existingContact != null)
                    {
                        db.ContactRequests.Remove(existingContact);
                        await db.SaveChangesAsync();

                        return new ContactResp
                        {
                            Status = true,
                            StatusMsg = "Contact request successfully deleted!"
                        };
                    }
                    else
                    {
                        return new ContactResp
                        {
                            Status = false,
                            StatusMsg = "Contact request not found!"
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return new ContactResp
                {
                    Status = false,
                    StatusMsg = "An error occurred while deleting the contact request!"
                };
            }
        }

        internal async Task<ContactResp> ChangeRequestStatusAction(int requestId)
        {
            try
            {
                using (var db = new ContactContext())
                {
                    var existingRequest = await db.ContactRequests
                        .FirstOrDefaultAsync(r => r.Id == requestId);

                    if (existingRequest == null)
                    {
                        return new ContactResp
                        {
                            Status = false,
                            StatusMsg = "Contact request not found!"
                        };
                    }

                    existingRequest.RequestStatus = existingRequest.RequestStatus == RequestStatus.Pending ? RequestStatus.Resolved : RequestStatus.Pending;

                    db.Entry(existingRequest).State = EntityState.Modified;
                    await db.SaveChangesAsync();

                    return new ContactResp
                    {
                        Status = true,
                        StatusMsg = "Contact request status updated successfully!"
                    };
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return new ContactResp
                {
                    Status = false,
                    StatusMsg = "An error occurred while updating the contact request status!"
                };
            }
        }
    }
}