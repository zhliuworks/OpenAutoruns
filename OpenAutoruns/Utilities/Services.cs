using System;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.Win32;

namespace OpenAutoruns.Utilities
{
    /// <summary>
    /// Autorun based on Services
    /// </summary>
    class Service : Base
    {
        // Registry Entry Related to Services
        public static readonly string ServiceRegEntry = @"HKLM\System\CurrentControlSet\Services";

        // Search Registry for Services       
        public static void SearchRegServices(string entry, ref ObservableCollection<Service> serviceRegs)
        {
            entry = entry.Substring(5);
            RegistryKey subKey = Registry.LocalMachine.OpenSubKey(entry, false);
            string[] subKeyNames = subKey.GetSubKeyNames();
            foreach (string subKeyName in subKeyNames)
            {
                RegistryKey subSubKey = subKey.OpenSubKey(subKeyName);
                string[] valueNames = subSubKey.GetValueNames();

                // `TYPE >= 16` means a service, also the path ends with `.exe`
                if (valueNames.Contains("Type") && (int)subSubKey.GetValue("Type") >= 16)
                {
                    if (valueNames.Contains("ImagePath"))
                    {
                        try
                        {
                            string imagePath = (string)subSubKey.GetValue("ImagePath");
                            imagePath = ServicesTool.FilterImagePath(subSubKey, imagePath, false);

                            var service = new Service
                            {
                                Entry = subKeyName,
                                Description = ServicesTool.GetAllDescription(subSubKey, imagePath),
                                Publisher = Tool.GetPublisher(imagePath),
                                ImagePath = imagePath,
                                TimeStamp = Tool.GetTimeStamp(imagePath),
                                Start = ServicesTool.GetStart(subSubKey),
                                Type = ServicesTool.GetType(subSubKey)
                            };

                            serviceRegs.Add(service);
                        }
                        catch {}
                    }
                }
            }
        }

        public string Start { get; set; }
        public string Type { get; set; }
    }
}