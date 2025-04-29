namespace eUseControl.BusinessLogic.Migrations.Product
{
    using System.Data.Entity.Migrations;

    public partial class CreateProductsTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        CategoryId = c.Int(nullable: false),
                        ProductName = c.String(nullable: false, maxLength: 100),
                        ProductAddress = c.String(nullable: false, maxLength: 100),
                        ProductQuantity = c.Int(nullable: false),
                        ProductQuality = c.String(nullable: false, maxLength: 100),
                        ProductPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ProductRegion = c.String(nullable: false, maxLength: 100),
                        ProductImageUrl = c.String(nullable: false, maxLength: 200),
                        ProductDescription = c.String(nullable: false, maxLength: 500),
                        ProductPostDate = c.DateTime(nullable: false),
                        ProductStatus = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id) 
                .Index(t => t.UserId)
                .Index(t => t.CategoryId);

            AddForeignKey("dbo.Products", "UserId", "dbo.Users", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Products", "CategoryId", "dbo.Categories", "Id", cascadeDelete: true);

        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Products", "CategoryId", "dbo.Categories");
            DropForeignKey("dbo.Products", "UserId", "dbo.Users");
            DropIndex("dbo.Products", new[] { "CategoryId" });
            DropIndex("dbo.Products", new[] { "UserId" });
            DropTable("dbo.Products");
        }
    }
}
