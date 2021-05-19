using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Data;

using OpenAutoruns.Utilities;

namespace OpenAutoruns.UserControls
{
    /// <summary>
    /// Interaction logic for LogonTab.xaml
    /// </summary>
    public partial class LogonTab : UserControl
    {
        ObservableCollection<Logon> logonRegs = new ObservableCollection<Logon>();

        public LogonTab()
        {
            InitializeComponent();
            Logon.SearchRegLogon(Logon.RegEntries, ref logonRegs);
            Logon.SearchDirLogon(Logon.StartupDirs, ref logonRegs);
            ItemList.ItemsSource = logonRegs;
            var collectionView = (CollectionView)CollectionViewSource.GetDefaultView(ItemList.ItemsSource);
            collectionView.GroupDescriptions.Clear();
            collectionView.GroupDescriptions.Add(new PropertyGroupDescription("Path"));
        }
    }
}
