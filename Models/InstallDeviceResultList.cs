using SELKIE.SimModels;
using System.Collections.Generic;

namespace SELKIE.InstallResultList
{
    public class InstallDeviceResultList
    {
        public static List<InstallDeviceAnalysis> all = new List<InstallDeviceAnalysis>();

        public static List<InstallDeviceAnalysis> GetAll()
        {
            return all;
        }

        public static void Add(InstallDeviceAnalysis value)
        {
            all.Add(value);
        }
        public static void Clear()
        {
            all = new List<InstallDeviceAnalysis>();
        }
    }
}
