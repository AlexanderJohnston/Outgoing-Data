using System;
using System.Collections.Generic;
using System.Security.RightsManagement;
using System.Text.RegularExpressions;
using System.Windows;

namespace Interface
{
    public class JobAnalysis
    {
        private bool _approvedJob = false;
        public bool Approved => _approvedJob;

        public void Sign(Job currentJob, User currentUser)
        {
            
        }
    }
}