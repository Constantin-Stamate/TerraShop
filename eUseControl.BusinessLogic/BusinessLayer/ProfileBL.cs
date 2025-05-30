﻿using eUseControl.BusinessLogic.Core;
using eUseControl.BusinessLogic.Interfaces;
using eUseControl.Domain.Entities.Profile;

namespace eUseControl.BusinessLogic.BusinessLayer
{
    public class ProfileBL : UserApi, IProfile
    {
        public ProfileData GetProfileByUserId(int userId)
        {
            return GetProfileByUserIdAction(userId);
        }

        public ProfileResp UpdateProfile(ProfileData profileData)
        {
            return UpdateProfileAction(profileData);
        }

        public ProfileResp ChangePassword(string currentPassword, string newPassword, int userId)
        {
            return ChangePasswordAction(currentPassword, newPassword, userId);
        }
    }
}
