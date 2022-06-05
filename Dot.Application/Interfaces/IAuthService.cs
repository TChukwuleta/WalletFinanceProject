using Dot.Application.ResponseModel;
using Dot.Core.Entities;
using Dot.Core.Entities.MerchantSide;
using Dot.Core.Enums;
using Dot.Core.ViewModels;

namespace Dot.Application.Interfaces
{
    public interface IAuthService
    {
        Task<ResultResponse> CreateUserAsync(User user);
        Task<ResultResponse> loginAsync(string email, string password, UserType userType);
        Task<ResultResponse> ChangeUserStatusAsync(User user);
        Task<ResultResponse> ChangePasswordAsync(string email, string oldPassword, string newPassword);
        Task<ResultResponse> GenerateOTP(string email);
        Task<ResultResponse> GenerateToken(string email);
        Task<bool> validateOTP(string email, string otp, string password);
        Task<ResultResponse> ResetPassword(string email, string password);
        Task<ResultResponse> GetUserByEmail(string email);
        Task<ResultResponse> CreateClientAsync(Client user, string password);
    }
}
