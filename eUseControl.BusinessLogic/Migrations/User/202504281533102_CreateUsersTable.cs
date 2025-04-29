namespace eUseControl.BusinessLogic.Migrations.User
{
    using System.Data.Entity.Migrations;

    public partial class CreateUsersTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Username = c.String(nullable: false, maxLength: 30),
                        Password = c.String(nullable: false, maxLength: 50),
                        Email = c.String(nullable: false, maxLength: 30),
                        LastLogin = c.DateTime(),
                        RegistrationDateTime = c.DateTime(nullable: false),
                        LastIp = c.String(maxLength: 30),
                        RegistrationIp = c.String(maxLength: 30),
                        Level = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Users");
        }
    }
}
