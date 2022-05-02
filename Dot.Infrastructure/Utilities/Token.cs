using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dot.Infrastructure.Utilities
{
    public static class Token
    {
        public static JwtSecurityToken ExtractToken(this string str)
        {
            if (str.Contains("Bearer"))
            {
                str = str.Remove(0, 7);
            }
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(str);
            var token = jsonToken as JwtSecurityToken;
            return token;
        }
        public static bool ValidateToken(JwtSecurityToken accessToken, string userId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(userId))
                {
                    throw new Exception("Invalid Token");
                }
                var tokenUser = accessToken.Claims.First(c => c.Type == "userId")?.Value;
                if (tokenUser != userId)
                {
                    throw new Exception("Invalid Token");
                }
                return true;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
