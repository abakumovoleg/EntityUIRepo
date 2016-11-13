using System;

namespace EntityUI
{
    public interface IDependencyContainer
    {
        object Resolve(Type t);

        void RegisterInstance<TI, TT>(TT instance)
            where TT : TI;
    }
}