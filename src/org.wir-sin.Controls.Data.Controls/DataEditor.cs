using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace org.wir_sin.Controls.Data
{
    public class DataEditor : ContentControl
    {
        private DataViewModelBase m_CurrentDataContext;
        private Grid m_ContentGrid;
        private Dictionary<Type, FieldProviderBase> m_Providers = new Dictionary<Type, FieldProviderBase>();

        public ObservableCollection<FieldProviderBase> FieldProviders
        {
            get { return (ObservableCollection<FieldProviderBase>)GetValue(FieldProvidersProperty); }
            set { SetValue(FieldProvidersProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FieldProviders.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FieldProvidersProperty =
            DependencyProperty.Register("FieldProviders", typeof(ObservableCollection<FieldProviderBase>), typeof(DataEditor), new UIPropertyMetadata(new ObservableCollection<FieldProviderBase>(), FieldProvidersChanged));

        private static void FieldProvidersChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            DataEditor obj = o as DataEditor;
            if (obj != null)
            {
                obj.m_Providers.Clear();
                obj.FieldProviders.CollectionChanged += obj.FieldProviders_CollectionChanged;
                foreach (FieldProviderBase prov in obj.FieldProviders)
                {
                    obj.m_Providers.Add(prov.TargetType, prov);
                }
            }
        }

        private void FieldProviders_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
        }

        public DataEditor()
        {
            DataContextChanged += DataEditor_DataContextChanged;
            FieldProviderBase sp = new StringFieldProvider();
            m_Providers.Add(sp.TargetType, sp);
        }

        private void DataEditor_DataContextChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            if (!(DataContext is DataViewModelBase))
            {
                Content = new TextBlock() { Text = string.Format("No valid DataContext specified. A valid DataContexts must inherit {0}. Specified DataContext is {1}.", typeof(DataViewModelBase), DataContext.GetType()) };
            }
            else
            {
                m_CurrentDataContext = DataContext as DataViewModelBase;

                InitializeGrid();

                AddRowsForContext();
            }
        }

        private void InitializeGrid()
        {
            m_ContentGrid = new Grid();
            Content = m_ContentGrid;
            m_ContentGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
            m_ContentGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
        }

        private PropertyInfo[] GetProperties()
        {
            Type dcType = m_CurrentDataContext.GetType();
            PropertyInfo[] props = dcType.GetProperties();
            props = props.OrderBy(pi =>
                {
                    var attrs = pi.GetCustomAttributes(typeof(DataEditorDisplayOrderAttribute), true);
                    if (attrs.Length > 0)
                        return ((DataEditorDisplayOrderAttribute)attrs[0]).Order;
                    else
                        return 0;
                }).ToArray();
            return props;
        }

        private void AddRowsForContext()
        {
            PropertyInfo[] props = GetProperties();

            int row = 0;
            foreach (PropertyInfo pi in props)
            {
                m_ContentGrid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });

                FrameworkElement field = GetFieldForRow(row, pi);
                if (field is DataEditor)
                    field.SetValue(Grid.ColumnProperty, 0);
                else
                    AddLabelToRow(row, pi);

                m_ContentGrid.Children.Add(field);
                row++;
            }
        }

        private FrameworkElement GetFieldForRow(int row, PropertyInfo pi)
        {
            Type propType = pi.PropertyType;

            FrameworkElement field = new TextBlock() { Text = string.Format("No Control defined for Type {0}.", propType) };

            Binding binding = CreateBinding(pi.Name);

            try
            {
                field = m_Providers[propType].GenerateInputField(binding);
            }
            catch (KeyNotFoundException ex)
            {
            }

            //if (propType.Equals(typeof(string)))
            //{
            //    field = new TextBox();
            //    field.SetBinding(TextBox.TextProperty, binding);
            //}
            //else if (!propType.IsValueType)
            //{
            //    field = new DataEditor();
            //    field.SetBinding(FrameworkElement.DataContextProperty, binding);
            //    field.SetValue(Grid.ColumnSpanProperty, 2);
            //}

            field.SetValue(Grid.ColumnProperty, 1);
            field.SetValue(Grid.RowProperty, row);
            field.Margin = new Thickness(1);
            return field;
        }

        private static Binding CreateBinding(string bindingPath)
        {
            Binding binding = new Binding(bindingPath);
            binding.ValidatesOnDataErrors = true;
            binding.ValidatesOnExceptions = true;
            binding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            return binding;
        }

        private void AddLabelToRow(int row, PropertyInfo pi)
        {
            string displayName = GetPropertyDisplayName(pi);

            TextBlock label = new TextBlock();
            label.SetValue(Grid.ColumnProperty, 0);
            label.SetValue(Grid.RowProperty, row);
            label.Text = displayName;
            label.Margin = new Thickness(0, 5, 5, 0);
            m_ContentGrid.Children.Add(label);
        }

        private static string GetPropertyDisplayName(PropertyInfo pi)
        {
            string displayName = pi.Name;

            object[] dnAttributes = pi.GetCustomAttributes(typeof(DataEditorDisplayNameAttribute), true);

            if (dnAttributes.Length > 0)
            {
                DataEditorDisplayNameAttribute dn = (DataEditorDisplayNameAttribute)dnAttributes[0];
                displayName = dn.DisplayName;
            }
            return displayName;
        }
    }
}