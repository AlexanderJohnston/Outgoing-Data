using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Outlook = Microsoft.Office.Interop.Outlook;
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

namespace Interface
{
    /// <summary>
    /// Interaction logic for Interface.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            Job testJob = new Job
            {
                Name = "AL9028E",
                Date = "01.22.17",
                Type = "Prospecting"
            };
            MessageBox.Show($"{testJob.Name} is mailing on {testJob.Date} as a {testJob.Type} job.");
            FileObserveration.House("02.06.17");
            FileObserveration.Prospecting();
        }
    }

    /// <summary>
    /// Interface for mailing jobs that enforces the various required properties.
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

    public class JobFile : Job
    {
        //
    }

    /// <summary>
    /// Interface for clients which enforces the properties they'll need for iteration later on.
    /// </summary>
    internal interface IClient
    {
        string Name { get; set; }

        List<string> Emails { get; set; }

        string FTP { get; set; }

        string Login { get; set; }

        string Password { get; set; }
    }

    /// <summary>
    /// This class represents a client and their properties to be accessed for sending out jobs.
    /// </summary>
    public class Client : IClient
    {
        // Backing store for the client.

        public string Name { get; set; } = "";

        public string FTP { get; set; } = "";

        public string Login { get; set; } = "";

        public string Password { get; set; } = "";

        public List<string> Emails { get; set; } = new List<string>();
    }

    /// <summary>
    /// Interface for basic email setup which will interact with Outlook interop.
    /// </summary>
    internal interface IEmail
    {
        string Subject { get; set; }

        string Body { get; set; }

        List<string> Attachments { get; set; }
    }

    /// <summary>
    /// This class represents a single email to be sent out.
    /// </summary>
    public class Email : IEmail
    {
        public string Subject { get; set; } = "";

        public string Body { get; set; } = "";

        public List<string> Attachments { get; set; } = new List<string>();
    }

    public class FileObserveration
    {
        /// <summary>
        /// Accesses the house job files to see what is available based on a date.
        /// </summary>
        /// <param name="date">Formatted as "MM.dd.yy".</param>
        /// <returns>An array of jobs available for that date.</returns>
        public static string[] House(string date)
        {
            const string baseDir = @"\\engagests1\Elements\Prospect Jobs\Conversions\00-HOUSE_PROUSE\Completed\";
            string[] house = Directory.GetDirectories(baseDir + date + @" House\");
            return house;
        }

        /// <summary>
        /// Accesses the prospecting house files to see what is available based on a date.
        /// </summary>
        /// <param name="date">Formatted as "MM.dd.yy".</param>
        /// <returns>An array of jobs available for that date.</returns>
        public static string[] Prouse(string date)
        {
            const string baseDir = @"\\engagests1\Elements\Prospect Jobs\Conversions\00-HOUSE_PROUSE\Completed\";
            string[] prouse = Directory.GetDirectories(baseDir + date + @" Prospecting-PROUSE\");
            return prouse;
        }

        /// <summary>
        /// Accesses the full prospecting files to see what is available.
        /// </summary>
        /// <returns>An array of jobs available in prospecting.</returns>
        public static List<string> Prospecting()
        {
            const string baseDir = @"\\engagests1\Elements\Prospect Jobs\Conversions\";
            List<string> prospecting = Directory.GetDirectories(baseDir).ToList();
            // This is used to differentiate job folders from general folders.
            Regex nameSchema = new Regex(@"\w{2}\d{4}\w$");
            List<string> jobList = new List<string>();
            foreach (string job in prospecting)
            {
                try
                {
                    if (!nameSchema.Match(job).Success) continue;
                    jobList.Add(job);
                }
                catch (ArgumentNullException e)
                {
                    MessageBox.Show("There are no folders, or your Prospecting drive has moved elsewhere.");
                    throw;
                }
            }
            return jobList;
        }
    }

}
