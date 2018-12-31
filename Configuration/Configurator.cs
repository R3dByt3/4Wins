using Configuration.Contracts;
using DataStoring;
using DataStoring.Contracts;
using ExtendedIO.SQLiteSupport;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;

namespace Configuration
{
    public class Configurator : IConfigurator
    {
        private readonly Dictionary<object, object> _applicationSettings;
        private readonly IDBAccess _dBAccess;
        private readonly Properties.Settings _appSettings;

        public Configurator(IDBAccess dBAccess)
        {
            _appSettings = Properties.Settings.Default;
            _appSettings.PropertyChanged += SaveSettings;
            if (string.IsNullOrWhiteSpace(_appSettings.PathToDb))
            {
                _appSettings.PathToDb = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                    _appSettings.AppName, _appSettings.DbName);
            }
            _dBAccess = dBAccess;
            _applicationSettings = new Dictionary<object, object>();
        }

        private void SaveSettings(object sender, PropertyChangedEventArgs e)
        {
            _appSettings.Save();
        }

        public void Load(IEnumerable<Type> types)
        {
            _dBAccess.InitDBA(_appSettings.PathToDb);
            _dBAccess.InsertTables(types);
            IList<Settings> settings = _dBAccess.GetAll<Settings>().ToList();
            if (settings != null && settings.Count != 0)
            {
                Set((ISettings)settings.First());
            }
            else
            {
                Set((ISettings)null);
            }
        }

        public void Save()
        {
            _dBAccess.SaveObject(Get<ISettings>(), true);
        }

        public T Get<T>() where T : class
        {
            if (!_applicationSettings.ContainsKey(typeof(T)))
            {
                return null;
            }

            return (T)_applicationSettings[typeof(T)];
        }

        public void Set<T>(T Setting) where T : class
        {
            if (_applicationSettings.ContainsKey(typeof(T)))
            {
                _applicationSettings.Remove(typeof(T));
            }

            _applicationSettings.Add(typeof(T), Setting);
        }
    }
}
