using SELKIE.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SELKIE.Models
{
    public class TotalPriventives
    {
        static List<PriventiveDetails> priventives = new List<PriventiveDetails>();

        public static List<PriventiveDetails> GetAllPriventives()
        {
            return priventives;
        }

        public static void Add(PriventiveDetails _value)
        {
            priventives.Add(_value);
        }

        public static void Update(int inx, PriventiveDetails _value)
        {
            priventives[inx] = _value;
        }
        public static PriventiveDetails GetObj(int inx)
        {
            return priventives[inx];
        }
        public static bool Delete(string pmName)
        {
            try
            {
                var itemtoRemove = GetObjByName(pmName);
                _ = priventives.Remove(itemtoRemove);
                return true;
            }
            catch (Exception)
            {
                //log 
            }
            return false;

        }
        public static PriventiveDetails GetObjByName(string name)
        {
            return priventives.Where(x => x.PMCategory == name).FirstOrDefault();
        }

        internal static void Init()
        {
            priventives = new List<PriventiveDetails>();
        }
    }
    public class PriventiveDetails
    {
        public string PMCategory { get; set; }
        public string Taskdescription { get; set; }
        public int NoOftechsReq { get; set; }
        public string VesselReq { get; set; }
        public string Base { get; set; }
        public double Frequency { get; set; }
        public OperationalLocation OperationLOC { get; set; } = OperationalLocation.Offshore;
        public double OprDurationOffs { get; set; }
        public double OprDurationOns { get; set; }
        public double Waveheightlimit { get; set; }
        public double Waveperiodlimit { get; set; }
        public double Windspeedlimit { get; set; }
        public double CurrentVelocityLimit { get; set; }
        public double Powerloss { get; set; }
        public double Sparepart { get; set; }
    }
}
