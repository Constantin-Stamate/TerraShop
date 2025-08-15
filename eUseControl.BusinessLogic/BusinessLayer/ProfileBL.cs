using System.Threading.Tasks;
using eUseControl.BusinessLogic.Core;
using eUseControl.BusinessLogic.Interfaces;
using eUseControl.Domain.Entities.Profile;

namespace eUseControl.BusinessLogic.BusinessLayer
{
    public class ProfileBL : UserApi, IProfile
    {
        public async Task<ProfileData> GetProfileByUserId(int userId)
        {
            return await GetProfileByUserIdAction(userId);
        }

        public async Task<ProfileResp> UpdateProfile(ProfileData profileData)
        {
            return await UpdateProfileAction(profileData);
        }

        public async Task<ProfileResp> ChangePassword(string currentPassword, string newPassword, int userId)
        {
            return await ChangePasswordAction(currentPassword, newPassword, userId);
        }
    }
}
