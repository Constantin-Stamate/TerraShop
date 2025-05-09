using System;
using eUseControl.Domain.Enums;

namespace eUseControl.Domain.Entities
{
    public class URegisterData
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        public string RegistrationIp { get; set; }

        public DateTime RegistrationDateTime { get; set; }

        public URole Level { get; set; }
    }
}
