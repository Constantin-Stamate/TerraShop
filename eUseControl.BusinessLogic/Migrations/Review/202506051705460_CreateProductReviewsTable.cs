namespace eUseControl.BusinessLogic.Migrations.Review
{
    using System.Data.Entity.Migrations;

    public partial class CreateProductReviewsTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProductReviews",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        ProductId = c.Int(nullable: false),
                        ReviewPostDate = c.DateTime(nullable: false),
                        Review = c.String(nullable: false, maxLength: 300),
                        Rating = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Products", t => t.ProductId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: false)
                .Index(t => t.UserId)
                .Index(t => t.ProductId);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProductReviews", "UserId", "dbo.Users");
            DropForeignKey("dbo.ProductReviews", "ProductId", "dbo.Products");
            DropIndex("dbo.ProductReviews", new[] { "ProductId" });
            DropIndex("dbo.ProductReviews", new[] { "UserId" });
            DropTable("dbo.ProductReviews");
        }
    }
}
