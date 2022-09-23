using System.Collections.Generic;
using System.Linq;

namespace SELKIE.InstallModelList
{
    public class InstallBaseList
    {
        static List<InstallBasesDetails> all = new List<InstallBasesDetails>();



        public static void Add(InstallBasesDetails value)
        {
            all.Add(value);
        }

        public static void Update(string name, InstallBasesDetails value)
        {
            int _index = all.FindIndex(i => i.Basename == name);
            if (_index > -1)
            {
                if (value.NoOfTechs > value.OriginalNoOfTechs)
                    value.NoOfTechs = value.OriginalNoOfTechs;
                all[_index] = value;
            }

        }

        public static InstallBasesDetails GetObjByName(string name)
        {
            return all.Where(x => x.Basename == name).FirstOrDefault();
        }

        public static double GetBaseAnnualCost(List<string> _basesUsedInOperationYearly)
        {
            return all.Where(x => _basesUsedInOperationYearly.Contains(x.Basename)).Sum(x => x.Annualcost);
        }



        public static void Clear()
        {
            all = new List<InstallBasesDetails>();
        }
    }
    public class InstallBasesDetails
    {
        public string Basename { get; set; }
        public double Annualcost { get; set; }
        public double Distancetofarm { get; set; }
        public int NoOfTechs { get; set; }
        public int OriginalNoOfTechs { get; set; }
        public double AnnualsalperTech { get; set; }
    }
}
