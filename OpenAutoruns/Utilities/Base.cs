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
    /// Autorun Record Base Class
    /// </summary>
    internal class Base
    {
        public string Entry { get; set; }
        public string Description { get; set; }
        public string Publisher { get; set; }
        public string ImagePath { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}