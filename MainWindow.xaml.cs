using System.Windows;
using VNCClient.ViewModels;

namespace VNCClient
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
        }
    }
}