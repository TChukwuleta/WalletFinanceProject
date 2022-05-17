using Dot.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dot.Application.Interfaces
{
    public interface IUploadService
    {
        Task<string> UploadImage(string username, string userid);
        Task<string> FromBase64ToFile(string base64File, string filename);
    }
}
