using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows;
using Microsoft.Office.Core;
using Microsoft.Office.Interop.Outlook;
using Outlook = Microsoft.Office.Interop.Outlook.Application;

namespace Interface
{
    public class EmailHandler
    {
        private static Outlook _emailApp = new Outlook();

        public Outlook Email
        {
            get
            {
                return _emailApp;
            }

            set
            {
                _emailApp = value;
            }
        }

        /// <summary>
        ///  Internal storage for the CreateEmail method.
        /// </summary>
        private MailItem _emailItem = _emailApp.CreateItem(OlItemType.olMailItem);

        /// <summary>
        /// Will permit the Send() method to be executed.
        /// </summary>
        private bool _emailCreated = false;

        public string Send()
        {
            try
            {
                if (_emailCreated == true)
                {
                    _emailItem.Send();
                    return "E-mail sent.";
                }
                else
                {
                    return "E-mail item was not been fully created. Sending declined for now.";
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// This method will create an email item internally based on the parameters.
        /// </summary>
        /// <param name="subject">String containing plaintext.</param>
        /// <param name="to">A List<string> containing e-mail addresses.</param>
        /// <param name="body">String containing plaintext.</param>
        /// <param name="attachments">A List<string> containing full Windows URI paths to files.</param>
        /// <returns>True or false to verify that it was created successfully.</returns>
        public bool CreateEmail(string subject, List<string> to, string body, List<string> attachments)
        {
            // Initialize a counter for a loop later on.
            int i = 0;

            _emailItem.Subject = subject;
            _emailItem.Body = body;
            foreach (string recipient in to)
            {
                try
                {
                    _emailItem.Recipients.Add(to[i]);
                }
                catch
                {
                    // Fail out from method.
                    return false;
                }   
                // Keep the counter outside of the try-catch so we don't get funky recursion.
                i++;
            }
            // The regex has 3 groups, (Path); (Filename); (Extension); and is based on Windows URI.
            try
            {
                string regex = @"^\\(.+\\)*(.+)\.(.+)$";
                foreach (string fileName in attachments)
                {
                    // Add an attachment from the path as string, using regex to get the display name.
                    MatchCollection matches = Regex.Matches(fileName, regex);
                    _emailItem.Attachments.Add(fileName, 1, 1, matches[0].Groups[2].Value);
                }
            }
            catch
            {
                // Fail out from method.
                return false;
            }
            
            // Success.
            _emailCreated = true;
            return true;
        }




    }


}