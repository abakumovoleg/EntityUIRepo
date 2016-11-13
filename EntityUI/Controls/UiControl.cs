using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace EntityUI.Controls
{
    public abstract class UiControl
    {
        protected List<object> Values;
        
        public void Init(PropertyInfo propertyInfo, IEnumerable values)
        {
            Values = new List<object>();

            if(values != null)
                Values.AddRange(values.Cast<object>());

            PropertyInfo = propertyInfo;

            AfterInit();
        }

        protected abstract void AfterInit();

        public Control Control { get; set; }

        public PropertyAttribute PropertyAttribute => PropertyInfo.GetCustomAttribute<PropertyAttribute>();

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