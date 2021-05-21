using System.Collections.ObjectModel;
using System.Windows.Controls;

using OpenAutoruns.Utilities;

namespace OpenAutoruns.UserControls
{
    /// <summary>
    /// Interaction logic for DriversTab.xaml
    /// </summary>
    public partial class DriversTab : UserControl
    {
        ObservableCollection<Driver> driverRegs = new ObservableCollection<Driver>();

        public DriversTab()
        {
            InitializeComponent();
            Driver.SearchRegDrivers(Driver.DriverRegEntry, ref driverRegs);
            ItemList.ItemsSource = driverRegs;
        }
    }
}