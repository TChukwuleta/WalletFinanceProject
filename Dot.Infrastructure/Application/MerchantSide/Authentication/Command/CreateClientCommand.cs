using Dot.Application.Interfaces;
using Dot.Application.ResponseModel;
using Dot.Core.Entities.MerchantSide;
using Dot.Core.Enums;
using Dot.Infrastructure.Application.WalletCommand;
using Dot.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dot.Infrastructure.Application.MerchantSide.Authentication.Command
{
    public class CreateClientCommand : IRequest<ResultResponse>
    {
        public string FullName { get; set; }
        public string Password { get; set; }
        public string CompanyName { get; set; }
        public string CompanyEmail { get; set; }
        public string Country { get; set; }
        public string PhoneNumber { get; set; }
        public string BusinessRegNo { get; set; }
        public string Role { get; set; }
        public ComapnySector ComapnySector { get; set; }
        public int Parent { get; set; }
    }

    public class CreateClientCommandHandler : IRequestHandler<CreateClientCommand, ResultResponse>
    {
        private readonly IAuthService _authService;
        private readonly ApplicationDbContext _context;
        public CreateClientCommandHandler(IAuthService authService, ApplicationDbContext context)
        {
            _authService = authService;
            _context = context;
        }
        public async Task<ResultResponse> Handle(CreateClientCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var findParent = new Client();
                if(request.Parent > 0)
                {
                    findParent = await _context.Clients.FirstOrDefaultAsync(c => c.Id == request.Parent);
                    if(findParent == null)
                    {
                        return ResultResponse.Failure("Parent account does not exist");
                    }
                }
                var newClient = new Client
                {
                    FullName = request.FullName,
                    CompanyName = request.CompanyName,
                    CompanyEmail = request.CompanyEmail,
                    Country = request.Country,
                    PhoneNumber = request.PhoneNumber,
                    BusinessRegNo = request.BusinessRegNo,
                    Role = request.Role,
                    ComapnySector = request.ComapnySector,
                    ParentId = request.Parent > 0 ? findParent.Id : 0,
                    ParentFullName = request.Parent > 0 ? findParent.FullName : ""
                };

                var createClient = await _authService.CreateClientAsync(newClient, request.Password);
                if (!createClient.Succeeded)
                {
                    var errorMessage = "";
                    if (createClient.Messages != null)
                    {
                        if (createClient.Messages.Count() > 0)
                        {
                            foreach (var item in createClient.Messages)
                            {
                                errorMessage = item;
                            }
                            return ResultResponse.Failure($"Unable to create client. {errorMessage}");
                        }
                    }
                    return ResultResponse.Failure($"Unable to create client. {createClient.Message}");
                }

                var createWallet = new CreateWalletCommand
                {
                    UserType = UserType.Client,
                    Email = request.CompanyEmail,
                    PhoneNumber = request.PhoneNumber,
                    UserId = Convert.ToString(createClient.Entity)
                };

                var walletCreationHelper = new WalletCreationHelper(_context);
                var clientWallet = await walletCreationHelper.WalletCreaion(createWallet, cancellationToken);
                if (!clientWallet)
                {
                    return ResultResponse.Failure("An Error occured while trying to create a wallet for the client");
                }

                return ResultResponse.Success("Client created successfully");
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}

/*{
    "fullName": "Tobe Leta",
  "password": "Tobeleta@1",
  "companyName": "Tobe And Sons",
  "companyEmail": "tobeandsons@yopmail.com",
  "country": "string",
  "phoneNumber": "string",
  "businessRegNo": "string",
  "role": "string",
  "comapnySector": 1,
  "parentId": 0
}*/