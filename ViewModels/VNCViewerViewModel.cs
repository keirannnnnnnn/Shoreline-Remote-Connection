using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using VNCClient.Models;

namespace VNCClient.ViewModels
{
    public class VNCViewerViewModel : INotifyPropertyChanged
    {
        private bool _isConnected;
        private bool _isConnecting;
        private string _statusMessage = "Ready to connect";
        private DateTime? _connectionStartTime;

        public VNCViewerViewModel(VNCConnection connection)
        {
            Connection = connection;
            ConnectCommand = new RelayCommand(ExecuteConnect, CanConnect);
            DisconnectCommand = new RelayCommand(ExecuteDisconnect, CanDisconnect);
        }

        public VNCConnection Connection { get; }

        public string WindowTitle => $"VNC - {Connection.DisplayName}";

        public bool IsConnected
        {
            get => _isConnected;
            private set
            {
                if (_isConnected != value)
                {
                    _isConnected = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(StatusText));
                    OnPropertyChanged(nameof(StatusColor));
                    CommandManager.InvalidateRequerySuggested();
                }
            }
        }

        public bool IsConnecting
        {
            get => _isConnecting;
            private set
            {
                if (_isConnecting != value)
                {
                    _isConnecting = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(StatusText));
                    OnPropertyChanged(nameof(StatusColor));
                    CommandManager.InvalidateRequerySuggested();
                }
            }
        }

        public string StatusText
        {
            get
            {
                if (IsConnecting) return "Connecting...";
                if (IsConnected) return "Connected";
                return "Disconnected";
            }
        }

        public Brush StatusColor
        {
            get
            {
                if (IsConnecting) return Brushes.Orange;
                if (IsConnected) return Brushes.Green;
                return Brushes.Gray;
            }
        }

        public string StatusMessage
        {
            get => _statusMessage;
            private set
            {
                if (_statusMessage != value)
                {
                    _statusMessage = value;
                    OnPropertyChanged();
                }
            }
        }

        public string ConnectionDuration
        {
            get
            {
                if (!_connectionStartTime.HasValue || !IsConnected)
                    return "Not connected";

                var duration = DateTime.Now - _connectionStartTime.Value;
                if (duration.TotalHours >= 1)
                    return $"{(int)duration.TotalHours:D2}:{duration.Minutes:D2}:{duration.Seconds:D2}";
                else
                    return $"{duration.Minutes:D2}:{duration.Seconds:D2}";
            }
        }

        public ICommand ConnectCommand { get; }
        public ICommand DisconnectCommand { get; }

        private bool CanConnect() => !IsConnected && !IsConnecting;
        private bool CanDisconnect() => IsConnected && !IsConnecting;

        private async void ExecuteConnect()
        {
            try
            {
                IsConnecting = true;
                StatusMessage = $"Connecting to {Connection.ConnectionString}...";

                // Simulate VNC connection process
                await Task.Delay(2000); // Simulate connection time

                // In a real implementation, you would:
                // 1. Create VNC client instance
                // 2. Connect to the VNC server
                // 3. Handle authentication if password is required
                // 4. Embed the VNC control in the VncDisplayBorder

                IsConnected = true;
                IsConnecting = false;
                _connectionStartTime = DateTime.Now;
                StatusMessage = $"Connected to {Connection.DisplayName}";

                // Start a timer to update connection duration
                StartDurationTimer();
            }
            catch (Exception ex)
            {
                IsConnecting = false;
                StatusMessage = $"Connection failed: {ex.Message}";
                MessageBox.Show($"Failed to connect to {Connection.DisplayName}:\n\n{ex.Message}", 
                    "Connection Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ExecuteDisconnect()
        {
            try
            {
                // In a real implementation, you would:
                // 1. Disconnect from VNC server
                // 2. Dispose VNC client resources
                // 3. Clear the VNC display

                IsConnected = false;
                _connectionStartTime = null;
                StatusMessage = "Disconnected";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Disconnect error: {ex.Message}";
            }
        }

        private void StartDurationTimer()
        {
            var timer = new System.Windows.Threading.DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            timer.Tick += (s, e) =>
            {
                if (IsConnected)
                {
                    OnPropertyChanged(nameof(ConnectionDuration));
                }
                else
                {
                    timer.Stop();
                }
            };
            timer.Start();
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}