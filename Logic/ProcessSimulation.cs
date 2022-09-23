using SELKIE.Enums;
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
    public class ProcessSimulation
    {
        DateTime projectStartDate = new DateTime(2000, 1, 1, 0, 0, 0);
        DateTime projectCurrentDate;
        DateTime projectEndDate;
       
        private Random rnd;
        private List<SimIncidents> daywiseIncidents;
        List<SimJobData> SimJobs;
        private List<SimVessel> vessellsAvailable;

        public ProcessSimulation()
        {
            rnd = new Random();
           
            SimJobs = new List<SimJobData>();
            vessellsAvailable = new List<SimVessel>();

        }
        public void StartSimProcess()
        {
            try
            {

            }
            catch (Exception e)
            {

            }
        }

        public bool ProcessSimStages(int simstageId)
        {
            List<VesselDetails> VesselInfo = new List<VesselDetails>();
            rnd = new Random();
            projectStartDate = new DateTime(2000, 1, 1, 0, 0, 0);
            projectCurrentDate = new DateTime(2000, 1, 1, 0, 0, 0);
            //yearly reports
            DateTime yearEnd = projectStartDate.AddYears(1);

            try
            {


                //set end date
                projectEndDate = projectStartDate.AddYears(ProjectDetails.ProjectLifeTime).AddDays(1);

                //add shift if not exists
                CreateDefaultShit();

                while (projectCurrentDate <= projectEndDate)
                {
                    //reset incidents
                    daywiseIncidents = new List<SimIncidents>();

                    //start simulate shift by shift
                    foreach (var shift in AllShifts.GetAll())
                    {
                        DateTime shiftStart = projectCurrentDate.Add(shift.Start.TimeOfDay);
                        DateTime shiftEnd = projectCurrentDate.Add(shift.End.TimeOfDay);

                        //get incidents of this shift
                        //TODO: also order by priority when levels are set
                        var getnextIncidents = SimIncidentsList.GetAll().Where(i => i.SimStageId == simstageId && i.Status != Task_Status.Completed && i.IncidentTime < shiftEnd).OrderBy(d => d.IncidentTime).ToList();
                        var getnextPendingIncidents = SimIncidentsList.GetAll().Where(i => i.SimStageId == simstageId && (i.Status == Task_Status.Pending || i.Status == Task_Status.ToCompFullRepair) && i.IncidentTime < shiftEnd).OrderBy(d => d.IncidentTime).ToList();

                        if (getnextIncidents.Any())
                        {
                            foreach (var incident in getnextIncidents)
                            {
                                //check if any previous incidents are on pending stage on the same turbine
                                var checkPreviousPendingIncidentsOnSameTurbine = getnextPendingIncidents.Find(x => x.SimTurbinesId == incident.SimTurbinesId);
                                if (checkPreviousPendingIncidentsOnSameTurbine == null)
                                {
                                    //check if local list already contains incident - this means it is ongoing issue which is not completed before
                                    var searchInLocalList = daywiseIncidents.Find(i => i.Id == incident.Id);
                                    if (searchInLocalList == null)
                                    {
                                        //add to locallist only if it is new
                                        daywiseIncidents.Add(incident);
                                    }
                                }
                                else
                                {
                                    //if previous and present are the same incident, then perform repair
                                    if (checkPreviousPendingIncidentsOnSameTurbine.Id == incident.Id)
                                    {
                                        //check if local list already contains incident - this means it is ongoing issue which is not completed before
                                        var searchInLocalList = daywiseIncidents.Find(i => i.Id == incident.Id);
                                        if (searchInLocalList == null)
                                        {
                                            //add to locallist only if it is new
                                            daywiseIncidents.Add(incident);
                                        }
                                    }
                                    else
                                    {
                                        daywiseIncidents.Remove(incident);
                                    }
                                }
                            }
                            if (daywiseIncidents.Any())
                            {
                                foreach (var inci in daywiseIncidents)
                                {
                                    if (inci.Status != Task_Status.Completed)
                                        FinalChecks(inci, shiftStart, shiftEnd);
                                }
                            }
                        }
                    }
                    projectCurrentDate = projectCurrentDate.AddHours(24);
                    //update incidents
                    if (daywiseIncidents != null && daywiseIncidents.Count > 0)
                        UPdateIncidents(projectCurrentDate >= projectEndDate);
                };
                //close off all incidents that pending...
                var getPendingIncidents = SimIncidentsList.GetAll().Where(i => i.SimStageId == simstageId && i.Status == Task_Status.Pending).OrderBy(d => d.IncidentTime).ToList();
                if (getPendingIncidents.Any())
                    FinalUpdatePendingIncidents(getPendingIncidents, projectEndDate);
                return true;
            }
            catch (Exception e)
            {
                // log.Error(e);
            }
            //set simrun status to false 
            return false;
        }

        private void CreateDefaultShit()
        {
            //create default shit
            var _shift = new Shifts
            {
                Date = new DateTime(),
                Start = new DateTime(2000, 01, 01, 07, 00, 00),
                End = new DateTime(2000, 01, 01, 19, 00, 00),
            };
            AllShifts.Add(_shift);
        }

        private void FinalChecks(SimIncidents incident, DateTime shiftStart, DateTime shiftEnd)
        {
            double repairToPerformDuration = 0, minrepairHoursCanbeperformed = 0;

            //save cross checking list data in _job
            SimJobData _job = new SimJobData();
            //repair info
            var repairToPerform = TotalRepairs.GetObjByName(incident.Repair);
            repairToPerformDuration = (repairToPerform.Operationlocation == OperationalLocation.Offshore) ? (double)repairToPerform.OperationdurationOffshore : (double)repairToPerform.OperatondurationOnshore;

            //component info
            var component = TotalComponents.GetObjByName(incident.Component);
            var oldInci = daywiseIncidents.FirstOrDefault(x => x.Id == incident.Id);

            _job.SimIncidentsId = oldInci.Id;
            _job.ShiftStart = shiftStart;
            _job.ShiftEnd = shiftEnd;
            _job.IncidentTime = oldInci.IncidentTime;
            _job.ComponentName = component.Componentname;
            _job.SimStageId = incident.SimStageId;

            //if repair is onshore maintainance work (stage two), just set as completed
            if (repairToPerform.Operationlocation == OperationalLocation.Onshore && incident.Stage == Task_Stage.OnshoreStage2)
            {
                _job.Completed = true;
                _job.AttemptedTime = oldInci.IncidentTime;
                _job.DelayedHours = (double)repairToPerform.OperatondurationOnshore;
                _job.FullRepair = true;
                _job.RepairStarted = oldInci.IncidentTime;
                _job.RepairEnded = ((DateTime)oldInci.IncidentTime).AddHours((double)repairToPerform.OperatondurationOnshore);
                _job.RepairName = repairToPerform.RepairName;
                _job.Notes = "Onshore Stage 2 completed.";
                _job.DelayedHours = ((DateTime)_job.RepairEnded - (DateTime)oldInci.IncidentTime).TotalHours;
                _job.Techsused = repairToPerform.NoOfTechs;
                _job.EnergyLoss = WeatherYearlyData.CalculateEngLoss(_job.IncidentTime, _job.RepairEnded);
                oldInci.Status = Task_Status.Completed;

                AddJobdata(_job);

                //create new incident to reinstall
                //create new task for stage 3
                var __simInci = new SimIncidents
                {
                    SimComponentId = incident.SimComponentId,
                    Stage = Task_Stage.OnshoreStage3,
                    Repair = incident.Repair,
                    IncidentTime = ((DateTime)oldInci.IncidentTime).AddHours((double)repairToPerform.OperatondurationOnshore),
                    SimStageId = incident.SimStageId,
                    Status = Task_Status.NotStarted,
                    Priority = PriorityLevels.Level1,
                    SimTurbinesId = incident.SimTurbinesId,
                    Component = incident.Component
                };

                SimIncidentsList.Add(__simInci);
            }
            else //offshore repairs
            {
                #region normal corrective process of performing job

                if (repairToPerform.Operationlocation == OperationalLocation.Onshore && incident.Stage == Task_Stage.OnshoreStage3)
                {
                    repairToPerformDuration = (double)repairToPerform.OperationdurationOffshore;
                }

                //set all vessels available for this shift at begining
                vessellsAvailable = SimVesselsList.GetAll().Where(v => v.SimStageId == incident.SimStageId).ToList();
                #region Set Availability
                foreach (var _vess in vessellsAvailable)
                {
                    _vess.Available = true;
                }
                #endregion

                //check vessel and technicians req
                if (AllShifts.GetObj(0) == null)
                    _job.ShiftHours = 12; //TODO: 12 is hardcoded here.. need to set on config file
                else
                    _job.ShiftHours = (AllShifts.GetObj(0).End - AllShifts.GetObj(0).Start).TotalHours;

                //act start of the shift as starting point if repair is in past, 
                //else check when repair occurs and then only start the working on it during the shift
                DateTime shiftStartIncaseMidDayRepair = shiftStart;
                if (oldInci.IncidentTime > shiftStart)
                {
                    _job.HoursAvailInShift = (shiftEnd - (DateTime)oldInci.IncidentTime).TotalHours;
                    _job.IdleHoursInShift = ((DateTime)oldInci.IncidentTime - shiftStart).TotalHours;
                    shiftStartIncaseMidDayRepair = shiftStart.AddHours(_job.IdleHoursInShift);
                }
                else
                {
                    _job.HoursAvailInShift = (shiftEnd - shiftStart).TotalHours;
                    _job.IdleHoursInShift = 0;
                }

                try
                {
                    double speedLimit = 0, fuelUsageT = 0, fuelUsageS = 0, fuelCost = 0;
                    TimeSpan journeyTime;

                    //check for vessel
                    if (repairToPerform != null)
                    {
                        var vesselInfo = PickVessel(repairToPerform);
                        if (vesselInfo.Proceed)
                        {
                            #region SpeedLimit
                            //check if farm speed limit is less than vessel speed
                            speedLimit = vesselInfo.Speed;
                            //speed Knots into km: because distance is in km
                            speedLimit *= 1.852;
                            #endregion

                            #region Distance
                            //get selected base
                            var selBase = TotalBases.GetObjByName(repairToPerform.Base);

                            if (selBase != null)
                            {
                                //TODO: check if base exists
                            }
                            double _distanceTOFarm = selBase.Distancetofarm;
                            #endregion

                            #region Time to travel 
                            _job.TotalTimeToTravel = (speedLimit > 0 ? ((_distanceTOFarm / speedLimit) * 2) : 0); //2 implies to and fro
                            #endregion

                            #region Timeconstraints
                            //calculate maxworkhouts
                            _job.MaxWorkHours = _job.HoursAvailInShift - _job.TotalTimeToTravel;



                            //here find how many hours worked on this incident before
                            var _wokredlist = SimJobs.Where(x => x.SimIncidentsId == _job.SimIncidentsId && x.WorkedHours > 0).ToList();
                            double _alreadyworkedhours = 0;
                            if (_wokredlist.Any())
                            {
                                foreach (var _wh in _wokredlist)
                                {
                                    _alreadyworkedhours += _wh.WorkedHours;
                                }
                                repairToPerformDuration -= _alreadyworkedhours;
                            }

                            TimeSpan _reqworkHours = TimeSpan.FromHours(repairToPerformDuration);
                            TimeSpan _maxworkHours = TimeSpan.FromHours(_job.MaxWorkHours);

                            double maxsafehoursReq = repairToPerformDuration + _job.TotalTimeToTravel;
                            double hoursToFix = repairToPerformDuration + (_job.TotalTimeToTravel / 2);

                            #endregion

                            #region Wave and Wind limits
                            double _waveLimit = vesselInfo.WaveHeightLimit;
                            #endregion



                            #region WW Check

                            //check WW
                            var ww = CheckWeatherConditions(shiftStartIncaseMidDayRepair, shiftEnd, _waveLimit, maxsafehoursReq);
                            if (ww != null)
                            {
                                if (!ww.Result)
                                {
                                    //fill WW data for ref
                                    _job.Notes = $"{_job.Notes},WW({shiftStartIncaseMidDayRepair:dd/MM/yyyy})";
                                    AddJobdata(_job);
                                    oldInci.Status = Task_Status.Pending;
                                    return;
                                }
                                else if (ww.Result && ww.FullRepairHours.CanTravel) //full required hours to fix is doable
                                    _job.FullRepair = true;
                                else if (ww.Result && !ww.FullRepairHours.CanTravel && ww.MinRepairHours.CanTravel) //do minimum work is possible
                                {
                                    //return as min repair 
                                    minrepairHoursCanbeperformed = ww.MinRepairHours.Safehours - _job.TotalTimeToTravel;

                                    //check pick what is possible to perform
                                    if (_job.MaxWorkHours < minrepairHoursCanbeperformed)
                                        minrepairHoursCanbeperformed = _job.MaxWorkHours;
                                    _job.FullRepair = false;
                                    _job.MinRepair = true;
                                }

                            }
                            else
                            {
                                //fill WW data for ref
                                _job.Notes = $"{_job.Notes},WW({shiftStartIncaseMidDayRepair:dd/MM/yyyy})";
                                AddJobdata(_job);
                                oldInci.Status = Task_Status.Pending;
                                return;
                            }
                            #endregion

                            #region Check Full or Min repair with other info

                            _job.AttemptedTime = shiftEnd.AddHours(-_job.MaxWorkHours); //for label

                            if (_job.MaxWorkHours > 0)
                            {
                                //if (_job.MaxWorkHours <= repairToPerformDuration || _job.MaxWorkHours <= minrepairHoursCanbeperformed)
                                //{
                                //DateTime tempjourneyStart = new DateTime();
                              //  _job.Completed = true; //this job will be done so marked it as completed

                                //tempjourneyStart = (_job.IdleHoursInShift > 0 ? (shiftStart.AddHours(_job.IdleHoursInShift)) : shiftStart);
                                journeyTime = TimeSpan.FromHours(_distanceTOFarm / speedLimit);


                                if (_job.FullRepair) //above WW will decide full or min repair first
                                {
                                    _job.Completed = true;
                                    _job.RepairEnded = shiftStartIncaseMidDayRepair.AddHours(_distanceTOFarm / speedLimit).AddHours(repairToPerformDuration);
                                    _job.FullRepair = true;
                                    //            _job.EnergyLoss = _commonLogic.CalcYearlyEnerygy(oldInci.IncidentTime, (DateTime)_job.RepairEnded, dbData.FarmInfo, dbData.ResourceDetails);
                                    _job.DelayedHours = ((DateTime)_job.RepairEnded - (DateTime)oldInci.IncidentTime).TotalHours;
                                    _job.EnergyLoss = WeatherYearlyData.CalculateEngLoss(_job.IncidentTime, _job.RepairEnded);
                                    _job.Notes = $"{_job.Notes},Job Completed!";
                                }
                                else if (_job.MinRepair)
                                {
                                    _job.Completed = true;
                                    //if delayed hours here are added just to bring in fuel cost when calc final reports
                                    _job.DelayedHours = 0.01;

                                    _job.RepairEnded = shiftStartIncaseMidDayRepair.AddHours(_distanceTOFarm / speedLimit).AddHours(minrepairHoursCanbeperformed);
                                    _job.WorkedHours = minrepairHoursCanbeperformed;
                                    _job.FullRepair = false;
                                    _job.Notes = $"{_job.Notes}, Minimum Repair carried out due to Weather conditions. ";
                                }
                                else
                                {
                                    //if delayed hours here are added just to bring in fuel cost when calc final reports
                                    _job.DelayedHours = 0.01;

                                    _job.RepairEnded = shiftStartIncaseMidDayRepair.AddHours(_distanceTOFarm / speedLimit).AddHours(minrepairHoursCanbeperformed);
                                    _job.WorkedHours = minrepairHoursCanbeperformed;
                                    _job.FullRepair = false;
                                    _job.Notes = $"{_job.Notes}, Minimum Repair carried out. ";
                                }

                                //fuel consumption
                                fuelCost = vesselInfo.FuelCost;
                                fuelUsageS = vesselInfo.FuelUsage;
                                fuelUsageT = vesselInfo.FuelUsage;

                                //jobs data
                                _job.VesselDeployment = vesselInfo.Name;
                                _job.DistanceTravelled = _distanceTOFarm;
                                _job.ShiftStart = shiftStart;
                                _job.ShiftEnd = shiftEnd;
                                _job.IncidentTime = oldInci.IncidentTime;
                                _job.DistanceToTravel = $"{_distanceTOFarm} km";
                                _job.Speedlimit = $"{speedLimit} km/s";
                                _job.JourneyStart = shiftStartIncaseMidDayRepair;
                                _job.JourneyTime = $"{Math.Round(_distanceTOFarm / speedLimit, 2)} hrs ({journeyTime.ToString(@"hh\:mm\:ss")})";
                                _job.JourneyEnd = shiftStartIncaseMidDayRepair.AddHours(_distanceTOFarm / speedLimit);
                                _job.RepairStarted = shiftStartIncaseMidDayRepair.AddHours(_distanceTOFarm / speedLimit);
                                _job.WorkStarted = DateTime.Now;
                                _job.WorkFinished = DateTime.Now;
                                _job.RepairName = repairToPerform.RepairName;
                                _job.RepairTimeToFix = $"{repairToPerformDuration} hr";
                                _job.RepairMinTimeToFix = $"{minrepairHoursCanbeperformed} hr";
                                _job.ComponentName = component.Componentname;
                                _job.StandbyTime = ((DateTime)_job.RepairEnded - (DateTime)_job.RepairStarted).TotalHours;
                                _job.OperationalTime = ((DateTime)_job.JourneyEnd - (DateTime)_job.JourneyStart).TotalHours * 2;

                                //_job.WorkedHours = (taskInAction.WorkFinished - tempjourneyStart).TotalHours;

                                _job.FuelUsedOper = (fuelUsageT * _job.OperationalTime);
                                double standByInHours = ((DateTime)_job.RepairEnded - (DateTime)_job.RepairStarted).TotalHours;
                                _job.FuelUsedSBy = fuelUsageS * standByInHours;

                                //_job.FuelUsed = (taskInAction.FuelUsageS + taskInAction.FuelUsageT);
                                //_job.FuelCost = fuelCost;
                                _job.TotalFuelCost = (fuelCost * (_job.FuelUsedSBy + _job.FuelUsedOper));

                                _job.Techsused = repairToPerform.NoOfTechs;
                                _job.TimeLeftInShift = ((DateTime)_job.JourneyEnd - shiftEnd).TotalHours;
                                _job.SimStageId = oldInci.SimStageId;
                                _job.SimVesselsId = vesselInfo.SimVesselsId;
                                //}
                                //else
                                //{
                                //    // note: max work hours is less than repair hours including minimum hours required to start to repair
                                //    //_job.FailedReason = FailedReason.MaxWorkingHours;
                                //    _job.Notes = $"{_job.Notes},NT({shiftStart.ToString("dd/MM/yyyy")})";// Available work hours ({_maxworkHours.ToString("h\\:mm")}hr) is less than repair required hours ({_reqworkHours.ToString("h\\:mm")}hr) including minimum hours {_minworkHours.ToString("h\\:mm")}hr required to start to repair";
                                //                                                                         //_job.Notes = $"{_job.Notes}{Environment.NewLine}Job not completed.";
                                //    AddJobdata(_job);
                                //    oldInci.Status = Task_Status.Pending;
                                //    return;
                                //}
                            }
                            else
                            {
                                _job.Notes = $"{_job.Notes},NT({shiftStartIncaseMidDayRepair.ToString("dd/MM/yyyy")})";// Available work hours ({_maxworkHours.ToString("h\\:mm")}hr) is less than repair required hours ({_reqworkHours.ToString("h\\:mm")}hr) including minimum hours {_minworkHours.ToString("h\\:mm")}hr required to start to repair";
                                                                                                                       //_job.Notes = $"{_job.Notes}{Environment.NewLine}Job not completed.";
                                AddJobdata(_job);
                                oldInci.Status = Task_Status.Pending;
                                return;
                                //_job.AttemptedTime = shiftStart;
                                //_job.FailedReason = FailedReason.MaxWorkingHours;
                                //_job.Notes = $"{_job.Notes}{Environment.NewLine}Maximum hours is less than required hours";
                                //_job.Notes = $"{_job.Notes}{Environment.NewLine}Job not completed.";
                                //AddJobdata(_job);
                                //oldInci.Status = Task_Status.Pending;
                                //return;

                            }
                            //perform next shift when activated. For now only one shift
                            #endregion
                        }
                        else
                        {
                            _job.Notes = $"{_job.Notes},NV({shiftStartIncaseMidDayRepair.ToString("dd/MM/yyyy")})";
                            AddJobdata(_job);
                            oldInci.Status = Task_Status.Pending;
                            return;
                            //_job.FailedReason = FailedReason.VesselOrTechs;
                            //_job.Notes = $"{_job.Notes}{Environment.NewLine}{vesselInfo.Comments}{Environment.NewLine} Job not completed.";
                            //AddJobdata(_job);
                            //oldInci.Status = Task_Status.Pending;
                            //return;
                        }

                        if (_job.FullRepair)
                        {
                            //check if repair is onshore and requires next stage maintenance
                            if (repairToPerform.Operationlocation == OperationalLocation.Onshore)
                            {
                                //check what stage is present repair completed
                                if (incident.Type == TaskType.Corrective)
                                {
                                    //create new task for stage 2
                                    var __simInci = new SimIncidents
                                    {
                                        SimComponentId = incident.SimComponentId,
                                        Stage = Task_Stage.OnshoreStage2,
                                        Repair = incident.Repair,
                                        IncidentTime = ((DateTime)_job.RepairEnded),
                                        SimStageId = incident.SimStageId,
                                        Status = Task_Status.NotStarted,
                                        Priority = PriorityLevels.Level1,
                                        SimTurbinesId = incident.SimTurbinesId,
                                        Component = incident.Component
                                    };

                                    SimIncidentsList.Add(__simInci);
                                }
                            }
                            oldInci.Status = Task_Status.Completed;
                        }
                        else //specifiy how many hours still required to complete the task
                        {
                            oldInci.Status = Task_Status.ToCompFullRepair;
                            oldInci.MinRepairs += 1;
                        }
                        AddJobdata(_job);
                    }
                }
                catch (Exception e)
                {

                    // log.Error(e);
                }
                #endregion
            }
        }

       

        private void AddJobdata(SimJobData _job)
        {
            if (_job != null)
            {
                var __existing = SimJobs.Find(s => s.SimIncidentsId == _job.SimIncidentsId);
                if (__existing != null)
                {
                    if (!_job.Completed)
                    {
                        __existing.NoOfAttempts += 1;
                        __existing.Notes = $"{__existing.Notes},{_job.Notes}";
                    }
                    else
                    {
                        if (_job.FullRepair)
                        {
                            __existing.Completed = true;
                            if (__existing.NoOfAttempts == 0)
                                __existing.NoOfAttempts = 1;
                        }
                        else
                        {
                            __existing.FullRepair = false;
                            __existing.DistanceTravelled += _job.DistanceTravelled;
                            __existing.WorkedHours += _job.WorkedHours;
                        }
                        SimJobs.Add(_job);
                    }
                }
                else
                {
                    //this will never happen but if does, just add job data 
                    _job.NoOfAttempts = 1;
                    SimJobs.Add(_job);
                }

            }
        }

        private VesselInfo PickVessel(RepairDetails repair)
        {
            VesselInfo result = new VesselInfo();
            try
            {
                //check if vessel is available in the list
                var checkVesselpresent = TotalVessels.GetObjByName(repair.Vesselrequired);
                if (checkVesselpresent != null)
                {
                    //check techs requirement
                    if (repair.NoOfTechs <= checkVesselpresent.TechsCapacity)
                    {
                        //check if vessel avail in sim vessels list
                        var simvesselAvail = vessellsAvailable.Find(v => v.VesselType == checkVesselpresent.VesselClassif && v.Available == true);
                        if (simvesselAvail != null)
                        {
                            bool allgoodWithRentalOrPurchased = true;
                            //check purchased or rental
                            if (simvesselAvail.Rented)
                            {
                                DateTime rentStart = new DateTime(Convert.ToInt32(projectCurrentDate.Year.ToString()), Convert.ToInt32(checkVesselpresent.RentalStartMonth.ToString()), Convert.ToInt32(checkVesselpresent.RentalStartDay.ToString()), 0, 0, 0);
                                DateTime rentEnd = new DateTime(Convert.ToInt32(projectCurrentDate.Year.ToString()), Convert.ToInt32(checkVesselpresent.RentalEndMonth.ToString()), Convert.ToInt32(checkVesselpresent.RentalEndDay.ToString()), 0, 0, 0);
                                if (projectCurrentDate >= rentStart && projectCurrentDate <= rentEnd)
                                {
                                    //do nothing as all good
                                }
                                else
                                {
                                    allgoodWithRentalOrPurchased = false;
                                    result.FailedReason = FailedReason.OutOfRentalPeriod;
                                    result.Comments = $"{result.Comments}{Environment.NewLine} Vessel is out of Rental period";
                                }
                            }

                            if (allgoodWithRentalOrPurchased)
                            {
                                //fill up with more info
                                result.Proceed = true;
                                result.WaveHeightLimit = repair.Waveheightlimit;
                                result.Speed = checkVesselpresent.Speed;
                                result.SimVesselsId = simvesselAvail.Id;
                                result.FuelUsage = checkVesselpresent.FuelConsumption;
                            }
                        }
                        else
                            result.FailedReason = FailedReason.VesselAvail;
                    }
                    else
                        result.FailedReason = FailedReason.Techs;
                }
                else
                    result.FailedReason = FailedReason.VesselAvail;
            }
            catch (Exception e)
            {

                //   log.Error(e);
            }
            return result;
        }

        

        /// <summary>
        /// This method to return max hours that can perform in the shift against WW
        /// if maxhrs are safe, then full repair can perform
        /// if hours to perform is greater than 0, then min hours repair is perfromed
        /// else return 0 due to bad ww condition
        /// </summary>
        /// <param name="From"></param>
        /// <param name="To"></param>
        /// <param name="limit"></param>
        /// <param name="maxhrs"></param>
        /// <returns></returns>
        public WWShift CheckWeatherConditions(DateTime From, DateTime To, double limit, double maxhrs)
        {
            WWShift result = new WWShift();
            List<WWResultData> resultData = new List<WWResultData>();
            result.WWResultForShift = resultData;
            try
            {
                DateTime safeStartingPoint = From;
                result.MinRepairHours = new SafeHours();
                result.FullRepairHours = new SafeHours();
                var totalHours = (To - From).TotalHours;
                var startHour = From.ToString("HH");
                int hrsCounter = 0;
                int longrunhours = 0;
                var wwshiftdata = WeatherYearlyData.GetWWData(From, To);

                if (wwshiftdata != null)
                {
                    int safehrcounter = 0;
                    foreach (var hrdata in wwshiftdata)
                    {
                        if (hrdata.Wave < limit)
                        {
                            hrsCounter++;
                            longrunhours = longrunhours < hrsCounter ? hrsCounter : longrunhours;
                        }
                        else
                        {
                            safeStartingPoint = safeStartingPoint.AddHours(safehrcounter);
                            hrsCounter = 0;
                        }
                        safehrcounter++;
                    }
                }

                if (longrunhours >= maxhrs)
                {
                    result.Result = true;
                    //can perform full repair
                    result.FullRepairHours = new SafeHours
                    {
                        CanTravel = true,
                        Safehours = longrunhours <= totalHours ? longrunhours : totalHours,
                        Safestart = safeStartingPoint
                    };
                }
                else if (longrunhours > 0) //if req hours not acheivable, return how many hours can perform in the shift
                {
                    result.Result = true;
                    //can perform full repair
                    result.MinRepairHours = new SafeHours
                    {
                        CanTravel = true,
                        Safehours = longrunhours,
                        Safestart = safeStartingPoint
                    };
                }
                else
                {
                    result.Result = false;
                }
            }
            catch (Exception e)
            {

                //log.Error(e);
            }
            return result;
        }

        private void UPdateIncidents(bool endDateReached)
        {
            try
            {
                foreach (var _inc in daywiseIncidents)
                {
                    if (!endDateReached) //update only those items are completed
                    {
                        if (_inc.Status == Task_Status.Completed || _inc.Status == Task_Status.ToCompFullRepair)
                        {
                            SimIncidentsList.Update(_inc.Id, _inc);
                            //find job that completed incident and add to jobs data
                            var findjobs = SimJobs.FindAll(s => s.SimIncidentsId == _inc.Id && s.Completed == true);
                            if (findjobs.Any())
                            {
                                foreach (var _eachJOb in findjobs)
                                {
                                    SimJobsDataList.Add(_eachJOb);
                                    //job and incident that are completed can be deleted from local lists
                                    SimJobs.Remove(_eachJOb);
                                }
                            }
                        }
                    }
                    else // update everything remaining in the list
                    {
                        foreach (var _incFinal in daywiseIncidents)
                        {
                            SimIncidentsList.Update(_incFinal.Id, _inc);

                        }
                        foreach (var _jobfinal in SimJobs)
                        {
                            SimJobsDataList.Add(_jobfinal);
                        }
                    }
                }

                //clean incidents that are completed from the local list
                var completedIncidents = daywiseIncidents.FindAll(i => i.Status == Task_Status.Completed);
                if (completedIncidents.Any())
                {
                    foreach (var _compInc in completedIncidents)
                    {
                        daywiseIncidents.Remove(_compInc);
                    }
                }
            }
            catch (Exception e)
            {
                //   log.Error(e);
            }
        }

        private void FinalUpdatePendingIncidents(List<SimIncidents> penIncidents, DateTime enddate)
        {
            try
            {
                foreach (var _inc in penIncidents)
                {
                    string _compoName = "";
                    var component = TotalComponents.GetObjByName(_inc.Component);
                    if (component != null)
                        _compoName = component.Componentname;

                    var _job = new SimJobData
                    {
                        AttemptedTime = enddate,
                        Completed = true,
                        DelayedHours = enddate.Subtract((DateTime)_inc.IncidentTime).TotalHours,
                        //EnergyLoss = CalcEnerygyLoss(_inc.IncidentTime, enddate),
                        ComponentName = _inc.Component,
                        IncidentTime = _inc.IncidentTime,
                        Notes = "Job failed to complete. End date reached",
                        SimStageId = _inc.SimStageId,
                        SimIncidentsId = _inc.Id,
                        RepairEnded = enddate
                    };
                    SimJobs.Add(_job);

                    _inc.Status = Task_Status.Completed;
                    SimIncidentsList.Update(_inc.Id, _inc);
                }
            }
            catch (Exception e)
            {
                //  log.Error(e);
            }
        }
    }
}
