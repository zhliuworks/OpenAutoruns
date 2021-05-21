using System;

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