using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Browser.Core.Framework
{
    /// <summary>
    /// 
    /// </summary>
    public static class FileUtils
    {
        /// <summary>
        /// Checks for the file once every second until the file exists and is not locked or the timeout is reached.
        /// </summary>
        /// <param name="filePath">The relative or absolute path to a file.</param>
        /// <param name="fileWaitTimeout">The timeout for this operation to keep trying in milliseconds.  Default is 10000 (10 seconds).</param>
        /// <exception cref="System.TimeoutException">Thrown if the file does not exist within the timeout specified.</exception>
        public static void WaitForFile(string filePath, double fileWaitTimeout = 10000)
        {
            var sw = System.Diagnostics.Stopwatch.StartNew();
            var fi = new FileInfo(filePath);
            while (!File.Exists(filePath) || IsFileLocked(fi))
            {
                if (sw.ElapsedMilliseconds > fileWaitTimeout)
                {
                    var msg = "The file \"{0}\" was not found within the timeout period of {1} milliseconds.";
                    if (!File.Exists(filePath))
                        msg += "  The file does not exist.";
                    else if (IsFileLocked(fi))
                        msg += "  The file is locked by another user or process.";
                    throw new TimeoutException(string.Format(msg, filePath, fileWaitTimeout));
                }
                Thread.Sleep(1000);
            }
        }

        /// <summary>
        /// Tries to lock the given file.  If locking fails returns true; otherwise, returns false.
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static bool IsFileLocked(FileInfo file)
        {
            FileStream stream = null;

            try
            {
                stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None);
            }
            catch (IOException)
            {
                //the file is unavailable because it is:
                //still being written to
                //or being processed by another thread
                //or does not exist (has already been processed)
                return true;
            }
            finally
            {
                if (stream != null)
                    stream.Dispose();
            }

            //file is not locked
            return false;
        }
    }
}