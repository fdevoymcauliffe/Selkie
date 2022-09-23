using SELKIE.SimModels;
using System.Collections.Generic;
using System.Linq;

namespace SELKIE.SimResults
{
    public class SimIterationList
    {
        public static List<SimIterationResult> SimIterationResults = new List<SimIterationResult>();
        public static List<SimYearlyIterationResult> SimYearlyIterationResults = new List<SimYearlyIterationResult>();

        #region SimIterationResults
        public static int GetIterationResultCount()
        {
            return SimIterationResults.Count;
        }

        public static List<SimIterationResult> GetAllSimIterationResults()
        {
            return SimIterationResults.ToList();
        }

        public static bool AddSimIterationResult(SimIterationResult value)
        {
            SimIterationResults.Add(value);
            return true;
        }

        public static bool UpdateSimIterationResult(SimIterationResult value, int Id)
        {
            var _inx = GetAllSimIterationResults().FindIndex(x => x.Id == Id);
            if (_inx > -1)
            {
                SimIterationResults[_inx] = value;
                return true;
            }
            return false;
        }

        public static SimIterationResult GetIterationById(int _id)
        {
            if (_id > 0)
                return SimIterationResults.Find(x => x.Id == _id);
            else
                return new SimIterationResult();
        }

        public static void ClearSimIterationResults()
        {
            SimIterationResults = new List<SimIterationResult>();
            SimYearlyIterationResults = new List<SimYearlyIterationResult>();
        }
        #endregion

        #region IterationYearlyResults
        public static List<SimYearlyIterationResult> GetYearlyIterationResult()
        {
            return SimYearlyIterationResults.ToList();
        }

        public static void AddYearlySimIterationResult(SimYearlyIterationResult value)
        {
            SimYearlyIterationResults.Add(value);
        }

        public static SimYearlyIterationResult GetYearlySimIterationResultByYear(int _year, int iterationId)
        {
            if (_year > 0 && iterationId > 0)
                return SimYearlyIterationResults.Find(x => x.Year == _year && x.Id == iterationId);
            else
                return new SimYearlyIterationResult();
        }

        public static List<SimYearlyIterationResult> GetAllYearlySimIterationResultsByIterationId(int id)
        {
            if (id > 0)
                return SimYearlyIterationResults.Where(c => c.Id == id).ToList();
            else
                return new List<SimYearlyIterationResult>();
        }


        #endregion
    }

    public class SimIterationResult
    {
        public int Id { get; set; }
        public double TBA { get; set; }
        public double EBA { get; set; }
        public double E_real { get; set; }
        public double E_theoretical { get; set; }
        public double E_downtime { get; set; }
        public double T_lifetime { get; set; }
        public double T_downtime { get; set; }
        public double Income { get; set; }
        public double Total_omcost { get; set; }
        public double V_fuelcost { get; set; }
        public double V_fixedcost { get; set; }
        public double Sparepartscost { get; set; }
        public double TechCost { get; set; }
        public double BaseCost { get; set; }
        public double AdditionalCost { get; set; }
    }

    public class SimYearlyIterationResult
    {
        public int Id { get; set; }
        public int Year { get; set; }
        public double TBA { get; set; }
        public double EBA { get; set; }
        public double E_real { get; set; }
        public double E_theoretical { get; set; }
        public double E_downtime { get; set; }
        public double T_lifetime { get; set; }
        public double T_downtime { get; set; }
        public double Income { get; set; }
        public double Total_omcost { get; set; }
        public double V_fuelcost { get; set; }
        public double V_fixedcost { get; set; }
        public double Sparepartscost { get; set; }
        public double TechCost { get; set; }
        public double BaseCost { get; set; }
        public double AdditionalCost { get; set; }
        public List<SimVesselUsageData> VesselUsage { get; set; }
        public List<SimTAnalysis> DeviceAnalysis { get; set; }


    }
}
