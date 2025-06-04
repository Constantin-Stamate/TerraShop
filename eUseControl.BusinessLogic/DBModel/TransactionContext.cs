using System.Data.Entity;
using eUseControl.Domain.Entities.Payment;

namespace eUseControl.BusinessLogic.DBModel
{
    public class TransactionContext : DbContext
    {
        public TransactionContext() : base("name=eUseControl")
        {
        }

        public virtual DbSet<TransactionDbTable> UserTransactions { get; set; }
    }
}
