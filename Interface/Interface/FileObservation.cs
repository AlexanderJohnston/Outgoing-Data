using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;

namespace Interface
{
    public class FileObservation
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