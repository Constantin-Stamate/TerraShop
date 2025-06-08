namespace eUseControl.BusinessLogic.Migrations.Category
{
    using System.Data.Entity.Migrations;
    using eUseControl.Domain.Entities.Product;

    internal sealed class Configuration : DbMigrationsConfiguration<eUseControl.BusinessLogic.DBModel.CategoryContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            MigrationsDirectory = @"Migrations\Category";
        }

        protected override void Seed(eUseControl.BusinessLogic.DBModel.CategoryContext context)
        {
            context.ProductCategories.AddOrUpdate(c => c.CategoryName,

                new CategoryDbTable { CategoryName = "Personal Care" },
                new CategoryDbTable { CategoryName = "Household" },
                new CategoryDbTable { CategoryName = "Kitchen" },
                new CategoryDbTable { CategoryName = "Eco Fashion" },
                new CategoryDbTable { CategoryName = "Organic Cosmetics" },
                new CategoryDbTable { CategoryName = "Health Care" },
                new CategoryDbTable { CategoryName = "Eco Chemicals" },
                new CategoryDbTable { CategoryName = "Renewable Energy" },
                new CategoryDbTable { CategoryName = "Bio Tableware" },
                new CategoryDbTable { CategoryName = "Eco Transport" },
                new CategoryDbTable { CategoryName = "Eco Car Cleaning" },
                new CategoryDbTable { CategoryName = "Eco Construction" },
                new CategoryDbTable { CategoryName = "Pet Care" },
                new CategoryDbTable { CategoryName = "Eco Gardening" },
                new CategoryDbTable { CategoryName = "Eco Stationery" }
            );

            context.SaveChanges();
        }
    }
}
