using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace EntityUI
{
    public abstract class PropertyLoader<T> : IPropertyLoader
    {
        public IList Load()
        {
            return LoadItems().ToList();
        }

        protected abstract IList<T> LoadItems();        
    }
}