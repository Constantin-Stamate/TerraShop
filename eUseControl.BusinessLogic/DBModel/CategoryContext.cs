using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eUseControl.Domain.Entities;
using eUseControl.Domain.Entities.Product;

namespace eUseControl.BusinessLogic.DBModel
{
    public class CategoryContext : DbContext
    {
        public CategoryContext() : base("name=eUseControl")
        {
        }

        public virtual DbSet<Category> Categories { get; set; }
    }
}
