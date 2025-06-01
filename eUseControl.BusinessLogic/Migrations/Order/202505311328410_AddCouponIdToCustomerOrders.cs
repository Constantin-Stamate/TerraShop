namespace eUseControl.BusinessLogic.Migrations.Order
{
    using System.Data.Entity.Migrations;

    public partial class AddCouponIdToCustomerOrders : DbMigration
    {
        public override void Up()
        {  
            AddColumn("dbo.CustomerOrders", "CouponId", c => c.Int());
            CreateIndex("dbo.CustomerOrders", "CouponId");
            AddForeignKey("dbo.CustomerOrders", "CouponId", "dbo.DiscountCoupons", "Id");
        }
        
        public override void Down()
        {
            DropColumn("dbo.CustomerOrders", "CouponId");
        }
    }
}
