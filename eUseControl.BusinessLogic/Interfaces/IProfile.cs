using System.Threading.Tasks;
using eUseControl.Domain.Entities.Profile;

namespace eUseControl.BusinessLogic.Interfaces
{
    public interface IProfile
    {
        Task<ProfileData> GetProfileByUserId(int userId);

        Task<ProfileResp> UpdateProfile(ProfileData profileData);

        Task<ProfileResp> ChangePassword(string currentPassword, string newPassword, int userId);
    }
}
