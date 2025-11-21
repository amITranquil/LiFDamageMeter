# 🛡️ Life is Feudal - Damage Meter

A powerful damage meter application for Life is Feudal game built with C# WinForms and MaterialSkin.

## ✨ Features

### 🔥 Core Functionality
- **Log File Analysis**: Parse and analyze Life is Feudal combat logs
- **Damage Tracking**: Track damage dealt and taken with damage types
- **Combat Statistics**: Monitor kills, deaths, and enslavements  
- **Modern Dark UI**: Beautiful Material Design interface
- **Real-time Timestamps**: Full date and time display

### 📊 Data Display
- **Organized Columns**: 
  - Damage dealt/taken with damage types
  - Target/source information
  - Timestamps with full dates
  - Kill/death/enslavement tracking
- **Auto-Resizing Columns**: Automatically fit content
- **Zebra Pattern**: Alternating row colors for better readability
- **Hover Effects**: Interactive row highlighting

### 🎯 Enhanced Features
- **Damage Type Support**: Slashing, piercing, blunt, chopping, etc.
- **Efficiency Tracking**: Calculate total damage and hit counts
- **Premium Styling**: Material Design dark theme
- **Responsive Layout**: Auto-sizing data grid

## 🚀 Installation

### Download Release
1. Go to [Releases](https://github.com/username/lif-damage-meter/releases)
2. Download the latest `LiFDamageMeter.exe`
3. Run the application

### Build from Source
1. Clone the repository
2. Open `LiFDamageMeter.sln` in Visual Studio
3. Build and run the project

## 📁 Log File Location

Life is Feudal logs are typically located at:
```
C:\\Games\\Life is Feudal Arden\\default\\game\\game\\eu2\\logs\\YYYY-MM-DD\\
```

## 🎮 How to Use

1. **Launch** the application
2. **Click** the "Select Log File" button  
3. **Navigate** to your LiF logs directory
4. **Choose** a `.log` file to analyze
5. **View** your combat statistics and damage breakdown

## 📊 Supported Log Formats

The application parses these combat events:
- **Damage Dealt**: `You have hit <target> for <damage> of <type> damage`
- **Damage Taken**: `<source> has hit you for <damage> of <type> damage`
- **Kills**: `You have killed <target>`
- **Deaths**: Death window messages
- **Enslavements**: `You have enslaved <target>`

### Supported Damage Types
- Slashing
- Piercing  
- Blunt
- Chopping
- And more...

## 🛠️ Technical Details

- **Framework**: .NET Framework 4.7.2
- **UI Library**: MaterialSkin for modern styling
- **Language**: C#
- **Platform**: Windows

### Dependencies
- MaterialSkin.2.2.3.1
- System.Windows.Forms
- .NET Framework 4.7.2+

## 🔧 Development

### Prerequisites
- Visual Studio 2019 or later
- .NET Framework 4.7.2
- MaterialSkin NuGet package

### Building
```bash
git clone https://github.com/username/lif-damage-meter.git
cd lif-damage-meter
# Open LiFDamageMeter.sln in Visual Studio
# Build -> Build Solution
```

## 📝 Version History

### v1.0 (Latest)
- ✅ Complete damage type support with regex parsing
- ✅ Full date timestamps (YYYY-MM-DD HH:MM:SS)
- ✅ Auto-resizing columns for better data display
- ✅ Enhanced Material Design UI
- ✅ Improved damage type detection
- ✅ Zebra pattern with hover effects

## 🤝 Contributing

1. Fork the repository
2. Create your feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 🙏 Acknowledgments

- Life is Feudal community for feedback and testing
- MaterialSkin library for the beautiful UI
- .NET Framework team

## 📞 Support

If you encounter any issues:
- Open an [Issue](https://github.com/username/lif-damage-meter/issues)
- Check the latest log format updates
- Verify your log file location

---

**Made with ❤️ for the Life is Feudal community**