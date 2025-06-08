namespace eUseControl.BusinessLogic.Migrations.Category
{
    using System.Data.Entity.Migrations;

    public partial class CreateProductCategoriesTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProductCategories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CategoryName = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.Id);
        }
        
        public override void Down()
        {
            DropTable("dbo.ProductCategories");
        }
    }
}
