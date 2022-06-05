﻿using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Dot.Application.Interfaces;
using Dot.Core.ViewModels;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dot.Infrastructure.Services
{
    public class UploadService : IUploadService
    {
        private readonly IConfiguration _config;
        /*private readonly Cloudinary _cloudinary;*/

        public UploadService(IConfiguration config)
        {
            _config = config;
            /*_cloudinary = cloudinary;*/
        }

        public async Task<string> FromBase64ToFile(string base64File, string filename)
        {
            try
            {
                var fileLocation = "";
                if (!string.IsNullOrEmpty(base64File))
                {
                    if (base64File.Contains("https:"))
                    {
                        return base64File;
                    }
                    var imagebytes = Convert.FromBase64String(base64File);
                    if(imagebytes.Length > 0)
                    {
                        string file = Path.Combine(Directory.GetCurrentDirectory(), filename);
                        using (var stream = new FileStream(file, FileMode.Create))
                        {
                            stream.Write(imagebytes, 0, imagebytes.Length);
                            stream.Flush();
                        }
                        fileLocation = file;
                    }
                    else
                    {
                        return "";
                    }

                }
                else
                {
                    return "";
                }

                return fileLocation;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<string> UploadImage(string username, string userid)
        {
            try
            {
                var fileLocation = Directory.GetCurrentDirectory() + $"\\{username}_{userid}.jpg";
                Account account = new Account
                {
                    ApiKey = _config["cloudinary:key"],
                    ApiSecret = _config["cloudinary:secret"],
                    Cloud = _config["cloudinary:cloudname"]
                };
                Cloudinary cloudinary = new Cloudinary(account);
                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(fileLocation)
                };

                var uploadResult = await cloudinary.UploadAsync(uploadParams);
                if(uploadResult.Error != null)
                {
                    throw new Exception("An error occured while uploading document");
                }

                //var fileUrl = uploadResult.Uri.ToString();
                string fileUrl = uploadResult.SecureUri.AbsoluteUri;

                return fileUrl;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
