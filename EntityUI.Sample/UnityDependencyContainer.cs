using System;
using Microsoft.Practices.Unity;

namespace EntityUI.Sample
{
    public class UnityDependencyContainer : IDependencyContainer
    {
        private readonly IUnityContainer _container;

        public UnityDependencyContainer(IUnityContainer container)
        {
            _container = container;
        }
        public object Resolve(Type t)
        {
            return _container.Resolve(t);
        }

        public void RegisterInstance<TI,TT>(TT instance)
            where TT : TI
        {
            _container.RegisterInstance<TI>(instance);
        }
    }
}