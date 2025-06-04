using eUseControl.BusinessLogic.Core;
using eUseControl.BusinessLogic.Interfaces;
using eUseControl.Domain.Entities.Payment;

namespace eUseControl.BusinessLogic.BusinessLayer
{
    public class TransactionBL : UserApi, ITransaction
    {
        public TransactionResp ProcessPayment(TransactionData transactionData, int userId)
        {
            return ProcessPaymentAction(transactionData, userId);
        }
    }
}
