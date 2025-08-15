using System.Data.Entity;
using eUseControl.Domain.Entities.Chat;

namespace eUseControl.BusinessLogic.DBModel
{
    public class ChatContext : DbContext
    {
        public ChatContext() : base("name=eUseControl")
        {
        }

        public virtual DbSet<ChatDbTable> ChatMessages { get; set; }
    }
}
