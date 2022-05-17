using Dot.Application.Interfaces;
using Dot.Application.ResponseModel;
using Dot.Core.Entities;
using Dot.Core.Enums;
using Dot.Core.ViewModels;
using Dot.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Dot.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMailService _mailService;
        private readonly IConfiguration _config;

        public AuthService(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IConfiguration config, IMailService mailService)
        {
            _context = context;
            _config = config;
            _userManager = userManager;
            _mailService = mailService;
        }
        public async Task<ResultResponse> ChangePasswordAsync(string email, string oldPassword, string newPassword)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    return ResultResponse.Failure("User does not exist");
                }
                var checkPassword = await _userManager.CheckPasswordAsync(user, newPassword);
                if (checkPassword)
                {
                    return ResultResponse.Failure("Please enter a new password");
                }
                var changePassword = await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);
                if (!changePassword.Succeeded)
                {
                    var errors = changePassword.Errors.Select(c => c.Description);
                    return ResultResponse.Failure(errors);
                }
                return ResultResponse.Success("Password change was successful");
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public Task<ResultResponse> ChangeUserStatusAsync(User user)
        {
            throw new NotImplementedException();
        }

        public async Task<ResultResponse> CreateUserAsync(User user)
        {
            try
            {
                var existingUser = await _userManager.FindByEmailAsync(user.Email);
                if (existingUser != null)
                {
                    return ResultResponse.Failure("User with that email already exist");
                }
                var newUser = new ApplicationUser
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    StudentID = user.StudentID,
                    Address = user.Address,
                    Email = user.Email,
                    Status = Status.Active,
                    StatusDesc = Status.Active.ToString(),
                    Gender = user.Gender,
                    GenderDesc = user.Gender.ToString(),
                    CreatedDate = DateTime.Now,
                    UserName = user.Email
                };
                if (user.Email != null)
                {
                    newUser.EmailConfirmed = true;
                    newUser.Email = user.Email;
                    newUser.NormalizedEmail = user.Email;
                }

                var result = await _userManager.CreateAsync(newUser, user.Password);
                if (!result.Succeeded)
                {
                    var errors = result.Errors.Select(c => c.Description);
                    return ResultResponse.Failure(errors);
                }

                newUser.UserId = newUser.Id;
                await _userManager.UpdateAsync(newUser);

                var student = new Student
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    StudentID = user.StudentID,
                    Address = user.Address,
                    Gender = user.Gender,
                    GenderDesc = user.Gender.ToString(),
                    UserId = newUser.Id,
                    Status = Status.Active,
                    SchoolName = user.SchoolName
                };

                await _context.Students.AddAsync(student);
                await _context.SaveChangesAsync();

                var newUserEmail = new WelcomeRequest
                {
                    UserName = $"{user.FirstName} {user.LastName}",
                    ToEmail = user.Email
                };
                var confirmSentMail = await _mailService.SendWelcomeEmailAsync(newUserEmail);
                if (!confirmSentMail)
                {
                    return ResultResponse.Failure("Cannot create user");
                }


                return ResultResponse.Success("User has been succwssfully created", student);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<ResultResponse> GenerateOTP(string email)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    throw new Exception("User with that details was not found");
                }

                var otp = await _userManager.GenerateTwoFactorTokenAsync(user, "Email");
                if (otp == null)
                {
                    throw new Exception("Unable to create OTP");
                }

                var resetPasswordRequest = new ResetPassword
                {
                    token = otp,
                    email = user.Email
                };

                var sendEmail = await _mailService.ResetPasswordAsync(resetPasswordRequest);
                if (!sendEmail)
                {
                    throw new Exception("Error occured while reseting user password");
                }
                return ResultResponse.Success(otp);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public Task<ResultResponse> GenerateToken(string email)
        {
            throw new NotImplementedException();
        }

        public async Task<ResultResponse> GetUserByEmail(string email)
        {
            try
            {
                var findUser = await _context.Students.FirstOrDefaultAsync(x => x.Email == email);
                if (findUser == null)
                {
                    throw new Exception("Student with this mail does not exist");
                }
                return ResultResponse.Success(findUser);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<ResultResponse> loginAsync(string email, string password)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    return ResultResponse.Failure("Invalid login details");
                }

                var checkPassword = await _userManager.CheckPasswordAsync(user, password);
                if (!checkPassword)
                {
                    return ResultResponse.Failure("Invalid Login details");
                }

                var jwtToken = GenerateJwtToken(user.Id, user.Email);
                return ResultResponse.Success(jwtToken);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public Task<ResultResponse> ResetPassword(string email, string password)
        {
            throw new NotImplementedException();
        }

        /*public async Task<string> UploadFile(string name, string userid)
        {
            try
            {
                *//*string filePath = Directory.GetCurrentDirectory() + $"\\{file.Name}_{file.UserId}.jpg";
                Account account = new Account { ApiKey = _config["cloudinary:key"], ApiSecret = _config["cloudinary:secret"], Cloud = _config["cloudinary:cloudname"] };
                Cloudinary cloudinary = new Cloudinary(account);
                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(filePath)
                };

                var uploadResult = cloudinary.Upload(uploadParams);
                string fileUrl = uploadResult.SecureUri.AbsoluteUri;

                return fileUrl;*//*
                return "";
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }*/

        public async Task<bool> validateOTP(string email, string otp, string password)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    throw new Exception("User not found");
                }
                var validate = await _userManager.VerifyTwoFactorTokenAsync(user, "Email", otp);
                if (!validate)
                {
                    return false;
                }
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var res = await _userManager.ResetPasswordAsync(user, token, password);
                if (res.Succeeded)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private string GenerateJwtToken(string UserId, string email)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_config["TokenConstants:key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(JwtRegisteredClaimNames.Email, email),
                    new Claim("userId", UserId),
                    new Claim(JwtRegisteredClaimNames.Sub, email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(6),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = jwtTokenHandler.WriteToken(token);
            return jwtToken;
        }
    }
}
