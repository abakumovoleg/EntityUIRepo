using System.Collections.Generic;

namespace EntityUI.Sample
{
    public class RoleDto
    {
        [Property(ReadOnly = true)]
        public int Id { get; set; }

        [Property(MaxLength = 100)]
        public string Name { get; set; }

        [Property(Required = true, ControlType = ControlType.Reference)]
        public List<FilialDto> Filial { get; set; }

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
            var other = obj as RoleDto;

            if (other == null)
                return false;

            return Id > 0 ? Id == other.Id : base.Equals(obj);
        }
    }
}