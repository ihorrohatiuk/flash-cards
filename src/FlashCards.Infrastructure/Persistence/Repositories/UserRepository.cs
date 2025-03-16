using FlashCards.Core.Domain.Entities;
using FlashCards.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace FlashCards.Infrastructure.Persistence.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }
    
    public IEnumerable<User> GetAll() //TODO: Add pagination
    {
        return _context.Set<User>()
            .AsNoTracking()
            .AsEnumerable();
    }

    public async Task<User?> GetByIdAsync(Guid id)
    {
        return await _context.Set<User>()
            .FindAsync(id);
    }
    
    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _context.Set<User>()
            .AsNoTracking() 
            .FirstOrDefaultAsync(user => user.Email == email);
    }

    public async Task AddAsync(User user)
    {
        await _context.Set<User>().AddAsync(user);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(User user)
    {
       _context.Entry(user).State = EntityState.Modified;
       await _context.SaveChangesAsync();
    }
    
    public async Task DeleteAsync(Guid id)
    {
        var user = await _context.Set<User>().FindAsync(id);
        if (user != null)
        {
            _context.Set<User>().Remove(user);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> Exists(Guid id)
    {
        return await _context.Set<User>().AnyAsync(u => u.Id == id);
    }
    
    public async Task<bool> Exists(string email)
    {
        return await _context.Set<User>().AnyAsync(u => u.Email == email);
    }
}