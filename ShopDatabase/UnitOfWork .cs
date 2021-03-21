using System;
using System.Collections.Generic;
using System.Text;
using ShopDatabase.Repositories;

namespace ShopDatabase
{
    class UnitOfWork:IUnitOfWork
    {
        private ShopDbContext _db;
        
        public UnitOfWork(ShopDbContext db) 
        { 
            _db = db; 
        }

        public void Save() 
        {
            _db.SaveChanges(); 
        }

        public void Dispose()
        {
            _db.Dispose();
        }

        public T GetRepository<T>() where T:class
        {
            if (typeof(T) == typeof(IProductRepository))
            { 
                return new ProductRepository(_db) as T; 
            }
            
            throw new Exception("Неизвестный тип репозитория: "+ typeof(T));
        }
    }
}
