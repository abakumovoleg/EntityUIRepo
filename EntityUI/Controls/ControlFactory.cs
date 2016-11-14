using System;
using System.Collections;
using System.Reflection;

namespace EntityUI.Controls
{
    public class ControlFactory
    {
        private readonly IDependencyContainer _container;

        public ControlFactory(IDependencyContainer container)
        {
            _container = container;
        }
        
        public UiControl Create(PropertyInfo prop, IStateProvider stateProvider)
        {
            var attr = prop.GetCustomAttribute<PropertyAttribute>();
            
            if (attr != null)
            {
                if (attr.PropertyLoader != null)
                {
                    UiControl control;

                    var isCollection = typeof (IEnumerable).IsAssignableFrom(prop.PropertyType) &&
                                       prop.PropertyType.IsGenericType;

                    var testType = isCollection
                        ? prop.PropertyType.GenericTypeArguments[0]
                        : prop.PropertyType;

                    if (isCollection || attr.ControlType == ControlType.Reference)
                    {
                        control = (UiControl)_container.Resolve(typeof(ReferUiControl<>).MakeGenericType(testType));
                        if (isCollection)
                            ((ICollectionUiControl)control).MultiSelect = true;
                    }
                    else
                        control = (UiControl)_container.Resolve(typeof(ComboUiControl));
                            

                    control.Init(prop, stateProvider );

                    return control;
                }
            }

            if (prop.PropertyType == typeof (string))
            {
                var control = (UiControl)_container.Resolve(typeof (TextEditUiControl));
                control.Init(prop, stateProvider);
                return control;
            }
            
            if (prop.PropertyType == typeof (int) || prop.PropertyType == typeof (int?))
            {
                var control = (UiControl)_container.Resolve(typeof(IntEditUiControl));
                control.Init(prop, stateProvider);
                return control;
            }

            throw new NotImplementedException();
        }
    }
}