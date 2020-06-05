using Caliburn.Micro;
using GUI.Enums;
using Networking.Contracts;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GUI.ViewModels
{
    public class NetworkSearchViewModel : ViewModelBase
    {
        private readonly IBroadCaster _broadCaster;
        private readonly IAsynchronousClient _client;

        private IObservableCollection<string> _hosts;

        public IObservableCollection<string> Hosts
        {
            get => _hosts;
            set 
            { 
                _hosts = value;
                NotifyOfPropertyChange();
            }
        }


        public NetworkSearchViewModel()
        {
            _broadCaster = _kernel.Get<IBroadCaster>();
            _client = _kernel.Get<IAsynchronousClient>();
        }

        public void Host()
        {
            var ip = _broadCaster.Listen();

            _client.Ip = ip;

            _eventAggregator.PublishOnUIThread(new GameFieldViewModel(GameType.Host));
        }

        public void Search()
        {
            var ip = _broadCaster.Search();

            _client.Ip = ip;

            _eventAggregator.PublishOnUIThread(new GameFieldViewModel(GameType.Client));
        }
    }
}
