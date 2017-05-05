using PHRMS.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PHRMS.BLL
{
    public partial class CatalogService
    {
        public bool IsValidUser(LoginModel loginModel)
        {
            bool result = false;
            CommonComponent commonMethods = null;
            UsersViewModel loggingUser = null;
            try
            {
                if (string.IsNullOrEmpty(loginModel.UserName) || string.IsNullOrEmpty(loginModel.Password))
                {
                    return false;
                }

                loggingUser = _repository.GetUsersDetails(loginModel.UserName);

                if (loggingUser == null)
                {
                    return false;
                }

                commonMethods = new CommonComponent();

                result = commonMethods.VerifyPassword(loginModel.Password, loggingUser.Password);
            }
            catch (Exception)
            {

            }
            commonMethods = null;
            loggingUser = null;
            return result;
        }

        public UsersViewModel LoginByOTP(Guid UserId)
        {

            UsersViewModel loggingUser = null;
            try
            {
                loggingUser = _repository.LoginByOTP(UserId);
            }
            catch (Exception)
            {

            }
            return loggingUser;
        }
        public UsersViewModel ValidateUser(LoginModel loginModel)
        {
            CommonComponent commonMethods = null;
            UsersViewModel loggingUser = null;
            try
            {
                if (string.IsNullOrEmpty(loginModel.UserName) || string.IsNullOrEmpty(loginModel.Password))
                {
                    return null;
                }

                loggingUser = _repository.GetUsersDetails(loginModel.UserName);

                commonMethods = new CommonComponent();

                if (loginModel.Password != loggingUser.Password)
                {
                    return null;
                }
            }
            catch (Exception)
            {

            }
            commonMethods = null;
            return loggingUser;
        }

        public RegistrationViewModel MigrateTempRegRecord(RegistrationViewModel oModel)
        {
            return _repository.MigrateTempRegRecord(oModel);
        }

        public async Task<RegistrationViewModel> Register(RegistrationViewModel oRegistrationViewModel)
        {
            try
            {
                CommonComponent commonMethods = new CommonComponent();
                oRegistrationViewModel.Password = "";
                commonMethods = null;
                return await _repository.Register(oRegistrationViewModel);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public string FetchOTPFromTemp(string strMobile)
        {
            return _repository.FetchOTPFromTemp(strMobile);
        }

        public async Task<bool> ResetPassword(Guid userId, string Password)
        {
            CommonComponent commonMethods = null;
            bool result = false;
            try
            {
                commonMethods = new CommonComponent();
                Password = "";
                result = await _repository.ResetPassword(userId, Password);
            }
            catch (Exception)
            {
            }
            commonMethods = null;
            return result;
        }

        public bool DoesEmailExist(string strEmail)
        {
            return _repository.DoesEmailExist(strEmail);
        }

        public bool DoesMobileExistForOTP(string strMobileNo)
        {
            return _repository.DoesMobileExistForOTP(strMobileNo);
        }
        public bool DoesMobileExist(string strMobileNo)
        {
            return _repository.DoesMobileExist(strMobileNo);
        }

        public bool DoesMobileExistMedical(Guid Userid, string Mobile, string EmailAddress)
        {
            return _repository.DoesMobileExistMedical(Userid, Mobile, EmailAddress);
        }

        public bool DoesEmailOrMobileExist(string strUserName)
        {
            return _repository.DoesEmailOrMobileExist(strUserName);
        }

        public ForgotPasswordModel VerifyUsernameforPasswordChange(ForgotPasswordModel oModel)
        {
            UsersViewModel oUsersViewModel = null;
            try
            {
                oUsersViewModel = _repository.GetUsersDetails(oModel.UserName);
                if (oUsersViewModel != null)
                {
                    oModel.Email = oUsersViewModel.Email;
                    oModel.MobileNo = oUsersViewModel.MobileNo;
                    oModel.StatusCode = FPProcessStatus.Success;
                    oModel.Id = oUsersViewModel.UserId;
                    oUsersViewModel = null;
                }
                else
                    oModel.StatusCode = FPProcessStatus.VerificationFailure;
            }
            catch (Exception)
            {
                oModel.StatusCode = FPProcessStatus.SysFailure;
            }

            return oModel;
        }
        public List<NotificationViewModel> GetNotifications(Guid userId)
        {
            try
            {
                return _repository.GetNotifications(userId);
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        public void updateNotificationAfterViewedByUser(Guid userId)
        {
            try
            {
                _repository.updateNotificationAfterViewedByUser(userId);
            }
            catch (Exception e)
            {
                throw e;
            }

        }
    }
}
