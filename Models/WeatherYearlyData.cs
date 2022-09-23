using System;
using System.Collections.Generic;
using System.Linq;

namespace SELKIE.SimModels
{
    public class WeatherYearlyData
    {
        static List<WWYearlyData> _list = new List<WWYearlyData>();

        public static List<WWYearlyData> GetAll()
        {
            return _list;
        }

        public static void Add(WWYearlyData value)
        {
            _list.Add(value);
        }

        public static WWYearlyData GetYearWWData(int _y)
        {
            var find = _list.Where(x => x.Year == _y).FirstOrDefault();
            if (find != null)
                return find;
            else
                return null;
        }

        public static double GetYearEnergy(int _y)
        {
            int _bootYear = BootstrapWW.GetMappedYear(_y);
            var find = _list.Where(x => x.Year == _bootYear).FirstOrDefault();
            if (find != null)
            {
                if (find.TotalYealyEnergyGen > 0)
                    return find.TotalYealyEnergyGen;
                else
                {
                    double result = 0;
                    foreach (var item in find.YearlyWWData)
                    {
                        result += item.EnergyGen;
                    }
                    find.TotalYealyEnergyGen = result;
                    return result;
                }
            }
            else
                return 0;
        }

        public static WWD GetWWDataOnHour(DateTime atHour)
        {
            var mappedY = BootstrapWW.GetMappedYear(atHour.Year);
            var yearsData = GetYearWWData(mappedY);
            if (yearsData != null)
            {
                var _hourData = yearsData.YearlyWWData.Where(x => x.Time == new DateTime(mappedY, atHour.Month, atHour.Day, atHour.Hour, 0, 0)).FirstOrDefault();
                if (_hourData != null)
                    return new WWD { EnergyGen = _hourData.EnergyGen, Time = atHour, Wave = _hourData.Wave, WavePC = _hourData.WavePC, Wind = _hourData.Wind };
            }
            return new WWD { EnergyGen = 0, Time = atHour, Wave = 0, WavePC = 0, Wind = 0 };
        }

        public static List<WWD> GetWWData(DateTime from, DateTime to)
        {
            List<WWD> result = new List<WWD>();
            try
            {
                //get bootstap years
                var _from = BootstrapWW.GetMappedYear(from.Year);
                var _to = BootstrapWW.GetMappedYear(to.Year);

                if (_from == _to) //all data within same year
                {
                    var yearsData = GetYearWWData(_from);
                    if (yearsData != null)
                    {
                        DateTime currentDate = new DateTime(_from, from.Month, from.Day, from.Hour, 0, 0);
                        DateTime endDate = new DateTime(_to, to.Month, to.Day, to.Hour, 0, 0);
                        for (int incHour = 0; endDate > currentDate; incHour++)
                        {
                            currentDate = currentDate.AddHours(incHour == 0 ? 0 : 1);
                            var _hourData = yearsData.YearlyWWData.Where(x => x.Time == currentDate).FirstOrDefault();
                            if (_hourData != null)
                                result.Add(new WWD { EnergyGen = _hourData.EnergyGen, Time = new DateTime(_from, currentDate.Month, currentDate.Day, currentDate.Hour, 0, 0), Wave = _hourData.Wave, WavePC = _hourData.WavePC, Wind = _hourData.Wind });
                            else
                                result.Add(new WWD { EnergyGen = 0, Time = new DateTime(_from, currentDate.Month, currentDate.Day, currentDate.Hour, 0, 0), Wave = 0, WavePC = 0, Wind = 0 });
                        }
                    }

                }
                else // mix two years
                {
                    #region first year data
                    var firstyearsData = GetYearWWData(_from);
                    if (firstyearsData != null)
                    {
                        DateTime currentDate = new DateTime(_from, from.Month, from.Day, from.Hour, 0, 0);
                        DateTime endDate = new DateTime(_from, 12, 31, 23, 0, 0);
                        for (int incHour = 0; endDate > currentDate; incHour++)
                        {
                            currentDate = currentDate.AddHours(incHour == 0 ? 0 : 1);
                            var _hourData = firstyearsData.YearlyWWData.Where(x => x.Time == currentDate).FirstOrDefault();
                            if (_hourData != null)
                                result.Add(new WWD { EnergyGen = _hourData.EnergyGen, Time = new DateTime(_from, currentDate.Month, currentDate.Day, currentDate.Hour, 0, 0), Wave = _hourData.Wave, WavePC = _hourData.WavePC, Wind = _hourData.Wind });
                            else
                                result.Add(new WWD { EnergyGen = 0, Time = new DateTime(_from, currentDate.Month, currentDate.Day, currentDate.Hour, 0, 0), Wave = 0, WavePC = 0, Wind = 0 });
                        }
                    }

                    #endregion
                    #region second year data
                    var secyearsData = GetYearWWData(_to);
                    if (secyearsData != null)
                    {
                        DateTime currentDate2 = new DateTime(_to, 01, 01, 0, 0, 0);
                        DateTime endDate2 = new DateTime(_to, to.Month, to.Day, to.Hour, 0, 0);

                        for (int incHour = 0; endDate2 > currentDate2; incHour++)
                        {
                            currentDate2 = currentDate2.AddHours(incHour == 0 ? 0 : 1);
                            var _hourData = secyearsData.YearlyWWData.Where(x => x.Time == currentDate2).FirstOrDefault();
                            if (_hourData != null)
                                result.Add(new WWD { EnergyGen = _hourData.EnergyGen, Time = new DateTime(_to, currentDate2.Month, currentDate2.Day, currentDate2.Hour, 0, 0), Wave = _hourData.Wave, WavePC = _hourData.WavePC, Wind = _hourData.Wind });
                            else
                                result.Add(new WWD { EnergyGen = 0, Time = new DateTime(_to, currentDate2.Month, currentDate2.Day, currentDate2.Hour, 0, 0), Wave = 0, WavePC = 0, Wind = 0 });
                        }
                    }

                    #endregion

                }

            }
            catch (Exception)
            {
                //log issue
            }
            return result;
        }

