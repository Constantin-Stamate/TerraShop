using eUseControl.Domain.Entities.Profile;

namespace eUseControl.BusinessLogic.Interfaces
{
    public interface IProfile
    {
        ProfileData GetProfileByUserId(int userId);

        ProfileResp UpdateProfile(ProfileData profileData);

        ProfileResp ChangePassword(string currentPassword, string newPassword, int userId);
    }
}
