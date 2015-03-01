using Microsoft.VisualBasic.Devices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
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
            Criteria.Focus();
            Criteria.SelectAll();
            TotalRamLabel.Content = String.Format("Total RAM: {0:n0}MB", totalRam / 1048576.0);
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
                if (!p.ProcessName.Contains("JavaEyes") && p.ProcessName.ToLower().Contains(criteria.ToLower()))
                {
                    javas.Add(new ProcessEye(p));
                    ram += p.WorkingSet64;
                }
            }
            ProcessLabel.Content = javas.Count == 1 ? "process" : "processes";
            CountBox.Content = javas.Count;
            SetTitle(criteria);
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
            AvailableRamLabel.Content = String.Format("Available RAM: {0:n0}MB", availableRam / 1048576.0);
            double bytesPerPixel = ((double)totalRam / (double)TotalRam.Width);
            RamInUse.Width = (totalRam - availableRam) / bytesPerPixel;
            CriteriaRam.Width = ram / bytesPerPixel;
        }

        private void SetTitle(string criteria)
        {
            string title = "";
            Regex filter = new Regex("[^A-z]");
            criteria = filter.Replace(criteria, "");
            if (criteria.Length == 0) title = "Eyes";
            else if (criteria.Length == 1) title = criteria.ToUpper() + "Eyes";
            else title = criteria.Substring(0, 1).ToUpper() + criteria.Substring(1).ToLower() + "Eyes";
            Title = title;
        }

        private void ProcessList_Initialized(object sender, EventArgs e)
        {
            ProcessList.ItemsSource = javas;
            var sortColumn = ProcessList.Columns[2];
            sortColumn.SortDirection = ListSortDirection.Descending;
            ProcessList.Items.SortDescriptions.Add(new SortDescription(sortColumn.SortMemberPath, ListSortDirection.Descending));
        }

        private void Criteria_KeyUp(object sender, KeyEventArgs e)
        {
            CountJava();
        }
    }
}
