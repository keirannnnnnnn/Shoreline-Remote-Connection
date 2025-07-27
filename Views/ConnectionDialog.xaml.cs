using System;
using System.Windows;
using VNCClient.Models;

namespace VNCClient.Views
{
    public partial class ConnectionDialog : Window
    {
        private readonly VNCConnection _connection;

        public ConnectionDialog(VNCConnection connection)
        {
            InitializeComponent();
            _connection = connection;
            LoadConnectionData();
        }

        private void LoadConnectionData()
        {
            DisplayNameTextBox.Text = _connection.DisplayName;
            IpAddressTextBox.Text = _connection.IpAddress;
            PortTextBox.Text = _connection.Port.ToString();
            PasswordBox.Password = _connection.Password;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateInput())
            {
                SaveConnectionData();
                DialogResult = true;
                Close();
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(DisplayNameTextBox.Text))
            {
                MessageBox.Show("Please enter a display name.", "Validation Error", 
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                DisplayNameTextBox.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(IpAddressTextBox.Text))
            {
                MessageBox.Show("Please enter an IP address.", "Validation Error", 
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                IpAddressTextBox.Focus();
                return false;
            }

            if (!int.TryParse(PortTextBox.Text, out int port) || port < 1 || port > 65535)
            {
                MessageBox.Show("Please enter a valid port number (1-65535).", "Validation Error", 
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                PortTextBox.Focus();
                return false;
            }

            // Basic IP address validation
            if (!IsValidIpAddress(IpAddressTextBox.Text.Trim()))
            {
                MessageBox.Show("Please enter a valid IP address.", "Validation Error", 
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                IpAddressTextBox.Focus();
                return false;
            }

            return true;
        }

        private bool IsValidIpAddress(string ipAddress)
        {
            return System.Net.IPAddress.TryParse(ipAddress, out _);
        }

        private void SaveConnectionData()
        {
            _connection.DisplayName = DisplayNameTextBox.Text.Trim();
            _connection.IpAddress = IpAddressTextBox.Text.Trim();
            _connection.Port = int.Parse(PortTextBox.Text);
            _connection.Password = PasswordBox.Password;
        }
    }
}