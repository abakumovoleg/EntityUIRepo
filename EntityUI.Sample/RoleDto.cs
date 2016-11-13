namespace EntityUI.Sample
{
    class RoleDto
    {
        [Property(ReadOnly = true)]
        public int Id { get; set; }

        [Property(MaxLength = 100)]
        public string Name { get; set; }

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