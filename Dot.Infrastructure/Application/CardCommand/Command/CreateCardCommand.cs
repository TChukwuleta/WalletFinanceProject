using Dot.Application.ResponseModel;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dot.Infrastructure.Application.CardCommand.Command
{
    public class CreateCardCommand : IRequest<ResultResponse>
    {
        public string UserId { get; set; }
    }

    public class CreateCardCommandHandler : IRequestHandler<CreateCardCommand, ResultResponse>
    {
        public Task<ResultResponse> Handle(CreateCardCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
