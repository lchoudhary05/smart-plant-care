using GreenMonitor.Models;

namespace GreenMonitor.Interfaces
{
    public interface IUserService
    {
        public Task Register(Users user);
        public Task<Users?> Login(LoginUser user);
        public Task<Users> CheckUser(Users user);
    }
}