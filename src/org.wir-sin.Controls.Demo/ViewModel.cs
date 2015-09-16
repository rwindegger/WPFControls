using org.wir_sin.Controls.Data;

namespace org.wir_sin.Controls.Demo
{
    public class ViewModel : DataViewModelBase
    {
        private string m_FirstName = "Max";
        private string m_LastName = "Musterman";
        private string m_PhoneNumber;
        private int m_Test;
        private AdressViewModel m_Adress;

        [DataEditorDisplayName("First Name")]
        [DataEditorDisplayOrder(1)]
        public string FirstName
        {
            get { return m_FirstName; }
            set
            {
                if (value == m_FirstName)
                    return;
                NotifyPropertyChanging("FirstName");
                m_FirstName = value;
                NotifyPropertyChanged("FirstName");
            }
        }

        [DataEditorDisplayName("Last Name")]
        [DataEditorDisplayOrder(2)]
        public string LastName
        {
            get { return m_LastName; }
            set
            {
                if (value == m_FirstName)
                    return;
                NotifyPropertyChanging("LastName");
                m_LastName = value;
                NotifyPropertyChanging("LastName");
            }
        }

        [DataEditorDisplayName("Phone Number")]
        [DataEditorDisplayOrder(3)]
        public string PhoneNumber
        {
            get { return m_PhoneNumber; }
            set { m_PhoneNumber = value; }
        }

        [DataEditorDisplayName("Test Integer")]
        [DataEditorDisplayOrder(5)]
        public int Test
        {
            get { return m_Test; }
            set { m_Test = value; }
        }

        [DataEditorDisplayName("Adress")]
        [DataEditorDisplayOrder(4)]
        public AdressViewModel Adress
        {
            get { return m_Adress; }
            set
            {
                if (value == m_Adress)
                    return;
                NotifyPropertyChanging("Adress");
                m_Adress = value;
                NotifyPropertyChanged("Adress");
            }
        }

        public ViewModel()
        {
            Adress = new AdressViewModel();
        }

        protected override string GetError()
        {
            return string.Empty;
        }

        protected override string GetError(string propertyName)
        {
            if (propertyName.Equals("PhoneNumber") && PhoneNumber != "1234")
                return "Phone Number must be 1234.";
            return string.Empty;
        }
    }
}