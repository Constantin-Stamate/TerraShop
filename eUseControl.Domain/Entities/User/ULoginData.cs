using System;

namespace eUseControl.Domain.Entities.User
{
    public class ULoginData
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public string LastIp { get; set; }

        public DateTime LastLogin { get; set; }
    }
}
