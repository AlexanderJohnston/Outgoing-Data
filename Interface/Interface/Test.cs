using System.Collections.Generic;
using System.Windows;

namespace Interface
{
    public class Test
    {
        public static void Sign(List<JobNode> nodeList)
        {
            JobNode testNode = new JobNode();
            User testUser = new User() {Name = "Alexander"};
            foreach (JobNode node in nodeList)
            {
                if (node.Type == "Prospecting")
                {
                    testNode = node;
                }
            }
            JobAnalysis testclass = new JobAnalysis();
            testclass.Sign(testNode.Jobs[0], testUser);
            MessageBox.Show(testclass.Approved.ToString());
        }
        
    }
}