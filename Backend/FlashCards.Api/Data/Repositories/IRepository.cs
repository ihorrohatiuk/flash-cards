using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FlashCards.Api.Data.Models;

namespace FlashCards.Api.Data.Repositories;

public interface IRepository<T> where T : class
{
    IEnumerable<T> GetAll();
    Task<T?> GetByIdAsync(Guid id);
    Task AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(Guid id);
    Task<bool> Exists(Guid id);
}