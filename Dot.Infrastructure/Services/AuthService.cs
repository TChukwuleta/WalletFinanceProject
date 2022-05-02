using Dot.Application.Interfaces;
using Dot.Application.ResponseModel;
using Dot.Core.Entities;
using Dot.Core.Enums;
using Dot.Core.ViewModels;
using Dot.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

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
                if(user == null)
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
                if(existingUser != null)
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
                if(user.Email != null)
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
                    Status = Status.Active
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

        public Task<ResultResponse> ForgotPassword(string email)
        {
            throw new NotImplementedException();
        }

        public async Task<ResultResponse> loginAsync(string email, string password)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(email);
                if(user == null)
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

                throw;
            }
        }

        public Task<ResultResponse> ResetPassword(string email, string password)
        {
            throw new NotImplementedException();
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
