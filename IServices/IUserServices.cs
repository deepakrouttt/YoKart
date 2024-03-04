using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using YoKart.Models;

namespace YoKart.IServices
{
    public interface IUserServices
    {
        Task<(ClaimsPrincipal, AuthenticationProperties, User)> ValidateUser(LoginUser _login);
    }
}