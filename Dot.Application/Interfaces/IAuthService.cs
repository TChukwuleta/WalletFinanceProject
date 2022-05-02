using Dot.Application.ResponseModel;
using Dot.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dot.Application.Interfaces
{
    public interface IAuthService
    {
        Task<ResultResponse> CreateUserAsync(User user);
        Task<ResultResponse> loginAsync(string email, string password);
        Task<ResultResponse> ChangeUserStatusAsync(User user);
        Task<ResultResponse> ChangePasswordAsync(string email, string oldPassword, string newPassword);
        Task<ResultResponse> ForgotPassword(string email);
        Task<ResultResponse> ResetPassword(string email, string password);
    }
}
