using SELKIE.Models;
using System.Collections.Generic;

namespace SELKIE.SimModelList
{
    public class AllShifts
    {
        public static List<Shifts> all = new List<Shifts>();
        public static void Add(Shifts value)
        {
            all.Add(value);
        }
        public static void Clear()
        {
            all = new List<Shifts>();
        }
    }
}
