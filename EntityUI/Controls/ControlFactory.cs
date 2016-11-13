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
        
        public UiControl Create(PropertyInfo prop)
        {
            var attr = prop.GetCustomAttribute<PropertyAttribute>();
            
            if (attr != null)
            {
                if (attr.PropertyLoader != null)
                {
                    var loader = (IPropertyLoader)_container.Resolve(attr.PropertyLoader);
                    var items = loader.Load();

                    UiControl control;

                    var isCollection = typeof (IEnumerable).IsAssignableFrom(prop.PropertyType) &&
                                       prop.PropertyType.IsGenericType;

                    var testType = isCollection
                        ? prop.PropertyType.GenericTypeArguments[0]
                        : prop.PropertyType;

                    switch (attr.ControlType)
                    {
                        case ControlType.ComboBox:
                            control = (UiControl)_container.Resolve(typeof(ComboUiControl));
                            break;
                        case ControlType.Reference:
                            control = (UiControl)_container.Resolve(typeof(ReferUiControl<>).MakeGenericType(testType));
                            if (isCollection)
                                ((ICollectionUiControl) control).MultiSelect = true;
                            break;
                        default:
                            throw new NotImplementedException();
                    }

                    control.Init(prop, items);

                    return control;
                }
            }

            if (prop.PropertyType == typeof (string))
            {
                var control = (UiControl)_container.Resolve(typeof (TextEditUiControl));
                control.Init(prop, null);
                return control;
            }
            
            if (prop.PropertyType == typeof (int) || prop.PropertyType == typeof (int?))
            {
                var control = (UiControl)_container.Resolve(typeof(IntEditUiControl));
                control.Init(prop, null);
                return control;
            }

            throw new NotImplementedException();
        }
    }
}