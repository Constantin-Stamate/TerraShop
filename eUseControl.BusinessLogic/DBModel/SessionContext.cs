using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eUseControl.Domain.Entities;

namespace eUseControl.BusinessLogic.DBModel
{
    public class SessionContext : DbContext
    {
        public SessionContext() : base("name=eUseControl")
        {
        }

        public virtual DbSet<Session> Sessions { get; set; }
    }
}
