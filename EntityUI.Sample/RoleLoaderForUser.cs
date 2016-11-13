using System.Collections.Generic;
using System.Linq;

namespace EntityUI.Sample
{
    public class DefaultLoader<T> : PropertyLoader<T>
    {
        private readonly IEntityProvider _entityProvider;
        public DefaultLoader(IEntityProvider entityProvider)
        {
            _entityProvider = entityProvider;
        }

        protected override IList<T> LoadItems()
        {
            return _entityProvider.GetList<T>().ToList();
        }
    }
}