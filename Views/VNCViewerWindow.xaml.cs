using System.Windows;
using VNCClient.Models;
using VNCClient.ViewModels;

namespace VNCClient.Views
{
    public partial class VNCViewerWindow : Window
    {
        public VNCViewerWindow(VNCConnection connection)
        {
            InitializeComponent();
            DataContext = new VNCViewerViewModel(connection);
        }
    }
}