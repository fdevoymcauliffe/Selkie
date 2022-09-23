using System.ComponentModel;

namespace SELKIE.Enums
{
    class AllEnums
    {
    }

    public enum TaskType
    {
        Corrective = 1,
        Condition = 2,
        Preventative = 3,
        Install = 4
    }

    public enum FailedReason
    {
        WW = 1,
        Techs = 2,
        VesselAvail = 3,
        MaxWorkingHours = 4,
        VesselOrTechs = 5,
        VesselInUse = 6,
        OutOfRentalPeriod = 7
    }

    public enum Task_Status
    {
        ToCompFullRepair = 0,
        NotStarted = 1,
        Completed = 2,
    }

    public enum Install_Status
    {
        NotStarted = 0,
        ToCompFullRepair = 1,
        Completed = 2,
        Failed = 3,
        Working = 4
    }

    public enum Task_Stage
    {
        OffshoreOnly = 0,
        OnshoreStage1 = 1,
        OnshoreStage2 = 2,
        OnshoreStage3 = 3
    }

    public enum PriorityLevels
    {
        Level1 = 1,
        Level2 = 2,
        Level3 = 3,
        Level4 = 4
    }

    public enum OperationalLocation
    {
        [Description("Onshore")]
        Onshore = 1,
        [Description("Offshore")]
        Offshore = 2
    }

    public enum InstallType
    {
        [Description("Device")]
        Device = 1,
        [Description("SubStructure")]
        SubStructure = 2
    }
}
