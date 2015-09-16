using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace org.wir_sin.Controls.Data
{
    public class StringFieldProvider : FieldProviderBase
    {
        public override Type TargetType
        {
            get { return typeof(string); }
        }

        public override FrameworkElement GenerateInputField(Binding ContentBinding)
        {
            TextBox field = new TextBox();
            field.SetBinding(TextBox.TextProperty, ContentBinding);
            return field;
        }

        public override bool DisplayLabel
        {
            get { return true; }
        }
    }
}