using System.Collections.ObjectModel;
using System.Windows.Controls;

using OpenAutoruns.Utilities;

namespace OpenAutoruns.UserControls
{
    /// <summary>
    /// Interaction logic for KnownDLLsTab.xaml
    /// </summary>
    public partial class KnownDLLsTab : UserControl
    {
        ObservableCollection<KnownDLL> knownDLLRegs = new ObservableCollection<KnownDLL>();

        public KnownDLLsTab()
        {
            InitializeComponent();
            KnownDLL.SearchKnownDLLs(KnownDLL.KnownDLLRegEntry, ref knownDLLRegs);
            ItemList.ItemsSource = knownDLLRegs;
        }
    }
}