using System;
using System.Windows;
using System.Windows.Data;

namespace org.wir_sin.Controls.Data
{
    public abstract class FieldProviderBase
    {
        public abstract Type TargetType
        {
            get;
        }

        public abstract FrameworkElement GenerateInputField(Binding ContentBinding);

        public abstract bool DisplayLabel
        {
            get;
        }
    }
}