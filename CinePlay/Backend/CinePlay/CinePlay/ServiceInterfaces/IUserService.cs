using Microsoft.AspNetCore.Identity;
using MyMDb.DTOs;
using MyMDb.Models;
using MyMDb.RepositoryInterfaces;

namespace MyMDb.ServiceInterfaces
{
    public interface IUserService
    {
        public Task<AppUser> UpdateUserAsync(AppUser user);

        public Task<UserProfile?> CreateUserProfileAsync(UserProfile profile);
        public Task<UserProfile?> GetUserProfileAsync(string userId);
        public Task<UserProfile?> EditUserProfileAsync(string userId, ProfileDto editedProfile);
    }
}
