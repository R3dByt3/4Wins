using Configuration.Contracts;
using DataStoring;
using ExtendedIO;
using System;
using System.Collections.Generic;
using System.IO;

namespace Configuration
{
    public class Configurator : IConfigurator
    {
        private Dictionary<object, object> _applicationSettings;
        private readonly string _cfgPath;
        private readonly ICompressor _compressor;

        public Configurator(ICompressor compressor)
        {
            _compressor = compressor;
            _applicationSettings = new Dictionary<object, object>();
            _cfgPath = Directory.GetCurrentDirectory() + @"\config\config.cfg";
            if (!Directory.Exists(Path.GetDirectoryName(_cfgPath)))
                Directory.CreateDirectory(Path.GetDirectoryName(_cfgPath));
        }

        public void Load()
        {
            string saveFile = System.IO.File.ReadAllText(_cfgPath);
            Set(saveFile);
            Set(_compressor.DeCompress<Settings>(System.IO.File.ReadAllBytes(saveFile)));
        }

        public void Save()
        {
            System.IO.File.WriteAllText(_cfgPath, Get<string>());
            System.IO.File.WriteAllBytes(Get<string>(), _compressor.Compress(Get<Settings>()));
        }

        public T Get<T>()
        {
            if (!_applicationSettings.ContainsKey(typeof(T))) throw new ArgumentNullException(
                "This value has not been set yet!");
            return (T)_applicationSettings[typeof(T)];
        }

        public void Set<T>(T Setting)
        {
            if (_applicationSettings.ContainsKey(typeof(T))) _applicationSettings.Remove(typeof(T));
            _applicationSettings.Add(typeof(T), Setting);
        }
    }
}
