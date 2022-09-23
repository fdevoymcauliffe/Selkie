using SELKIE.Entities;
using SELKIE.Enums;
using SELKIE.InstallModelList;
using SELKIE.Models;
using SELKIE.SimModelList;
using SELKIE.SimModels;
using SELKIE.SimResults;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SELKIE.Logic
{
    public class ReportsLogic
    {
        public ReportsLogic()
        {

        }

        public bool PrepareSimresults(int iterationId)
        {
            try
            {
                var _simturbines = SimDeviceList.GetAll().Where(x => x.SimStageId == iterationId).ToList();
                var _simvessels = SimVesselsList.GetAll().Where(v => v.SimIterationId == iterationId).ToList();

                for (int i = 2000; i < (2000 + ProjectDetails.ProjectLifeTime); i++)
                {
                    List<string> _basesUsedInOperationYearly = new List<string>();
                    List<int> _vesselsUsedInOperationYearly = new List<int>();
                    List<SimTAnalysis> deviceAnalysis = new List<SimTAnalysis>();
                    List<SimVesselUsageData> vesselanalysis = new List<SimVesselUsageData>();
                    DateTime current = new DateTime(i, 01, 01, 00, 00, 00);
                    DateTime till = new DateTime(i, 12, 31, 23, 59, 59);

                    #region init lists

                    SimYearlyIterationResult _iterationYearly = new SimYearlyIterationResult
                    {
                        Id = iterationId,
                        E_real = 0,
                        V_fuelcost = 0,
                        T_downtime = 0,
                        Sparepartscost = 0
                    };

                    foreach (var _simtdb in _simturbines)
                    {
                        var simana = new SimTAnalysis
                        {
                            SimTurbineId = _simtdb.Id,
                            Losthours = 0,
                            SimstageId = iterationId,
                            SimyearlyId = i,
                            EnergyLoss = 0
                        };
                        deviceAnalysis.Add(simana);
                    }

                    if (_simvessels != null)
                    {
                        foreach (var _v in _simvessels)
                        {
                            var v = new SimVesselUsageData
                            {
                                VesselId = _v.Id,
                                OperationalTime = 0,
                                StandbyTime = 0,
                                Vtype = _v.VesselType,
                                SimyearlyId = i,
                                SimstageId = iterationId,
                                Fuelcost = 0
                            };
                            vesselanalysis.Add(v);
                        }
                    }
                    #endregion

                    var toProcessIncidents = new List<SimIncidents>();
                    var _currentYearIncidents = SimIncidentsList.GetAll().Where(x => x.SimStageId == iterationId && x.IncidentTime >= current && x.IncidentTime <= till && x.ReportCompleted == false).OrderBy(d => d.IncidentTime).ToList();
                    var datescheck = _currentYearIncidents.Select(x => "" + x.IncidentTime + ", " + x.IncidentFixed).ToList();
                    var _pastYearIncidents = SimIncidentsList.GetAll().Where(x => x.SimStageId == iterationId && x.IncidentTime < current && x.ReportCompleted == false).ToList();

                    if (_currentYearIncidents.Count > 0)
                    {
                        foreach (var item in _currentYearIncidents)
                        {
                            toProcessIncidents.Add(item);
                        }
                    }

                    if (_pastYearIncidents.Count > 0)
                    {
                        foreach (var item in _pastYearIncidents)
                        {
                            toProcessIncidents.Add(item);
                        }
                    }

                    #region detect outage of each turbine for this year
                    foreach (var _t in deviceAnalysis)
                    {
                        List<FromTo> outageDates = new List<FromTo>();
                        //get all incidents on turbine
                        var _tincis = toProcessIncidents.FindAll(x => x.SimTurbinesId == _t.SimTurbineId).OrderBy(d => d.IncidentTime).ToList();
                        //outageDates = CheckForanyOverLaps(_tincis, current, till);

                        foreach (var _out in _tincis)
                        {
                            FromTo inciPeriod = new FromTo();

                            if (_out.IncidentTime < current)
                                inciPeriod.From = current;
                            else
                                inciPeriod.From = (DateTime)_out.IncidentTime;

                            if (_out.IncidentFixed.HasValue)
                            {
                                if (_out.IncidentFixed > till)
                                    inciPeriod.To = till;
                                else
                                    inciPeriod.To = (DateTime)_out.IncidentFixed;
                            }
                            else
                                inciPeriod.To = (DateTime)_out.IncidentFixed;

                            //lost hours
                            double _losedHours = (inciPeriod.To - inciPeriod.From).TotalHours;
                            _t.Losthours += _losedHours;
                            _iterationYearly.T_downtime += _losedHours;

                            //energy lost
                            double _tempELoss = WeatherYearlyData.CalculateEngLoss(inciPeriod.From, inciPeriod.To);
                            _t.EnergyLoss += _tempELoss;
                            _iterationYearly.E_downtime += _tempELoss;
                        }
                    }
                    #endregion

                    foreach (var inci in toProcessIncidents)
                    {
                        var T = deviceAnalysis.Where(x => x.SimTurbineId == inci.SimTurbinesId).FirstOrDefault();
                        if (T != null)
                        {
                            if (inci.IncidentFixed != null)
                            {
                                //calc other metrics
                                #region Calc spare parts and V analysis
                                var _jobsList = SimJobsDataList.GetAll().Where(x => x.SimIncidentsId == inci.Id).ToList();
                                if (_jobsList != null)
                                {
                                    foreach (var job in _jobsList)
                                    {
                                        if (job.FullRepair)
                                        {
                                            RepairDetails _repair = null;

                                            if (inci.Type == TaskType.Corrective)
                                            {
                                                _repair = TotalRepairs.GetObjByName(inci.Repair);
                                                var _comp = SimComponentsList.GetAll().Where(x => x.ComponentId == inci.SimComponentId).FirstOrDefault();
                                                if (_comp != null)
                                                {
                                                    _repair.Sparepart = _comp.SparepartPrice;
                                                }
                                            }
                                            else
                                            {
                                                var prev = TotalPriventives.GetAllPriventives().Where(x => x.PMCategory == inci.Repair).FirstOrDefault();
                                                _repair = new RepairDetails
                                                {
                                                    Operationlocation = prev.OperationLOC,
                                                    Sparepart = prev.Sparepart,
                                                    Base = prev.Base
                                                };

                                                if (_iterationYearly.AdditionalCost == 0)
                                                    _iterationYearly.AdditionalCost = PreventiveStartEnd.AdditionalAnnualCost;
                                            }
                                            // spare parts cost
                                            if (_repair != null)
                                            {
                                                if (!_basesUsedInOperationYearly.Contains(_repair.Base))
                                                    _basesUsedInOperationYearly.Add(_repair.Base);

                                                //skip counting repair cost if it is onshore maintainance
                                                //count only once, i.e. at stage 1
                                                if (_repair.Operationlocation == OperationalLocation.Onshore && inci.Stage == Task_Stage.OnshoreStage1)
                                                {
                                                    _iterationYearly.Sparepartscost += _repair.Sparepart;

                                                }
                                                else if (_repair.Operationlocation == OperationalLocation.Offshore)
                                                {
                                                    if (inci.Stage == Task_Stage.OffshoreOnly || inci.Stage == Task_Stage.OnshoreStage3)
                                                    {
                                                        _iterationYearly.Sparepartscost += _repair.Sparepart;
                                                    }

                                                }
                                            }
                                        }

                                        //calc fuel cost
                                        if (job.SimVesselsId > 0)
                                        {
                                            var findV = vesselanalysis.Find(x => x.VesselId == job.SimVesselsId);
                                            if (findV != null)
                                            {
                                                if (!_vesselsUsedInOperationYearly.Contains(findV.VesselId))
                                                    _vesselsUsedInOperationYearly.Add(findV.VesselId);

                                                double _fcost = 0;
                                                var _vessInfo = TotalVessels.GetObjByName(findV.Vtype);
                                                if (_vessInfo != null)
                                                {
                                                    _fcost = ((_vessInfo.FuelConsumption * _vessInfo.FuelCost) * job.OperationalTime) + ((_vessInfo.FuelConsumption * _vessInfo.FuelCost) * job.StandbyTime);
                                                    findV.Fuelcost += _fcost;
                                                    _iterationYearly.V_fuelcost += _fcost;
                                                    findV.OperationalTime += job.OperationalTime;
                                                    findV.StandbyTime += job.StandbyTime;
                                                }
                                            }
                                        }
                                    }
                                }
                                #endregion
                            }
                            else //test to see if any leaks in catching incidents
                            {

                            }

                        }
                    }

                    double _wakeloss = (1 - (ProjectDetails.WakeLoss / 100));
                    double _transloss = (1 - (ProjectDetails.TransLoss / 100));
                    double _elecloss = (1 - (ProjectDetails.ProdLoss / 100));

                    //fuel cost
                    //get all vessels and calc total cost

                    //foreach (var vess in vesselanalysis)
                    //{
                    //    //var _tempFcost = vess.Fuelcost; //CalcFuelCost(vess, dbData.VesselCTV, dbData.VesselHLV, dbData.VesselSOV);
                    //    //vess.Fuelcost += _tempFcost;
                    //    _iterationYearly.V_fuelcost += vess.Fuelcost;
                    //}

                    //tba
                    var totalHOurs = ((till - current).TotalHours) * deviceAnalysis.Count;
                    _iterationYearly.T_lifetime = totalHOurs;
                    //_iterationYearly.TBA = ((totalHOurs - _iterationYearly.T_downtime) / totalHOurs) * (_elecloss);
                    _iterationYearly.TBA = (totalHOurs - _iterationYearly.T_downtime) / totalHOurs;

                    //eba

                    var allweatherYearlyData = WeatherYearlyData.GetAll();
                    _iterationYearly.E_theoretical = WeatherYearlyData.GetYearEnergy(current.Year) * deviceAnalysis.Count;
                    _iterationYearly.E_real = (double)(_wakeloss * _transloss * _elecloss) * (_iterationYearly.E_theoretical - _iterationYearly.E_downtime);
                    //_iterationYearly.EBA = _iterationYearly.E_real / (double)(_iterationYearly.E_theoretical * (_wakeloss * _transloss));
                    _iterationYearly.EBA = _iterationYearly.E_real / _iterationYearly.E_theoretical;

                    //techcost
                    double _onshoreT = CalcTechSalaryCostPeryear(_basesUsedInOperationYearly);
                    //techcost
                    _iterationYearly.TechCost = _onshoreT;
                    //v_fixed
                    _iterationYearly.V_fixedcost = CalcFixedVesselCostPeryear(true, i, _vesselsUsedInOperationYearly);
                    //income
                    _iterationYearly.Income = _iterationYearly.E_real * ProjectDetails.GridSaleRate;
                    //basecost
                    _iterationYearly.BaseCost = CalcAnnualBaseFixedCost(_basesUsedInOperationYearly);
                    _iterationYearly.Total_omcost = _iterationYearly.V_fuelcost + _iterationYearly.V_fixedcost + _iterationYearly.Sparepartscost + _iterationYearly.TechCost + _iterationYearly.BaseCost + _iterationYearly.AdditionalCost;


                    var _idata = new SimYearlyIterationResult
                    {
                        Year = i,
                        Id = iterationId,
                        TBA = Math.Round(_iterationYearly.TBA, 3),
                        EBA = Math.Round(_iterationYearly.EBA, 3),
                        E_real = Math.Round(_iterationYearly.E_real, 2),
                        E_theoretical = Math.Round(_iterationYearly.E_theoretical, 2),
                        E_downtime = Math.Round(_iterationYearly.E_downtime, 2),
                        T_downtime = Math.Round(_iterationYearly.T_downtime, 2),
                        Income = Math.Round(_iterationYearly.Income, 2),
                        Total_omcost = Math.Round(_iterationYearly.Total_omcost, 2),
                        V_fuelcost = Math.Round(_iterationYearly.V_fuelcost, 2),
                        V_fixedcost = Math.Round(_iterationYearly.V_fixedcost, 2),
                        Sparepartscost = Math.Round(_iterationYearly.Sparepartscost, 2),
                        AdditionalCost = Math.Round(_iterationYearly.AdditionalCost, 2),
                        TechCost = Math.Round(_iterationYearly.TechCost, 2),
                        BaseCost = Math.Round(_iterationYearly.BaseCost, 2),
                        T_lifetime = Math.Round(_iterationYearly.T_lifetime, 2),
                        VesselUsage = new List<SimVesselUsageData>(),
                        DeviceAnalysis = new List<SimTAnalysis>()
                    };

                    if (_iterationYearly.EBA < 0 || _iterationYearly.TBA < 0)
                    {
                        //unknown error occured while performing sim
                        _iterationYearly.EBA = 0;
                        _iterationYearly.TBA = 0;
                    }

                    //add vessel usage into the result list
                    foreach (var vess in vesselanalysis)
                    {
                        _idata.VesselUsage.Add(vess);
                    }
                    //add devices into the result list
                    foreach (var _device in deviceAnalysis)
                    {
                        _idata.DeviceAnalysis.Add(_device);
                    }

                    SimIterationList.AddYearlySimIterationResult(_idata);

                    #region set inci report complete
                    foreach (var item in toProcessIncidents)
                    {
                        if (item.IncidentFixed.HasValue)
                        {
                            if (item.IncidentFixed <= till)
                            {
                                item.ReportCompleted = true;
                                SimIncidentsList.Update(item.Id, item);
                            }
                        }
                    }
                    #endregion
                }

                #region finally add simstage data

                var allIterationYearlyAddedData = SimIterationList.GetAllYearlySimIterationResultsByIterationId(iterationId);

                var _iterationMother = SimIterationList.GetIterationById(iterationId);

                _iterationMother.BaseCost = Math.Round(allIterationYearlyAddedData.Average(b => b.BaseCost), 2);
                _iterationMother.EBA = Math.Round(allIterationYearlyAddedData.Average(b => b.EBA), 3);
                _iterationMother.E_downtime = Math.Round(allIterationYearlyAddedData.Average(b => b.E_downtime), 2);
                _iterationMother.E_real = Math.Round(allIterationYearlyAddedData.Average(b => b.E_real), 2);
                _iterationMother.E_theoretical = Math.Round(allIterationYearlyAddedData.Average(b => b.E_theoretical), 2);
                _iterationMother.Income = Math.Round(allIterationYearlyAddedData.Average(b => b.Income), 2);
                _iterationMother.Sparepartscost = Math.Round(allIterationYearlyAddedData.Average(b => b.Sparepartscost), 2);
                _iterationMother.TBA = Math.Round(allIterationYearlyAddedData.Average(b => b.TBA), 3);
                _iterationMother.TechCost = Math.Round(allIterationYearlyAddedData.Average(b => b.TechCost), 2);
                _iterationMother.Total_omcost = Math.Round(allIterationYearlyAddedData.Average(b => b.Total_omcost), 2);
                _iterationMother.T_downtime = Math.Round(allIterationYearlyAddedData.Average(b => b.T_downtime), 2);
                _iterationMother.T_lifetime = Math.Round(allIterationYearlyAddedData.Average(b => b.T_lifetime), 2);
                _iterationMother.V_fixedcost = Math.Round(allIterationYearlyAddedData.Average(b => b.V_fixedcost), 2);
                _iterationMother.V_fuelcost = Math.Round(allIterationYearlyAddedData.Average(b => b.V_fuelcost), 2);
                _iterationMother.AdditionalCost = Math.Round(allIterationYearlyAddedData.Average(b => b.AdditionalCost), 2);

                _ = SimIterationList.UpdateSimIterationResult(_iterationMother, iterationId);
                #endregion

                return true;
            }
            catch (Exception)
            {
                // log.Error(e);
            }
            return false;
        }

        public double CalcAnnualBaseFixedCost(List<string> _basesUsedInOperationYearly)
        {
            double result = 0;
            foreach (var _base in TotalBases.GetBases())
            {
                if (_basesUsedInOperationYearly.Contains(_base.Basename))
                    result += _base.Annualcost;
            }
            return result;
        }

        public double CalcFixedVesselCostPeryear(bool ome, int year, List<int> _vesselsUsedInOperationYearly)
        {
            double result = 0;
            try
            {
                if (ome)
                {
                    foreach (var _vessel in SimVesselsList.GetAll())
                    {
                        //calculate only if the vessel is used
                        if (_vesselsUsedInOperationYearly.Contains(_vessel.Id))
                        {
                            VesselDetails vdetails = TotalVessels.GetObjByName(_vessel.VesselType);
                            if (vdetails != null)
                            {
                                if (vdetails.Purchased)
                                    result += vdetails.AnnualrunningCost;
                                else
                                {
                                    if (vdetails.Hireasrequired.ToLower() == "no")
                                    {
                                        result += vdetails.DailyRentalCost * (new DateTime(2000, vdetails.RentalEndMonth, vdetails.RentalEndDay, 0, 0, 0) - new DateTime(2000, vdetails.RentalStartMonth, vdetails.RentalStartDay, 0, 0, 0)).TotalDays;
                                        result += vdetails.MobilizationCost;
                                    }
                                    else
                                    {
                                        if (_vessel.HiredAsReqByYears != null)
                                        {
                                            var _getDaysHired = _vessel.HiredAsReqByYears.Where(x => x.Year == year).FirstOrDefault();
                                            if (_getDaysHired != null)
                                            {
                                                result += vdetails.DailyRentalCost * _getDaysHired.NoOfDaysHired;
                                                result += vdetails.MobilizationCost;
                                            }
                                        }

                                    }
                                }
                            }
                        }

                    }
                }
                else
                {
                    foreach (var _vessel in InstallVesselsList.GetAll())
                    {
                        //calculate only if the vessel is used
                        if (_vesselsUsedInOperationYearly.Contains(_vessel.Id))
                        {
                            VesselDetails vdetails = TotalVessels.GetObjByName(_vessel.VesselType);
                            if (vdetails != null)
                            {
                                if (vdetails.Purchased)
                                    result += vdetails.AnnualrunningCost;
                                else
                                {
                                    if (vdetails.Hireasrequired.ToLower() == "no")
                                    {
                                        result += vdetails.DailyRentalCost * (new DateTime(2000, vdetails.RentalEndMonth, vdetails.RentalEndDay, 0, 0, 0) - new DateTime(2000, vdetails.RentalStartMonth, vdetails.RentalStartDay, 0, 0, 0)).TotalDays;
                                        result += vdetails.MobilizationCost;
                                    }
                                    else
                                    {
                                        if (_vessel.RentedDaysForHireAsReq > 0)
                                        {
                                            result += vdetails.DailyRentalCost * _vessel.RentedDaysForHireAsReq;
                                            result += vdetails.MobilizationCost;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                // log.Error(e);
            }
            return result;
        }

        public double CalcTechSalaryCostPeryear(List<string> _basesUsedInOperationYearly)
        {
            double result = 0;
            foreach (var _base in TotalBases.GetBases())
            {
                if (_basesUsedInOperationYearly.Contains(_base.Basename))
                    result += _base.NoOfTechs * _base.AnnualsalperTech;
            }
            return result;
        }
    }
}
