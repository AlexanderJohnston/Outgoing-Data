using System.Collections.Generic;
using System;
using System.Configuration;

namespace Interface
{


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
}