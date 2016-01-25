using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.IO;

namespace DirWatcher
{
    /// <summary>
    /// Check each path defined in configuration. For each path check all files present there and see if
    /// any is older then the max allowed age.
    /// </summary>
    class MonitorFiles
    {
        private readonly string _checkPath;
        private static List<string> _doNotWarnAbout;

        public MonitorFiles(string path, List<string> lstDoNotWarn)
        {
            _checkPath = path;
            _doNotWarnAbout = lstDoNotWarn;
        }

        public Dictionary<string, bool> CheckDir(Dictionary<string, bool> fileList, int waitTime)
        {
            try
            {
                var dirInfo = new DirectoryInfo(_checkPath);

                foreach (FileInfo file in dirInfo.GetFiles())
                {
                    if (!DoNotWarnAbout(file.Name))
                    {
                        // Check for new files
                        if (file.LastWriteTime < DateTime.Now.AddMinutes( -waitTime))
                        {
                            if (!fileList.Keys.Contains(file.FullName))
                            {
                                fileList.Add(file.FullName, false);
                            }
                        }
                    }
                }
                return fileList;
            }
            catch (Exception ex)
            {
                System.Diagnostics.EventLog.WriteEntry("DirWatcher", ex.ToString(), EventLogEntryType.Error);
            }
            return fileList;
        }

        private static bool DoNotWarnAbout(string file)
        {
            if (_doNotWarnAbout.Any(f => file.Contains(f)))
                return true;
            else
                return false;
        }
    }
}
