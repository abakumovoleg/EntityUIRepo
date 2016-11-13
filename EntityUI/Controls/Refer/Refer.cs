using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraBars;

namespace EntityUI.Controls.Refer
{
    public partial class Refer<T> : UserControl where T : class
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

        public List<T> Items { get; } = new List<T>();

        private void buttonOpenReestr_Click(object sender, EventArgs e)
        {
            using (var form = (RegistryFormBase<T>)_engine.CreateReestrForm<T>())
            {
                form.SaveButton.Visibility = BarItemVisibility.Always;
                form.GridView.OptionsSelection.MultiSelect = _multiSelect;

                if (form.ShowDialog(this) == DialogResult.OK)
                {
                    Items.Clear();
                    Items.AddRange(form.SelectedItems);
                    SetText();
                    OnSelectedValueChanged();
                }
            }
        }

        public event EventHandler SelectedValueChanged;

        private void OnSelectedValueChanged()
        {
            SelectedValueChanged?.Invoke(this, EventArgs.Empty);
        }

        private void SetText()
        {
            textBox1.Text = string.Join(",", Items);
        }
    }
}
