using System.Collections.Generic;
using System.Linq;

namespace SELKIE.SimModelList
{
    public class SimBaseList
    {
        static List<SimBasesDetails> all = new List<SimBasesDetails>();

        public static void Add(SimBasesDetails value)
        {
            all.Add(value);
        }
        public static void Update(string name, SimBasesDetails value)
        {
            int _index = all.FindIndex(i => i.Basename == name);
            if (_index > -1)
            {
                if (value.NoOfTechs > value.OriginalNoOfTechs)
                    value.NoOfTechs = value.OriginalNoOfTechs;
                if (value.NoOfTechs < 0)
                    value.NoOfTechs = 0;
                all[_index] = value;
            }

        }
        public static SimBasesDetails GetObjByName(string name)
        {
            return all.Where(x => x.Basename == name).FirstOrDefault();
        }
        public static void Clear()
        {
            all = new List<SimBasesDetails>();
        }
    }

    public class SimBasesDetails
    {
        public string Basename { get; set; }
        public double Annualcost { get; set; }
        public double Distancetofarm { get; set; }
        public int NoOfTechs { get; set; }
        public int OriginalNoOfTechs { get; set; }
        public double AnnualsalperTech { get; set; }
    }
}
