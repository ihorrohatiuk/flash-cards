using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlashCards.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace FlashCards.Api.Data.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }
    
    public IEnumerable<User> GetAllUsers() 
    {
        return _context.Set<User>().AsEnumerable();
    }

    public User? GetUserById(int id)
    {
        return _context.Set<User>().Find(id);
    }

    public void AddUser(User user)
    {
        _context.Set<User>().Add(user);
        _context.SaveChanges();
    }

    public void UpdateUser(User user)
    {
       _context.Entry(user).State = EntityState.Modified;
       _context.SaveChanges();
    }
    
    public void DeleteUser(int id)
    {
        User? user = _context.Set<User>().Find(id);
        if (user != null)
        {
            _context.Set<User>().Remove(user);
            _context.SaveChanges();
        }
    }
}