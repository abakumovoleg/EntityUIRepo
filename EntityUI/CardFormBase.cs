using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using EntityUI.Controls;

namespace EntityUI
{
    public partial class CardFormBase<T> : Form where T : class
    {
        public CardFormBase()
        {
            InitializeComponent();
        }

        public CardFormBase(ControlFactory controlFactory, IEntityProvider entityProvider)
        {
            InitializeComponent();

            _entityProvider = entityProvider;
            _controlFactory = controlFactory;
        }

        public void Init(T entity)
        {
            var action = entity == null ? "Add" : "Edit";
            Text = $"{typeof (T).Name} {action}";

            var props = typeof(T).GetProperties();
                       
            foreach (var prop in props)
            {
                CreateControls(prop, entity);
            }
        }
        
        private T _entity;
        private readonly List<UiControl> _controls = new List<UiControl>();
        private int _controlCount;
        private readonly ControlFactory _controlFactory;
        private readonly IEntityProvider _entityProvider;

        private void CreateControls(PropertyInfo prop, T entity)
        {
            _entity = entity;
            tableLayoutPanel1.Controls.Add(new Label { Text = prop.Name }, 0, _controlCount);
            var control = _controlFactory.Create(prop);
            control.Control.Margin = new Padding(0,0,25,0);
            control.ValueChanged += (sender, args) => errorProvider1.SetError(control.Control, string.Empty);

            if (entity != null)
            {
                var value = prop.GetValue(entity);

                control.SetValue(value);
            }
            control.Control.Dock = DockStyle.Fill;
            tableLayoutPanel1.Controls.Add(control.Control, 1, _controlCount);

            tableLayoutPanel1.RowStyles.Add(new RowStyle { SizeType = SizeType.AutoSize });

            _controls.Add(control);
            _controlCount++;
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (!ValidateControls())
                return;

            var entity = _entity ?? Activator.CreateInstance(typeof(T)) as T;

            foreach (var control in _controls)
            {
                var value = control.GetValue();

                control.PropertyInfo.SetValue(entity, value);
            }

            if (_entity == null)
                _entityProvider.Add(entity);
            else
                _entityProvider.Update(entity);

            Close();
        }

        protected bool ValidateControls()
        {
            var validate = true;

            foreach (var control in _controls)
            {
                var value = control.GetValue();
                var strValue = Convert.ToString(value);

                var attr = control.PropertyInfo.GetCustomAttribute<PropertyAttribute>();
                if (attr != null)
                {
                    if (attr.Required && string.IsNullOrWhiteSpace(strValue))
                    {
                        errorProvider1.SetError(control.Control, "Required");
                        validate = false;
                    }
                }
            }

            return validate;
        }
    }
}
