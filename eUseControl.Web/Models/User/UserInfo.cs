namespace eUseControl.Web.Models.User
{
    public class UserInfo
    {
        public int Id { get; set; }

        public string ProfileImageUrl { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public string Address { get; set; }

        public string PhoneNumber { get; set; }

        public int OrderCount { get; set; }

        public decimal TotalSpent { get; set; }
    }
}