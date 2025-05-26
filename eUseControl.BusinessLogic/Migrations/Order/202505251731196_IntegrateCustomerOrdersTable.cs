namespace eUseControl.BusinessLogic.Migrations.Order
{
    using System.Data.Entity.Migrations;

    public partial class IntegrateCustomerOrdersTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CustomerOrders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        FirstName = c.String(nullable: false, maxLength: 30),
                        LastName = c.String(nullable: false, maxLength: 30),
                        DeliveryAddress = c.String(nullable: false, maxLength: 70),
                        PhoneNumber = c.String(nullable: false, maxLength: 20),
                        Email = c.String(nullable: false, maxLength: 30),
                        Notes = c.String(maxLength: 500),
                        PaymentMethod = c.String(nullable: false, maxLength: 50),
                        OrderDate = c.DateTime(nullable: false),
                        OrderStatus = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CustomerOrders", "UserId", "dbo.Users");
            DropIndex("dbo.CustomerOrders", new[] { "UserId" });
            DropTable("dbo.CustomerOrders");
        }
    }
}
