using System;
using System.Collections.Generic;
using System.Linq;

namespace SELKIE.Models
{
    public static class TotalBases
    {
        static List<BasesDetails> bases = new List<BasesDetails>();

        public static List<BasesDetails> GetBases()
        {
            return bases;
        }

        public static void InitList()
        {
            bases = new List<BasesDetails>();
        }

        public static void AddBase(BasesDetails value)
        {
            bases.Add(value);
        }

        public static void UpdateBase(int index, BasesDetails value)
        {
            bases[index] = value;
        }
        public static BasesDetails GetObj(int inx)
        {

            return bases[inx];
        }
        public static bool Delete(string baseName)
        {
            try
            {
                var itemtoRemove = GetObjByName(baseName);
                _ = bases.Remove(itemtoRemove);
                return true;
            }
            catch (Exception)
            {
                //log 
            }
            return false;

        }

        public static BasesDetails GetObjByName(string name)
        {
            return bases.Where(x => x.Basename == name).FirstOrDefault();
        }

    }
    public class BasesDetails
    {
        public string Basename { get; set; }
        public double Annualcost { get; set; } = 0;
        public double Distancetofarm { get; set; }
        public int NoOfTechs { get; set; }
        public double AnnualsalperTech { get; set; }
    }
}
