using System.Collections;

namespace EntityUI
{
    public interface IPropertyLoader
    {
        IList Load(object state);
    }
}