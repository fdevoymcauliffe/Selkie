using SELKIE.Enums;
using SELKIE.Models;
using System;

namespace SELKIE.SimModels
{
    public class SimIncidents
    {
        public string Id { get; set; }
        public string Repair { get; set; }
        public TaskType Type { get; set; }
        public int SimComponentId { get; set; }
        public int SimTurbinesId { get; set; }
        public string Component { get; set; }
        public Task_Status Status { get; set; }
        public Task_Stage Stage { get; set; }
        public DateTime? IncidentTime { get; set; }
        public DateTime? IncidentFixed { get; set; }
        public DateTime? FinalFixed { get; set; } = DateTime.Now.AddYears(100);
        public PriorityLevels Priority { get; set; }
        public int SimStageId { get; set; }
        public bool ReportCompleted { get; set; }
        public string RootId { get; set; }
        public double WorkedHours { get; set; }
    }

    public class InstallJobs
    {
        public int Id { get; set; }
        public string Job { get; set; }
        public TaskType Type { get; set; }
        public int InstallDeviceId { get; set; }
        public Install_Status Status { get; set; }
        public DateTime? CompletedDate { get; set; }
        public DateTime? IncidentTime { get; set; }
        public InstallationDetails InstallData { get; set; }

    }
}
