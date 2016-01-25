using System;
using System.Collections.Generic;
using System.IO;

namespace DirWatcher
{
    class WriteFiles
    {
        /// <summary>
        /// Write to the warned file so we do not send another warning in 15 minutes about the same files..
        /// </summary>
        /// <param name="fileList"></param>
        /// <returns></returns>
        public void WriteData(Dictionary<string, bool> fileList)
        {
            try
            {
                var sw = new StreamWriter("warned.txt", false);

                foreach (string filePath in fileList.Keys)
                {
                    sw.WriteLine(filePath);
                }

                sw.Close();
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// The warned file is deleted each day. Problems not resolved will get a new warning the next day.
        /// </summary>
        /// <returns></returns>
        public static void CleanUp()
        {
            if (File.Exists("dateStamp"))
            {
                DateTime stampDate = File.GetCreationTime("dateStamp");

                if (stampDate.DayOfWeek != DateTime.Now.DayOfWeek)
                {
                    try
                    {
                        File.Delete("dateStamp");
                        File.Delete("warned.txt");
                    }
                    catch(Exception ex)
                    {
                        System.Diagnostics.EventLog.WriteEntry("DirWatcher", "Error deleting dateStamp / warned files:\n\n" + ex.ToString());
                    }
                }
            }
            else
            {
                try
                {
                    var sw = new StreamWriter("dateStamp", false);

                    sw.Close();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.EventLog.WriteEntry("DirWatcher", "Error writing dateStamp:\n\n" + ex.ToString());
                }
            }
        }
    }
}
