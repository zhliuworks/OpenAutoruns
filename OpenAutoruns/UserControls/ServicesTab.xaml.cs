using System.Collections.ObjectModel;
using System.Windows.Controls;

using OpenAutoruns.Utilities;

namespace OpenAutoruns.UserControls
{
    /// <summary>
    /// Interaction logic for ServicesTab.xaml
    /// </summary>
    public partial class ServicesTab : UserControl
    {
        ObservableCollection<Service> serviceRegs = new ObservableCollection<Service>();

        public ServicesTab()
        {
            InitializeComponent();
            Service.SearchRegServices(Service.ServiceRegEntry, ref serviceRegs);
            ItemList.ItemsSource = serviceRegs;
        }
    }
}
