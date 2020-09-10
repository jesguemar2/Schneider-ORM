namespace Schneider
{
    using System;
    using System.Collections.Generic;
    /// <summary>
    /// Class created by Entity Framework
    /// </summary>
    public partial class WaterMeter
    {
        public int Id { get; set; }
        public int SerialNumber { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
    }
}
