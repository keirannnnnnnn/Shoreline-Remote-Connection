# VNC Remote Desktop Client

A modern Windows VNC client application with a beautiful UI inspired by Windows Copilot and Apple's liquid glass design aesthetic.

## Features

### 🎨 Modern UI Design
- **Windows Copilot inspired** color scheme and layout
- **Liquid glass effects** with subtle shadows and transparency
- **Clean, minimal interface** focused on usability
- **Responsive design** that adapts to different window sizes

### 🖥️ VNC Connection Management
- **Saved Connections**: Store and organize your frequently used VNC connections
- **Recent Connections**: Quick access to recently used connections
- **Custom Display Names**: Rename your connections for easy identification
- **Connection Details**: Store IP addresses, ports, and optional passwords

### ⚡ Smart Features
- **Quick Connect**: Double-click any connection card to connect instantly
- **Edit Connections**: Easily modify connection settings
- **Connection Status**: Real-time status indicators and connection duration
- **Persistent Storage**: All connections are saved locally and restored on app restart

## Getting Started

### Prerequisites
- Windows 10/11
- .NET 8.0 Runtime
- VNC server running on target machines

### Installation
1. Clone or download this repository
2. Open the solution in Visual Studio 2022 or later
3. Build and run the application

### First Run
1. Click "New Connection" to add your first VNC server
2. Fill in the connection details:
   - **Display Name**: A friendly name for your connection (e.g., "Home PC", "Work Server")
   - **IP Address**: The IP address of the VNC server
   - **Port**: VNC port (default is 5900)
   - **Password**: Optional VNC password
3. Save the connection
4. Double-click the connection card to connect

## User Interface

### Main Window
- **Header**: Application title and quick "New Connection" button
- **Saved Connections**: Grid of connection cards for your saved connections
- **Recent Connections**: Recently used connections for quick access
- **Empty State**: Helpful guidance when no connections are saved

### Connection Cards
Each connection is displayed as a modern card with:
- **Display Name**: Your custom name for the connection
- **IP Address and Port**: Connection details
- **Last Connected**: When you last used this connection
- **Quick Actions**: Edit and delete buttons

### VNC Viewer Window
- **Modern Toolbar**: Connection info, status, and control buttons
- **Status Indicator**: Visual connection status with colored indicator
- **VNC Display**: Full-screen VNC viewer area
- **Status Bar**: Connection duration and quality information

## Architecture

### Project Structure
```
VNCClient/
├── Models/
│   └── VNCConnection.cs         # Connection data model
├── Services/
│   └── ConnectionService.cs     # Connection management and persistence
├── ViewModels/
│   ├── MainViewModel.cs         # Main window logic
│   └── VNCViewerViewModel.cs    # VNC viewer logic
├── Views/
│   ├── ConnectionDialog.xaml    # Add/edit connection dialog
│   └── VNCViewerWindow.xaml     # VNC viewer window
├── Converters/
│   └── BooleanToVisibilityConverter.cs
├── App.xaml                     # Application resources and styling
└── MainWindow.xaml              # Main application window
```

### Key Technologies
- **WPF (Windows Presentation Foundation)**: Modern Windows desktop UI framework
- **MVVM Pattern**: Clean separation of UI and business logic
- **Data Binding**: Reactive UI updates
- **JSON Persistence**: Local storage of connection data

## Customization

### Styling
The application uses a comprehensive styling system defined in `App.xaml`:
- **Color Palette**: Windows Copilot inspired colors
- **Modern Controls**: Custom button, textbox, and card styles
- **Liquid Glass Effects**: Subtle gradients and shadow effects

### Adding VNC Libraries
To add actual VNC connectivity, you can integrate libraries such as:
- **VncSharp**: Open-source .NET VNC client library
- **TightVNC .NET**: Commercial VNC components
- **RemoteViewing**: Modern .NET VNC implementation

The VNC viewer window is already set up with placeholders for VNC control integration.

## Development Notes

### VNC Integration
The current implementation includes:
- ✅ UI framework and design
- ✅ Connection management
- ✅ Persistence layer
- ⚠️ VNC protocol implementation (placeholder)

To add real VNC functionality:
1. Add a VNC client library to the project
2. Update `VNCViewerViewModel.ExecuteConnect()` to use the VNC library
3. Embed the VNC control in the `VncDisplayBorder` in `VNCViewerWindow.xaml`

### Data Storage
Connection data is stored in JSON files in the user's AppData folder:
- `%APPDATA%/VNCClient/saved_connections.json`
- `%APPDATA%/VNCClient/recent_connections.json`

## Screenshots

*Screenshots would show the beautiful modern UI with connection cards, the VNC viewer window, and the connection dialog.*

## Contributing

Feel free to contribute to this project by:
- Adding VNC protocol implementations
- Improving the UI/UX design
- Adding new features
- Fixing bugs

## License

This project is open source. Please check the license file for details.