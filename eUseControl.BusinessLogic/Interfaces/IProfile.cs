using eUseControl.Domain.Entities.Profile;

namespace eUseControl.BusinessLogic.Interfaces
{
    public interface IProfile
    {
        ProfileData GetProfileByUserId(int userId);

        ProfileResp UpdateProfile(int userId, ProfileData profileData);

        ProfileResp ChangePassword(string currentPassword, string newPassword, int userId);
    }
}
