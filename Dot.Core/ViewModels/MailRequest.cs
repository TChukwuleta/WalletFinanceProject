using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dot.Core.ViewModels
{
    public class MailRequest
    {
        public string ToEmail { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        /*public List<string> Attachments { get; set; }*/
        /*public List<IFormFile> Attachments { get; set; }*/ // Coming back to handling file attachments
    }
}
