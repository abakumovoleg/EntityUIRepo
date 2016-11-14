using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using EntityUI.Controls.Refer;

namespace EntityUI.Controls
{
    public interface ICollectionUiControl
    {
        bool MultiSelect { get; set; }
    }
    public class ReferUiControl<T> : UiControl, IDataControl, ICollectionUiControl where T : class, new()
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
            return _comboBox.SelectedItems;
        }

        protected override void AfterInit()
        {
            Control = _comboBox = new Refer<T>(_engine, MultiSelect);

            _comboBox.SelectedValueChanged += (sender, args) => OnValueChanged();

            ReloadData();
        }

        public override void SetValue(object value)
        {
            _comboBox.SelectedItems.Clear();
            _comboBox.SelectedItems.AddRange((List<T>) value);
        }

        public void ReloadData()
        {
            var loader = _engine.GetLoader(PropertyAttribute.PropertyLoader);
            _comboBox.Loader = loader;
            _comboBox.StateProvider = StateProvider;

            var items = loader.Load(StateProvider.State);
            
            var newItems = _comboBox.SelectedItems.Where(x => items.Contains(x)).ToList();
            _comboBox.SelectedItems.Clear();
            _comboBox.SelectedItems.AddRange(newItems);
            _comboBox.RefreshText();
        }
    }
}