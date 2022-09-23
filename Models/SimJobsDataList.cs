using SELKIE.SimModels;
using System;
using System.Collections.Generic;

namespace SELKIE.SimModelList
{
    public class SimJobsDataList
    {
        public static List<SimJobData> all = new List<SimJobData>();
        public static List<SimJobData> GetAll()
        {
            return all;
        }
        public static void Add(SimJobData value)
        {
            value.Id = Guid.NewGuid().ToString();
            all.Add(value);
        }
        public static void Clear()
        {
            all = new List<SimJobData>();
        }
    }
}
