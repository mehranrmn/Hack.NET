using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
using Telerik.Windows.Persistence.Services;
using Telerik.Windows.Persistence;

namespace GridPersistence
{
    public partial class MainPage : UserControl
    {
        System.IO.Stream stream;
        public MainPage()
        {
            InitializeComponent();

            ServiceProvider.RegisterPersistenceProvider<ICustomPropertyProvider>(typeof(RadGridView), new GridCustomPropertyProvider());
            this.DataContext = DataLoader.LoadStudents();
        }

        private void OnSave(object sender, System.Windows.RoutedEventArgs e)
        {
            PersistenceManager manager = new PersistenceManager();
            this.stream = manager.Save(this.gridView);
        }

        private void OnLoad(object sender, System.Windows.RoutedEventArgs e)
        {
            this.stream.Position = 0L;
            PersistenceManager manager = new PersistenceManager();
            manager.Load(this.gridView, this.stream);
        }
    }
}
