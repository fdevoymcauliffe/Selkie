using System;
using System.Collections.Generic;
using System.Linq;

namespace SELKIE.Models
{
    public static class TotalVessels
    {
        static List<VesselDetails> vessels = new List<VesselDetails>();

        public static List<VesselDetails> GetVessels()
        {
            return vessels;
        }

        public static void AddVessel(VesselDetails _vessel)
        {
            vessels.Add(_vessel);
        }

        public static void UpdateVessel(int index, VesselDetails _vessel)
        {
            vessels[index] = _vessel;
        }
        public static VesselDetails GetObj(int inx)
        {
            return vessels[inx];
        }
        public static bool Delete(string vslName)
        {
            try
            {
                var itemtoRemove = GetObjByName(vslName);
                _ = vessels.Remove(itemtoRemove);
                return true;
            }
            catch (Exception)
            {
                //log 
            }
            return false;

        }

        public static VesselDetails GetObjByName(string name)
        {
            return vessels.Where(x => x.VesselClassif == name).FirstOrDefault();
        }

        internal static void Init()
        {
            vessels = new List<VesselDetails>();
        }
    }
    public class VesselDetails
    {
        public string VesselClassif { get; set; }
        public int Number { get; set; }
        public int TechsCapacity { get; set; }
        public string NightWork { get; set; }
        public bool Purchased { get; set; }
        public double AnnualrunningCost { get; set; }
        public int RentalStartDay { get; set; }
        public int RentalEndDay { get; set; }
        public int RentalStartMonth { get; set; }
        public int RentalEndMonth { get; set; }
        public double DailyRentalCost { get; set; }
        public double MobilizationCost { get; set; }
        public double FuelConsumption { get; set; }
        public double FuelCost { get; set; }
        public double Speed { get; set; }
        public string Hireasrequired { get; set; }
        public double VesselLeadtime { get; set; }
    }
}
