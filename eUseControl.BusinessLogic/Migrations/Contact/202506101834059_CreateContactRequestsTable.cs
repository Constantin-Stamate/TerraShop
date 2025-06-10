namespace eUseControl.BusinessLogic.Migrations.Contact
{
    using System.Data.Entity.Migrations;

    public partial class CreateContactRequestsTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ContactRequests",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        Username = c.String(nullable: false, maxLength: 30),
                        Email = c.String(nullable: false, maxLength: 30),
                        Message = c.String(nullable: false, maxLength: 500),
                        RequestStatus = c.Int(nullable: false),
                        RequestPostDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId); 
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ContactRequests", "UserId", "dbo.Users");
            DropIndex("dbo.ContactRequests", new[] { "UserId" });
            DropTable("dbo.ContactRequests");
        }
    }
}
