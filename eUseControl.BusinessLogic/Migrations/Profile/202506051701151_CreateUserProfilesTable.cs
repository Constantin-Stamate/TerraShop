namespace eUseControl.BusinessLogic.Migrations.Profile
{
    using System.Data.Entity.Migrations;

    public partial class CreateUserProfilesTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserProfiles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        FirstName = c.String(nullable: false, maxLength: 30),
                        LastName = c.String(nullable: false, maxLength: 30),
                        Email = c.String(nullable: false, maxLength: 30),
                        Address = c.String(nullable: false, maxLength: 50),
                        PhoneNumber = c.String(nullable: false, maxLength: 50),
                        LastProfileUpdate = c.DateTime(nullable: false),
                        ProfileImageUrl = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserProfiles", "UserId", "dbo.Users");
            DropIndex("dbo.UserProfiles", new[] { "UserId" });
            DropTable("dbo.UserProfiles");
        }
    }
}
