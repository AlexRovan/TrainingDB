using System;
using System.Collections.Generic;
using System.Text;

namespace ShopDatabase.Repositories
{
    public interface IRepository<T> where T : class
    {
        void Create(T item);

        void Delete(T item);

        void Update(T item);

        T GetById(int id);

        T[] GetAll();
    }
}
