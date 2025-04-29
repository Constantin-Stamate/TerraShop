namespace eUseControl.BusinessLogic.Migrations.ProductCategory
{
    using System.Data.Entity.Migrations;

    public partial class CreateProductCategoriesTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Categories",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    CategoryName = c.String(nullable: false, maxLength: 100),
                })
                .PrimaryKey(t => t.Id);
        }

        public override void Down()
        {
            DropTable("dbo.Categories");
        }
    }
}
