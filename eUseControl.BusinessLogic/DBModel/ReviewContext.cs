using System.Data.Entity;
using eUseControl.Domain.Entities.Review;

namespace eUseControl.BusinessLogic.DBModel
{
    public class ReviewContext : DbContext
    {
        public ReviewContext() : base("name=eUseControl")
        {
        }

        public virtual DbSet<ReviewDbTable> Reviews { get; set; }
    }
}
