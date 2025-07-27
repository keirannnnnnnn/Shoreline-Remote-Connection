using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace VNCClient.Models
{
    public class VNCConnection : INotifyPropertyChanged
    {
        private string _displayName = string.Empty;
        private string _ipAddress = string.Empty;
        private int _port = 5900;
        private string _password = string.Empty;
        private DateTime? _lastConnected;
        private bool _isFavorite;

        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string DisplayName
        {
            get => _displayName;
            set
            {
                if (_displayName != value)
                {
                    _displayName = value;
                    OnPropertyChanged();
                }
            }
        }

        public string IpAddress
        {
            get => _ipAddress;
            set
            {
                if (_ipAddress != value)
                {
                    _ipAddress = value;
                    OnPropertyChanged();
                }
            }
        }

        public int Port
        {
            get => _port;
            set
            {
                if (_port != value)
                {
                    _port = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                if (_password != value)
                {
                    _password = value;
                    OnPropertyChanged();
                }
            }
        }

        public DateTime? LastConnected
        {
            get => _lastConnected;
            set
            {
                if (_lastConnected != value)
                {
                    _lastConnected = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(LastConnectedDisplay));
                }
            }
        }

        public bool IsFavorite
        {
            get => _isFavorite;
            set
            {
                if (_isFavorite != value)
                {
                    _isFavorite = value;
                    OnPropertyChanged();
                }
            }
        }

        public string LastConnectedDisplay
        {
            get
            {
                if (!LastConnected.HasValue)
                    return "Never";

                var timeDiff = DateTime.Now - LastConnected.Value;
                if (timeDiff.TotalMinutes < 1)
                    return "Just now";
                if (timeDiff.TotalHours < 1)
                    return $"{(int)timeDiff.TotalMinutes} minutes ago";
                if (timeDiff.TotalDays < 1)
                    return $"{(int)timeDiff.TotalHours} hours ago";
                if (timeDiff.TotalDays < 7)
                    return $"{(int)timeDiff.TotalDays} days ago";
                
                return LastConnected.Value.ToString("MMM dd, yyyy");
            }
        }

        public string ConnectionString => $"{IpAddress}:{Port}";

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}