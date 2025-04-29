namespace eUseControl.Domain.Entities.User
{
    public class ULoginResp
    {
        public bool Status { get; set; }

        public string StatusMsg { get; set; }

        public UserMinimal UserMinimal { get; set; }
    }
}
