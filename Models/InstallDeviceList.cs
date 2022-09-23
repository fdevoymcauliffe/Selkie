using SELKIE.SimModels;
using System.Collections.Generic;

namespace SELKIE.InstallModelList
{
    public class InstallDeviceList
    {
        public static List<InstallDevice> all = new List<InstallDevice>();

        public static List<InstallDevice> GetAll()
        {
            return all;
        }

        public static void Add(InstallDevice value)
        {
            all.Add(value);
        }


        public static void Clear()
        {
            all = new List<InstallDevice>();
        }
    }
}
