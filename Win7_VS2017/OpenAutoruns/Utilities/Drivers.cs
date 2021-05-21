using System;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.Win32;

namespace OpenAutoruns.Utilities
{
    /// <summary>
    /// Autorun based on Drivers
    /// </summary>
    class Driver : Base
    {
        // Registry Entry Related to Drivers
        public static readonly string DriverRegEntry = @"HKLM\System\CurrentControlSet\Services";

        // Search Registry for Drivers       
        public static void SearchRegDrivers(string entry, ref ObservableCollection<Driver> driverRegs)
        {
            entry = entry.Substring(5);
            RegistryKey subKey = Registry.LocalMachine.OpenSubKey(entry, false);
            string[] subKeyNames = subKey.GetSubKeyNames();
            foreach (string subKeyName in subKeyNames)
            {
                RegistryKey subSubKey = subKey.OpenSubKey(subKeyName);
                string[] valueNames = subSubKey.GetValueNames();

                // `TYPE <= 8` means a driver, also the path ends with `.sys`
                if (valueNames.Contains("Type") && (int)subSubKey.GetValue("Type") <= 8)
                {
                    if (valueNames.Contains("ImagePath"))
                    {
                        string imagePath = (string)subSubKey.GetValue("ImagePath");
                        imagePath = ServicesTool.FilterImagePath(subSubKey, imagePath, true);

                        var driver = new Driver
                        {
                            Entry = subKeyName,
                            Description = ServicesTool.GetAllDescription(subSubKey, imagePath),
                            Publisher = Tool.GetPublisher(imagePath),
                            ImagePath = imagePath,
                            TimeStamp = Tool.GetTimeStamp(imagePath),
                            Start = ServicesTool.GetStart(subSubKey),
                            Type = ServicesTool.GetType(subSubKey)
                        };

                        driverRegs.Add(driver);
                    }
                }
            }
        }

        public string Start { get; set; }
        public string Type { get; set; }
    }
}