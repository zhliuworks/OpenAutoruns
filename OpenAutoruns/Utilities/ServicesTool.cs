using System;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Win32;

namespace OpenAutoruns.Utilities
{
    /// <summary>
    /// Autorun Record Tool for Services/Drivers Information Extraction
    /// </summary>
    internal class ServicesTool
    {
        public static string FilterImagePath(string imagePath)
        {
            // unify the path to lowercase
            imagePath = imagePath.ToLower();

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
                    return "SERVICE_BOOT_START (0)";
                case 1:
                    return "SERVICE_SYSTEM_START (1)";
                case 2:
                    return "SERVICE_AUTO_START (2)";
                case 3:
                    return "SERVICE_DEMAND_START (3)";
                case 4:
                    return "SERVICE_DISABLED (4)";
                default:
                    return "";
            }
        }

        public static string GetType(RegistryKey key)
        {
            switch ((int)key.GetValue("Type"))
            {
                // Drivers
                case 1:
                    return "SERVICE_KERNEL_DRIVER (1)";
                case 2:
                    return "SERVICE_FILE_SYSTEM_DRIVER (2)";
                case 4:
                    return "SERVICE_ADAPTER (4)";
                case 8:
                    return "SERVICE_RECOGNIZER_DRIVER (8)";

                // Services
                case 16:
                    return "SERVICE_WIN32_OWN_PROCESS (16)";
                case 32:
                    return "SERVICE_WIN32_SHARE_PROCESS (32)";
                case 256:
                    return "SERVICE_INTERACTIVE_PROCESS (256)";

                default:
                    return "";
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