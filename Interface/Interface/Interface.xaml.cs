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
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            InitializeComponent();
            Job testJob = new Job();
            testJob.Name = "AL9028E";
            testJob.Date = "01.22.17";
            testJob.Type = "Prospecting";

            MessageBox.Show(String.Format("{0} is mailing on {1} as a {2} job.", testJob.Name, testJob.Date, testJob.Type));
        }
    }

    /// <summary>
    /// Interface for mailing jobs that enforces the various required properties.
    /// </summary>
    interface IJob
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
        private string jobName = "";
        private string jobType = "";
        private string jobDate = "";

        public string Name //Read-Write
        {
            get
            {
                return jobName;
            }
            set
            {
                jobName = value;
            }
        }

        public string Type // Read-Write
        {
            get
            {
                return jobType;
            }
            set
            {
                jobType = value;
            }
        }

        public string Date // Read-Write
        {
            get
            {
                return jobDate;
            }
            set
            {
                jobDate = value;
            }
        }

        public Job() // Constructor
        {
        }

    }

    /// <summary>
    /// Interface for clients which enforces the properties they'll need for iteration later on.
    /// </summary>
    interface IClient
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
        private string clientName = "";
        private string clientFTP = "";
        private string clientLogin = "";
        private string clientPassword = "";
        private List<String> clientEmails = new List<String>();

        public string Name //Read-Write
        {
            get
            {
                return clientName;
            }
            set
            {
                clientName = value;
            }
        }

        public string FTP //Read-Write
        {
            get
            {
                return clientFTP;
            }
            set
            {
                clientFTP = value;
            }
        }

        public string Login //Read-Write
        {
            get
            {
                return clientLogin;
            }
            set
            {
                clientLogin = value;
            }
        }

        public string Password //Read-Write
        {
            get
            {
                return clientLogin;
            }
            set
            {
                clientLogin = value;
            }
        }

        public List<String> Emails //Read-Write
        {
            get
            {
                return clientEmails;
            }
            set
            {
                clientEmails = value;
            }
        }

        public Client() //Constructor
        {
        }
    }
}
