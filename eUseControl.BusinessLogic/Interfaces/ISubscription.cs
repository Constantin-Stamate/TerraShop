using System.Threading.Tasks;
using eUseControl.Domain.Entities.Subscription;

namespace eUseControl.BusinessLogic.Interfaces
{
    public interface ISubscription
    {
        Task<SubscriptionResp> CreateSubscription(string email);
    }
}
