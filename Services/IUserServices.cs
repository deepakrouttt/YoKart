using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using YoKart.Models;

namespace YoKart.Services
{
    public interface IUserServices
    {
        Task<(ClaimsPrincipal, AuthenticationProperties,User)> ValidateUser(LoginUser _login);
    }
}