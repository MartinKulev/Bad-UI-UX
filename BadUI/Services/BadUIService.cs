using BadUI.Data;
using BadUI.Data.Dtos;
using BadUI.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace BadUI.Services
{
    public class BadUIService : IBadUIService
    {
        private readonly BadUIDbContext _context;

        public BadUIService(BadUIDbContext badUIDbContext)
        {
            _context = badUIDbContext;
        }

        public async Task<StatusDto> CreateUser(string username, string password, int gender)
        {
            CustomUser userSameUsername = _context.Users.FirstOrDefault(u => u.Username == username);
            CustomUser userSamePassword = _context.Users.FirstOrDefault(u => u.Password == password);

            if (userSameUsername != null)
            {
                return new StatusDto
                {
                    Message = $"User with that username already exists! The password is <b>{userSameUsername.Password}</b> if you meant to login.",
                    IsSuccess = false
                };
            }

            if (userSamePassword != null)
            {
                return new StatusDto
                {
                    Message = $"Cannot use that password. Password already used by <b>{userSamePassword.Username}</b>.",
                    IsSuccess = false
                };
            }

            if (password.Length < 3)
            {
                return new StatusDto
                {
                    Message = "Password must be at least 3 characters long.",
                    IsSuccess = false
                };
            }

            await _context.Users.AddAsync(new CustomUser
            {
                Username = username,
                Password = password,
                Gender = gender
            });
            await _context.SaveChangesAsync();

            return new StatusDto
            {
                Message = "User created successfully.",
                IsSuccess = true
            };
        }

        public async Task<StatusDto> Login(string username, string password)
        {
            CustomUser userWithThatPassword = _context.Users.FirstOrDefault(u => u.Password == password);
            CustomUser userWithThatUsername = _context.Users.FirstOrDefault(u => u.Username == username);
            CustomUser user = _context.Users.FirstOrDefault(u => u.Username == username && u.Password == password);

            if(user != null)
            {
                return new StatusDto
                {
                    Message = $"Successful login.",
                    IsSuccess = true
                };
            }

            if (userWithThatPassword != null)
            {
                return new StatusDto
                {
                    Message = $"That is the password of <b>{userWithThatPassword.Username}</b>.",
                    IsSuccess = false
                };
            }

            if (userWithThatUsername != null)
            {
                return new StatusDto
                {
                    Message = $"Wrong password for <b>{username}</b>. Want to play hangman to guess your password?",
                    IsSuccess = false,
                    Hangman = userWithThatUsername.Password
                };
            }

            if (!_context.Users.Any(p => p.Username == username))
            {
                return new StatusDto
                {
                    Message = $"User with that username doesn't exist.",
                    IsSuccess = false
                };
            }

            return new StatusDto();
        }

        public async Task<CustomUser> LoginAsRandomUser()
        {
            List<CustomUser> customUsers = await _context.Users.ToListAsync();
            Random random = new Random();
            CustomUser customUser = customUsers[random.Next(0, customUsers.Count)];
            return customUser;
        }

        public async Task<CustomUser> GetCustomUser(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task DeleteUser(string username)
        {
            CustomUser customUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
            _context.Remove(customUser);
            await _context.SaveChangesAsync();
        }
    }
}
