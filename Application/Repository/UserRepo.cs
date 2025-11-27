using Domain.Models;
using Infrastructure.Data;
using Infrastructure.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static Microsoft.AspNetCore.Hosting.Internal.HostingApplication;

namespace Application.Repository
{
    public class UserRepo : IUserRepo
    {
        private readonly YoKartDbContext _context;

        public UserRepo(YoKartDbContext context)
        {
            _context = context;
        }

        public async Task<List<User>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }
        public async Task<User> GetUser(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
            return user;
        }
        public async Task<User?> ValidateUser(LoginUser login)
        {
            if (string.IsNullOrEmpty(login.Username) || string.IsNullOrEmpty(login.Password))
                return null;

            return await _context.Users.FirstOrDefaultAsync(s => s.Username == login.Username && s.Password == login.Password);
        }


    }
}