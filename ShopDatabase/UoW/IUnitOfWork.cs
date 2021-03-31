using System;

namespace ShopDatabase
{
    interface IUnitOfWork:IDisposable
    {
        void Save();
        
        T GetRepository<T>() where T: class;

        void BeginTransaction();
    }
}
