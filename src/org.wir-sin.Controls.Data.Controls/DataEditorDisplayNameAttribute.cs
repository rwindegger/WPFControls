using System;

namespace org.wir_sin.Controls.Data
{
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public sealed class DataEditorDisplayNameAttribute : Attribute
    {
        readonly string m_DisplayName;

        public DataEditorDisplayNameAttribute(string displayName)
        {
            m_DisplayName = displayName;
        }

        public string DisplayName
        {
            get { return m_DisplayName; }
        }
    }

    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public sealed class DataEditorDisplayOrderAttribute : Attribute
    {
        private int m_Order;

        public int Order
        {
            get { return m_Order; }
        }

        // This is a positional argument
        public DataEditorDisplayOrderAttribute(int order)
        {
            m_Order = order;
        }
    }
}