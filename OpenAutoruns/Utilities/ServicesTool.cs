using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using Microsoft.Win32;

namespace OpenAutoruns.Utilities
{
    /// <summary>
    /// Autorun Record Tool for Services/Drivers Information Extraction
    /// </summary>
    internal class ServicesTool
    {
        public static string FilterImagePath(RegistryKey subSubKey, string imagePath, bool isDriver)
        {
            // unify the path to lowercase
            imagePath = imagePath.ToLower();

            /* for Drivers */
            if (isDriver)
            {
                // begin with `\??\`
                if (imagePath.StartsWith(@"\??\"))
                {
                    imagePath = imagePath.Substring(4);
                }

                // begin with `\systemroot\`
                if (imagePath.StartsWith(@"\systemroot\"))
                {
                    imagePath = imagePath.Substring(12);
                }

                // begin with `system32` or `syswow64`
                if (imagePath.StartsWith("system32") || imagePath.StartsWith("syswow64"))
                {
                    imagePath = @"c:\windows\" + imagePath;
                }
            }

            /* for Services */
            else
            {
                // remove double quotes
                if (imagePath.StartsWith('\"'))
                {
                    imagePath = imagePath.Substring(1, imagePath.IndexOf('\"', 1) - 1);
                }

                // get the DLL path hosted inside a `svchost.exe` process
                if (imagePath.Contains("svchost.exe"))
                {
                    if (subSubKey.GetSubKeyNames().Contains("Parameters"))
                    {
                        try
                        {
                            RegistryKey subSubSubKey = subSubKey.OpenSubKey("Parameters");
                            imagePath = (string)subSubSubKey.GetValue("ServiceDll");
                            imagePath = imagePath.ToLower();
                        }
                        catch (SecurityException)
                        {
                            imagePath = "<< WARNING: Fail to access requested registry, please run as an administrator. >>";
                        }
                    }
                }
            }

            return imagePath;
        }
        public static string GetAllDescription(RegistryKey key, string imagePath)
        {

            string description = (string)key.GetValue("Description");
            string displayName = (string)key.GetValue("DisplayName");

            // access localized string using registry string redirection
            if (description != null && description.StartsWith('@'))
            {
                description = RegistryStringRedirection.RetrieveMUIString(key, "Description");
            }
            if (displayName != null && displayName.StartsWith('@'))
            {
                displayName = RegistryStringRedirection.RetrieveMUIString(key, "DisplayName");
            }

            // combine description and display name from registry, and file description to an overall description
            string AllDescription = "";

            if (displayName != null)
            {
                AllDescription += displayName + ": ";
            }
            if (description != null)
            {
                AllDescription += description + "/ ";
            }
            AllDescription += Tool.GetDescription(imagePath);

            return AllDescription;
        }

        public static string GetStart(RegistryKey key)
        {
            switch ((int)key.GetValue("Start"))
            {
                case 0:
                    return "0 (SERVICE_BOOT_START)";
                case 1:
                    return "1 (SERVICE_SYSTEM_START)";
                case 2:
                    return "2 (SERVICE_AUTO_START)";
                case 3:
                    return "3 (SERVICE_DEMAND_START)";
                case 4:
                    return "4 (SERVICE_DISABLED)";
                default:
                    return ((int)key.GetValue("Start")).ToString();
            }
        }

        public static string GetType(RegistryKey key)
        {
            switch ((int)key.GetValue("Type"))
            {
                // Drivers
                case 1:
                    return "1   (SERVICE_KERNEL_DRIVER)";
                case 2:
                    return "2   (SERVICE_FILE_SYSTEM_DRIVER)";
                case 4:
                    return "4   (SERVICE_ADAPTER)";
                case 8:
                    return "8   (SERVICE_RECOGNIZER_DRIVER)";

                // Services
                case 16:
                    return "16  (SERVICE_WIN32_OWN_PROCESS)";
                case 32:
                    return "32  (SERVICE_WIN32_SHARE_PROCESS)";
                case 256:
                    return "256 (SERVICE_INTERACTIVE_PROCESS)";

                default:
                    return ((int)key.GetValue("Type")).ToString();
            }
        }
    }


    /// <summary>
    /// Registry String Redirection
    /// *Declaration: the implementation of this class refers to Stackoverflow:
    /// https://stackoverflow.com/questions/22273956/how-to-get-redirected-string-from-the-registry
    /// *Note: this class only works for Windows Vista and higher.
    /// </summary>
    public static class RegistryStringRedirection
    {
        // Snippet of ErrorCode.
        enum ErrorCode
        {
            Success = 0x0000,
            MoreData = 0x00EA
        }

        [DllImport("advapi32.dll", CharSet = CharSet.Unicode)]
        private extern static int RegLoadMUIString(
            IntPtr registryKeyHandle, string value,
            StringBuilder outputBuffer, int outputBufferSize, out int requiredSize,
            RegistryLoadMUIStringOptions options, string path);

        /// <summary>
        ///   Determines the behavior of <see cref="RegLoadMUIString" />.
        /// </summary>
        [Flags]
        internal enum RegistryLoadMUIStringOptions : uint
        {
            /// <summary>
            ///   The string is truncated to fit the available size of the output buffer.
            ///   If this flag is specified, copiedDataSize must be NULL.
            /// </summary>
            None = 0,
            Truncate = 1
        }

        /// <summary>
        ///   Retrieves the multilingual string associated with the specified name.
        ///   Returns null if the name/value pair does not exist in the registry.
        ///   The key must have been opened using 
        /// </summary>
        /// <param name = "key">The registry key to load the string from.</param>
        /// <param name = "name">The name of the string to load.</param>
        /// <returns>
        /// The language-specific string, or null if the name/value pair does not exist in the registry.
        /// </returns>
        public static string RetrieveMUIString(this RegistryKey key, string name)
        {
            const int initialBufferSize = 1024;
            var output = new StringBuilder(initialBufferSize);
            int requiredSize;
            IntPtr keyHandle = key.Handle.DangerousGetHandle();
            ErrorCode result = (ErrorCode)RegLoadMUIString(
                keyHandle, name, output, output.Capacity,
                out requiredSize, RegistryLoadMUIStringOptions.None, null);

            if (result == ErrorCode.MoreData)
            {
                output.EnsureCapacity(requiredSize);
                result = (ErrorCode)RegLoadMUIString(
                    keyHandle, name, output, output.Capacity,
                    out requiredSize, RegistryLoadMUIStringOptions.None, null);
            }

            return result == ErrorCode.Success ? output.ToString() : null;
        }
    }
}