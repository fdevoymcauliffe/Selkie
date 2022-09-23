using System.Collections.Generic;

namespace SELKIE.SimModels
{
    public class PowerCurveData
    {
        static List<PowerCurves> _list = new List<PowerCurves>();

        public static List<PowerCurves> GetAll()
        {
            return _list;
        }

        public static void Add(PowerCurves value)
        {
            _list.Add(value);
        }

        public static void Reset()
        {
            _list = new List<PowerCurves>();
        }
    }

    public class PowerCurves
    {
        public double WindOrWave { get; set; }
        public double Generation { get; set; }
        public double WavePeriod { get; set; }
    }
}
