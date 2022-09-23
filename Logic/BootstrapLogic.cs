using SELKIE.SimModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SELKIE.Logic
{
    public class BootstrapLogic
    {
        private int WWMaxYear;
        public BootstrapLogic()
        {
            WWMaxYear = 2024;
        }

        public BootstrapedDates BootstrapYear(DateTime from, DateTime to)
        {
            try
            {
                int yearcheck = from.Year;

                if (yearcheck <= WWMaxYear)
                {
                    return new BootstrapedDates
                    {
                        From = from,
                        To = to
                    };
                }
                else
                {
                    do
                    {
                        yearcheck -= (WWMaxYear - 2000);
                    } while (yearcheck > WWMaxYear);

                    return new BootstrapedDates
                    {
                        From = new DateTime(yearcheck, from.Month, from.Day, from.Hour, from.Minute, from.Second),
                        To = new DateTime(yearcheck, to.Month, to.Day, to.Hour, to.Minute, to.Second),
                    };
                }

            }
            catch (Exception e)
            {
                // log.Error(e);
                //log issue here
            }

            return new BootstrapedDates
            {
                From = from,
                To = to
            };
        }

        public BootstrapedDates BootstrapYearNullable(DateTime? _from, DateTime? _to)
        {
            try
            {
                int maxY = WWMaxYear;
                DateTime from = (DateTime)_from;
                DateTime to = (DateTime)_to;

                if (WWMaxYear == 2000)
                {
                    var _findMaxYear = WeatherData.GetMaxYear();
                    maxY = _findMaxYear;
                    WWMaxYear = maxY;
                }

                int yearcheck = from.Year;


                if (yearcheck <= maxY)
                {
                    return new BootstrapedDates
                    {
                        From = from,
                        To = to
                    };
                }
                else
                {
                    do
                    {
                        yearcheck -= (maxY - 2000);
                    } while (yearcheck > maxY);

                    return new BootstrapedDates
                    {
                        From = new DateTime(yearcheck, from.Month, from.Day, from.Hour, from.Minute, from.Second),
                        To = new DateTime(yearcheck, to.Month, to.Day, to.Hour, to.Minute, to.Second),
                    };
                }

            }
            catch (Exception e)
            {
                //log.Error(e);
                //log issue here
            }

            return new BootstrapedDates
            {
                From = (DateTime)_from,
                To = (DateTime)_to
            };
        }

    }
}
