using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinSCP;

namespace Interface
{
    /// <summary>
    /// This class will upload and report back on the status of the attempt.
    /// </summary>
    class SFTP
    {
        /// <summary>
        /// Main handler for SFTP class.
        /// </summary>
        /// <param name="userName">String containing username.</param>
        /// <param name="userPass">String containing password.</param>
        /// <param name="destination">String containing delivery folder.</param>
        /// <param name="files">List of string containing URI paths to files.</param>
        /// <returns></returns>
        public static bool Main(string userName, string userPass, string destination, List<string> files)
        {
            try
            {
                // Set up SessionOptions from the WinSCP assembly.
                SessionOptions sessionOptions = new SessionOptions
                {
                    Protocol = Protocol.Sftp,
                    HostName = "engageftp",
                    UserName = userName,
                    Password = userPass,
                    SshHostKeyFingerprint = "ssh-rsa 4096 d2:3f:16:5c:60:64:76:97:25:9c:f1:20:15:4e:63:26"
                };

                using (Session session = new Session())
                {
                    // Connect
                    session.Open(sessionOptions);

                    // Upload files
                    TransferOptions transferOptions = new TransferOptions();
                    transferOptions.TransferMode = TransferMode.Binary;

                    // Try each file
                    foreach (string file in files)
                    {
                        TransferOperationResult transferResult;
                        transferResult = session.PutFiles(file, destination, false, transferOptions);

                        // Throw on any error
                        transferResult.Check();

                        // Print results
                        foreach (TransferEventArgs transfer in transferResult.Transfers)
                        {
                            Console.WriteLine("Upload of {0} succeeded", transfer.FileName);
                        }
                    }
                }
                // Upload was a success.
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: {0}", e);
                return false;
            }
        }
    }
}

