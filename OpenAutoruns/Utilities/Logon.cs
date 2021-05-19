using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Microsoft.Win32;

namespace OpenAutoruns.Utilities
{
    /// <summary>
    /// Autorun based on Startup Directory and Registry
    /// </summary>
    class Logon : Base
    {
        // Startup Directories
        public static readonly string[] StartupDirs =
        {
            @"%USERPROFILE%\AppData\Roaming\Microsoft\Windows\Start Menu\Programs\Startup",
            @"C:\ProgramData\Microsoft\Windows\Start Menu\Programs\Startup"
        };

        // Registry Entries Related to Autorun
        public static readonly string[] RegEntries =
        {
            @"HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Run",
            @"HKCU\SOFTWARE\Microsoft\Windows\CurrentVersion\Run",
            @"HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer\Run",
            @"HKCU\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer\Run",
            @"HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\RunOnce",
            @"HKCU\SOFTWARE\Microsoft\Windows\CurrentVersion\RunOnce",
            @"HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\RunOnceEx",
            @"HKCU\SOFTWARE\Microsoft\Windows\CurrentVersion\RunOnceEx",
            @"HKLM\Software\Wow6432Node\Microsoft\Windows\CurrentVersion\Run",
            @"HKCU\Software\Wow6432Node\Microsoft\Windows\CurrentVersion\Run"
        };

        // Search Registry for Logon
        public static void SearchRegLogon(string[] entries, ref ObservableCollection<Logon> logonRegs)
        {
            foreach (string entry in entries)
            {
                RegistryKey rootKey;
                switch (entry.Substring(0, 4))
                {
                    case "HKLM":
                        rootKey = Registry.LocalMachine;
                        break;
                    case "HKCU":
                        rootKey = Registry.CurrentUser;
                        break;
                    default:
                        return;
                }

                string childPath = entry.Substring(5);
                RegistryKey subKey = rootKey.OpenSubKey(childPath, false);
                if (subKey != null)
                {
                    foreach (string valueName in subKey.GetValueNames())
                    {
                        string imagePath = Tool.GetImagePath(valueName, subKey);
                        var logon = new Logon
                        {
                            Path = entry,
                            Entry = valueName,
                            Description = Tool.GetDescription(imagePath),
                            Publisher = Tool.GetPublisher(imagePath),
                            ImagePath = imagePath,
                            TimeStamp = Tool.GetTimeStamp(imagePath)
                        };
                        logonRegs.Add(logon);
                    }
                }
            }
        }

        public string Path { get; set; }
    }
}