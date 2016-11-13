using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using EntityUI.Controls.Refer;

namespace EntityUI.Controls
{
    public interface ICollectionUiControl
    {
        bool MultiSelect { get; set; }
    }
    public class ReferUiControl<T> : UiControl, ICollectionUiControl where T : class
    {
        private readonly Engine _engine;
        private Refer<T> _comboBox;
        public ReferUiControl(Engine engine)
        {
            _engine = engine;
        }

        public bool MultiSelect { get; set; }
        
        public override object GetValue()
        {
            return _comboBox.Items;
        }

        protected override void AfterInit()
        {
            Control = _comboBox = new Refer<T>(_engine, MultiSelect);

            _comboBox.SelectedValueChanged += (sender, args) => OnValueChanged();

            foreach (var val in Values)
                _comboBox.Items.Add((T)val);
        }

        public override void SetValue(object value)
        {
            _comboBox.Items.Clear();
            _comboBox.Items.AddRange((List<T>) value);
        }
    }
}