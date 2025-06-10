using eUseControl.Domain.Entities.Contact;

namespace eUseControl.BusinessLogic.Interfaces
{
    public interface IContact
    {
        ContactResp SubmitContactRequest(ContactData contactData, int userId);
    }
}
