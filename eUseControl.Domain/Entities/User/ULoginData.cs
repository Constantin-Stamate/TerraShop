using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eUseControl.Domain.Enums;

namespace eUseControl.Domain.Entities.User
{
    public class ULoginData
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public string LastIp { get; set; }

        public URole Level { get; set; }

        public DateTime LastLogin { get; set; }
    }
}
