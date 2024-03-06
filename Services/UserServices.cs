using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Newtonsoft.Json;
using NuGet.Protocol.Plugins;
using System.Security.Claims;
using System.Security.Policy;
using System.Text;
using YoKart.Models;
using YoKart.IServices;
using YoKartApi.Models;
using System.Net.Http.Headers;
using NuGet.Common;
using System.IdentityModel.Tokens.Jwt;

namespace YoKart.Services
{
    public class UserServices : IUserServices
    {
        private readonly HttpClient _client = new HttpClient();
        private readonly string url = "https://localhost:44373/api/UserApi/Login";

        public async Task<String> ValidateUser(LoginUser _login)
        {
            var data = JsonConvert.SerializeObject(_login);
            StringContent stringContent = new StringContent(data, Encoding.UTF8, "application/json");
            using (var response = await _client.PostAsync(url, stringContent))
            {
                if (response.IsSuccessStatusCode)
                {
                    string validateToken = await response.Content.ReadAsStringAsync();

                    if (validateToken != null)
                    {
                        var decodedValue = new JwtSecurityTokenHandler().ReadJwtToken(validateToken);

                        myVar.UserId = Int32.Parse(decodedValue.Claims.FirstOrDefault(x => x.Type == "Id").Value);
                        myVar.Roles = decodedValue.Claims.FirstOrDefault(x => x.Type == "Roles").Value;
                        myVar.UserName = decodedValue.Claims.FirstOrDefault(x => x.Type == "UserName").Value;

                        return validateToken;
                    }
                    if (validateToken.Contains("Unauthorized"))
                    {
                        return null;
                    }
                }
            }
            return null;
        }

    }
}