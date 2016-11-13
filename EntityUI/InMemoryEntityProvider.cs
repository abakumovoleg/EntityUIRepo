using System;
using System.Collections.Generic;
using System.Linq;

namespace EntityUI
{
    public class InMemoryEntityProvider : IEntityProvider
    {
        public Dictionary<Type, List<object>> Storage = new Dictionary<Type, List<object>>();

        private List<object> GetStorage<T>()
        {
            if (!Storage.ContainsKey(typeof(T)))
                Storage[typeof(T)] = new List<object>();

            return Storage[typeof(T)];
        }


        public void Add<T>(T entity)
        {
            GetStorage<T>().Add(entity);
        }

        public void Delete<T>(T entity)
        {
            GetStorage<T>().Remove(entity);
        }

        public IEnumerable<T> GetList<T>()
        {
            return GetStorage<T>().Cast<T>();
        }

        public void Update<T>(T entity)
        {
            GetStorage<T>().Remove(entity);
            GetStorage<T>().Add(entity);
        }
    }
}