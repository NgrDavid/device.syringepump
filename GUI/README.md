# Syringe Pump GUI

This repository folder contains the Syringe Pump GUI (henceforth GUI) application that allows to configure the Syringe Pump device, developed by the Scientific Hardware Platform at the Champalimaud Foundation.

The Syringe Pump device is a Harp device and has all the inherent functionality of Harp devices.

The GUI was developed using [.NET 5](https://dotnet.microsoft.com/), [Avalonia](https://avaloniaui.net/) with ReactiveUI and makes direct use of the [Bonsai.Harp](https://github.com/bonsai-rx/harp) library.

As with other Harp devices, the Syringe Pump can also be used in [Bonsai](bonsai-rx.org/) using the [Bonsai.Harp.CF](https://github.com/bonsai-rx/harp.cf) package. 

## Installation

Go to the [Downloads](https://bitbucket.org/fchampalimaud/device.pump/downloads/) page to download the latest version for your Operating System.

Currently there are x64 builds for Windows and Linux. Mac builds will be available in the future.

Portable builds are also available.

### Linux

Since the application accesses the serial port, your user needs to be on the `dialout`group or equivalent.

There might be other alternatives to this, but at least on Ubuntu and Fedora que command that you need to run to add your user to the `dialout` group is:

```sh
sudo usermod -a -G dialout <USERNAME>
```

## Usage

Usage information is available in the [Wiki](https://bitbucket.org/fchampalimaud/device.pump/wiki/Home).

## For developers

### Build Windows installer using NSIS manually

- Install NSIS 3 on your Windows machine
- Build and publish the application using the .NET 6 SDK command-line tools
  ```
    dotnet publish -r win-x64 /p:PublishSingleFile=false /p:IncludeNativeLibrariesInSingleFile=true /p:Configuration=Release
  ```
- Run makesis to generate the installer
    ```
     makensis.exe /DVERSION_MAJOR=0 /DVERSION_MINOR=1 /DVERSION_BUILD=0 .\SyringePump.nsi
    ```
- The installer will be available at `.\bin\Release\net6.0\win-x64\SyringePump.vx.x.x-win-x64.self-contained.exe`

## Roadmap

See the [open issues](https://bitbucket.org/fchampalimaud/device.pump/issues) for a list of proposed features (and known issues).

## Contributing

Contributions are what make the open source community such an amazing place to be learn, inspire, and create. Any contributions you make are **greatly appreciated**.

1. Fork the Project
2. Create your Feature Branch (`git checkout -b feature/AmazingFeature`)
3. Commit your Changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the Branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## Authors

Scientific Hardware Platform and Scientific Software Platform of the Champalimaud Foundation.

### Main contributors

- Artur Silva
- Luís Teixeira

## License

See `LICENSE` file for more information.

