namespace Schneider
{
    using System;
    using System.Collections.Generic;
    /// <summary>
    /// Class created by entity framework 
    /// </summary>
    public partial class Gateway
    {
        public int Id { get; set; }
        public int SerialNumber { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public string Ip { get; set; }
        public Nullable<int> PortNumber { get; set; }
    }
}
