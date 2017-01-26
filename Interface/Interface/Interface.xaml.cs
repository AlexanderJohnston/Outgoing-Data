using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

            MessageBox.Show(String.Format("{0} is mailing on {1} as a {2} job.", testJob.Name, testJob.Date, testJob.Type));
        }
    }

    /// <summary>
    /// Interface for mailing jobs that enforces the various required properties.
    /// </summary>
    internal interface IJob
    {
        string Name
        {
            get;
            set;
        }

        string Type
        {
            get;
            set;
        }

        string Date
        {
            get;
            set;
        }
    }

    /// <summary>
    /// Management class to simplify interaction with our jobs network.
    /// </summary>
    public class Job : IJob
    {
        // Backing store for a specific job.

        public string Name //Read-Write
        { get; set; } = "";

        public string Type // Read-Write
        { get; set; } = "";

        public string Date // Read-Write
        { get; set; } = "";
    }

    /// <summary>
    /// Interface for clients which enforces the properties they'll need for iteration later on.
    /// </summary>
    internal interface IClient
    {
        string Name
        {
            get;
            set;
        }

        List<String> Emails
        {
            get;
            set;
        }

        string FTP
        {
            get;
            set;
        }

        string Login
        {
            get;
            set;
        }

        string Password
        {
            get;
            set;
        }
    }

    /// <summary>
    /// This class represents a client and their properties to be accessed for sending out jobs.
    /// </summary>
    public class Client : IClient
    {
        // Backing store for the client.

        public string Name //Read-Write
        { get; set; } = "";

        public string FTP //Read-Write
        { get; set; } = "";

        public string Login //Read-Write
        { get; set; } = "";

        public string Password //Read-Write
        { get; set; } = "";

        public List<String> Emails //Read-Write
        { get; set; } = new List<String>();
    }
}
