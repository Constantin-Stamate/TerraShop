namespace eUseControl.BusinessLogic.Migrations.Coupon
{
    using System;
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<eUseControl.BusinessLogic.DBModel.CouponContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            MigrationsDirectory = @"Migrations\Coupon";
        }

        protected override void Seed(eUseControl.BusinessLogic.DBModel.CouponContext context)
        {
            context.DiscountCoupons.AddOrUpdate(
                c => c.Code,

                new eUseControl.Domain.Entities.Cart.Coupon
                {
                    Code = "WELCOME10",
                    DiscountPercent = 10,
                    ExpirationDate = new DateTime(2025, 12, 31),
                    IsActive = true
                },

                new eUseControl.Domain.Entities.Cart.Coupon
                {
                    Code = "SPRING25",
                    DiscountPercent = 25,
                    ExpirationDate = new DateTime(2025, 5, 30),
                    IsActive = true
                },

                new eUseControl.Domain.Entities.Cart.Coupon
                {
                    Code = "BLACKFRIDAY50",
                    DiscountPercent = 13,
                    ExpirationDate = new DateTime(2025, 11, 30),
                    IsActive = false
                },

                new eUseControl.Domain.Entities.Cart.Coupon
                {
                    Code = "SUMMER15",
                    DiscountPercent = 15,
                    ExpirationDate = new DateTime(2025, 8, 1),
                    IsActive = true
                },

                new eUseControl.Domain.Entities.Cart.Coupon
                {
                    Code = "NEWYEAR30",
                    DiscountPercent = 30,
                    ExpirationDate = new DateTime(2025, 1, 10),
                    IsActive = false
                },

                new eUseControl.Domain.Entities.Cart.Coupon
                {
                    Code = "VIP20",
                    DiscountPercent = 20,
                    ExpirationDate = new DateTime(2026, 3, 31),
                    IsActive = true
                },

                new eUseControl.Domain.Entities.Cart.Coupon
                {
                    Code = "FREESHIP5",
                    DiscountPercent = 5,
                    ExpirationDate = new DateTime(2026, 12, 31),
                    IsActive = true
                },

                new eUseControl.Domain.Entities.Cart.Coupon
                {
                    Code = "EXTRA40",
                    DiscountPercent = 35,
                    ExpirationDate = new DateTime(2025, 10, 15),
                    IsActive = false
                }
            );
        }
    }
}
