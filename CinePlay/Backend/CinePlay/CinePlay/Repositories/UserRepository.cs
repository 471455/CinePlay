using Microsoft.EntityFrameworkCore;
using MyMDb.Data;
using MyMDb.Models;
using MyMDb.RepositoryInterfaces;

namespace MyMDb.Repositories
{
    public class UserRepository : Repository<AppUser>, IUserRepository
    {
        public UserRepository(ApplicationDbContext context) : base(context)
        {

        }

        public async Task<UserProfile> AddProfileAsync(UserProfile profile)
        {
            _context.UserProfiles.Add(profile);
            await SaveChangesAsync();
            return profile;
        }

        public async Task<UserProfile?> GetProfileByUserIdAsync(string Id)
        {
            return await _context.UserProfiles
                .FirstOrDefaultAsync(p => p.UserId == Id);
        }

        public async Task<UserProfile?> UpdateProfileAsync(UserProfile profile)
        {
            _context.Entry(profile).State = EntityState.Modified;
            await SaveChangesAsync();
            return profile;
        }

        public async Task<AppUser> UpdateUserAsync(AppUser user)
        {
            _context.Entry(user).State = EntityState.Modified;
            await SaveChangesAsync();
            return user;
        }
    }
}