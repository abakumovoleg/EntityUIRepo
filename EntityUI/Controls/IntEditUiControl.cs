using System;
using System.Reflection;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Mask;

namespace EntityUI.Controls
{
    public class IntEditUiControl : UiControl
    {
        private  TextEdit _textBox;
   
        private void TextBoxOnEditValueChanging(object sender, ChangingEventArgs e)
        {
            if (PropertyAttribute != null)
            {
                decimal val;
                if (!decimal.TryParse(Convert.ToString(e.NewValue), out val))
                {
                    e.Cancel = true;
                    return;
                }

                if (PropertyAttribute.MaxValue < val)
                    e.Cancel = true;

                if (PropertyAttribute.MinValue > val)
                    e.Cancel = true;
            }
        }

        public override object GetValue()
        {
            int val;
            if(!int.TryParse(_textBox.Text, out val))
                return null;

            return val;
        }

        protected override void AfterInit()
        {
            Control = _textBox = new TextEdit();
            _textBox.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            _textBox.EditValueChanging += TextBoxOnEditValueChanging;
            _textBox.Properties.Mask.EditMask = PropertyAttribute.Mask ?? "N0";
            _textBox.TextChanged += (sender, args) => OnValueChanged();
            _textBox.Properties.ReadOnly = PropertyAttribute.ReadOnly;
        }

        public override void SetValue(object value)
        {
            _textBox.Text = Convert.ToString(value);
        }
    }
}