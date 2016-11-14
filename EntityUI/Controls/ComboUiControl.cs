using System.Collections;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace EntityUI.Controls
{
    public interface IDataControl
    {
        void ReloadData();
    }

    public class ComboUiControl : UiControl, IDataControl
    {
        private readonly Engine _engine;
        private  ComboBox _comboBox;

        public ComboUiControl(Engine engine)
        {
            _engine = engine;
        }

        public override object GetValue()
        {
            return _comboBox.SelectedItem;
        }

        public void ReloadData()
        {
            var loader = _engine.GetLoader(PropertyAttribute.PropertyLoader);

            var items = loader.Load(StateProvider.State).Cast<object>();

            _comboBox.Items.Clear();
            _comboBox.Items.AddRange(items.ToArray());
        }

        protected override void AfterInit()
        {
            Control = _comboBox = new ComboBox();
            _comboBox.DropDownStyle= ComboBoxStyle.DropDownList;

            _comboBox.SelectedValueChanged += (sender, args) => OnValueChanged();

            ReloadData();
        }

        public override void SetValue(object value)
        {
            _comboBox.SelectedItem = value;
        }
    }
}