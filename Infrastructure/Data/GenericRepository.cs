using System;
using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class GenericRepository<T>(StoreContext _context) : IGenericRepository<T> where T : BaseEntity
{
    public void Add(T entity)
    {
        _context.Set<T>().Add(entity);
    }

    public bool Exists(int id)
    {
        return _context.Set<T>().Any(x => x.Id == id);
    }

    public async Task<T?> GetByIdAsync(int id)
    {
        return await _context.Set<T>().FindAsync(id);
    }

    public async Task<IReadOnlyList<T>> ListAllAsync()
    {
        return await _context.Set<T>().ToListAsync();
    }

    public void Remove(T entity)
    {
        _context.Set<T>().Remove(entity);
    }

    public async Task<bool> SaveAllAsync()
    {
        return await _context.SaveChangesAsync() > 0;
    }

    public void Update(T entity)
    {
       _context.Set<T>().Attach(entity);
        _context.Entry(entity).State = EntityState.Modified;
    }
}
