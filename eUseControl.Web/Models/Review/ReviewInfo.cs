using System;

namespace eUseControl.Web.Models.Review
{
    public class ReviewInfo
    {
        public int Id { get; set; }

        public string ProfileImageUrl { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime ReviewPostDate { get; set; }

        public string Review { get; set; }

        public int Rating { get; set; }
    }
}