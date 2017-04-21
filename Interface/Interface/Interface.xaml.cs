using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using Outlook = Microsoft.Office.Interop.Outlook.Application;
using System.Text;
using System.Text.RegularExpressions;
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
using Microsoft.Office.Interop.Outlook;


/* Hello!
 * Please feel free to check out the basic Async code here, and then browse
   through the rest of the files/folders in this repository. I tried to
   organize better this time.
    */
namespace Interface
{
    /// <summary>
    /// Interaction logic for Interface.xaml
    /// </summary>
    public partial class MainWindow
    {
        private List<JobNode> _jobNodes = new List<JobNode>();
        private Job _selectedJob = new Job();
        private User _currentUser = new User() {Name = "Alexander"};
        private readonly JobAnalysis Daemon = new JobAnalysis(); // Our trustworthy companion!
        public string inputDate { get; set; } = "03.06.17";

        public MainWindow()
        {
            InitializeComponent();

        }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            // Removed the async worker from OnLoaded and moved it to the search button OnClick.
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            BackgroundWorker work = new BackgroundWorker();
            work.RunWorkerCompleted += Work_RunWorkerCompleted;
            work.DoWork += Work_DoWork;
            work.RunWorkerAsync();
        }

        /// <summary>
        /// Populate the list of jobs from House, Prouse, and Prospecting for our data binding.
        /// </summary>
        private void Work_DoWork(object sender, DoWorkEventArgs e)
        {
            System.Windows.Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal,(ThreadStart)delegate {
                inputDate = TextBoxDate.Text;
                _jobNodes = GatherJobs.Run(inputDate);
            });
        }

        /// <summary>
        /// Jobs have been constructed and are now ready to bind to the treeview.
        /// </summary>
        private void Work_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            JobsTreeView.ItemsSource = _jobNodes;
        }

        /// <summary>
        /// Capture when the user selects a new job, to prepare work in the background.
        /// </summary>
        private void JobsTreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            // Ensure that we aren't auditing a JobNode.
            if (e.NewValue.GetType() == typeof(Job))
            {
                _selectedJob = (Job) e.NewValue;
                Signator.Content = Daemon.Audit(_selectedJob); // Check for an approval signature on the job.
            }
        }

        private void Sign_Click(object sender, RoutedEventArgs e)
        {
            // Don't attempt to sign a job before selecting one.
            if (_selectedJob.Name != "")
            {
                Daemon.Sign(_selectedJob, _currentUser);
                Signator.Content = Daemon.Audit(_selectedJob);
            }
        }
    }
}
