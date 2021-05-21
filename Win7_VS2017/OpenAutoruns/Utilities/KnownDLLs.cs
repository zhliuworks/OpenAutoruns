using System;
using System.Collections.ObjectModel;
using Microsoft.Win32;

namespace OpenAutoruns.Utilities
{
    /// <summary>
    /// Autorun based on Known DLL
    /// </summary>
    class KnownDLL : Base
    {
        // Registry Entry Related to Known DLLs
        public static readonly string KnownDLLRegEntry = @"HKLM\System\CurrentControlSet\Control\Session Manager\KnownDlls";

        // Search Registry for Known DLLs 
        public static void SearchKnownDLLs(string entry, ref ObservableCollection<KnownDLL> knownDLLRegs)
        {
            entry = entry.Substring(5);
            RegistryKey subKey = Registry.LocalMachine.OpenSubKey(entry, false);
            if (subKey != null)
            {
                foreach (string valueName in subKey.GetValueNames())
                {
                    string baseImagePath = Tool.GetImagePath(valueName, subKey);
                    try
                    {
                        string sys32ImagePath = Environment.ExpandEnvironmentVariables(@"%SystemRoot%\system32\").ToLower() + baseImagePath;

                        var sys32DLL = new KnownDLL
                        {
                            Entry = baseImagePath,
                            Description = Tool.GetDescription(sys32ImagePath),
                            Publisher = Tool.GetPublisher(sys32ImagePath),
                            ImagePath = sys32ImagePath,
                            TimeStamp = Tool.GetTimeStamp(sys32ImagePath)
                        };

                        knownDLLRegs.Add(sys32DLL);
                    }
                    catch { }

                    try
                    {
                        string sys64ImagePath = Environment.ExpandEnvironmentVariables(@"%SystemRoot%\syswow64\").ToLower() + baseImagePath;

                        var sys64DLL = new KnownDLL
                        {
                            Entry = baseImagePath,
                            Description = Tool.GetDescription(sys64ImagePath),
                            Publisher = Tool.GetPublisher(sys64ImagePath),
                            ImagePath = sys64ImagePath,
                            TimeStamp = Tool.GetTimeStamp(sys64ImagePath)
                        };

                        knownDLLRegs.Add(sys64DLL);
                    }
                    catch { }
                }
            }
        }
    }
}
