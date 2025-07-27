using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using VNCClient.Models;

namespace VNCClient.Services
{
    public class ConnectionService
    {
        private readonly string _savedConnectionsPath;
        private readonly string _recentConnectionsPath;
        private readonly ObservableCollection<VNCConnection> _savedConnections;
        private readonly ObservableCollection<VNCConnection> _recentConnections;

        public ConnectionService()
        {
            var appDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "VNCClient");
            Directory.CreateDirectory(appDataPath);
            
            _savedConnectionsPath = Path.Combine(appDataPath, "saved_connections.json");
            _recentConnectionsPath = Path.Combine(appDataPath, "recent_connections.json");
            
            _savedConnections = new ObservableCollection<VNCConnection>();
            _recentConnections = new ObservableCollection<VNCConnection>();
        }

        public ObservableCollection<VNCConnection> SavedConnections => _savedConnections;
        public ObservableCollection<VNCConnection> RecentConnections => _recentConnections;

        public async Task LoadConnectionsAsync()
        {
            await LoadSavedConnectionsAsync();
            await LoadRecentConnectionsAsync();
        }

        private async Task LoadSavedConnectionsAsync()
        {
            try
            {
                if (File.Exists(_savedConnectionsPath))
                {
                    var json = await File.ReadAllTextAsync(_savedConnectionsPath);
                    var connections = JsonConvert.DeserializeObject<VNCConnection[]>(json);
                    
                    _savedConnections.Clear();
                    if (connections != null)
                    {
                        foreach (var connection in connections)
                        {
                            _savedConnections.Add(connection);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading saved connections: {ex.Message}");
            }
        }

        private async Task LoadRecentConnectionsAsync()
        {
            try
            {
                if (File.Exists(_recentConnectionsPath))
                {
                    var json = await File.ReadAllTextAsync(_recentConnectionsPath);
                    var connections = JsonConvert.DeserializeObject<VNCConnection[]>(json);
                    
                    _recentConnections.Clear();
                    if (connections != null)
                    {
                        foreach (var connection in connections.OrderByDescending(c => c.LastConnected).Take(10))
                        {
                            _recentConnections.Add(connection);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading recent connections: {ex.Message}");
            }
        }

        public async Task SaveConnectionAsync(VNCConnection connection)
        {
            if (string.IsNullOrWhiteSpace(connection.DisplayName) || string.IsNullOrWhiteSpace(connection.IpAddress))
                return;

            var existing = _savedConnections.FirstOrDefault(c => c.Id == connection.Id);
            if (existing != null)
            {
                var index = _savedConnections.IndexOf(existing);
                _savedConnections[index] = connection;
            }
            else
            {
                _savedConnections.Add(connection);
            }

            await SaveSavedConnectionsAsync();
        }

        public async Task DeleteConnectionAsync(VNCConnection connection)
        {
            _savedConnections.Remove(connection);
            await SaveSavedConnectionsAsync();
        }

        public async Task AddToRecentAsync(VNCConnection connection)
        {
            connection.LastConnected = DateTime.Now;

            // Remove if already exists
            var existing = _recentConnections.FirstOrDefault(c => c.IpAddress == connection.IpAddress && c.Port == connection.Port);
            if (existing != null)
            {
                _recentConnections.Remove(existing);
            }

            // Add to beginning
            _recentConnections.Insert(0, connection);

            // Keep only last 10
            while (_recentConnections.Count > 10)
            {
                _recentConnections.RemoveAt(_recentConnections.Count - 1);
            }

            await SaveRecentConnectionsAsync();
        }

        private async Task SaveSavedConnectionsAsync()
        {
            try
            {
                var json = JsonConvert.SerializeObject(_savedConnections.ToArray(), Formatting.Indented);
                await File.WriteAllTextAsync(_savedConnectionsPath, json);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error saving connections: {ex.Message}");
            }
        }

        private async Task SaveRecentConnectionsAsync()
        {
            try
            {
                var json = JsonConvert.SerializeObject(_recentConnections.ToArray(), Formatting.Indented);
                await File.WriteAllTextAsync(_recentConnectionsPath, json);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error saving recent connections: {ex.Message}");
            }
        }

        public VNCConnection CreateNewConnection()
        {
            return new VNCConnection
            {
                DisplayName = "New Connection",
                IpAddress = "",
                Port = 5900
            };
        }
    }
}