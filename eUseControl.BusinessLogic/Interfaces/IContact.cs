using System.Threading.Tasks;
using eUseControl.Domain.Entities.Contact;

namespace eUseControl.BusinessLogic.Interfaces
{
    public interface IContact
    {
        Task<ContactResp> SubmitContactRequest(ContactData contactData, int userId);
    }
}
