namespace eUseControl.BusinessLogic.Migrations.Order
{
    using System.Data.Entity.Migrations;

    public partial class AddTotalPriceToCustomerOrders : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CustomerOrders", "TotalPrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CustomerOrders", "TotalPrice");
        }
    }
}
