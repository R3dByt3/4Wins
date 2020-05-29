using Caliburn.Micro;
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
            _eventAggregator.PublishOnUIThread(new GameFieldViewModel(true));
        }

        public void PlayVsPlayer()
        {
            _eventAggregator.PublishOnUIThread(new GameFieldViewModel(false));
        }

        public void PlayVsNetwork()
        {

        }
    }
}
