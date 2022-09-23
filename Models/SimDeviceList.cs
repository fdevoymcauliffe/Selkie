using SELKIE.SimModels;
using System.Collections.Generic;

namespace SELKIE.SimModelList
{
    public class SimDeviceList
    {
        public static List<SimDevice> all = new List<SimDevice>();

        public static List<SimDevice> GetAll()
        {
            return all;
        }
        public static void Add(SimDevice value)
        {
            all.Add(value);
        }
        public static void Update(int id, SimDevice value)
        {
            int _index = all.FindIndex(i => i.Id == id);
            if (_index > -1)
                all[_index] = value;
        }
        public static void Clear()
        {
            all = new List<SimDevice>();
        }
    }
}
