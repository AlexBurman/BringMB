using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

namespace DirWatcher
{
    class Program
    {
        /// <summary>
        /// Will be executed by clock job on executing server
        /// </summary>
        static void Main()
        {
            // Declare objects
            ReadFiles rf = null;
            WriteFiles wf = null;
            ArrayList theConfig = null;
            Dictionary<string, bool> fileList = null;
            List<string> lstDoNotWarn = null;
            var sendNotification = new SendMail();

            // Initilize config / warning reading object
            try
            {
                rf = new ReadFiles();
            }
            catch(Exception ex)
            {
                System.Diagnostics.EventLog.WriteEntry("DirWatcher", ex.ToString());
            }

            // Initilize config / warning writing object
            try
            {
                wf = new WriteFiles();
            }
            catch(Exception ex)
            {
                System.Diagnostics.EventLog.WriteEntry("DirWatcher", ex.ToString());
            }

            // First...Get the config and the files that have been warned about
            if (rf != null)
            {
                theConfig = rf.GetAllConfig();
                fileList = rf.GetWarnedFiles();
                lstDoNotWarn = rf.DoNotWarn();
            }

            // Itterate trough config and check if we have any files stuck
            if (theConfig != null)
                foreach (ConfigObject co in theConfig)
                {
                    // Set up monitor to check new path
                    var mf = new MonitorFiles(co.Path, lstDoNotWarn);

                    // Get the updated file list
                    fileList = mf.CheckDir(fileList, int.Parse(co.Timeout));
                }

            // Is there a file in the file list that has NOT been warned about?
            if (fileList != null && fileList.Values.Contains(false))
            {
                try
                {
                    // Send the mail. If it succeeds then add the files to the fileList (mark as notified).
                    if (sendNotification.SendWarning(fileList))
                    {
                        // Write warned files
                        if (wf != null) 
                            wf.WriteData(fileList);
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.EventLog.WriteEntry("DirWatcher", ex.ToString());
                }
                finally
                {
                    // Do cleanup of the notification file (if needed).
                    if (wf != null) 
                        WriteFiles.CleanUp();
                }
            }
        }
    }
}
