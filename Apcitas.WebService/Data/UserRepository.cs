using Apcitas.WebService.Entities;
using Apcitas.WebService.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Apcitas.WebService.Data;

public class UserRepository : IUserRepository
{
    private readonly DataContext _context;
    public UserRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<AppUser> GetUserByIdAsync(int id)
    {
        return await _context.Users.FindAsync(id);
    }

    public async Task<AppUser> GetUserByUsernameAsync(string userName)
    {
        return await _context.Users
            .Include(p => p.Photos)
            .SingleOrDefaultAsync(x => x.UserName == userName);
    }

    public async Task<IEnumerable<AppUser>> GetUsersAsync()
    {
        return await _context.Users.Include(p => p.Photos).ToListAsync();
    }

    public async Task<bool> SaveAllAsync()
    {
        return await _context.SaveChangesAsync() > 0;
    }

    public void Update(AppUser user)
    {
        _context.Entry(user).State = EntityState.Modified;
    }

}
