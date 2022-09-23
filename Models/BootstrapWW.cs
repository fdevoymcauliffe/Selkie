using System;
using System.Collections.Generic;
using System.Linq;

namespace SELKIE.SimModels
{
    public class BootstrapWW
    {
        static List<BootWW> _list = new List<BootWW>();
        static Random rnd;

        public static bool Reset()
        {
            _list = new List<BootWW>();
            return true;
        }

        public static void Add(BootWW value)
        {
            _list.Add(value);
        }

        public static int GetMappedYear(int _year)
        {
            if (_year > 0)
            {
                if (_year == 2000)
                {

                }
                var findY = _list.Where(x => x.Year == _year).FirstOrDefault();
                if (findY != null)
                    return findY.Mappedyear;
            }
            return _year;
        }

        public static bool PrepareBootstrapYears(int maxyear, int availyears)
        {
            rnd = new Random();
            if (!_list.Any())
            {
                if (maxyear > 0 && availyears > 0)
                {
                    maxyear = 2000 + maxyear;
                    availyears = 2000 + availyears - 1;

                    for (int i = 2000; i < maxyear; i++)
                    {
                        var _n = new BootWW
                        {
                            Year = i,
                            Mappedyear = rnd.Next(2000, availyears)
                        };
                        Add(_n);
                    }
                }
            }
            return true;
        }
    }

    public class BootWW
    {
        public int Year { get; set; }
        public int Mappedyear { get; set; }

    }

}
