using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Security.RightsManagement;
using System.Text.RegularExpressions;
using System.Windows;

namespace Interface
{
    public class JobAnalysis
    {
        private string _path = "";
        private bool _approvedJob = false;
        public bool Approved => _approvedJob;

        public string Audit(Job currentJob)
        {
            _path = ConfigurationManager.AppSettings.Get(currentJob.Type);
            string signature = "";
            try
            {
                switch (currentJob.Type)
                {
                    case "House":
                        // House jobs.
                        string housePath = _path + currentJob.Date + @" House\" + currentJob.Name + @"\_meta\";
                        signature = File.ReadLines(housePath + "sign").Last();
                        break;
                    case "Prouse":
                        // Prouse jobs.
                        string prousePath = _path + currentJob.Date + @" Prospecting-PROUSE\" + currentJob.Name +
                                            @"\_meta\";
                        signature = File.ReadLines(prousePath + "sign").Last();
                        break;
                    case "Prospecting":
                        // Prospecting jobs.
                        string prospectingPath = _path + currentJob.Name + @"\_meta\";
                        signature = File.ReadLines(prospectingPath + "sign").Last();
                        break;
                    default:
                        // Default out.
                        return "Unsigned";
                        break;
                }
            }

            catch (Exception e)
            {
                signature = "Unsigned";
            }

            return signature;
        }

        public void Sign(Job currentJob, User currentUser)
        {
            _path = ConfigurationManager.AppSettings.Get(currentJob.Type);
            switch (currentJob.Type)
            {
                case "House":
                    // House jobs.
                    string housePath = _path + currentJob.Date + @" House\" + currentJob.Name + @"\_meta\";
                    Directory.CreateDirectory(housePath);
                    using (StreamWriter sw = new StreamWriter(housePath + "sign", true))
                    {
                        sw.WriteLine(currentUser.Name + " " + DateTime.Today);
                        _approvedJob = true;
                    }
                    break;
                case "Prouse":
                    // Prouse jobs.
                    string prousePath = _path + currentJob.Date + @" Prospecting-PROUSE\" + currentJob.Name + @"\_meta\";
                    Directory.CreateDirectory(prousePath);
                    using (StreamWriter sw = new StreamWriter(prousePath + "sign", true))
                    {
                        sw.WriteLine(currentUser.Name + " " + DateTime.Today);
                        _approvedJob = true;
                    }
                    break;
                case "Prospecting":
                    // Prospecting jobs.
                    string prospectingPath = _path + currentJob.Name + @"\_meta\";
                    Directory.CreateDirectory(prospectingPath);
                    using (StreamWriter sw = new StreamWriter(prospectingPath + "sign", true))
                    {
                        sw.WriteLine(currentUser.Name + " " + DateTime.Today);
                        _approvedJob = true;
                    }
                    break;
                default:
                    // Default out.
                    _approvedJob = false;
                    break;
            }

        }
    }
}