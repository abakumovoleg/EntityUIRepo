using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using EntityUI.Controls;

namespace EntityUI
{
    public partial class CardFormBase<T> : Form, IStateProvider where T : class, new()
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
        private readonly Dictionary<string,UiControl> _controls = new Dictionary<string, UiControl>();
        private int _controlCount;
        private readonly ControlFactory _controlFactory;
        private readonly IEntityProvider _entityProvider;

        private void CreateControls(PropertyInfo prop, T entity)
        {
            _entity = entity;
            tableLayoutPanel1.Controls.Add(new Label { Text = prop.Name }, 0, _controlCount);
            var control = _controlFactory.Create(prop, this);
            control.Control.Margin = new Padding(0,0,25,0);
            control.ValueChanged += (sender, args) =>
            {
                errorProvider1.SetError(control.Control, string.Empty);

                var ctrl = (UiControl) sender;

                foreach (var value in _controls.Values)
                {
                    if (value is IDataControl && value.PropertyAttribute != null &&
                        value.PropertyAttribute.DependentProperties != null &&
                        value.PropertyAttribute.DependentProperties.Contains(ctrl.PropertyInfo.Name))
                    {
                        (value as IDataControl).ReloadData();
                    }
                }
            };

            if (entity != null)
            {
                var value = prop.GetValue(entity);

                control.SetValue(value);
            }
            control.Control.Dock = DockStyle.Fill;
            tableLayoutPanel1.Controls.Add(control.Control, 1, _controlCount);

            tableLayoutPanel1.RowStyles.Add(new RowStyle { SizeType = SizeType.AutoSize });

            _controls[prop.Name] = control;
            _controlCount++;
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (!ValidateControls())
                return;

            var entity = _entity ?? Activator.CreateInstance(typeof(T)) as T;

            foreach (var control in _controls.Values)
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

            foreach (var control in _controls.Values)
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

        public object State
        {
            get
            {
                var state = new T();

                foreach (var control in _controls.Values)
                {
                    var value = control.GetValue();

                    control.PropertyInfo.SetValue(state, value);
                }

                return state;
            }
        }
    }
}
