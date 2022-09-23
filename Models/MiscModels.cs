using SELKIE.Enums;
using System;

namespace SELKIE.SimModels
{
    public class MiscModels
    {
    }

    public class VesselInfo
    {
        public string Name { get; set; }
        public double WaveHeightLimit { get; set; }
        public string NightWork { get; set; }
        public bool Purchased { get; set; }
        public DateTime RentalStart { get; set; }
        public DateTime RentalEnd { get; set; }
        public double Speed { get; set; }
        public double FuelCost { get; set; }
        public double FuelUsage { get; set; }
        public bool Proceed { get; set; }
        public FailedReason FailedReason { get; set; }
        public string Comments { get; set; }
        public int SimVesselsId { get; set; }
    }

    public class WWShift
    {
        public bool Result { get; set; }
        public SafeHours FullRepairHours { get; set; }
        public SafeHours MinRepairHours { get; set; }
    }

    public class SafeHours
    {
        public bool CanTravel { get; set; }
        public DateTime Safestart { get; set; }
        public double Safehours { get; set; }
    }
}
