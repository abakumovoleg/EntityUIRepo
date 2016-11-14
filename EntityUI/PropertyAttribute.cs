using System;
using System.Linq.Expressions;

namespace EntityUI
{
    public enum MaskType
    {
        Simple,
        RegularExpression
    }

    public class PropertyAttribute : Attribute
    {
        public PropertyAttribute()
        {
            MinValue = long.MinValue;
            MaxValue = long.MaxValue;
            MaxLength = int.MaxValue;
        }

        public Type PropertyLoader { get; set; }
        public string[] DependentProperties { get; set; }
        public ControlType ControlType { get; set; }
        public bool Required { get; set; }
        public int MaxLength { get; set; }
        public long MaxValue { get; set; }
        public long MinValue { get; set; }
        public bool ReadOnly { get; set; }
        public string Mask { get; set; }
        public MaskType MaskType { get; set; }
    }
}