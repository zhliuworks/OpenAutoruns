using System.Collections.ObjectModel;
using System.Windows.Controls;

using OpenAutoruns.Utilities;

namespace OpenAutoruns.UserControls
{
    /// <summary>
    /// Interaction logic for SchedTasksTab.xaml
    /// </summary>
    public partial class SchedTasksTab : UserControl
    {
        ObservableCollection<SchedTask> schedTasks = new ObservableCollection<SchedTask>();

        public SchedTasksTab()
        {
            InitializeComponent();
            SchedTask.SearchSchedTasks(ref schedTasks);
            ItemList.ItemsSource = schedTasks;
        }
    }
}