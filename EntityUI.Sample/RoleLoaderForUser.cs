using System;
using System.Collections.Generic;
using System.Linq;

namespace EntityUI.Sample
{
    public class DefaultLoader<T,TE> : PropertyLoader<T,TE>
    {
        private readonly IEntityProvider _entityProvider;
        public DefaultLoader(IEntityProvider entityProvider)
        {
            _entityProvider = entityProvider;
        }

        protected override IList<T> LoadItems(TE state)
        {
            return _entityProvider.GetList<T>().ToList();
        }
    }
}