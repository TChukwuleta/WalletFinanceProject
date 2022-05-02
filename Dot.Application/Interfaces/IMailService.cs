using Dot.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dot.Application.Interfaces
{
    public interface IMailService
    {
        Task<bool> SendRequestEmailAsync(MailRequest mailRequest);
        Task<bool> SendWelcomeEmailAsync(WelcomeRequest mailRequest);
    }
}
