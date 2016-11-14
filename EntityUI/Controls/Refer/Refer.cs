using System;
using System.Collections.Generic;
using System.Windows.Forms;
using DevExpress.XtraBars;

namespace EntityUI.Controls.Refer
{
    public partial class Refer<T> : UserControl where T : class, new()
    {
        private readonly Engine _engine;
        private readonly bool _multiSelect;

        public Refer()
        {
            InitializeComponent();
        }

        public Refer(Engine engine, bool multiSelect)
            :this()
        {
            _engine = engine;
            _multiSelect = multiSelect;

            Load+= OnLoad;
        }

        private void OnLoad(object sender, EventArgs eventArgs)
        {
            SetText();
        }

        public List<T> SelectedItems { get; } = new List<T>();

        public void RefreshText()
        {
            SetText();
        }

        private void buttonOpenReestr_Click(object sender, EventArgs e)
        {
            using (var form = (RegistryFormBase<T>)_engine.CreateReestrForm<T>())
            {
                form.Loader = Loader;
                form.StateProvider = StateProvider;
                form.SaveButton.Visibility = BarItemVisibility.Always;
                form.GridView.OptionsSelection.MultiSelect = _multiSelect;

                if (form.ShowDialog(this) == DialogResult.OK)
                {
                    SelectedItems.Clear();
                    SelectedItems.AddRange(form.SelectedItems);
                    SetText();
                    OnSelectedValueChanged();
                }
            }
        }

        public IPropertyLoader Loader { get; set; }
        public IStateProvider StateProvider { get; set; }

        public event EventHandler SelectedValueChanged;

        private void OnSelectedValueChanged()
        {
            SelectedValueChanged?.Invoke(this, EventArgs.Empty);
        }

        private void SetText()
        {
            textBox1.Text = string.Join(",", SelectedItems);
        }
    }
}
