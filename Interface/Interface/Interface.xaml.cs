using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using Outlook = Microsoft.Office.Interop.Outlook.Application;
using System.Text;
using System.Text.RegularExpressions;
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
using Microsoft.Office.Interop.Outlook;

namespace Interface
{
    /// <summary>
    /// Interaction logic for Interface.xaml
    /// </summary>
    public partial class MainWindow
    {
        private List<JobNode> _jobNodes = new List<JobNode>();

        public MainWindow()
        {
            InitializeComponent();
            List<string> subjects = new List<string>
            {
                "alexanderj@engageusa.com", "leslies@engageusa.com"
            };
            List<string> attachments = new List<string>
            {
                @"\\ENGAGESTS1\Elements\Prospect Jobs\Conversions\CN9004A\Final Mailing Files\CN9004A01X_SAMPLE.CSV",
                @"\\ENGAGESTS1\Elements\Prospect Jobs\Conversions\CN9004A\Final Mailing Files\CN9004A02X_SAMPLE.CSV"
            };
            /*EmailHandler test = new Interface.EmailHandler();
            bool valid = test.CreateEmail("Test Email", subjects, "Test Body of Email", attachments);
            if (valid == true)
            {
                var result = test.Send();
                MessageBox.Show(result, "test");
            }*/

            JobAnalysis testclass = new JobAnalysis();
            MessageBox.Show(testclass.Approved.ToString());
        }

        /// <summary>
        ///  Create a new work thread to run async for data binding.
        /// </summary>
        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
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
            _jobNodes = GatherJobs.Run("03.06.17");
        }

        /// <summary>
        /// Jobs have been constructed and are now ready to bind to the treeview.
        /// </summary>
        private void Work_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            JobsTreeView.ItemsSource = _jobNodes;
            LoadingGrid.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// Capture when the user selects a new job, to prepare work in the background.
        /// </summary>
        private void JobsTreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {

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
                    if (!nameSchema.Match(job).Success) continue;
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
                    if (!nameSchema.Match(job).Success) continue;
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
            Regex nameSchema = new Regex(@"\w{2}\d{4}\w$");
            JobNode jobList = new JobNode();
            foreach (string job in prospecting)
            {
                try
                {
                    if (!nameSchema.Match(job).Success) continue;
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
