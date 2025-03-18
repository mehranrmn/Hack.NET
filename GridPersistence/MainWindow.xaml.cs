using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Telerik.Windows.Controls;
using Telerik.Windows.Persistence;
using Telerik.Windows.Persistence.Services;
using Telerik.Windows.Persistence.Storage;
using Telerik.Windows.Controls.GridView;
using Telerik.Windows.Controls.Input;
using System.Collections.ObjectModel;
using GridPersistence.data;

namespace GridPersistence
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private PersistenceManager manager = new PersistenceManager()
            .AllowDataAssembly()
            .AllowCoreControls()
            .AllowGridViewControls()
            .AllowTypes(
                typeof(ColumnProxy),
                typeof(SortDescriptorProxy),
                typeof(GroupDescriptorProxy),
                typeof(FilterDescriptorProxy),
                typeof(FilterSetting),
                typeof(List<ColumnProxy>),
                typeof(List<SortDescriptorProxy>),
                typeof(List<GroupDescriptorProxy>),
                typeof(List<FilterDescriptorProxy>),
                typeof(List<FilterSetting>),
                typeof(List<object>)
            );
        public MainWindow()
        {
            ServiceProvider.RegisterPersistenceProvider<ICustomPropertyProvider>(typeof(RadGridView), new GridCustomPropertyProvider());
            this.DataContext = DataLoader.LoadStudents();
        }
        private void OnSave(object sender, System.Windows.RoutedEventArgs e)
        {
            IsolatedStorageProvider isoProvider = new IsolatedStorageProvider(manager);
            isoProvider.SaveToStorage();
        }

        private void OnLoad(object sender, System.Windows.RoutedEventArgs e)
        {
            IsolatedStorageProvider isoProvider = new IsolatedStorageProvider(manager);
            isoProvider.LoadFromStorage();
        }


    }
}