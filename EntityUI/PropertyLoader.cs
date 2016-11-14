using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace EntityUI
{
    public abstract class PropertyLoader<T,TE> : IPropertyLoader
    {
        public IList Load(object state)
        {
            return LoadItems((TE)state).ToList();
        }

        protected abstract IList<T> LoadItems(TE state);        
    }
}