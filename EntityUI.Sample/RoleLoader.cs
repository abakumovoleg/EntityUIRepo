using System.Collections.Generic;
using System.Linq;

namespace EntityUI.Sample
{
    public class RoleLoader : PropertyLoader<RoleDto, UserDto>
    {
        private readonly IEntityProvider _entityProvider;
        public RoleLoader(IEntityProvider entityProvider)
        {
            _entityProvider = entityProvider;
        }

        protected override IList<RoleDto> LoadItems(UserDto state)
        {
            return _entityProvider.GetList<RoleDto>().Where(x => x.Filial.Contains(state.Filial))
                .ToList();
        }
    }
}