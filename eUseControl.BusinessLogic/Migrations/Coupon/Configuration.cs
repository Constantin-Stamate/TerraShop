namespace eUseControl.BusinessLogic.Migrations.Coupon
{
    using System;
    using System.Data.Entity.Migrations;
    using eUseControl.Domain.Entities.Cart;

    internal sealed class Configuration : DbMigrationsConfiguration<eUseControl.BusinessLogic.DBModel.CouponContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            MigrationsDirectory = @"Migrations\Coupon";
        }

        protected override void Seed(eUseControl.BusinessLogic.DBModel.CouponContext context)
        {
            context.DiscountCoupons.AddOrUpdate(c => c.Code, 

                new CouponDbTable
                {
                    Code = "WELCOME10",
                    DiscountPercent = 10,
                    ExpirationDate = DateTime.Now.AddMonths(1),
                    IsActive = true
                },

                new CouponDbTable
                {
                    Code = "SUMMER15",
                    DiscountPercent = 15,
                    ExpirationDate = DateTime.Now.AddMonths(2),
                    IsActive = true
                },

                new CouponDbTable
                {
                    Code = "BLACKFRIDAY20",
                    DiscountPercent = 20,
                    ExpirationDate = new DateTime(DateTime.Now.Year, 11, 30),
                    IsActive = true
                },

                new CouponDbTable
                {
                    Code = "XMAS25",
                    DiscountPercent = 25,
                    ExpirationDate = new DateTime(DateTime.Now.Year, 12, 26),
                    IsActive = true
                },

                new CouponDbTable
                {
                    Code = "NEWYEAR30",
                    DiscountPercent = 30,
                    ExpirationDate = new DateTime(DateTime.Now.Year + 1, 1, 15),
                    IsActive = true
                },

                new CouponDbTable
                {
                    Code = "STUDENT5",
                    DiscountPercent = 5,
                    ExpirationDate = DateTime.Now.AddMonths(6),
                    IsActive = true
                },

                new CouponDbTable
                {
                    Code = "FLASH40",
                    DiscountPercent = 40,
                    ExpirationDate = DateTime.Now.AddDays(7),
                    IsActive = true
                },

                new CouponDbTable
                {
                    Code = "FREEDELIVERY",
                    DiscountPercent = 100,
                    ExpirationDate = DateTime.Now.AddDays(3),
                    IsActive = true
                },

                new CouponDbTable
                {
                    Code = "BULK50",
                    DiscountPercent = 50,
                    ExpirationDate = DateTime.Now.AddMonths(3),
                    IsActive = true
                },

                new CouponDbTable
                {
                    Code = "LOYAL20",
                    DiscountPercent = 20,
                    ExpirationDate = DateTime.Now.AddMonths(12),
                    IsActive = true
                }
            );

            context.SaveChanges();
        }
    }
}
