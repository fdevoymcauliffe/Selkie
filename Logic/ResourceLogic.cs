using Microsoft.Office.Interop.Excel;
using SELKIE.Models;
using SELKIE.SimModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using Excel = Microsoft.Office.Interop.Excel;

namespace SELKIE.Logic
{
    public class ResourceLogic
    {
        bool closeWB = false;
        List<PowerCurves> _powerCurveList;
        Excel.Application xlApp;
        Excel.Workbook wb;
        Excel.Worksheet ws;
        public ResourceLogic()
        {
            xlApp = new Excel.Application();
            _powerCurveList = new List<PowerCurves>();
        }

        public bool PrepareWWDataForProject()
        {
            //get WW available years
            try
            {
                if (GetPCData())
                {
                    if (!WeatherYearlyData.GetAll().Any())
                    {
                        _powerCurveList = PowerCurveData.GetAll().ToList();
                        //read excel data
                        string filePath = ResourceDetails.Specifymetociandata;
                        string[] files = File.ReadAllLines(filePath);

                        //string filePath = Path.Combine(DataFolders.WWFolder, ResourceDetails.Specifymetociandata + ".xlsx");

                        wb = xlApp.Workbooks.Open(filePath);
                        ResourceDetails.NoOfYears = wb.Sheets.Count;
                        _ = BootstrapWW.PrepareBootstrapYears(ProjectDetails.ProjectLifeTime, ResourceDetails.NoOfYears);
                        closeWB = true;
                        try
                        {
                            for (int i = 2000; i < (ResourceDetails.NoOfYears) + 2000; i++)
                            {
                                var _testlist = WeatherYearlyData.GetAll().Count;
                                var previousYearList = WeatherYearlyData.GetYearWWData(i - 1);


                                WWYearlyData presentYdata = new WWYearlyData();
                                presentYdata.Year = i;
                                List<WWD> presentYearlyData = new List<WWD>();

                                ws = (Excel.Worksheet)wb.Worksheets.get_Item((i - 2000) + 1);

                                Range DataRange = ws.UsedRange;
                                object[,] WWValues = (object[,])DataRange.Value2;
                                var RowCount = DataRange.Rows.Count + 1;
                                var ColumnCount = DataRange.Columns.Count + 1;

                                DateTime currentTime = new DateTime(i, 01, 01, 00, 00, 00);
                                int startHour = 1;
                                int endHour = Convert.ToInt32((new DateTime(i, 12, 31, 23, 00, 00) - new DateTime(i, 01, 01, 00, 00, 00)).TotalHours);

                                for (int yHour = startHour; yHour <= endHour; yHour++)
                                {
                                    try
                                    {
                                        if (currentTime.AddHours(yHour) <= new DateTime(i, 12, 31, 23, 00, 00))
                                        {
                                            double _wind = 0, _wave = 0, _waveP = 0, _waveC = 0;
                                            if (RowCount > yHour)
                                            {
                                                for (int j = 2; j < 6; j++)
                                                {
                                                    double _value = 0;
                                                    try
                                                    {
                                                        if (WWValues[yHour + 1, j] != null)
                                                            _value = ConvertValue(WWValues[yHour + 1, j]);
                                                    }
                                                    catch (Exception)
                                                    {


                                                    }

                                                    if (j == 2)
                                                        _wind = _value;
                                                    else if (j == 3)
                                                        _wave = _value;
                                                    else if (j == 4)
                                                        _waveP = _value;
                                                    else if (j == 5)
                                                        _waveC = _value;
                                                }
                                            }
                                            //add data
                                            var _wwdata = new WWD
                                            {
                                                Time = currentTime.AddHours(yHour),
                                                Wind = _wind,
                                                Wave = _wave,
                                                WavePC = _waveP,
                                                CurrentSpeed = _waveC,
                                                EnergyGen = CalculateEnergyForThisHour(currentTime.AddHours(yHour), _wave, _waveP, _waveC)
                                            };
                                            presentYearlyData.Add(_wwdata);
                                        }
                                    }
                                    catch (Exception)
                                    {

                                        // log.Error(e);
                                    }

                                }
                                presentYdata.YearlyWWData = presentYearlyData;
                                WeatherYearlyData.Add(presentYdata);

                            }
                            return true;
                        }
                        catch (Exception)
                        {
                            //log issue
                        }
                    }
                }
                else
                    return false;
                return true;
            }
            catch (Exception)
            {
                //log issue
            }
            finally
            {
                if (closeWB)
                {
                    if (wb != null)
                        wb.Close();
                }
                closeWB = false;
            }
            return false;
        }
        public bool GetPCData()
        {
            //
            if (PowerCurveData.GetAll().Count == 0)
            {
                try
                {
                    //read excel data
                    string filePath = FarmDetails.PowerCurve;
                    string[] files = File.ReadAllLines(filePath);

                    //cell read for wave and tide
                    int readDataLength = 3;
                    if (FarmDetails.TechType.ToLower() == "wave")
                        readDataLength = 4;

                    try
                    {
                        wb = xlApp.Workbooks.Open(filePath);
                        ws = (Excel.Worksheet)wb.Worksheets.get_Item(1); //read only first sheet

                        //reset powercurve data
                        PowerCurveData.Reset();

                        bool endofData = false;
                        for (int rowData = 2; !endofData; rowData++)
                        {
                            double _wind = 0, _gen = 0, _waveP = 0;

                            for (int j = 1; j < readDataLength; j++)
                            {
                                if (ws.Cells[rowData, j].value != null)
                                {
                                    double _value = ConvertValue(ws.Cells[rowData, j].value);
                                    if (j == 1)
                                        _wind = _value;
                                    else if (j == 2)
                                        _gen = _value;
                                    else
                                        _waveP = _value;
                                }
                                else
                                    endofData = true;
                            }
                            //add data
                            if (!endofData) // means no nulls found while recording
                            {
                                var _pcData = new PowerCurves
                                {
                                    WindOrWave = _wind,
                                    Generation = _gen,
                                    WavePeriod = _waveP
                                };
                                PowerCurveData.Add(_pcData);
                            }
                        }
                        return true;
                    }
                    catch (Exception)
                    {
                    }
                }
                catch (Exception)
                {
                }
                finally
                {
                    if (wb != null)
                        wb.Close();
                }
                return false;
            }
            else
                return true;
        }
        private double ConvertValue(dynamic value)
        {
            try
            {
                if (double.TryParse(Convert.ToString(value), out double _v))
                    return value;
            }
            catch (Exception)
            {

            }
            return 0;
        }
        public double CalculateEnergyForThisHour(DateTime currentTime, double wave, double wavePeriod, double currentSpeed)
        {
            try
            {
                if (FarmDetails.TechType.ToLower() == "wave")
                {
                    double wavemin = 0, wavemax = 0, wpmin = 0, wpmax = 0;
                    wavemin = _powerCurveList.Min(x => x.WindOrWave);
                    wavemax = _powerCurveList.Max(x => x.WindOrWave);
                    wpmin = _powerCurveList.Min(x => x.WavePeriod);
                    wpmax = _powerCurveList.Max(x => x.WavePeriod);

                    if (wave < wavemin || wave > wavemax || wavePeriod < wpmin || wavePeriod > wpmax)
                        return 0;
                    else
                    {
                        //round wave 
                        var nearestWaveHeight = _powerCurveList.Where(x => x.WindOrWave <= Convert.ToDouble(wave)).OrderBy(x => Math.Abs(Convert.ToDecimal(wave) - Convert.ToDecimal(x.WindOrWave))).FirstOrDefault();
                        var nearestWPeriodHeight = _powerCurveList.Where(x => x.WavePeriod <= Convert.ToDouble(wavePeriod)).OrderBy(x => Math.Abs(Convert.ToDecimal(wavePeriod) - Convert.ToDecimal(x.WavePeriod))).FirstOrDefault();

                        if (nearestWaveHeight != null && nearestWPeriodHeight != null)
                        {
                            var genObject = _powerCurveList.Where(c => c.WavePeriod == nearestWPeriodHeight.WavePeriod && c.WindOrWave == nearestWaveHeight.WindOrWave).FirstOrDefault();
                            if (genObject != null)
                                return genObject.Generation;
                        }
                    }
                }
                else if (FarmDetails.TechType.ToLower() == "tidal")
                {
                    //convert waveperiod to selected hub height measurement
                    double modCurrentPeriod = currentSpeed * (Math.Pow(((double)FarmDetails.Tidal_hubHeight / (double)FarmDetails.Tidal_height), 0.14286));

                    if (modCurrentPeriod > 0)
                    {
                        ////cut in check
                        //if (modCurrentPeriod <= FarmDetails.Tidal_minCurrSpeed)
                        //    return 0;
                        var findCloset = _powerCurveList.Where(x => x.WindOrWave <= Convert.ToDouble(modCurrentPeriod)).OrderBy(x => Math.Abs(Convert.ToDecimal(wavePeriod))).ToList();
                        if (findCloset.Any())
                            return findCloset.LastOrDefault().Generation;
                    }
                }
            }
            catch (Exception)
            {
                // log.Error(e);
            }
            return 0;
        }
    }
}
