using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
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
using System.Windows.Threading;

namespace JavaEyes
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<ProcessEye> javas;
        private string criteria = "";

        public MainWindow()
        {
            javas = new List<ProcessEye>();
            InitializeComponent();
            CountJava();
            DispatcherTimer timer = new DispatcherTimer();
            timer.Tick += new EventHandler(CountJava);
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Start();
        }

        public void CountJava()
        {
            criteria = Criteria.Text;
            javas = new List<ProcessEye>();
            Process[] processes = Process.GetProcesses();
            long ram = 0;
            foreach (Process p in processes)
            {
                if (p.ProcessName.Contains(criteria))
                {
                    javas.Add(new ProcessEye(p));
                    ram += p.WorkingSet64;
                }
            }
            CountBox.Content = javas.Count;
            RamLabel.Content = String.Format("{0:n0} MB RAM", ram / 1048576.0);
            ProcessList_Initialized(null, null);
        }

        private void CountJava(object sender, EventArgs e)
        {
            CountJava();
        }

        private void ProcessList_Initialized(object sender, EventArgs e)
        {
            ProcessList.ItemsSource = javas;
            var sortColumn = ProcessList.Columns[2];
            sortColumn.SortDirection = ListSortDirection.Descending;
            ProcessList.Items.SortDescriptions.Add(new SortDescription(sortColumn.SortMemberPath, ListSortDirection.Descending));
        }
    }
}
