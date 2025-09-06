using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using Windows.Storage.Pickers;
using WinRT.Interop;

namespace VncRdpLauncher
{
    public sealed partial class MainWindow : Window
    {
        private ObservableCollection<ConnectionItem> connections = new();
        private string savePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "VncRdpLauncher", "connections.json");

        public MainWindow()
        {
            this.InitializeComponent();
            FileList.ItemsSource = connections;
            LoadConnections();
        }

        private async void AddFile_Click(object sender, RoutedEventArgs e)
        {
            var picker = new FileOpenPicker();
            picker.FileTypeFilter.Add(".vnc");
            picker.FileTypeFilter.Add(".rdp");

            // Required for WinUI 3
            var hwnd = WindowNative.GetWindowHandle(this);
            InitializeWithWindow.Initialize(picker, hwnd);

            var file = await picker.PickSingleFileAsync();
            if (file != null)
            {
                connections.Add(new ConnectionItem { Name = file.Name, Path = file.Path });
                SaveConnections();
            }
        }

        private void RemoveFile_Click(object sender, RoutedEventArgs e)
        {
            if (FileList.SelectedItem is ConnectionItem item)
            {
                connections.Remove(item);
                SaveConnections();
            }
        }

        private void FileList_DoubleTapped(object sender, Microsoft.UI.Xaml.Input.DoubleTappedRoutedEventArgs e)
        {
            if (FileList.SelectedItem is ConnectionItem item)
            {
                try
                {
                    if (item.Path.EndsWith(".vnc", StringComparison.OrdinalIgnoreCase))
                    {
                        Process.Start(new ProcessStartInfo
                        {
                            FileName = @"C:\Program Files\UltraVNC\vncviewer.exe",
                            Arguments = $"\"{item.Path}\"",
                            UseShellExecute = true
                        });
                    }
                    else if (item.Path.EndsWith(".rdp", StringComparison.OrdinalIgnoreCase))
                    {
                        Process.Start(new ProcessStartInfo
                        {
                            FileName = "mstsc.exe",
                            Arguments = $"\"{item.Path}\"",
                            UseShellExecute = true
                        });
                    }
                }
                catch (Exception ex)
                {
                    var dialog = new ContentDialog
                    {
                        Title = "Error",
                        Content = ex.Message,
                        CloseButtonText = "OK",
                        XamlRoot = this.Content.XamlRoot
                    };
                    _ = dialog.ShowAsync();
                }
            }
        }

        private void SaveConnections()
        {
            Directory.CreateDirectory(Path.GetDirectoryName(savePath)!);
            File.WriteAllText(savePath, JsonSerializer.Serialize(connections));
        }

        private void LoadConnections()
        {
            if (File.Exists(savePath))
            {
                var data = File.ReadAllText(savePath);
                var loaded = JsonSerializer.Deserialize<ObservableCollection<ConnectionItem>>(data);
                if (loaded != null)
                {
                    foreach (var item in loaded)
                        connections.Add(item);
                }
            }
        }
    }

    public class ConnectionItem
    {
        public required string Name { get; set; }
        public required string Path { get; set; }
    }
}