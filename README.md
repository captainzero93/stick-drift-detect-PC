# Controller Drift Detector for PC

A Windows application designed to detect and measure controller stick drift. 

## Features

- Real-time stick position visualization
- Drift detection and measurement
- Visual tracking of stick movement patterns
- Dead zone visualization
- Calibration system for accurate measurements
- Detailed analysis of drift magnitude

## Supported Controllers

This application supports any DirectInput compatible controllers, including:
- Xbox Controllers (when using DirectInput mode)
- PlayStation Controllers (DS4/DualShock when using DirectInput mode)
- Generic USB Game Controllers
- PC Joysticks
- Most USB/Wireless gamepads that support DirectInput

## System Requirements

- Windows 7 or later
- .NET Framework 4.7.2 or higher
- DirectInput compatible controller
- USB port or Bluetooth (depending on controller type)

## Required NuGet Packages

The following NuGet packages are required:
- SharpDX (v4.2.0)
- SharpDX.DirectInput (v4.2.0)

## Installation

1. Clone or download this repository
2. Open the solution in Visual Studio
3. Restore NuGet packages
4. Build and run the application

```bash
# Using Git
git clone https://github.com/captainzero93/stick-drink-detect-PC.git
```

## Usage ( Also provided a built Exe under Releases )

1. Connect your controller to your PC
2. Launch the application
3. The app will automatically detect your controller
4. Click "Start Check" to begin drift detection
5. Keep your controller still during the test
6. After 5 seconds, the app will show the results
7. The visualization will show:
   - Red trail: Stick movement pattern
   - Gray circle: Dead zone
   - Coordinates: Current stick position
   - Status: Analysis of detected drift

## How It Works

The application samples the controller's stick position over time and analyzes any unwanted movement (drift). It uses the following parameters:

- Dead Zone: 1000 units (adjustable)
- Drift Threshold: 500 units
- Sample Time: 5 seconds
- Sampling Rate: 60Hz

## Building from Source

1. Prerequisites:
   - Visual Studio 2019 or later
   - .NET Framework 4.7.2 SDK
   - NuGet Package Manager

2. Build Steps:
   ```bash
   git clone https://github.com/captainzero93/stick-drink-detect-PC.git
   cd stick-drink-detect-PC
   ```
   - Open StickDrift.sln in Visual Studio
   - Restore NuGet packages
   - Build solution (F5 or Ctrl+Shift+B)

## License

This project is open source and available under the MIT License.


## Support

If you encounter any issues or have questions:
- Open an issue on GitHub
- The most common issues are related to controller compatibility or drivers
