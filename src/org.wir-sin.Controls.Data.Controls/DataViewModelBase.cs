using System.ComponentModel;

namespace org.wir_sin.Controls.Data
{
    public abstract class DataViewModelBase : INotifyPropertyChanged, IDataErrorInfo, INotifyPropertyChanging
    {
        private PropertyChangedEventHandler m_PropertyChanged;
        private PropertyChangingEventHandler m_PropertyChanging;

        event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
        {
            add { m_PropertyChanged += value; }
            remove { m_PropertyChanged -= value; }
        }

        event PropertyChangingEventHandler INotifyPropertyChanging.PropertyChanging
        {
            add { m_PropertyChanging += value; }
            remove { m_PropertyChanging -= value; }
        }

        string IDataErrorInfo.Error
        {
            get { return GetError(); }
        }

        string IDataErrorInfo.this[string columnName]
        {
            get { return GetError(columnName); }
        }

        protected void NotifyPropertyChanging(string propertyName)
        {
            if (m_PropertyChanging != null)
                m_PropertyChanging(this, new PropertyChangingEventArgs(propertyName));
        }

        protected void NotifyPropertyChanged(string propertyName)
        {
            if (m_PropertyChanged != null)
                m_PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        protected abstract string GetError();

        protected abstract string GetError(string propertyName);
    }
}