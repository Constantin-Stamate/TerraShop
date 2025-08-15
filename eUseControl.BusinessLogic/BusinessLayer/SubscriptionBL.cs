using System.Threading.Tasks;
using eUseControl.BusinessLogic.Core;
using eUseControl.BusinessLogic.Interfaces;
using eUseControl.Domain.Entities.Subscription;

namespace eUseControl.BusinessLogic.BusinessLayer
{
    public class SubscriptionBL : UserApi, ISubscription
    {
        public async Task<SubscriptionResp> CreateSubscription(string email)
        {
            return await CreateSubscriptionAction(email);
        }
    }
}
