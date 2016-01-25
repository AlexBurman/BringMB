namespace DirWatcher
{
    /// <summary>
    /// Only contains information about the monitored location
    /// </summary>
    class ConfigObject
    {
        public ConfigObject(string configLine)
        {
            string[] splitConfig = configLine.Split('|');

            if (splitConfig.Length == 3)
            {
                Path = splitConfig[0];
                Timeout = splitConfig[1];
                Comment = splitConfig[2];
            }
        }

        public string Path { get; private set; }
        public string Timeout { get; private set; }
        public string Comment { get; private set; }
    }
}
