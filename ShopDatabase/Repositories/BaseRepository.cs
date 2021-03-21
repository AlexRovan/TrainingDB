using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShopDatabase.Repositories
{
    public class BaseRepository<T> : IRepository<T> where T : class
    {
        protected DbContext _context;
        protected DbSet<T> _dbSet;

        public BaseRepository(DbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public virtual T GetById(int id)
        {
            return _dbSet.Find(id);
        }

        public virtual T[] GetAll()
        {
            return _dbSet.ToArray();
        }

        public virtual void Create(T item)
        {
            _dbSet.Add(item);
        }

        public virtual void Update(T item)
        {
            _dbSet.Attach(item);
            _context.Entry(item).State = EntityState.Modified;
        }

        public virtual void Delete(T item)
        {
            if (_context.Entry(item).State == EntityState.Detached)
            {
                _dbSet.Attach(item);
            }

            _dbSet.Remove(item);
        }

        public virtual void Save()
        {
            _context.SaveChanges();
        }
    }
}
