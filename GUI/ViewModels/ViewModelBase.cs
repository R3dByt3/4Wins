using Caliburn.Micro;
using NetStandard.Logger;
using Ninject;

namespace GUI.ViewModels
{
    public abstract class ViewModelBase : Screen
    {
        protected readonly IEventAggregator _eventAggregator;
        protected readonly IKernel _kernel;
        protected readonly ILogger _logger;

        public ViewModelBase()
        {
            _kernel = Controller.Kernel;
            _eventAggregator = _kernel.Get<IEventAggregator>();
            _eventAggregator.Subscribe(this);
            var factory = _kernel.Get<ILoggerFactory>();
            _logger = factory.CreateFileLogger();
        }
    }
}
