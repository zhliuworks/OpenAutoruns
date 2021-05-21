using System;
using System.Collections.ObjectModel;
using System.IO;
using Microsoft.Win32;

namespace OpenAutoruns.Utilities
{
    /// <summary>
    /// Autorun based on Browser Helper Objects
    /// </summary>
    class BHO : Base
    {
       // Registry Entries Related to Browser Helper Objects
        public static readonly string[] BHORegEntries =
        {
            @"HKLM\Software\Microsoft\Windows\CurrentVersion\Explorer\Browser Helper Objects",
            @"HKLM\Software\Wow6432Node\Microsoft\Windows\CurrentVersion\Explorer\Browser Helper Objects"
        };

        // Class ID Base Entry
        private static readonly string CLSIDEntry = @"Software\Classes\CLSID\";

        // Search Registry for Browser Helper Objects
        public static void SearchRegBHOs(string[] entries, ref ObservableCollection<BHO> BHORegs)
        {
            foreach (string entry in entries)
            {
                string childPath = entry.Substring(5);
                RegistryKey subKey = Registry.LocalMachine.OpenSubKey(childPath, false);
                
                if (subKey != null)
                {
                    foreach (string subSubKeyName in subKey.GetSubKeyNames())
                    {
                        RegistryKey entryKey = Registry.LocalMachine.OpenSubKey(
                            CLSIDEntry + subSubKeyName, false);
                        RegistryKey imagePathKey = Registry.LocalMachine.OpenSubKey(
                            CLSIDEntry + subSubKeyName + @"\InprocServer32", false);
                        if (imagePathKey != null)
                        {
                            string imagePath = ((string)imagePathKey.GetValue("")).ToLower();

                            var bho = new BHO
                            {
                                Path = "  " + entry,
                                Entry = (string)entryKey.GetValue(""),
                                Description = Tool.GetDescription(imagePath),
                                Publisher = Tool.GetPublisher(imagePath),
                                ImagePath = imagePath,
                                TimeStamp = Tool.GetTimeStamp(imagePath)
                            };

                            BHORegs.Add(bho);
                        }
                    }
                }
            }
        }

        public string Path { get; set; }
    }
}