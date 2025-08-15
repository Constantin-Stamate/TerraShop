using System.Threading.Tasks;
using eUseControl.Domain.Entities.Payment;

namespace eUseControl.BusinessLogic.Interfaces
{
    public interface ITransaction
    {
        Task<TransactionResp> ProcessPayment(TransactionData transactionData, int userId);
    }
}
