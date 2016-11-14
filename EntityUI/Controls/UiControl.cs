using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace EntityUI.Controls
{
    public interface IStateProvider
    {
        object State { get; }
    }

    public abstract class UiControl
    {
        public void Init(PropertyInfo propertyInfo, IStateProvider stateProvider)
        {
            PropertyInfo = propertyInfo;

            StateProvider = stateProvider;

            AfterInit();
        }

        protected abstract void AfterInit();

        public Control Control { get; set; }

        public PropertyAttribute PropertyAttribute => PropertyInfo.GetCustomAttribute<PropertyAttribute>();

        public IStateProvider StateProvider { get; set; }
        public PropertyInfo PropertyInfo { get; set; }
        public abstract void SetValue(object value);
        public abstract object GetValue();
        public event EventHandler ValueChanged;
        

        protected void OnValueChanged()
        {
            ValueChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}