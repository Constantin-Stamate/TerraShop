using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eUseControl.Domain.Entities
{
    public class URegisterData
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        public string RegistrationIp { get; set; }

        public DateTime RegistrationDateTime { get; set; }
    }
}
