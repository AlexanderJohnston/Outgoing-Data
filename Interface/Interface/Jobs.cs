using System.Collections.Generic;
using System.IO;

namespace Interface
{
    public class Jobs
    {
        
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
            JobNode houseNode = FileObservation.House(date);
            JobNode prouseNode = FileObservation.Prouse(date);
            JobNode prospectingNode = FileObservation.Prospecting();
            List<JobNode> currentJobs = new List<JobNode> { houseNode, prouseNode, prospectingNode };
            return currentJobs;
        }
    }
}