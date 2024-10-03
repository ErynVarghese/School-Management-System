using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMS.Repositories
{
    interface ICommon<T>
    {
        List<T> GetAll();

        string Create(T obj);

        string Update(T obj);

        string Delete(int Id);

        T GetById(int Id);
    }
}