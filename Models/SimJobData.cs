using System;

namespace SELKIE.SimModels
{
    public class SimJobData
    {
        public DateTime? JourneyStart { get; set; }
        public double DistanceTravelled { get; set; }
        public DateTime? JourneyEnd { get; set; }
        public bool MinRepair { get; set; }
        public DateTime? RepairStarted { get; set; }
        public double IdleHoursInShift { get; set; }
        public double HoursAvailInShift { get; set; }
        public double TotalTimeToTravel { get; set; }
        public double MaxWorkHours { get; set; }
        public double WorkedHours { get; set; }
        public DateTime? RepairEnded { get; set; }
        public double DelayedHours { get; set; }
        public DateTime? IncidentTime { get; set; }
        public string Id { get; set; }
        public string SimIncidentsId { get; set; }
        public bool FullRepair { get; set; }
        public bool Completed { get; set; }
        public int SimVesselsId { get; set; }
        public double OperationalTime { get; set; }
        public double StandbyTime { get; set; }
    }

    public class InstallJobData
    {
        public string Id { get; set; }
        public int InstallId { get; set; }
        public DateTime? ShiftStart { get; set; }
        public DateTime? JourneyStart { get; set; }
        public double StandbyTime { get; set; }
        public DateTime? JourneyEnd { get; set; }
        public bool Completed { get; set; }
        public int NoOfAttempts { get; set; }
        public double TotalTimeToTravel { get; set; }
        public double MaxWorkHours { get; set; }
        public double HoursAvailInShift { get; set; }
        public double IdleHoursInShift { get; set; }
        public double WorkedHours { get; set; }
        public DateTime? RepairStarted { get; set; }
        public DateTime? RepairEnded { get; set; }
        public double OperationalTime { get; set; }
        public bool RotationShift { get; set; }
        public bool SingleShift { get; set; }
    }
}
