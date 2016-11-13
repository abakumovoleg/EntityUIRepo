using System.Collections.Generic;

namespace EntityUI
{
    public interface IEntityProvider
    {
        void Add<T>(T entity);
        void Update<T>(T entity);
        void Delete<T>(T entity);
        IEnumerable<T> GetList<T>();
    }
}