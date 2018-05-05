using Configuration.Contracts;
using System;
using System.Collections.Generic;

namespace Configuration
{
    public class Configurator : IConfigurator
    {
        private Dictionary<object, object> _applicationSettings;

        public Configurator()
        {
            _applicationSettings = new Dictionary<object, object>();
        }

        public void Load()
        {
            //ToDo: IMPL
            //string saveFile = File.ReadAllText(Directory.GetCurrentDirectory() + @"\config\conf.MDc");
            //Set(saveFile);
            //Set(Compressor.DeCompress<Settings>(File.ReadAllBytes(saveFile)));
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
