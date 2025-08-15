using System.Threading.Tasks;
using eUseControl.BusinessLogic.Core;
using eUseControl.BusinessLogic.Interfaces;
using eUseControl.Domain.Entities.Payment;

namespace eUseControl.BusinessLogic.BusinessLayer
{
    public class TransactionBL : UserApi, ITransaction
    {
        public async Task<TransactionResp> ProcessPayment(TransactionData transactionData, int userId)
        {
            return await ProcessPaymentAction(transactionData, userId);
        }
    }
}
