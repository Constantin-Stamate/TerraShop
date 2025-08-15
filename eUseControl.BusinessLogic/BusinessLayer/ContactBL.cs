using System.Threading.Tasks;
using eUseControl.BusinessLogic.Core;
using eUseControl.BusinessLogic.Interfaces;
using eUseControl.Domain.Entities.Contact;

namespace eUseControl.BusinessLogic.BusinessLayer
{
    public class ContactBL : UserApi, IContact
    {
        public async Task<ContactResp> SubmitContactRequest(ContactData contactData, int userId)
        {
            return await SubmitContactRequestAction(contactData, userId);
        }
    }
}
