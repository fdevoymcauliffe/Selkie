using SELKIE.Models;
using SELKIE.SimModelList;
using SELKIE.SimModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SELKIE.Logic
{
    public class EnergyCalcLogic
    {
        List<PowerCurves> _powerCurveList;
        public EnergyCalcLogic()
        {
        
        }

        //public double CalcEnerygyLossFinal(DateTime? incidentTime, DateTime? repairEnded)
        //{
        //    double result = 0;
        //    try
        //    {
        //        InitPCData();

        //        var bsdates = bootstarpLogic.BootstrapYearNullable(incidentTime, repairEnded);

        //        if (bsdates.From != null && bsdates.To != null)
        //        {
        //            var hourlydata = _rsrLogic.GetWWData(bsdates.From, bsdates.To);//_wwgateway.GetAll().Where(x => x.Time >= incidentTime && x.Time <= repairEnded && x.Source == _rsdId).ToList();
        //            if (hourlydata != null)
        //            {
        //                foreach (var hour in hourlydata)
        //                {
        //                    result += CalculateEnergyForThisHour(hour.Time, hour.Wave, hour.WavePeriod);
        //                }
        //            }

        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        // log.Error(e);
        //    }
        //    return result;
        //}

        public double CalculateEnergyForThisHour(DateTime currentTime, double wave, double wavePeriod)
        {
            try
            {
                double wavemin = 0, wavemax = 0, wpmin = 0, wpmax = 0;
                wavemin = _powerCurveList.Min(x => x.Windspeed);
                wavemax = _powerCurveList.Max(x => x.Windspeed);
                wpmin = _powerCurveList.Min(x => x.WavePeriod);
                wpmax = _powerCurveList.Max(x => x.WavePeriod);

                if (wave < wavemin || wave > wavemax || wavePeriod < wpmin || wavePeriod > wpmax)
                    return 0;
                else
                {
                    //round wave 

                    var nearestWaveHeight = _powerCurveList.Where(x => x.Windspeed <= Convert.ToDouble(wave)).OrderBy(x => Math.Abs(Convert.ToDecimal(wave) - Convert.ToDecimal(x.Windspeed))).FirstOrDefault();
                    var nearestWPeriodHeight = _powerCurveList.Where(x => x.WavePeriod <= Convert.ToDouble(wavePeriod)).OrderBy(x => Math.Abs(Convert.ToDecimal(wavePeriod) - Convert.ToDecimal(x.WavePeriod))).FirstOrDefault();

                    if (nearestWaveHeight != null && nearestWPeriodHeight != null)
                    {
                        var genObject = _powerCurveList.Where(c => c.WavePeriod == nearestWPeriodHeight.WavePeriod && c.Windspeed == nearestWaveHeight.Windspeed).FirstOrDefault();
                        if (genObject != null)
                            return genObject.Generation;
                    }
                }
            }
            catch (Exception e)
            {
                // log.Error(e);
            }
            return 0;
        }
    }
}
