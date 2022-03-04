using System.Text.Json;
using System.IdentityModel.Tokens.Jwt;
using KABINET_Domain.Entities;
using KABINET_Application.ViewModels.User;

namespace KABINET_Application.Helpers
{
    public static class JwtTokenHelper
    {
        public static string ConvertToToken(User user)
        {
            var JWTHeader = new JwtHeader();

            var payload = new JwtPayload
            {
                { "Id" ,user.Id},
                { "Email", user.Email},
                { "FirstName", user.FirstName},
                { "LastName", user.LastName },
                { "Room", user.Room },
                { "Role", user.Role },
                { "Warnings", user.Warnings }
            };

            var secToken = new JwtSecurityToken(JWTHeader, payload);
            var handler = new JwtSecurityTokenHandler();
            var tokenString = handler.WriteToken(secToken);

            return tokenString;
        }

        public static UserVm DecodeToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var tokenString = handler.ReadToken(token);
            string[] tokenPieces = tokenString.ToString().Split("{}.");

            string userObjStr = tokenPieces[1];

            var result = JsonSerializer.Deserialize<UserVm>(userObjStr);

            return result;
        }
    }
}
