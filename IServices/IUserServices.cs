using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using YoKart.Models;
using YoKartApi.Models;

namespace YoKart.IServices
{
    public interface IUserServices
    {
        Task<String> ValidateUser(LoginUser _login);
    }
}