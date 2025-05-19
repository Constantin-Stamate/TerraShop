namespace eUseControl.BusinessLogic.Migrations.Wishlist
{
    using System.Data.Entity.Migrations;

    public partial class IntegrateWishlistProductsTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.WishlistProducts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        ProductId = c.Int(nullable: false),
                        AddedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserId)
                .Index(t => t.ProductId);

            AddForeignKey("dbo.WishlistProducts", "UserId", "dbo.Users", "Id");
            AddForeignKey("dbo.WishlistProducts", "ProductId", "dbo.Products", "Id", cascadeDelete: true);

        }
        
        public override void Down()
        {
            DropForeignKey("dbo.WishlistProducts", "ProductId", "dbo.Products");
            DropForeignKey("dbo.WishlistProducts", "UserId", "dbo.Users");
            DropIndex("dbo.WishlistProducts", new[] { "ProductId" });
            DropIndex("dbo.WishlistProducts", new[] { "UserId" });
            DropTable("dbo.WishlistProducts");
        }
    }
}
