using System;

namespace eUseControl.Domain.Entities.Review
{
    public class ReviewSummary
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
