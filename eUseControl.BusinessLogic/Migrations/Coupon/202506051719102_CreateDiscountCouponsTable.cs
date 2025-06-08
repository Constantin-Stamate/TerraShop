namespace eUseControl.BusinessLogic.Migrations.Coupon
{
    using System.Data.Entity.Migrations;

    public partial class CreateDiscountCouponsTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DiscountCoupons",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(nullable: false, maxLength: 50),
                        DiscountPercent = c.Int(nullable: false),
                        ExpirationDate = c.DateTime(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
        }
        
        public override void Down()
        {
            DropTable("dbo.DiscountCoupons");
        }
    }
}
