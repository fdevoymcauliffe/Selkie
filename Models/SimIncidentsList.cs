using SELKIE.SimModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SELKIE.SimModelList
{
    public class SimIncidentsList
    {
        public static List<SimIncidents> all = new List<SimIncidents>();

        public static List<SimIncidents> GetAll()
        {
            return all;
        }
        public static void Add(SimIncidents value)
        {
            value.Id = Guid.NewGuid().ToString();
            all.Add(value);
        }
        public static void Update(string id, SimIncidents value)
        {
            var inx = all.FindIndex(x => x.Id == id);
            if (inx > -1)
                all[inx] = value;
        }
        public static bool SetDelete(string id)
        {
            try
            {
                var obj = all.Find(x => x.Id == id);
                if (obj != null)
                    _ = all.Remove(obj);
                return true;
            }
            catch (Exception)
            {

                // log.Error(e);
            }
            return false;
        }
        public static void Clear()
        {
            all = new List<SimIncidents>();
        }
        public static void SetFinalComplete(string rootId, DateTime? fixedTime)
        {
            if (!string.IsNullOrEmpty(rootId))
            {
                var mainInci = all.Find(x => x.Id == rootId);
                if (mainInci != null)
                {
                    mainInci.FinalFixed = fixedTime;
                }
                var findallList = all.FindAll(x => x.RootId == rootId).ToList();
                if (findallList.Any())
                {
                    foreach (var item in findallList)
                    {
                        item.FinalFixed = fixedTime;
                    }
                }
            }
        }

    }
}
