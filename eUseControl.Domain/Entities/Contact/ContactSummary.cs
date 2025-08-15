using System;
using eUseControl.Domain.Enums;

namespace eUseControl.Domain.Entities.Contact
{
    public class ContactSummary
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public string Message { get; set; }

        public DateTime RequestPostDate { get; set; }

        public RequestStatus RequestStatus { get; set; }
    }
}
