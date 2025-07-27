using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using VNCClient.Models;
using VNCClient.Services;
using VNCClient.Views;

namespace VNCClient.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly ConnectionService _connectionService;

        public MainViewModel()
        {
            _connectionService = new ConnectionService();
            
            NewConnectionCommand = new RelayCommand(ExecuteNewConnection);
            ConnectCommand = new RelayCommand<VNCConnection>(ExecuteConnect);
            EditConnectionCommand = new RelayCommand<VNCConnection>(ExecuteEditConnection);
            DeleteConnectionCommand = new RelayCommand<VNCConnection>(ExecuteDeleteConnection);
            
            LoadConnections();
        }

        public ObservableCollection<VNCConnection> SavedConnections => _connectionService.SavedConnections;
        public ObservableCollection<VNCConnection> RecentConnections => _connectionService.RecentConnections;

        public bool HasSavedConnections => SavedConnections.Count > 0;
        public bool HasRecentConnections => RecentConnections.Count > 0;

        public ICommand NewConnectionCommand { get; }
        public ICommand ConnectCommand { get; }
        public ICommand EditConnectionCommand { get; }
        public ICommand DeleteConnectionCommand { get; }

        private async void LoadConnections()
        {
            await _connectionService.LoadConnectionsAsync();
            OnPropertyChanged(nameof(HasSavedConnections));
            OnPropertyChanged(nameof(HasRecentConnections));
        }

        private void ExecuteNewConnection()
        {
            var connection = _connectionService.CreateNewConnection();
            var dialog = new ConnectionDialog(connection);
            
            if (dialog.ShowDialog() == true)
            {
                SaveConnection(connection);
            }
        }

        private async void ExecuteConnect(VNCConnection? connection)
        {
            if (connection == null) return;

            try
            {
                // Add to recent connections
                await _connectionService.AddToRecentAsync(connection);
                
                // Open VNC viewer window
                var vncWindow = new VNCViewerWindow(connection);
                vncWindow.Show();
                
                OnPropertyChanged(nameof(HasRecentConnections));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to connect: {ex.Message}", "Connection Error", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ExecuteEditConnection(VNCConnection? connection)
        {
            if (connection == null) return;

            var dialog = new ConnectionDialog(connection);
            if (dialog.ShowDialog() == true)
            {
                SaveConnection(connection);
            }
        }

        private async void ExecuteDeleteConnection(VNCConnection? connection)
        {
            if (connection == null) return;

            var result = MessageBox.Show($"Are you sure you want to delete '{connection.DisplayName}'?", 
                "Delete Connection", MessageBoxButton.YesNo, MessageBoxImage.Question);
                
            if (result == MessageBoxResult.Yes)
            {
                await _connectionService.DeleteConnectionAsync(connection);
                OnPropertyChanged(nameof(HasSavedConnections));
            }
        }

        private async void SaveConnection(VNCConnection connection)
        {
            await _connectionService.SaveConnectionAsync(connection);
            OnPropertyChanged(nameof(HasSavedConnections));
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class RelayCommand : ICommand
    {
        private readonly Action _execute;
        private readonly Func<bool>? _canExecute;

        public RelayCommand(Action execute, Func<bool>? canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object? parameter) => _canExecute?.Invoke() ?? true;

        public void Execute(object? parameter) => _execute();
    }

    public class RelayCommand<T> : ICommand
    {
        private readonly Action<T?> _execute;
        private readonly Func<T?, bool>? _canExecute;

        public RelayCommand(Action<T?> execute, Func<T?, bool>? canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object? parameter) => _canExecute?.Invoke((T?)parameter) ?? true;

        public void Execute(object? parameter) => _execute((T?)parameter);
    }
}