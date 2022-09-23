using System;

namespace SELKIE.SimModels
{
    class SimReportModels
    {
    }

    public class SimTAnalysis
    {
        public int SimTurbineId { get; set; }
        public int SimstageId { get; set; }
        public int SimyearlyId { get; set; }
        public double Losthours { get; set; }
        public double EnergyLoss { get; set; }
    }

    public class InstallDeviceAnalysis
    {
        public int Iteration { get; set; }
        public int DeviceID { get; set; }
        public string DeviceType { get; set; }
        public DateTime? Start { get; set; }
        public DateTime? Complete { get; set; }
        public int Jobs { get; set; }
        public string Status { get; set; }
        public double InstallTime { get; set; }
        public string Notes { get; set; }
    }

    public class SimVesselUsageData
    {
        public int SimstageId { get; set; }
        public int SimyearlyId { get; set; }
        public int VesselId { get; set; }
        public double OperationalTime { get; set; }
        public double StandbyTime { get; set; }
        public string Vtype { get; set; }
        public double Fuelcost { get; set; }
    }
}
