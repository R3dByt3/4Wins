using MahApps.Metro.Controls;
using GUI.ViewModels;

namespace GUI.Views
{
    /// <summary>
    /// Interaktionslogik fÃ¼r MainWindow.xaml
    /// </summary>
    public partial class MainWindowView : MetroWindow
    {
        private MainWindowViewModel _viewModel;

        public MainWindowView()
        {
            InitializeComponent();
            base.DataContext = this;
        }
    }