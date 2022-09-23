using SELKIE.SimModels;
using System.Collections.Generic;

namespace SELKIE.InstallResultList
{
    public class InstallYearlyResultList
    {
        public static List<Installyearly> all = new List<Installyearly>();

        public static List<Installyearly> GetAll()
        {
            return all;
        }

        public static void Add(Installyearly value)
        {
            all.Add(value);
        }
        public static void Clear()
        {
            all = new List<Installyearly>();
        }
    }

}
