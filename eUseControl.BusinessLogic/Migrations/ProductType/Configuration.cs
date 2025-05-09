namespace eUseControl.BusinessLogic.Migrations.ProductType
{
    using System.Data.Entity.Migrations;
    using eUseControl.Domain.Entities.Product;

    internal sealed class Configuration : DbMigrationsConfiguration<eUseControl.BusinessLogic.DBModel.CategoryContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            MigrationsDirectory = @"Migrations\ProductType";
        }

        protected override void Seed(eUseControl.BusinessLogic.DBModel.CategoryContext context)
        {
            context.Categories.AddOrUpdate(
                c => c.CategoryName,
                new Category { CategoryName = "Personal Care" },
                new Category { CategoryName = "Household" },
                new Category { CategoryName = "Kitchen" },
                new Category { CategoryName = "Eco Fashion" },
                new Category { CategoryName = "Organic Cosmetics" },
                new Category { CategoryName = "Health Care" },
                new Category { CategoryName = "Eco Chemicals" },
                new Category { CategoryName = "Renewable Energy" },
                new Category { CategoryName = "Bio Tableware" },
                new Category { CategoryName = "Eco Transport" },
                new Category { CategoryName = "Eco Car Cleaning" },
                new Category { CategoryName = "Eco Construction" },
                new Category { CategoryName = "Pet Care" },
                new Category { CategoryName = "Eco Gardening" },
                new Category { CategoryName = "Eco Stationery" }
            );

            context.SaveChanges();
        }
    }
}
