using System;
using System.Collections.Generic;

namespace SELKIE.SimModels
{
    public class SimVessel
    {
        public int Id { get; set; }
        public string VesselType { get; set; }
        public bool Rented { get; set; }
        public DateTime? OperationFrom { get; set; }
        public DateTime? OperationTo { get; set; }
        public bool Available { get; set; }
        public bool HireAsReq { get; set; }
        public double LeadHrs { get; set; }
        public bool HireTriggered { get; set; }
        public int SimIterationId { get; set; }
        public DateTime? OnTaskFrom { get; set; }
        public DateTime? OnTaskT0 { get; set; }
        public int CarryingTechs { get; set; }
        public string Base { get; set; }
        public List<HiredAsReqByYear> HiredAsReqByYears { get; set; }
    }

    public class InstallVessel
    {
        public int Id { get; set; }
        public string VesselType { get; set; }
        public bool Rented { get; set; }
        public DateTime? OperationFrom { get; set; }
        public DateTime? OperationTo { get; set; }
        public bool Available { get; set; }
        public bool HireAsReq { get; set; }
        public double LeadHrs { get; set; }
        public bool HireTriggered { get; set; }
        public DateTime? OnTaskFrom { get; set; }
        public DateTime? OnTaskT0 { get; set; }
        public int CarryingTechs { get; set; }
        public string Base { get; set; }
        public double TFuelCost { get; set; }
        public double RentedDaysForHireAsReq { get; set; }
    }

}
