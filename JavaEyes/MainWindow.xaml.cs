using Microsoft.VisualBasic.Devices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
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
        private ulong totalRam;

        public MainWindow()
        {
            javas = new List<ProcessEye>();
            totalRam = new ComputerInfo().TotalPhysicalMemory;
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
                if (!p.ProcessName.Contains("JavaEyes") && p.ProcessName.ToLower().Contains(criteria))
                {
                    javas.Add(new ProcessEye(p));
                    ram += p.WorkingSet64;
                }
            }
            CountBox.Content = javas.Count;
            Title = criteria + "Eyes";
            RamLabel.Content = String.Format("{0:n0} MB RAM", ram / 1048576.0);
            ProcessList_Initialized(null, null);
            UpdateGraph(ram);
        }

        private void CountJava(object sender, EventArgs e)
        {
            CountJava();
        }

        private void UpdateGraph(long ram)
        {
            ulong availableRam = new ComputerInfo().AvailablePhysicalMemory;
            double bytesPerPixel = ((double)totalRam / (double)TotalRam.Width);
            RamInUse.Width = (totalRam - availableRam) / bytesPerPixel;
            CriteriaRam.Width = ram / bytesPerPixel;
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
