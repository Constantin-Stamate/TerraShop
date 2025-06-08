namespace eUseControl.BusinessLogic.Migrations.Session
{
    using System.Data.Entity.Migrations;

    public partial class CreateUserSessionsTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserSessions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        CookieString = c.String(nullable: false, maxLength: 500),
                        ExpireTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserSessions", "UserId", "dbo.Users");
            DropIndex("dbo.UserSessions", new[] { "UserId" });
            DropTable("dbo.UserSessions");
        }
    }
}
