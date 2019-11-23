using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSharp_Redis_Razor_Sqlite.Interfaces
{
    public  interface IDataRepository<TEntity>
    {
        IEnumerable<TEntity> GetAllKeys();
        IEnumerable<TEntity> SaveAllKeys();
        IEnumerable<TEntity> GetValue(string key);
        IEnumerable<TEntity> GetKey(string value);
        IEnumerable<TEntity> Add(TEntity entity);
        IEnumerable<TEntity> Update(TEntity oldEntity, TEntity newEntity);
        IEnumerable<TEntity> Delete(TEntity entity);
    }
}
