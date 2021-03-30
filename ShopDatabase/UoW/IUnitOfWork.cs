using System;
using System.Collections.Generic;
using System.Text;

namespace ShopDatabase
{
    interface IUnitOfWork:IDisposable
    {
        void Save();
        
        T GetRepository<T>() where T: class;

        void BeginTransaction();
    }
}
