using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
            ItemList.ItemsSource = logonRegs;
        }
    }
}
