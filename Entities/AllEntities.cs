using SELKIE.Enums;
using System;

namespace SELKIE.Entities
{
    class AllEntities
    {
    }


    public class ValidationMessage
    {
        public bool Result { get; set; }
        public string Message { get; set; }
        public bool FileExists { get; set; } = true;
    }


    public class TTFEntity
    {
        public DateTime TTFDate { get; set; }
        public double TTFHours { get; set; }
    }

    public class CompInstallJobFeedback
    {
        public Install_Status Status { get; set; }
        public int VesselUsed { get; set; }
    }

    public static class ProjectSettings
    {
        public static bool ProjectLock { get; set; }
    }

    public class FromTo
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }
    }

}
