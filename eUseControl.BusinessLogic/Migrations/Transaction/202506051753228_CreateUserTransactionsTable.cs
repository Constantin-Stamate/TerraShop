namespace eUseControl.BusinessLogic.Migrations.Transaction
{
    using System.Data.Entity.Migrations;

    public partial class CreateUserTransactionsTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserTransactions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OrderId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                        TransactionDate = c.DateTime(nullable: false),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TransactionStatus = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CustomerOrders", t => t.OrderId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: false)
                .Index(t => t.OrderId)
                .Index(t => t.UserId);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserTransactions", "UserId", "dbo.Users");
            DropForeignKey("dbo.UserTransactions", "OrderId", "dbo.CustomerOrders");
            DropIndex("dbo.UserTransactions", new[] { "UserId" });
            DropIndex("dbo.UserTransactions", new[] { "OrderId" });
            DropTable("dbo.UserTransactions");
        }
    }
}
