using eUseControl.Domain.Entities.Payment;

namespace eUseControl.BusinessLogic.Interfaces
{
    public interface ITransaction
    {
        TransactionResp ProcessPayment(TransactionData transactionData, int userId);
    }
}
