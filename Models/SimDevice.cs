using System;

namespace SELKIE.SimModels
{
    public class SimDevice
    {
        public int Id { get; set; }
        public int SimStageId { get; set; }
        public DateTime? OnTaskFrom { get; set; }
        public DateTime? OnTaskT0 { get; set; }
    }

    public class InstallDevice
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }


}
