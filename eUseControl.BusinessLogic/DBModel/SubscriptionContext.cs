using System.Data.Entity;
using eUseControl.Domain.Entities.Subscription;

namespace eUseControl.BusinessLogic.DBModel
{
    public class SubscriptionContext : DbContext
    {
        public SubscriptionContext() : base("name=eUseControl")
        {
        }

        public virtual DbSet<SubscriptionDbTable> Subscribers { get; set; }
    }
}
