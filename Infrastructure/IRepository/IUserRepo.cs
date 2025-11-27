using Domain.Models;

namespace Infrastructure.IRepository
{
    public interface IUserRepo
    {
        Task<List<User>> GetUsers();
        Task<User> GetUser(int id);
        Task<User?> ValidateUser(LoginUser _login);  
    }
}