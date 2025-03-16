using FlashCards.Core.Domain.Entities;

namespace FlashCards.Infrastructure.Persistence.Repositories;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByEmailAsync(string email);
    Task<bool> Exists(string email);
}