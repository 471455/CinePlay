using MyMDb.Models;

namespace MyMDb.RepositoryInterfaces
{
    public interface IUserRepository
    {
        Task<AppUser> UpdateUserAsync(AppUser user);

        Task<UserProfile> AddProfileAsync(UserProfile profile);
        Task<UserProfile?> GetProfileByUserIdAsync(string Id);
        Task<UserProfile?> UpdateProfileAsync(UserProfile profile);
    }
}
