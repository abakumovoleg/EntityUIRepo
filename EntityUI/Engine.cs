using System.Collections.Generic;
using System.Windows.Forms;
using EntityUI.Controls;

namespace EntityUI
{
    public class Engine
    {
        private readonly IDependencyContainer _container;
        private readonly IEntityProvider _entityProvider;

        public Engine(IEntityProvider entityProvider, IDependencyContainer container)
        {
            _entityProvider = entityProvider;
            _container = container;
            _container.RegisterInstance<Engine, Engine>(this);
        }

        public Form CreateCardForm<T>(T entity) where T : class
        {
            var form = new CardFormBase<T>(new ControlFactory(_container), _entityProvider);

            form.Init(entity);

            //add save button

            //onsave?

            return form;
        }

        public Form CreateReestrForm<T>() where T : class
        {
            var form = new ReestrFormBase<T>(_entityProvider, this);
            
            //add save button

            //onsave?

            return form;
        }

        private void OnSave<T>(T entity)
        {

        }
    }
}