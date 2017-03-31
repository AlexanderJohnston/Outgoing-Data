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
                Signator.Content = Daemon.Audit(_selectedJob);
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

    /// <summary>
    /// Interface for mailing jobs that enforces the various required properties. To be expanded upon.
    /// </summary>
    internal interface IJob
    {
        string Name { get; set; }

        string Type { get; set; }

        string Date { get; set; }
    }

    /// <summary>
    /// Management class to simplify interaction with our jobs network.
    /// </summary>
    public class Job : IJob
    {
        // Backing store for a specific job.

        public string Name { get; set; } = "";

        public string Type { get; set; } = "";

        public string Date { get; set; } = "";
    }

    public class JobNode
    {
        public List<Job> Jobs { get; set; }

        public string Type { get; set; }

        public JobNode()
        {
            
            this.Jobs = new List<Job>();
        }
    }

    public class GatherJobs
    {
        public static List<JobNode> Run(string date)
        {
            JobNode houseNode = FileObserveration.House(date);
            JobNode prouseNode = FileObserveration.Prouse(date);
            JobNode prospectingNode = FileObserveration.Prospecting();
            List<JobNode> currentJobs = new List<JobNode> {houseNode, prouseNode, prospectingNode};
            return currentJobs;
        }
    }

    public class FileObserveration
    {
        /// <summary>
        /// Accesses the house job files to see what is available based on a date.
        /// </summary>
        /// <param name="date">Formatted as "MM.dd.yy".</param>
        /// <returns>A JobNode (List of Jobs with a Type) of available products in House for that date.</returns>
        public static JobNode House(string date)
        {
            const string baseDir = @"\\engagests1\Elements\Prospect Jobs\Conversions\00-HOUSE_PROUSE\Completed\";
            string[] house = Directory.GetDirectories(path: baseDir + date + @" House\");
            Regex nameSchema = new Regex(@"\w{2}\d{4}$");
            JobNode jobList = new JobNode();
            foreach (string job in house)
            {
                try
                {
                    if (!nameSchema.Match(job).Success) continue; // If regex doesn't match.
                    Job foundHouse = new Job
                    {
                        Name = (nameSchema.Match(job).Value),
                        Date = date,
                        Type = "House"
                    };
                    jobList.Jobs.Add(foundHouse);
                }
                catch (ArgumentNullException e)
                {
                    MessageBox.Show("There are no folders, or your Prospecting drive has moved elsewhere.");
                    throw;
                }
            }
            jobList.Type = "House";
            return jobList;
        }

        /// <summary>
        /// Accesses the prospecting house files to see what is available based on a date.
        /// </summary>
        /// <param name="date">Formatted as "MM.dd.yy".</param>
        /// <returns>A JobNode (List of Jobs with a Type) of available products in Prouse for that date.</returns>
        public static JobNode Prouse(string date)
        {
            const string baseDir = @"\\engagests1\Elements\Prospect Jobs\Conversions\00-HOUSE_PROUSE\Completed\";
            string[] prouse = Directory.GetDirectories(path: baseDir + date + @" Prospecting-PROUSE\");
            Regex nameSchema = new Regex(@"\w{2}\d{4}\w(?=\d+\w$)?");
            JobNode jobList = new JobNode();
            foreach (string job in prouse)
            {
                try
                {
                    if (!nameSchema.Match(job).Success) continue; // If regex doesn't match.
                    Job foundProuse = new Job
                    {
                        Name = (nameSchema.Match(job).Value),
                        Date = date,
                        Type = "Prouse"
                    };
                    jobList.Jobs.Add(foundProuse);
                }
                catch (ArgumentNullException e)
                {
                    MessageBox.Show("There are no folders, or your Prospecting drive has moved elsewhere.");
                    throw;
                }
            }
            jobList.Type = "Prouse";
            return jobList;
        }

        /// <summary>
        /// Accesses the full prospecting files to see what is available.
        /// </summary>
        /// <returns>A JobNode (List of Jobs with a Type) of available products in prospecting.</returns>
        public static JobNode Prospecting()
        {
            const string baseDir = @"\\engagests1\Elements\Prospect Jobs\Conversions\";
            List<string> prospecting = Directory.GetDirectories(path: baseDir).ToList();
            // This is used to differentiate job folders from general folders.
            Regex nameSchema = new Regex(@"(?<!x)\w{2}\d{4}\w$"); // The neg lookbehind is to remove "testing" jobs which start with a literal x.
            JobNode jobList = new JobNode();
            foreach (string job in prospecting)
            {
                try
                {
                    if (!nameSchema.Match(job).Success) continue; // If regex doesn't match.
                    Job foundProspecting = new Job
                    {
                        Name = (nameSchema.Match(job).Value),
                        Date = "",
                        Type = "Prospecting"
                    };
                    jobList.Jobs.Add(foundProspecting);
                }
                catch (ArgumentNullException e)
                {
                    MessageBox.Show("There are no folders, or your Prospecting drive has moved elsewhere.");
                    throw;
                }
            }
            jobList.Type = "Prospecting";
            return jobList;
        }
    }

}
