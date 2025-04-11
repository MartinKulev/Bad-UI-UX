using BadUI.Data.Dtos;
using BadUI.Data.Entities;

namespace BadUI.Services
{
    public interface IBadUIService
    {
        Task<StatusDto> CreateUser(string username, string password, int gender);

        Task<StatusDto> Login(string username, string password);

        Task<CustomUser> LoginAsRandomUser();

        Task<CustomUser> GetCustomUser(string username);

        Task DeleteUser(string username);
    }
}
