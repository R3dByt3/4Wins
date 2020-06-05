using Caliburn.Micro;
using GUI.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI.ViewModels
{
    public class MainMenuViewModel : ViewModelBase
    {
        public void PlayVsAI()
        {
            _eventAggregator.PublishOnUIThread(new GameFieldViewModel(GameType.VsBot));
        }

        public void PlayVsPlayer()
        {
            _eventAggregator.PublishOnUIThread(new GameFieldViewModel(GameType.VsPlayer));
        }

        public void PlayVsNetwork()
        {
            _eventAggregator.PublishOnUIThread(new NetworkSearchViewModel());
        }
    }
}
