namespace eUseControl.BusinessLogic.Migrations.Cart
{
    using System.Data.Entity.Migrations;

    public partial class CreateCartItemsTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CartItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        ProductId = c.Int(nullable: false),
                        SelectedQuantity = c.Int(nullable: false),
                        Subtotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                        AddedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Products", t => t.ProductId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: false)
                .Index(t => t.UserId)
                .Index(t => t.ProductId);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CartItems", "ProductId", "dbo.Products");
            DropForeignKey("dbo.CartItems", "UserId", "dbo.Users");
            DropIndex("dbo.CartItems", new[] { "ProductId" });
            DropIndex("dbo.CartItems", new[] { "UserId" });
            DropTable("dbo.CartItems");
        }
    }
}
