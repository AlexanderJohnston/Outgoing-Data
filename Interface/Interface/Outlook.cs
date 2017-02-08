using System.Collections.Generic;
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

        private MailItem _emailItem = _emailApp.CreateItem(OlItemType.olMailItem);

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
            
            foreach (string fileName in attachments)
            _emailItem.Attachments.Add()

        }




    }
}