using System.Collections;
using System.Reflection;
using System.Windows.Forms;

namespace EntityUI.Controls
{
    public class ComboUiControl : UiControl
    {
        private  ComboBox _comboBox;
 

        public override object GetValue()
        {
            return _comboBox.SelectedItem;
        }

        protected override void AfterInit()
        {
            Control = _comboBox = new ComboBox();

            _comboBox.SelectedValueChanged += (sender, args) => OnValueChanged();

            foreach (var val in Values)
                _comboBox.Items.Add(val);
        }

        public override void SetValue(object value)
        {
            _comboBox.SelectedItem = value;
        }
    }
}