using Caliburn.Micro;
using NetStandard.Logger;
using Ninject;

namespace GUI.ViewModels
{
    public class MainWindowViewModel : ViewModelBase, IHandle<Screen>
    {
        private Screen _currentModel;
        public Screen CurrentModel
        {
            get => _currentModel;
            set 
            { 
                _currentModel = value; 
                NotifyOfPropertyChange(); 
            }
        }

        public MainWindowViewModel()
        {
            CurrentModel = new MainMenuViewModel();
        }

        public void Handle(Screen message)
        {
            CurrentModel = message;
        }
    }
}
