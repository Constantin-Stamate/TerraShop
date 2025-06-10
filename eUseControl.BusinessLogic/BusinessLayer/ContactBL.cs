using eUseControl.BusinessLogic.Core;
using eUseControl.BusinessLogic.Interfaces;
using eUseControl.Domain.Entities.Contact;

namespace eUseControl.BusinessLogic.BusinessLayer
{
    public class ContactBL : UserApi, IContact
    {
        public ContactResp SubmitContactRequest(ContactData contactData, int userId)
        {
            return SubmitContactRequestAction(contactData, userId);
        }
    }
}
