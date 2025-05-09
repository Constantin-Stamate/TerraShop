using System;

namespace eUseControl.Domain.Entities.Review
{
    public class ReviewData
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int ProductId { get; set; }

        public DateTime ReviewPostDate { get; set; }

        public string Review { get; set; }

        public int Rating { get; set; }
    }
}
