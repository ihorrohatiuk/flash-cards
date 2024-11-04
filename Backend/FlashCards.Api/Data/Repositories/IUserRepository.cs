using System.Collections.Generic;
using System.Threading.Tasks;
using FlashCards.Api.Models;

namespace FlashCards.Api.Data.Repositories;

public interface IUserRepository
{
    IEnumerable<User> GetAllUsers();
    User? GetUserById(int id);
    void AddUser(User user);
    void UpdateUser(User user);
    void DeleteUser(int id);
}