        public static double CalculateEngLoss(DateTime __from, DateTime __to)
        {
            double result = 0;
            try
            {
                //get bootstap years
                var _from = BootstrapWW.GetMappedYear(__from.Year);
                var _to = BootstrapWW.GetMappedYear(__to.Year);

                if (_from == _to) //all data within same year
                {
                    var yearsData = GetYearWWData(_from);

                    if (yearsData != null)
                    {
                        DateTime currentDate = new DateTime(_from, __from.Month, __from.Day, __from.Hour, 0, 0);
                        DateTime endDate = new DateTime(_to, __to.Month, __to.Day, __to.Hour, 0, 0);

                        do
                        {
                            var _hourData = yearsData.YearlyWWData.Where(x => x.Time == currentDate).FirstOrDefault();
                            if (_hourData != null)
                                result += _hourData.EnergyGen;
                            currentDate = currentDate.AddHours(1);
                        } while (endDate > currentDate);
                    }
                }
                else // mix two years
                {
                    #region first year data
                    var firstyearsData = GetYearWWData(_from);
                    if (firstyearsData != null)
                    {
                        DateTime currentDate = new DateTime(_from, __from.Month, __from.Day, __from.Hour, 0, 0);
                        DateTime endDate = new DateTime(_from, 12, 31, 23, 0, 0);
                        do
                        {
                            var _hourData = firstyearsData.YearlyWWData.Where(x => x.Time == currentDate).FirstOrDefault();
                            if (_hourData != null)
                                result += _hourData.EnergyGen;
                            currentDate = currentDate.AddHours(1);
                        } while (endDate > currentDate);
                    }
                    #endregion

                    #region second year data
                    var secyearsData = GetYearWWData(_to);

                    if (secyearsData != null)
                    {
                        DateTime currentDate2 = new DateTime(_to, 01, 01, 0, 0, 0);
                        DateTime endDate2 = new DateTime(_to, __to.Month, __to.Day, __to.Hour, 0, 0);

                        do
                        {
                            var _hourData = secyearsData.YearlyWWData.Where(x => x.Time == currentDate2).FirstOrDefault();
                            if (_hourData != null)
                                result += _hourData.EnergyGen;
                            currentDate2 = currentDate2.AddHours(1);
                        } while (endDate2 > currentDate2);
                    }
                    #endregion
                }
            }
            catch (Exception)
            {
                //log issue
            }
            return result;
        }

        public static bool Reset()
        {
            _list = new List<WWYearlyData>();
            _ = BootstrapWW.Reset();
            return true;
        }
    }

    public class WWYearlyData
    {
        public int Year { get; set; }
        public List<WWD> YearlyWWData { get; set; }
        public double TotalYealyEnergyGen { get; set; }
    }

    public class WWD
    {
        public DateTime Time { get; set; }
        public double Wave { get; set; }
        public double WavePC { get; set; }
        public double Wind { get; set; }
        public double CurrentSpeed { get; set; }
        public double EnergyGen { get; set; }
    }

}
