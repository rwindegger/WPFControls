using org.wir_sin.Controls.Data;

namespace org.wir_sin.Controls.Demo
{
    public class AdressViewModel : DataViewModelBase
    {
        private string _street;
        private string _city;

        public string Street
        {
            get { return _street; }
            set { _street = value; }
        }

        public string City
        {
            get { return _city; }
            set { _city = value; }
        }

        protected override string GetError()
        {
            return string.Empty;
        }

        protected override string GetError(string propertyName)
        {
            return string.Empty;
        }
    }
}