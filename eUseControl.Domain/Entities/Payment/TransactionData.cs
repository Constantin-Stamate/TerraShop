namespace eUseControl.Domain.Entities.Payment
{
    public class TransactionData
    {
        public int OrderId { get; set; }

        public string FullName { get; set; }

        public string CardNumber { get; set; }

        public string ExpiryDate { get; set; }

        public string Cvv { get; set; }
    }
}
