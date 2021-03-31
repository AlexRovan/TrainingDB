using System;
using Microsoft.EntityFrameworkCore.Storage;
using ShopDatabase.Repositories;

namespace ShopDatabase
{
    class UnitOfWork : IUnitOfWork
    {
        private ShopDbContext _db;

        private IDbContextTransaction dbTransaction;

        public UnitOfWork(ShopDbContext db)
        {
            _db = db;
        }

        public void Save()
        {
            if (!Equals(dbTransaction, null))
            {
                dbTransaction.Commit();
            }

            _db.SaveChanges();
        }

        public void Dispose()
        {
            if (!Equals(dbTransaction, null))
            {
                dbTransaction.Rollback();
            }

            _db.Dispose();
        }

        public T GetRepository<T>() where T : class
        {
            if (typeof(T) == typeof(IProductRepository))
            {
                return new ProductRepository(_db) as T;
            }

            if (typeof(T) == typeof(ICustomersRepository))
            {
                return new CustomersRepository(_db) as T;
            }

            if (typeof(T) == typeof(ICategoryRepository))
            {
                return new CategoryRepository(_db) as T;
            }

            if (typeof(T) == typeof(IOrderRepository))
            {
                return new OrderRepository(_db) as T;
            }

            if (typeof(T) == typeof(IPositionsOrderRepository))
            {
                return new PositionsOrderRepository(_db) as T;
            }

            throw new Exception("Неизвестный тип репозитория: " + typeof(T));
        }

        public void BeginTransaction()
        {
            dbTransaction = _db.Database.BeginTransaction();
        }
    }
}
