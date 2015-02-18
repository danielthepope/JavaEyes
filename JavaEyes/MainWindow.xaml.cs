using System;
using System.Collections.Generic;
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
        private List<Process> javas;

        public MainWindow()
        {
            javas = new List<Process>();
            InitializeComponent();
            //CountJava();
            DispatcherTimer timer = new DispatcherTimer();
            timer.Tick += new EventHandler(CountJava);
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Start();
        }

        public void CountJava()
        {
            javas = new List<Process>();
            Process[] processes = Process.GetProcesses();
            long ram = 0;
            foreach (Process p in processes)
            {
                if (p.ProcessName.Contains("java"))
                {
                    javas.Add(p);
                    ram += p.WorkingSet64;
                }
            }
            CountBox.Content = javas.Count;
            RamLabel.Content = String.Format("{0} MB RAM", ram / 1048576);
            ProcessList.DataContext = javas;
        }

        private void CountJava(object sender, EventArgs e)
        {
            CountJava();
        }
    }
}
