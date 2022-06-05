using Dot.Application.ResponseModel;
using Dot.Core.Entities;
using Dot.Core.Enums;
using Dot.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dot.Infrastructure.Application.WalletCommand
{
    internal class WalletCreationHelper
    {
        private readonly ApplicationDbContext _context;
        public WalletCreationHelper(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> WalletCreaion(CreateWalletCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Creating dynamic account
                Random rnd = new Random(DateTime.Now.Millisecond);
                int key = 0;
                key = rnd.Next(1000000000, int.MaxValue);
                switch (request.UserType)
                {
                    case UserType.Student:
                        var findStudent = await _context.Students.FirstOrDefaultAsync(c => c.UserId == request.UserId && c.Status == Status.Active);
                        if (findStudent == null)
                        {
                            return false;
                        }
                        var newStudentWallet = new Wallet
                        {
                            WalletAccountNumber = key.ToString(),
                            ClosingBalance = 0,
                            Balance = 0,
                            Email = request.Email,
                            PhoneNumber = request.PhoneNumber,
                            UserId = request.UserId,
                            UserName = $"{findStudent.FirstName} {findStudent.LastName}",
                            UserType = UserType.Student
                        };
                        await _context.Wallets.AddAsync(newStudentWallet);
                        break;
                    case UserType.Client:
                        var findClient = await _context.Clients.FirstOrDefaultAsync(c => c.UserId == request.UserId && c.Status == Status.Active);
                        if (findClient == null)
                        {
                            return false;
                        }
                        var newClientWallet = new Wallet
                        {
                            WalletAccountNumber = key.ToString(),
                            ClosingBalance = 0,
                            Balance = 0,
                            Email = request.Email,
                            PhoneNumber = request.PhoneNumber,
                            UserId = request.UserId,
                            UserName = $"{findClient.FullName}",
                            UserType = UserType.Client
                        };
                        await _context.Wallets.AddAsync(newClientWallet);
                        break;
                    default:
                        break;
                }

                await _context.SaveChangesAsync(cancellationToken);
                return true;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
