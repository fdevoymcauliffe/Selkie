using System;
using System.Collections.Generic;
using System.Linq;

namespace SELKIE.Models
{
    public class TotalComponents
    {
        static List<ComponentDetails> comps = new List<ComponentDetails>();

        public static List<ComponentDetails> GetAllComponents()
        {
            return comps;
        }

        public static void Add(ComponentDetails _value)
        {
            comps.Add(_value);
        }

        public static void Update(int inx, ComponentDetails _value)
        {
            comps[inx] = _value;
        }

        public static ComponentDetails GetObj(int inx)
        {
            return comps[inx];
        }
        public static bool Delete(string componentName)
        {
            try
            {
                var itemtoRemove = GetObjByName(componentName);
                _ = comps.Remove(itemtoRemove);
                return true;
            }
            catch (Exception)
            {
                //log 
            }
            return false;

        }

        public static ComponentDetails GetObjByName(string name)
        {
            return comps.Where(x => x.Componentname == name).FirstOrDefault();
        }

        internal static void Init()
        {
            comps = new List<ComponentDetails>();
        }
    }

    public class ComponentDetails
    {
        public string Componentname { get; set; }
        public int Numberperdevice { get; set; }
        public double AnnualFailRate { get; set; }
        public string Repair { get; set; }
        public double SpareParts { get; set; }
    }
}
