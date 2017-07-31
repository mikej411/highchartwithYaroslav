using System;
using System.IO;

namespace Browser.Core.Framework
{
    public class PerformanceUtils
    {
        #region Methods

        /// <summary>
        /// Stores the performance timing information into a CSV file on the network
        /// </summary>
        /// <param name="dt">The date and time when the performance test was started</param>
        /// <param name="str">The description of the test</param>
        /// <param name="ts">How long it took to perform the test</param>
        public static void StoreResultsInCSV(DateTime dt, string desc, TimeSpan ts)
        {
            var folderLocation = "c:\\TestCases\\PerformanceResults";

            // Create the above directory folder if it doesnt exist
            if (!Directory.Exists(folderLocation))
            {
                Directory.CreateDirectory(folderLocation);
            }

            string filePath = folderLocation + "\\\\Results.csv";

            // Create the above file if it doesnt exist
            if (!File.Exists(filePath))
            {
                File.Create(filePath).Close();
            }

            string line = String.Format("{0},{1},{2}", dt, desc, ts);

            // write to excel the variables that were passed to this method
            using (StreamWriter sw = File.AppendText(filePath))
            {
                sw.WriteLine(line);
            }
        }

        #endregion Methods
    }
}