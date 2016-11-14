using System.Collections.Generic;

namespace EntityUI.Sample
{
    public class UserDto
    {   
        [Property(ReadOnly = true)]
        public int Id { get; set; }
             
        [Property(MaxLength = 20, Required = true)]
        public string Name { get; set; }

        [Property(Mask = @"(\w|[\.\-])+@(\w|[\-]+\.)*(\w|[\-]){2,63}\.[a-zA-Z]{2,4}", MaskType = MaskType.RegularExpression)]
        public string Email { get; set; }

        [Property(MaxValue = 100, MinValue = 0)]
        public int? Age { get; set; }
        
        [Property(Required = true, 
            PropertyLoader = typeof(DefaultLoader<FilialDto, UserDto>))]
        public FilialDto Filial { get; set; }

        [Property(PropertyLoader = typeof (RoleLoader),
            DependentProperties = new[] {"Filial"},
            Required = true,
            ControlType = ControlType.Reference)]
        public List<RoleDto> Role { get; set; }

        public override string ToString()
        {
            return Name;
        }

        public override int GetHashCode()
        {
            return Id > 0 ? Id.GetHashCode() : base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var other = obj as UserDto;

            if (other == null)
                return false;

            return Id > 0 ? Id == other.Id : base.Equals(obj);
        }
    }
}