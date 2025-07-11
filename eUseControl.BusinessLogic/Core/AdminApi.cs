using eUseControl.BusinessLogic.DBModel;
using eUseControl.Domain.Entities;
using eUseControl.Domain.Entities.Cart;
using eUseControl.Domain.Entities.Order;
using eUseControl.Domain.Entities.Product;
using eUseControl.Domain.Entities.Profile;
using eUseControl.Domain.Entities.Review;
using eUseControl.Domain.Entities.User;
using eUseControl.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace eUseControl.BusinessLogic.Core
{
    public class AdminApi
    {
        internal List<UserLite> GetAllUsersAction()
        {
            try
            {
                List<UDbTable> users;
                List<ProfileDbTable> profiles;
                List<OrderDbTable> orders;

                using (var userDb = new UserContext())
                {
                    users = userDb.Users
                        .ToList();
                }

                using (var profileDb = new ProfileContext())
                {
                    profiles = profileDb.UserProfiles
                        .ToList();
                }

                using (var orderDb = new OrderContext())
                {
                    orders = orderDb.CustomerOrders
                        .Where(c => c.OrderStatus == OrderStatus.Pending || c.OrderStatus == OrderStatus.Delivering)
                        .ToList();
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
                        ProfileImageUrl = profile.ProfileImageUrl,
                        PhoneNumber = profile.PhoneNumber,
                        Address = profile.Address,
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

        internal CouponResp AddDiscountCouponAction(CouponData couponData)
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
                    var existing = db.DiscountCoupons
                        .FirstOrDefault(c => c.Code == couponData.Code);

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
                    db.SaveChanges();

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

        internal List<CategoryData> GetAllCategoriesAction()
        {
            try
            {
                using (var db = new CategoryContext())
                {
                    var categories = db.ProductCategories
                        .ToList();

                    var allCategories = new List<CategoryData>();

                    foreach (var category in categories)
                    {
                        var newCategory = new CategoryData
                        {
                            Id = category.Id,
                            CategoryName = category.CategoryName
                        };

                        allCategories.Add(newCategory);
                    }

                    return allCategories;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return new List<CategoryData>();
            }
        }

        internal CategoryResp RemoveCategoryAction(int categoryId)
        {
            try
            {
                using (var db = new CategoryContext())
                {
                    var currentCategory = db.ProductCategories
                        .FirstOrDefault(c => c.Id == categoryId);

                    if (currentCategory != null)
                    {
                        db.ProductCategories.Remove(currentCategory);
                        db.SaveChanges();

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

        internal CategoryResp CreateCategoryAction(string categoryName)
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
                    var existingCategory = db.ProductCategories
                        .FirstOrDefault(c => c.CategoryName.ToLower() == categoryName.ToLower());

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
                    db.SaveChanges();

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

        internal List<CouponData> GetAllDiscountCouponsAction()
        {
            try
            {
                using (var db = new CouponContext())
                {
                    var discountCoupons = db.DiscountCoupons
                        .ToList();

                    var allDiscountCoupons = new List<CouponData>();

                    foreach (var discount in discountCoupons)
                    {
                        var newCoupon = new CouponData
                        {
                            Id = discount.Id,
                            ExpirationDate = discount.ExpirationDate,
                            Code = discount.Code,
                            IsActive = discount.IsActive,
                            DiscountPercent = discount.DiscountPercent
                        };

                        allDiscountCoupons.Add(newCoupon);
                    }

                    return allDiscountCoupons;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return new List<CouponData>();
            }
        }

        internal CouponResp RemoveDiscountCouponAction(int couponId)
        {
            try
            {
                using (var db = new CouponContext())
                {
                    var existingDiscountCoupon = db.DiscountCoupons
                        .FirstOrDefault(c => c.Id == couponId);

                    if (existingDiscountCoupon != null)
                    {
                        db.DiscountCoupons.Remove(existingDiscountCoupon);
                        db.SaveChanges();

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
                    StatusMsg = "An error occured while deleting the discount coupon!"
                };
            }
        }

        internal List<ReviewSummary> RetrieveAllReviewsAction()
        {
            try
            {
                List<ProfileDbTable> profiles;
                List<ReviewDbTable> reviews;

                using (var profilesDb = new ProfileContext())
                {
                    profiles = profilesDb.UserProfiles
                        .ToList();
                }

                using (var reviewsDb = new ReviewContext())
                {
                    reviews = reviewsDb.ProductReviews
                        .ToList();
                }

                var reviewsList = new List<ReviewSummary>();

                foreach (var review in reviews)
                {
                    var profile = profiles
                        .FirstOrDefault(p => p.UserId == review.UserId);

                    var item = new ReviewSummary
                    {
                        Id = review.Id,
                        ProfileImageUrl = profile.ProfileImageUrl,
                        FirstName = profile.FirstName,
                        LastName = profile.LastName,
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

        internal ReviewResp RemoveReviewAction(int reviewId)
        {
            try
            {
                using (var db = new ReviewContext())
                {
                    var existingReview = db.ProductReviews
                        .FirstOrDefault(r => r.Id == reviewId);


                    if (existingReview != null)
                    {
                        db.ProductReviews.Remove(existingReview);
                        db.SaveChanges();

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

        internal List<ProductLite> RetrieveAllProductsAction()
        {
            try
            {
                List<CategoryDbTable> categories;

                using (var categoriesDb = new CategoryContext())
                {
                    categories = categoriesDb.ProductCategories
                        .ToList();
                }

                using (var db = new ProductContext())
                {
                    var products = db.Products
                        .ToList();

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

                        item.ProductCategory = category.CategoryName;

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

        internal ProductResp RemoveProductAction(int productId)
        {
            try
            {
                using (var db = new ProductContext())
                {
                    var existingproduct = db.Products
                        .FirstOrDefault(p => p.Id == productId);

                    if (existingproduct != null)
                    {
                        db.Products.Remove(existingproduct);
                        db.SaveChanges();

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
                    StatusMsg = "An error occured while deleting the product!"
                };
            }
        }

        internal ProductResp ChangeRecommendationStatusAction(int productId)
        {
            try
            {
                using (var db = new ProductContext())
                {
                    var product = db.Products
                        .FirstOrDefault(p => p.Id == productId);

                    if (product != null)
                    {
                        product.RecommendationStatus = product.RecommendationStatus == RecommendationStatus.Preferred
                            ? RecommendationStatus.Ignored
                            : RecommendationStatus.Preferred;

                        db.Entry(product).State = EntityState.Modified;
                        db.SaveChanges();

                        return new ProductResp
                        {
                            Status = true,
                            StatusMsg = "Recommendation status updated successfully!"
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
                    StatusMsg = "An error occurred while updating the recommendation status!"
                };
            }
        }

        internal List<OrderLite> RetrieveAllOrdersAction()
        {
            try
            {
                using (var db = new OrderContext())
                {
                    var orders = db.CustomerOrders
                        .ToList();

                    var allOrders = new List<OrderLite>();

                    foreach (var order in orders)
                    {
                        var orderLite = new OrderLite
                        {
                            Id = order.Id,
                            OrderDate = order.OrderDate,
                            OrderStatus = order.OrderStatus,
                            DeliveryAddress = order.DeliveryAddress,
                            PaymentMethod = order.PaymentMethod,
                            PhoneNumber = order.PhoneNumber,
                            TotalPrice = order.TotalPrice,
                            OrderImageUrl = order.PaymentMethod == "Card" ? "~/Assets/img/order/order-icon-1.jpg" : "~/Assets/img/order/order-icon-2.jpg"
                        };

                        allOrders.Add(orderLite);
                    }

                    return allOrders;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return new List<OrderLite>();
            }
        }

        internal OrderResp RemoveOrderAction(int orderId)
        {
            try
            {
                using (var db = new OrderContext())
                {
                    var existingOrder = db.CustomerOrders
                        .FirstOrDefault(o => o.Id == orderId);

                    if (existingOrder != null)
                    {
                        db.CustomerOrders.Remove(existingOrder);
                        db.SaveChanges();

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
                    StatusMsg = "An error occured while deleting the order!"
                };
            }
        }

        internal OrderResp ChangeOrderStatusAction(int orderId)
        {
            try
            {
                using (var db = new OrderContext())
                {
                    var existingOrder = db.CustomerOrders.FirstOrDefault(o => o.Id == orderId);

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

                    existingOrder.OrderStatus = existingOrder.OrderStatus == OrderStatus.Pending
                        ? OrderStatus.Delivering
                        : OrderStatus.Pending;

                    db.Entry(existingOrder).State = EntityState.Modified;
                    db.SaveChanges();

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
    }
}
