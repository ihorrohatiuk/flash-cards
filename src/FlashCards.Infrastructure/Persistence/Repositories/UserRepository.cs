using FlashCards.Core.Domain.Entities;
using FlashCards.Infrastructure.Persistence.Contexts;
using FlashCards.Infrastructure.Persistence.DataModels;
using Microsoft.EntityFrameworkCore;

namespace FlashCards.Infrastructure.Persistence.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }
    
    public IEnumerable<UserEntity> GetAll() //TODO: Add pagination
    {
        return _context.Set<UserEntity>()
            .AsNoTracking()
            .AsEnumerable();
    }

    public async Task<UserEntity?> GetByIdAsync(Guid id)
    {
        return await _context.Set<UserEntity>()
            .FindAsync(id);
    }
    
    public async Task<UserEntity?> GetByEmailAsync(string email)
    {
        return await _context.Set<UserEntity>()
            .AsNoTracking() 
            .FirstOrDefaultAsync(user => user.Email == email);
    }

    public async Task AddAsync(UserEntity userEntity)
    {
        await _context.Set<UserEntity>().AddAsync(userEntity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(UserEntity userEntity)
    {
       _context.Entry(userEntity).State = EntityState.Modified;
       await _context.SaveChangesAsync();
    }
    
    public async Task DeleteAsync(Guid id)
    {
        var user = await _context.Set<UserEntity>().FindAsync(id);
        if (user != null)
        {
            _context.Set<UserEntity>().Remove(user);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> Exists(Guid id)
    {
        return await _context.Set<UserEntity>().AnyAsync(u => u.Id == id);
    }
    
    public async Task<bool> Exists(string email)
    {
        return await _context.Set<UserEntity>().AnyAsync(u => u.Email == email);
    }
}