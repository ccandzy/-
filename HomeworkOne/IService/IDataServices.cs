using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IService
{
    public interface IDataServices
    {
        T Quary<T>(int id) where T : BaseModel;
        List<T> QuaryList<T>() where T : BaseModel, new();
        bool Update<T>(T t) where T : BaseModel;
        bool Insert<T>(T t) where T : BaseModel;
        bool Delete<T>(T t) where T : BaseModel;
    }
}
