using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Data;

using OpenAutoruns.Utilities;

namespace OpenAutoruns.UserControls
{
    /// <summary>
    /// Interaction logic for IEBHOTab.xaml
    /// </summary>
    public partial class IEBHOTab : UserControl
    {
        ObservableCollection<BHO> BHORegs = new ObservableCollection<BHO>();

        public IEBHOTab()
        {
            InitializeComponent();
            BHO.SearchRegBHOs(BHO.BHORegEntries, ref BHORegs);
            ItemList.ItemsSource = BHORegs;
            var collectionView = (CollectionView)CollectionViewSource.GetDefaultView(ItemList.ItemsSource);
            collectionView.GroupDescriptions.Clear();
            collectionView.GroupDescriptions.Add(new PropertyGroupDescription("Path"));
        }
    }
}
