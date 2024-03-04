using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Newtonsoft.Json;
using NuGet.Protocol.Plugins;
using System.Security.Claims;
using System.Security.Policy;
using System.Text;
using YoKart.Models;
using YoKart.IServices;

namespace YoKart.Services
{
    public class UserServices : IUserServices
    {
        private readonly HttpClient _client = new HttpClient();
        private readonly string url = "https://localhost:44373/api/UserApi/Login";

        public async Task<(ClaimsPrincipal, AuthenticationProperties,User)> ValidateUser(LoginUser _login)
        {
            var data = JsonConvert.SerializeObject(_login);
            StringContent stringContent = new StringContent(data, Encoding.UTF8, "application/json");

            using (var response = await _client.PostAsync(url, stringContent))
            {
                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    var user = JsonConvert.DeserializeObject<User>(result);

                    if (user != null)
                    {
                        var claims = new List<Claim>
                                 {
                                     new Claim(ClaimTypes.Name, user.Username),
                                     new Claim(ClaimTypes.Email, user.Email),
                                 };

                        foreach (var role in user.Roles.Split(','))
                        {
                            claims.Add(new Claim(ClaimTypes.Role, role.Trim()));
                        }

                        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        var principal = new ClaimsPrincipal(identity);

                        var authProperties = new AuthenticationProperties
                        {
                            IsPersistent = true,
                        };

                        return (principal, authProperties,user);
                    }
                    if (result.Contains("Unauthorized("))
                    {
                        return (null, null,null);
                    }
                }
            }
            return (null, null,null);
        }

    }
}