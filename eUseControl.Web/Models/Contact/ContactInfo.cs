using System;
using eUseControl.Domain.Enums;

namespace eUseControl.Web.Models.Contact
{
    public class ContactInfo
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public string Message { get; set; }

        public DateTime RequestPostDate { get; set; }

        public RequestStatus RequestStatus { get; set; }
    }
}