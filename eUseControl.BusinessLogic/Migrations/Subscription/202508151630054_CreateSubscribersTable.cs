namespace eUseControl.BusinessLogic.Migrations.Subscription
{
    using System.Data.Entity.Migrations;

    public partial class CreateSubscribersTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Subscribers",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Email = c.String(nullable: false, maxLength: 30),
                    SubscriptionDate = c.DateTime(nullable: false),
                })
                .PrimaryKey(t => t.Id);
        }

        public override void Down()
        {
            DropTable("dbo.Subscribers");
        }
    }
}
