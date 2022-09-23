using SELKIE.SimModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SELKIE.InstallModelList
{
    public class InstallJobsDataList
    {
        public static List<InstallJobData> all = new List<InstallJobData>();

        public static List<InstallJobData> GetAll()
        {
            return all;
        }

        public static void Add(InstallJobData value)
        {
            value.Id = Guid.NewGuid().ToString();
            all.Add(value);
        }
        public static List<InstallJobData> GetJobsByIncidentId(string id)
        {
            int actId = Convert.ToInt32(id);
            var lis = all.Where(s => s.InstallId == actId).OrderBy(d => d.ShiftStart).ToList();
            return lis;
        }
        public static void Clear()
        {
            all = new List<InstallJobData>();
        }
    }
}
