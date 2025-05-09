namespace eUseControl.BusinessLogic.Migrations.Review
{
    using System.Data.Entity.Migrations;

    public partial class IntegrateReviewsTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Reviews",
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
                .Index(t => t.UserId)
                .Index(t => t.ProductId);

            AddForeignKey("dbo.Reviews", "UserId", "dbo.Users", "Id");
            AddForeignKey("dbo.Reviews", "ProductId", "dbo.Products", "Id", cascadeDelete: true);

        }

        public override void Down()
        {
            DropForeignKey("dbo.Reviews", "ProductId", "dbo.Products");
            DropForeignKey("dbo.Reviews", "UserId", "dbo.Users");
            DropIndex("dbo.Reviews", new[] { "ProductId" });
            DropIndex("dbo.Reviews", new[] { "UserId" });
            DropTable("dbo.Reviews");
        }
    }
}
