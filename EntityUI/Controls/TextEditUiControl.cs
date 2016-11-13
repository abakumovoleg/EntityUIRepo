using System.Reflection;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace EntityUI.Controls
{
    public class TextEditUiControl : UiControl
    {
        private TextEdit _textBox;


        public override object GetValue()
        {
            return _textBox.Text;
        }

        protected override void AfterInit()
        {
            Control = _textBox = new TextEdit();

            var attr = PropertyInfo.GetCustomAttribute<PropertyAttribute>();
            if (attr != null)
            {
                _textBox.Properties.MaxLength = attr.MaxLength;
                _textBox.Properties.ReadOnly = attr.ReadOnly;
                _textBox.Properties.Mask.EditMask = attr.Mask;
                _textBox.Properties.Mask.MaskType = attr.MaskType == MaskType.RegularExpression
                    ? DevExpress.XtraEditors.Mask.MaskType.RegEx
                    : DevExpress.XtraEditors.Mask.MaskType.None;
            }

            _textBox.TextChanged += (sender, args) => OnValueChanged();
        }

        public override void SetValue(object value)
        {
            _textBox.Text = value?.ToString();
        }
    }
}