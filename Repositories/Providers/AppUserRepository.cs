using Microsoft.EntityFrameworkCore;
using Template.MinimalApi.NET8.Data;
using Template.MinimalApi.NET8.Data.Entities;
using Template.MinimalApi.NET8.Repositories.Interfaces;

namespace Template.MinimalApi.NET8.Repositories.Providers;

public class AppUserRepository : IAppUserRepository
{
    private readonly ApplicationDbContext _context;

    public AppUserRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> CreateAsync(AppUser user)
    {
        _context.Users.Add(user);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<AppUser?> GetAsync(string id)
    {
        return await _context.Users.FindAsync(id);
    }

    public async Task<AppUser?> GetByUseNameAndEmailAsync(string userName, string email)
    {
        return await _context.Users.AsNoTracking()
            .FirstOrDefaultAsync(x => userName.Equals(x.UserName) && email.Equals(x.Email));
    }

    public async Task<AppUser?> GetByUseNameAsync(string userName)
    {
        return await _context.Users.AsNoTracking()
            .FirstOrDefaultAsync(x => userName.Equals(x.UserName));
    }

    public IQueryable<AppUser> GetAsQueryable()
    {
        return _context.Users.AsNoTracking().AsQueryable();
    }

    public async Task<bool> UpdateAsync(AppUser user)
    {
        _context.Users.Update(user);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteAsync(string id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
        {
            return false;
        }

        _context.Users.Remove(user);
        return await _context.SaveChangesAsync() > 0;
    }
}