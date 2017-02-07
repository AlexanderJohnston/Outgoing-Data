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

        public void CreateEmail(string subject, string to, string body, List<string> attachments)
        {
            // do stuff
        }




    }
}