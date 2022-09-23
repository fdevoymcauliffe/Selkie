using SELKIE.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SELKIE.Models
{
    public class TotalRepairs
    {
        static List<RepairDetails> allreps = new List<RepairDetails>();

        public static List<RepairDetails> GetAllRepairs()
        {
            return allreps;
        }

        public static void Add(RepairDetails value)
        {
            allreps.Add(value);
        }

        public static void Update(int inx, RepairDetails value)
        {
            allreps[inx] = value;
        }
        public static RepairDetails GetObj(int inx)
        {
            if (allreps.Count > 0)
                return allreps[inx];
            else
                return null;
        }
        public static bool Delete(string repairName)
        {
            try
            {
                var itemtoRemove = GetObjByName(repairName);
                _ = allreps.Remove(itemtoRemove);
                return true;
            }
            catch (Exception)
            {
                //log 
            }
            return false;

        }
        public static RepairDetails GetObjByName(string name)
        {
            return allreps.Where(x => x.RepairName == name).FirstOrDefault();
        }

        internal static void Init()
        {
            allreps = new List<RepairDetails>();
        }
    }

    public class RepairDetails
    {
        public string RepairName { get; set; }
        public string RepairDesc { get; set; }
        public int NoOfTechs { get; set; }
        public string Vesselrequired { get; set; }
        public string Base { get; set; }
        public OperationalLocation Operationlocation { get; set; }
        public double OperationdurationOffshore { get; set; }
        public double OperatondurationOnshore { get; set; }
        public double Waveheightlimit { get; set; }
        public double Waveperiodlimit { get; set; }
        public double Windspeedlimit { get; set; }
        public double Currentvelocitylimit { get; set; }
        public double Powerloss { get; set; }
        public double Sparepart { get; set; }
    }
}
