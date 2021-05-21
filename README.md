# OpenAutoruns
<p align="center"><img src=".imgs/logo.png"/></p>

### Overview

**OpenAutoruns** is an open-source autoruns viewer for Windows built upon [Microsoft WPF Framework](https://docs.microsoft.com/en-us/dotnet/desktop/wpf/). The window style is powered by [MahApps.Metro](https://github.com/MahApps/MahApps.Metro). The basic feature is referred to [Sysinternals Autoruns](https://docs.microsoft.com/en-us/sysinternals/downloads/autoruns). With this tool, you can easily view all kinds of autorun entries on your Windows system, including:

##### *Compulsory*

- [x] **Logon**: Startup Directories and Registries based Autoruns
- [x] **Services**: Services based Autoruns
- [x] **Drivers**: Drivers based Autoruns
- [x] **Scheduled Tasks**: Scheduled Tasks based Autoruns

##### *Optional*

- [x] **Internet Explorer**: Browser Helper Objects (BHOs) of Internet Explorer based Autoruns
- [ ] **Boot Execute**: Boot Execute based Autoruns
- [ ] **Image Hijacks**: Image Hijacks based Autoruns
- [x] **Known DLLs**: Known DLLs based Autoruns
- [ ] **Winsock Providers**: Winsock Service Provider based Autoruns
- [ ] **Winlogon**: Windows User Logon based Autoruns

### Usage

Just download the Visual Studio Solution in this repo, and run with your Visual Studio! 

Note: Add the reference to `./OpenAutoruns/Utilities/Interop.TaskScheduler.dll` to use the namespace `TaskScheduler`.

* **Development Environment**
  * Windows 10 
  * Visual Studio 2019
  * .NET Framework 4.8
  * .NET Core 3.1
  * Visual C# WPF App

### License

GNU General Public License v3.0

