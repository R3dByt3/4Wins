using MahApps.Metro.Controls;
using GUI.ViewModels;

namespace GUI.Views
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindowView : MetroWindow
    {
        private MainWindowViewModel _viewModel;

        public MainWindowView()
        {
            InitializeComponent();
            _viewModel = new MainWindowViewModel();
            base.DataContext = _viewModel;
        }
    }
}
