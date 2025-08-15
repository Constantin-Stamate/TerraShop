namespace eUseControl.BusinessLogic.Migrations.Chat
{
    using System.Data.Entity.Migrations;

    public partial class CreateChatMessagesTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ChatMessages",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    UserId = c.Int(nullable: false),
                    Prompt = c.String(nullable: false, unicode: false, storeType: "text"),
                    Message = c.String(nullable: false, unicode: false, storeType: "text"),
                    ResponseDate = c.DateTime(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
        }

        public override void Down()
        {
            DropForeignKey("dbo.ChatMessages", "UserId", "dbo.Users");
            DropIndex("dbo.ChatMessages", new[] { "UserId" });
            DropTable("dbo.ChatMessages");
        }
    }
}
