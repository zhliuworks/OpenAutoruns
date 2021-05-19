using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Microsoft.Win32;


namespace OpenAutoruns.Utilities
{
    /// <summary>
    /// Autorun Record Tool Class
    /// </summary>
    internal class Tool
    {
        public static string GetDescription(string imagePath)
        {
            FileVersionInfo fileVersionInfo = FileVersionInfo.GetVersionInfo(imagePath);
            return fileVersionInfo.FileDescription;
        }

        public static string GetPublisher(string imagePath)
        {
            string publisher;
            try
            {
                var cert = X509Certificate.CreateFromSignedFile(imagePath); // maybe raise exception
                var cert2 = new X509Certificate2(cert);

                // get the subject and issuer names from a x509 certificate
                publisher = cert2.GetNameInfo(X509NameType.SimpleName, false);
            }
            catch
            {
                // cannot create x509 certificate from file
                publisher = "";
            }
            return "(Verified) " + publisher;
        }

        public static string GetImagePath(string file, RegistryKey key)
        {
            string imagePath = (string)key.GetValue(file);

            // unify the path to lowercase
            imagePath = imagePath.ToLower();

            // ignore parameters and double quotes to get the actual file path
            if (imagePath.Contains('\"'))
            {
                imagePath = imagePath.Substring(1, imagePath.IndexOf('\"', 1) - 1);
            }

            return imagePath;
        }

        public static DateTime GetTimeStamp(string imagePath)
        {
            FileInfo fileInfo = new FileInfo(imagePath);

            // timestamp is considered to be the last modified time
            return fileInfo.LastWriteTime;
        }
    }

}