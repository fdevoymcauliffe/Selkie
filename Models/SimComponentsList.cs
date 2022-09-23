using SELKIE.SimModels;
using System.Collections.Generic;

namespace SELKIE.SimModelList
{
    public class SimComponentsList
    {
        public static List<SimComponent> all = new List<SimComponent>();
        public static List<SimComponent> GetAll()
        {
            return all;
        }
        public static void Add(SimComponent value)
        {
            value.ComponentId = all.Count + 1;
            all.Add(value);
        }
        public static void Clear()
        {
            all = new List<SimComponent>();
        }
    }
}
