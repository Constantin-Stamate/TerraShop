namespace eUseControl.BusinessLogic.Migrations.Wishlist
{
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<eUseControl.BusinessLogic.DBModel.WishlistContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            MigrationsDirectory = @"Migrations\Wishlist";
        }

        protected override void Seed(eUseControl.BusinessLogic.DBModel.WishlistContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.
        }
    }
}
