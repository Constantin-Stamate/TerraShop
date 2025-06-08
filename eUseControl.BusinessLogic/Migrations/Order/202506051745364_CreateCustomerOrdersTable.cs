namespace eUseControl.BusinessLogic.Migrations.Order
{
    using System.Data.Entity.Migrations;

    public partial class CreateCustomerOrdersTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CustomerOrders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        CouponId = c.Int(),
                        FirstName = c.String(nullable: false, maxLength: 30),
                        LastName = c.String(nullable: false, maxLength: 30),
                        DeliveryAddress = c.String(nullable: false, maxLength: 70),
                        PhoneNumber = c.String(nullable: false, maxLength: 20),
                        Email = c.String(nullable: false, maxLength: 30),
                        Notes = c.String(maxLength: 500),
                        PaymentMethod = c.String(nullable: false, maxLength: 50),
                        OrderDate = c.DateTime(nullable: false),
                        OrderStatus = c.Int(nullable: false),
                        TotalPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DiscountCoupons", t => t.CouponId)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.CouponId);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CustomerOrders", "CouponId", "dbo.DiscountCoupons");
            DropForeignKey("dbo.CustomerOrders", "UserId", "dbo.Users");
            DropIndex("dbo.CustomerOrders", new[] { "CouponId" });
            DropIndex("dbo.CustomerOrders", new[] { "UserId" });
            DropTable("dbo.CustomerOrders");
        }
    }
}
