using System;
using System.Collections.Generic;
using System.IO;
using System.Collections;

namespace DirWatcher
{
    /// <summary>
    /// Reads the configuration file and the "warned" file.
    /// 
    /// - Config contains folders to monitor
    /// - Warned contains information about files that the users previously this day has been warned about
    /// </summary>
    class ReadFiles
    {
        private readonly ArrayList _configList = new ArrayList();
        private readonly Dictionary<string, bool> _allreadyWarned = new Dictionary<string, bool>();
        private readonly List<string> _lstDoNotWarn;

        /// <summary>
        /// Do the reading and create the lists 
        /// </summary>
        public ReadFiles()
        {
            _lstDoNotWarn = new List<string>();
            StreamReader sReadConfig = null;
            StreamReader sReadWarned = null;

            try
            {

                // Get all files that have been warned about previously
                sReadWarned = new StreamReader("warned.txt");
                
                while (sReadWarned.Peek() != -1)
                {
                    string key = sReadWarned.ReadLine();
                    if (key != null && !_allreadyWarned.ContainsKey(key))
                        _allreadyWarned.Add(key, true);
                }
            }
            catch
            {
                // Just create an empty file
                File.Create("warned.txt");
            }
            finally
            {
                // Close the StreamReader
                if (sReadWarned != null) 
                    sReadWarned.Close();
            }

            try
            {
                // Read config for the locations we will check
                sReadConfig = new StreamReader("dirwatch.ini");
                while (sReadConfig.Peek() != -1)
                {
                    var co = new ConfigObject(sReadConfig.ReadLine());
                    _configList.Add(co);
                }

                sReadConfig.Close();
            }
            catch (FileNotFoundException fEx)
            {
                throw new FileNotFoundException("Missing config file!");
            }
            catch
            {
                throw;
            }
            finally
            {
                // Close the StreamReader
                if (sReadConfig != null) 
                    sReadConfig.Close();
            }

            try
            {
                sReadConfig = new StreamReader("doNotWarn.txt");

                while (sReadConfig.Peek() != -1)
                {
                    _lstDoNotWarn.Add(sReadConfig.ReadLine());
                }

            }
            finally
            {
                sReadConfig.Close();
            }
        }
    

        public ArrayList GetAllConfig() 
        {
            return _configList;
        }

        public Dictionary<string, bool> GetWarnedFiles()
        {
            return _allreadyWarned;
        }

        public List<string> DoNotWarn()
        {
            return _lstDoNotWarn;
        } 
    }
}
