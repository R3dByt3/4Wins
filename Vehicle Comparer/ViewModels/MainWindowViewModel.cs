using Logger;
using Ninject;
using System;

namespace GUI.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private ILogger _logger;
        private StandardKernel _standardKernel;

        private VehicleViewModel _vehicleViewModel;
        private EquipmentViewModel _equipmentViewModel;
        private ComparerViewModel _comparerViewModel;

        private ViewModelBase _currentModel;
        public ViewModelBase CurrentModel
        {
            get { return _currentModel; }
            set { _currentModel = value; OnPropertyChanged(); }
        }

        public MainWindowViewModel()
        {
            _standardKernel = Controller.StandardKernel;
            _logger = _standardKernel.Get<ILogger>();
            _vehicleViewModel = new VehicleViewModel();
            _equipmentViewModel = new EquipmentViewModel();
            _comparerViewModel = new ComparerViewModel();
            SwitchPage(0);
        }

        public void SwitchPage(int pageId)
        {
            if (pageId == 0)
                CurrentModel = _vehicleViewModel;
            else if (pageId == 1)
                CurrentModel = _equipmentViewModel;
            else if (pageId == 2)
                CurrentModel = _comparerViewModel;
            else
                throw new Exception("Tab not found");
        }
    }
}
