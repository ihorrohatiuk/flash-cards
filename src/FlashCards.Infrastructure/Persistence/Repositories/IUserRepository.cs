using FlashCards.Core.Domain.Entities;
using FlashCards.Infrastructure.Persistence.DataModels;

namespace FlashCards.Infrastructure.Persistence.Repositories;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByEmailAsync(string email);
    Task<bool> Exists(string email);
}