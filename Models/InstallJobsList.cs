using SELKIE.Enums;
using SELKIE.SimModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SELKIE.InstallModelList
{
    public class InstallJobsList
    {
        public static List<InstallJobs> all = new List<InstallJobs>();

        public static List<InstallJobs> GetAll()
        {
            return all;
        }

        public static void Add(InstallJobs value)
        {
            value.Id = all.Count + 1;
            all.Add(value);
        }

        public static void Update(int id, InstallJobs value)
        {
            var inx = all.FindIndex(x => x.Id == id);
            if (inx > -1)
                all[inx] = value;
        }

        public static InstallJobs GetObj(int inx)
        {
            return all[inx];
        }
        public static List<InstallJobs> GetIncidentsByDeviceId(int id)
        {
            return all.Where(s => s.InstallDeviceId == id).OrderBy(d => d.IncidentTime).ToList();
        }
        public static void Clear()
        {
            all = new List<InstallJobs>();
        }

        public static bool CheckIfSubCompleted(int installDeviceId, DateTime projectCurrentDate)
        {
            var _check = all.Where(x => x.Status == Install_Status.Completed && x.InstallData.InstallType == InstallType.SubStructure && x.InstallDeviceId == installDeviceId).FirstOrDefault();
            if (_check != null)
            {
                if (_check.CompletedDate.HasValue)
                    if (_check.CompletedDate <= projectCurrentDate)
                        return true;
            }

            return false;

        }

        //this is to check if current need O&M
        public static bool CheckAllTasksCompletedBeforeDate(int year)
        {
            var _check = all.Where(x => x.Status != Install_Status.Completed).ToList();
            if (all != null)
                return false;
            else
            {
                //check date
                var _dateCheck = all.OrderByDescending(d => d.CompletedDate).FirstOrDefault();
                if (_dateCheck != null)
                {
                    if (Convert.ToDateTime(_dateCheck.CompletedDate).Year <= year)
                        return false;
                }
            }
            return true;
        }

        public static bool CheckAllTasksCompleted()
        {
            var _check = all.Where(x => x.Status != Install_Status.Completed).ToList();
            if (_check.Count > 0)
                return false;
            return true;
        }
    }
}
