namespace eUseControl.Domain.Entities.Profile
{
    public class ProfileData
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Address { get; set; }

        public string PhoneNumber { get; set; }

        public string ProfileImageUrl { get; set; }
    }
}
