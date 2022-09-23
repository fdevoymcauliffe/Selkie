using SELKIE.SimModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SELKIE.SimModelList
{
    public class SimVesselsList
    {
        static List<SimVessel> all = new List<SimVessel>();

        public static List<SimVessel> GetAll()
        {
            return all;
        }
        public static void Add(SimVessel value)
        {
            all.Add(value);
        }
        public static void Update(int inx, SimVessel value)
        {
            int _index = all.FindIndex(i => i.Id == inx);
            if (_index > -1)
                all[_index] = value;
        }
        public static void Clear()
        {
            all = new List<SimVessel>();
        }
        public static bool CheckIfAnyVesselAvailable(DateTime projectCurrentDate, int simstageId, string vesselReq)
        {
            DateTime modCurrentDate = new DateTime(2000, projectCurrentDate.Month, projectCurrentDate.Day, projectCurrentDate.Hour, projectCurrentDate.Minute, projectCurrentDate.Second);
            //check if any vessel available and purchased
            if (all.Where(x => x.Available == true && x.Rented == false && x.VesselType == vesselReq).Count() > 0)
                return true;
            if (all.Where(x => x.Available == true && x.Rented == true && x.HireAsReq == false && x.OperationFrom <= modCurrentDate && x.OperationTo >= modCurrentDate && x.VesselType == vesselReq).Count() > 0)
                return true;
            if (all.Where(x => x.Available == true && x.Rented == true && x.HireAsReq == true && x.OnTaskFrom == null && x.OnTaskT0 == null && x.VesselType == vesselReq).Count() > 0)
                return true;
            return false;
        }
        internal static void TriggerLeadTimeForVessel(DateTime shiftStart, string vesselReq)
        {
            var firstordefaultV = all.Where(x => x.Available == false && x.Rented == true && x.HireAsReq == true && x.OnTaskFrom == null && x.OnTaskT0 == null && x.VesselType == vesselReq && x.HireTriggered == false).FirstOrDefault();
            if (firstordefaultV != null)
            {
                //the following will allow only one type vessel gets hired at one time only
                //var alreadyHired = all.Where(x => x.Available == false && x.Rented == true && x.HireAsReq == true && x.OnTaskFrom != null && x.OnTaskT0 != null && x.VesselType == vesselReq && x.HireTriggered == true).FirstOrDefault();
                //if (alreadyHired != null)
                //    return;

                firstordefaultV.HireTriggered = true;
                firstordefaultV.OnTaskFrom = shiftStart;
                firstordefaultV.OnTaskT0 = shiftStart.AddHours(firstordefaultV.LeadHrs);
                firstordefaultV.Base = "";
                firstordefaultV.CarryingTechs = 0;
                Update(firstordefaultV.Id, firstordefaultV);
            }
        }

    }
}
