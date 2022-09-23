using SELKIE.Entities;
using SELKIE.Enums;
using SELKIE.InstallModelList;
using SELKIE.InstallResultList;
using SELKIE.Logic;
using SELKIE.Models;
using SELKIE.SimModelList;
using SELKIE.SimModels;
using SELKIE.SimResults;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace SELKIE
{
    public partial class SimForm : Form
    {
        #region init
        Bases _bases = new Bases();
        Vessels _vessels = new Vessels();
        Installation_Strategy _installStrategy = new Installation_Strategy();
        PM_Maintenance _pmMaintenance = new PM_Maintenance();
        Repairs _repairs = new Repairs();
        Components _components = new Components();
        DateTime _simstarted = new DateTime(), _simcompleted = new DateTime();
        DateTime _installstarted = new DateTime(), _installcompleted = new DateTime();
        ReportsLogic _reportLogic = new ReportsLogic();
        private Random rnd;

        delegate void SetSimStatusList(int percentage);
        delegate void SetStatusText(string _updatetext);
        delegate void SetIterationStatusText(string _updatetext);

        ResourceLogic _rsrLogic = new ResourceLogic();

        DateTime projectStartDate = new DateTime(2000, 1, 1, 0, 0, 0);
        DateTime projectCurrentDate;
        public ValidationCheck validCheck = new ValidationCheck();
        DateTime projectEndDate;
        double _pmstart = 0, _pmend = 0;

        private List<SimIncidents> daywiseIncidents;
        List<SimJobData> SimJobs;

        List<string> deletedIncidents = new List<string>();
        int currentIteration = 0;

        private void InitAllList(bool install)
        {
            if (install)
            {
                #region init sim models
                InstallBaseList.Clear();
                InstallDeviceList.Clear();
                InstallJobsList.Clear();
                InstallJobsDataList.Clear();
                InstallVesselsList.Clear();
                InstallBaseList.Clear();
                #endregion
            }
            else
            {
                #region init sim models
                AllShifts.Clear();
                SimComponentsList.Clear();
                SimBaseList.Clear();
                SimDeviceList.Clear();
                SimIncidentsList.Clear();
                SimJobsDataList.Clear();
                SimVesselsList.Clear();
                #endregion
            }
        }

        #endregion

        public SimForm()
        {
            InitializeComponent();
            rnd = new Random();
            if (SimIterationList.GetIterationResultCount() > 0)
                BtnReportstoExcel.Visible = true;
        }

        private async void btnSimStart_Click(object sender, EventArgs e)
        {
            try
            {
                currentIteration = 0;
                this.btnSimStart.Visible = false;
                this.BtnReportstoExcel.Visible = false;
                if (!await checkValidations())
                    return;

                //clear iteration list 
                SimIterationList.ClearSimIterationResults();
                InstallDeviceResultList.Clear();
                InstallYearlyResultList.Clear();

                _simstarted = DateTime.Now; ;
                this.simstratedTime.Text = _simstarted.ToString();
                this.simstratedTime.Visible = true;

                for (int i = 1; i <= ProjectDetails.NoOfIterations; i++)
                {
                    currentIteration = i;
                    #region Installation Strategy
                    //init
                    InitAllList(true);

                    //perform installation Strategy
                    bool instStatus = false;
                    #region Step 1: Installation Validation
                    installchecklbl.Visible = true;
                    installchecklbl.Text = "processing...";
                    UpdateAllStatus(6, null, true);
                    Task<bool> installsV = new Task<bool>(CheckInstallsInputValdation);
                    installsV.Start();
                    if (await installsV)
                    {
                        UpdateAllStatus(0, "Installation started...", true);
                        // installchecklbl.Text = "OK";
                    }
                    else
                    {
                        installchecklbl.Text = "Failed";
                        return;
                    }
                    #endregion

                    #region Step 2: Proj LOCK
                    projLockGrp.BackColor = Color.Green;
                    projLockLbl.Visible = true;
                    projLockLbl.Text = "Locked";
                    projLockLbl.ForeColor = Color.White;
                    ProjectSettings.ProjectLock = true;
                    #endregion

                    #region Step 3:  Resource DATA
                    UpdateAllStatus(5, "reading resource data. do not close application..", true);
                    _ = WeatherYearlyData.Reset();

                    Task<bool> _rsrWWCheck = new Task<bool>(_rsrLogic.PrepareWWDataForProject);
                    _rsrWWCheck.Start();
                    if (!await _rsrWWCheck)
                    {
                        //log issue as failed
                        _ = MessageBox.Show("Error reading WW data from excel.");
                        ProjectSettings.ProjectLock = false;
                        InitAllList(true);
                        return;
                    }
                    #endregion

                    #region Step 4: START installation 

                    //record start
                    _installstarted = DateTime.Now;
                    this.insStratedTime.Text = _installstarted.ToString();
                    this.insStratedTime.Visible = true;

                    //process installation
                    Task<bool> _startInstall = new Task<bool>(ProcessInstall);
                    _startInstall.Start();
                    if (await _startInstall)
                    {
                        instStatus = true;
                        //check if all jobs completed
                        if (InstallJobsList.GetAll().Where(x => x.Status != Install_Status.Completed).Count() > 0)
                        {
                            installchecklbl.Text = "Installation Jobs cannot complete in time";
                            instStatus = false;
                            ProjectSettings.ProjectLock = true;
                        }
                        else
                        {
                            installchecklbl.Text = "Installation Completed";
                            instStatus = true;
                        }
                    }
                    else
                    {
                        installchecklbl.Text = "Installation Failed";
                        ProjectSettings.ProjectLock = true;
                    }

                    //record end
                    _installcompleted = DateTime.Now;
                    this.insEndedTime.Text = _installcompleted.ToString();
                    this.insEndedTime.Visible = true;

                    #endregion

                    #endregion
                    #region START SIM
                    if (instStatus)
                    {
                        Task<bool> _startSim = new Task<bool>(ProcessSim);
                        _startSim.Start();
                        if (!await _startSim)
                        {
                            _ = MessageBox.Show("Sim stopped due to failure to O&M");
                            ProjectSettings.ProjectLock = false;
                            break;
                        }
                    }
                    else
                    {
                        _ = MessageBox.Show("Sim stopped due to failure to Install");
                        ProjectSettings.ProjectLock = false;
                        break;
                    }
                    #endregion
                }

                _simcompleted = DateTime.Now;
                this.simendedTime.Text = _simcompleted.ToString();
                this.simendedTime.Visible = true;
                ProjectSettings.ProjectLock = false;
            }
            catch (Exception)
            {

            }

            this.btnSimStart.Visible = true;
            this.BtnReportstoExcel.Visible = true;
        }

        #region update status text

        public void UpdateSimstatusList(int percentage)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.

            if (this.totalSimProgressBar.InvokeRequired)
            {
                SetSimStatusList d = new SetSimStatusList(UpdateSimstatusList);
                _ = this.Invoke(d, new object[] { percentage });
            }
            else
            {
                totalSimProgressBar.Value = percentage;
            }
        }

        public void UpdateInstallstatusList(int percentage)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.

            if (this.installprogressBar.InvokeRequired)
            {
                SetSimStatusList d = new SetSimStatusList(UpdateInstallstatusList);
                _ = this.Invoke(d, new object[] { percentage });
            }
            else
            {
                installprogressBar.Value = percentage;
            }
        }

        public void UpdateStatusText(string _updatetext)
        {
            if (this.statustext.InvokeRequired)
            {
                SetStatusText s = new SetStatusText(UpdateStatusText);
                _ = this.Invoke(s, new object[] { _updatetext });
            }
            else
            {
                statustext.Text = _updatetext;
            }
        }

        public void UpdateIterationStatusText(string _updatetext)
        {
            if (this.IterationLabel.InvokeRequired)
            {
                SetIterationStatusText s = new SetIterationStatusText(UpdateIterationStatusText);
                _ = this.Invoke(s, new object[] { _updatetext });
            }
            else
            {
                IterationLabel.Text = _updatetext;
            }
        }

        public void UpdateErrormessageText(string _updatetext)
        {
            if (this.Errormessage.InvokeRequired)
            {
                SetStatusText s = new SetStatusText(UpdateErrormessageText);
                _ = this.Invoke(s, new object[] { _updatetext });
            }
            else
            {
                Errormessage.Text = _updatetext;
            }
        }

        public void UpdateInstallStatusText(string _updatetext)
        {
            if (this.installstatustext.InvokeRequired)
            {
                SetStatusText s = new SetStatusText(UpdateInstallStatusText);
                _ = this.Invoke(s, new object[] { _updatetext });
            }
            else
            {
                installstatustext.Text = _updatetext;
            }
        }

        public void UpdateAllStatus(int percentage, string statustext, bool installSt)
        {
            if (!installSt)
            {
                if (percentage > 0)
                    UpdateSimstatusList(percentage);
                if (!string.IsNullOrEmpty(statustext))
                    UpdateStatusText(statustext);

            }
            else
            {
                if (percentage > 0)
                    UpdateInstallstatusList(percentage);
                if (!string.IsNullOrEmpty(statustext))
                    UpdateInstallStatusText(statustext);
            }

        }

        #endregion

        #region set failures

        public TTFEntity TTF_PM(double failRate, double startHour, double endHour, int yearCount, DateTime defaultDate, double rand2)
        {
            TTFEntity result = new TTFEntity();
            //convert to hours
            double boundaryHours = failRate * (endHour - startHour);
            //365*24=8760
            failRate = (failRate > 0 ? (failRate / (endHour - startHour)) : 0.01); //to escape divide by zero => 0.01

            try
            {
                result.TTFHours = -(Math.Log(rand2) / failRate);

                if (result.TTFHours > boundaryHours)
                {
                    result.TTFHours = -1;
                    return result;
                }

                if (result.TTFHours > endHour)
                    result.TTFHours = endHour;

                if (yearCount > 0)
                    result.TTFDate = defaultDate.AddYears(yearCount).AddHours(startHour + result.TTFHours);
                else
                    result.TTFDate = defaultDate.AddHours(startHour + result.TTFHours);

            }
            catch (Exception)
            {
                //  log.Error(e);
            }
            return result;
        }

        public TTFEntity TTF(double failRate, int yearCount, DateTime defaultDate, double rand2)
        {
            _ = rnd.NextDouble() + failRate;
            TTFEntity result = new TTFEntity();
            //365*24=8760
            double additionalRnd = failRate > 0 ? failRate / 8760 : 0.01;
            try
            {
                result.TTFHours = -(Math.Log(rand2) / additionalRnd);

                if (result.TTFHours > failRate * 8760)
                {
                    //return to perform another failure rate
                    result.TTFHours = -1;
                    return result;
                }

                if (yearCount > 0)
                    result.TTFDate = defaultDate.AddYears(yearCount).AddHours(result.TTFHours);
                else
                    result.TTFDate = defaultDate.AddHours(result.TTFHours);

            }
            catch (Exception)
            {
                //  log.Error(e);
            }
            return result;
        }

        #endregion

        #region create incidents
        public void GenerateInstalJobs(int turbineId)
        {
            try
            {
                var installList = TotalInstallations.GetInstalls();
                //prepare components from installation inputs
                if (installList.Any())
                {

                    for (int i = installList.Count; i > 0; i--)
                    {
                        var testcount = InstallJobsList.GetAll().Count;
                        var insJob = new InstallJobs
                        {
                            Id = InstallJobsList.GetAll().Count + 1,
                            InstallData = installList[i - 1],
                            Status = Install_Status.NotStarted,
                            InstallDeviceId = turbineId,
                            Type = TaskType.Install,
                            Job = installList[i - 1].Taskname
                        };
                        InstallJobsList.Add(insJob);
                    }




                }
            }
            catch (Exception)
            {

                // log.Error(e);
            }
        }

        public void GenerateCMTasks(int turbineId, int simStageId)
        {
            try
            {
                //  rnd = new Random();
                if (TotalComponents.GetAllComponents().Count > 0)
                {
                    double failRate = 0;
                    //create tasks for each component located on Turbine with Id provided
                    foreach (var compo in TotalComponents.GetAllComponents())
                    {
                        var _repair = TotalRepairs.GetObjByName(compo.Repair);
                        #region ADD SimComp
                        //add components
                        var _simcomp = new SimComponent
                        {
                            SparepartPrice = compo.SpareParts
                        };
                        SimComponentsList.Add(_simcomp);
                        #endregion ADD SimComp

                        //Get AnnualFailRate either by user input or by dropdown selection
                        //ignore dropdown selection if user enters failure rate
                        if (compo.AnnualFailRate > -1)
                            failRate = (float)compo.AnnualFailRate;
                        else
                            failRate = 0.01;

                        //create task for each year based on annual failure rate
                        if (ProjectDetails.ProjectLifeTime > 0 && failRate > 0)
                        {
                            double topup = failRate;
                            for (int y = 0; y < ProjectDetails.ProjectLifeTime; y++)
                            {
                                bool repeatAnother = true;
                                do
                                {
                                    if (topup < 1)
                                    {
                                        topup = topup + failRate;
                                        repeatAnother = false;
                                    }
                                    else
                                    {
                                        int retry = 0;
                                        do
                                        {
                                            double testrnd = rnd.NextDouble();
                                            var findTTF = TTF(failRate, y, new DateTime(2000, 1, 1, 0, 0, 0), testrnd);
                                            if (findTTF.TTFHours > 0)
                                            {
                                                DateTime _ttf = TTF(failRate, y, new DateTime(2000, 1, 1, 0, 0, 0), testrnd).TTFDate;
                                                if (_ttf <= new DateTime(2000 + ProjectDetails.ProjectLifeTime, 1, 1, 0, 0, 0))
                                                {
                                                    var _simInci = new SimIncidents
                                                    {
                                                        SimComponentId = _simcomp.ComponentId,
                                                        Type = TaskType.Corrective,
                                                        Stage = _repair.Operationlocation == OperationalLocation.Onshore ? Task_Stage.OnshoreStage1 : Task_Stage.OffshoreOnly,
                                                        Repair = compo.Repair,
                                                        IncidentTime = _ttf,
                                                        SimStageId = simStageId,
                                                        Status = Task_Status.NotStarted,
                                                        Priority = PriorityLevels.Level1,
                                                        SimTurbinesId = turbineId,
                                                        Component = compo.Componentname
                                                    };

                                                    SimIncidentsList.Add(_simInci);

                                                    topup -= 1;
                                                }
                                            }
                                            else
                                            {
                                                //failure rate cause date out of scope
                                            }
                                            retry++;
                                        } while (retry > 1000);
                                    }
                                } while (repeatAnother);
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {

                // log.Error(e);// log.Error(e);
            }
        }

        public void GeneratePMTasks(int turbineId, int simStageId, double startHourInYear, double endHourInYear)
        {
            //create tasks for each component located on Turbine with Id provided
            try
            {
               
                double failRate = 0;

                //take each preventative defined for each device
                foreach (var _prev in TotalPriventives.GetAllPriventives())
                {

                    var _repair = TotalPriventives.GetAllPriventives().Where(x => x.PMCategory == _prev.PMCategory).FirstOrDefault();
                    if (startHourInYear >= 0 && endHourInYear > 0)
                    {
                        //Get AnnualFailRate either by user input or by dropdown selection
                        //ignore dropdown selection if user enters failure rate
                        if (_prev.Frequency > -1)
                            failRate = (float)_prev.Frequency;
                        else
                            failRate = 0.01;


                        //create task for each year based on annual failure rate
                        if (ProjectDetails.ProjectLifeTime > 1 && failRate > 0)
                        {
                            double topup = failRate;
                            for (int y = 0; y < ProjectDetails.ProjectLifeTime - 1; y++)
                            {
                                bool repeatAnother = true;
                                do
                                {
                                    if (topup < 1)
                                    {
                                        topup = topup + failRate;
                                        repeatAnother = false;
                                    }
                                    else
                                    {
                                        int retry = 0;
                                        bool inRange = false;
                                        do
                                        {
                                            var findTTF = TTF_PM(failRate + rnd.NextDouble(), startHourInYear, endHourInYear, y, new DateTime(2000, 1, 1, 0, 0, 0), rnd.NextDouble());
                                            if (findTTF.TTFHours > 0)
                                            {
                                                DateTime _ttf = findTTF.TTFDate;
                                                inRange = (PMTaskInSeason(_ttf));
                                                if (inRange)
                                                {
                                                    if (_ttf <= new DateTime(2000 + ProjectDetails.ProjectLifeTime, 1, 1, 0, 0, 0))
                                                    {
                                                        var _simInci = new SimIncidents
                                                        {
                                                            SimComponentId = turbineId,
                                                            Type = TaskType.Preventative,
                                                            Stage = _repair.OperationLOC == OperationalLocation.Onshore ? Task_Stage.OnshoreStage1 : Task_Stage.OffshoreOnly,
                                                            Repair = _prev.PMCategory,
                                                            IncidentTime = _ttf,
                                                            SimStageId = simStageId,
                                                            Status = Task_Status.NotStarted,
                                                            Priority = PriorityLevels.Level1,
                                                            SimTurbinesId = turbineId,
                                                            Component = turbineId.ToString()
                                                        };
                                                        SimIncidentsList.Add(_simInci);
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                //TTF is out of boundaries
                                            }
                                            retry++;
                                        } while (!inRange || (inRange && retry > 1000));


                                        topup -= 1;
                                    }
                                } while (repeatAnother);
                            }
                        }
                    }


                }
            }
            catch (Exception)
            {
                // log.Error(e);// log.Error(e);
            }
        }
        #endregion

        #region SIM
        private bool ProcessInstall()
        {
            try
            {
                UpdateAllStatus(10, "preparing installation...", true);

                #region add bases
                UpdateAllStatus(11, "preparing bases...", true);
                foreach (var _base in TotalBases.GetBases())
                {
                    var _inbase = new InstallBasesDetails()
                    {
                        Annualcost = _base.Annualcost,
                        NoOfTechs = _base.NoOfTechs,
                        OriginalNoOfTechs = _base.NoOfTechs,
                        AnnualsalperTech = _base.AnnualsalperTech,
                        Basename = _base.Basename,
                        Distancetofarm = _base.Distancetofarm
                    };
                    InstallBaseList.Add(_inbase);
                }
                #endregion

                #region add vessels
                UpdateAllStatus(11, "lining up vessels...", true);
                int _vId = 1;
                foreach (var _vessel in TotalVessels.GetVessels())
                {
                    bool _rented = !_vessel.Purchased;
                    bool hireasReq = _vessel.Hireasrequired.ToLower() == "yes";

                    for (int i = 0; i < _vessel.Number; i++)
                    {
                        var _simV = new InstallVessel
                        {
                            Id = _vId,
                            Available = !_rented ? true : !hireasReq,
                            Rented = _rented,
                            HireAsReq = hireasReq,
                            LeadHrs = hireasReq ? _vessel.VesselLeadtime : 0,
                            OperationFrom = _rented == true ? (!hireasReq ? new DateTime(2000, _vessel.RentalStartMonth, _vessel.RentalStartDay) : DateTime.Now) : DateTime.Now,
                            OperationTo = _rented == true ? (!hireasReq ? new DateTime(2000, _vessel.RentalEndMonth, _vessel.RentalEndDay) : DateTime.Now) : DateTime.Now,
                            VesselType = _vessel.VesselClassif
                        };
                        InstallVesselsList.Add(_simV);
                        _vId++;
                    }

                }
                #endregion

                #region add devices (Turbines)

                UpdateAllStatus(12, "lining up devices...", true);

                for (int j = 0; j < FarmDetails.NoOfDivices; j++)
                {
                    var _deviceObj = new InstallDevice
                    {
                        Id = j + 1,
                        Name = "Device " + (j + 1)
                    };
                    InstallDeviceList.Add(_deviceObj);

                    #region create jobs
                    UpdateAllStatus(13, "calculating jobs...", true);
                    GenerateInstalJobs(_deviceObj.Id);
                    #endregion
                }
                #endregion

                if (StartInstallation())
                {
                    UpdateAllStatus(75, "preparing results...", true);
                    //prepare iteration results
                    foreach (var item in InstallDeviceList.GetAll())
                    {
                        var dData = InstallJobsList.GetIncidentsByDeviceId(item.Id);
                        if (dData.Any())
                        {
                            foreach (var _ins in dData)
                            {
                                var insData = InstallJobsDataList.GetJobsByIncidentId(_ins.Id.ToString());
                                if (insData.Any())
                                {
                                    int jobcounter = 1;
                                    foreach (var jdata in insData)
                                    {
                                        //installJobCarriedOutDataGrid.Rows.Add(_iteration.Id, _iteration.ShiftStart, _iteration.ComponentName, _iteration.Completed ? "Completed" : "Pending");
                                        var insdeviceresult = new InstallDeviceAnalysis
                                        {
                                            DeviceID = item.Id,
                                            Iteration = currentIteration,
                                            Jobs = jobcounter,
                                            Status = jdata.Completed ? "Completed" : "Pending",
                                            DeviceType = _ins.Job,
                                            Complete = jdata.RepairEnded,
                                            Start = jdata.RepairStarted,
                                            InstallTime = jdata.WorkedHours
                                            //Notes = jdata.Notes
                                        };
                                        //get 
                                        InstallDeviceResultList.Add(insdeviceresult);
                                        jobcounter++;
                                    }
                                }
                            }
                        }
                    }
                }
                UpdateAllStatus(100, "installation completed.", true);
                return true;
            }
            catch (Exception)
            {
                //close sim and display errors
            }
            return false;
        }
        public bool StartInstallation()
        {
            List<VesselDetails> VesselInfo = new List<VesselDetails>();
            List<string> _basesUsedInOperationYearly = new List<string>();
            List<int> _vesselUsedInOperationYearly = new List<int>();
            // rnd = new Random();
            projectStartDate = new DateTime(2000, Convert.ToInt32(InstStrategyDetails.Instalstartmonth), 1, 0, 0, 0);
            projectCurrentDate = new DateTime(2000, Convert.ToInt32(InstStrategyDetails.Instalstartmonth), 1, 7, 0, 0);

            //yearly reports
            DateTime yearEnd = projectStartDate.AddYears(1);
            int yearcounter = 2000;

            try
            {
                UpdateAllStatus(15, "processing installation strategy ...", true);
                //set end date
                projectEndDate = new DateTime(projectStartDate.Year + (ProjectDetails.InstallTime - 1), 12, 31, 23, 0, 0);

                //for percentage fill up
                var totaldays = (projectEndDate - projectStartDate).TotalDays;

                //add shift if not exists
                CreateDefaultShit(false);

                while (projectCurrentDate <= projectEndDate)
                {
                    int _testperc = 15 + (60 - Convert.ToInt32((projectEndDate - projectCurrentDate).TotalDays * 60 / totaldays));
                    UpdateAllStatus(_testperc, projectCurrentDate.ToString(), true);

                    //clean status for all remaining intalls
                    if (InstallJobsList.GetAll().Any())
                    {
                        foreach (var item in InstallJobsList.GetAll().Where(x => x.Status != Install_Status.Completed).ToList())
                        {
                            if (item.Status == Install_Status.ToCompFullRepair)
                            {
                                item.Status = Install_Status.NotStarted;
                                InstallJobsList.Update(item.Id, item);
                            }
                        }
                    }

                    //start simulate hour by hour

                    bool proceed = true;
                    DateTime shiftStart = projectCurrentDate;
                    DateTime shiftEnd = shiftStart.AddHours(12);

                    #region Release or ADD Vessel and Techs based on task status
                    foreach (var deployedVessel in InstallVesselsList.GetAll().Where(c => c.OnTaskFrom.HasValue).ToList())
                    {
                        if (deployedVessel.OnTaskT0 != null && deployedVessel.OnTaskT0 < shiftStart)
                        {
                            //release techs from base
                            if (!string.IsNullOrEmpty(deployedVessel.Base))
                            {
                                var _deployedBase = InstallBaseList.GetObjByName(deployedVessel.Base);
                                if (_deployedBase != null)
                                {
                                    _deployedBase.NoOfTechs += deployedVessel.CarryingTechs;
                                    //update base
                                    InstallBaseList.Update(_deployedBase.Basename, _deployedBase);
                                }
                            }

                            //reset vessel and tech
                            deployedVessel.OnTaskFrom = null;
                            deployedVessel.OnTaskT0 = null;


                            if (deployedVessel.HireAsReq)
                            {
                                // if hiretriggered is false that means hired vessel period is finished
                                if (deployedVessel.HireTriggered)
                                    deployedVessel.Available = true;
                                else
                                    deployedVessel.Available = false;
                            }
                            else
                                deployedVessel.Available = true;

                            if (!deployedVessel.Rented)
                                deployedVessel.Available = true;

                            deployedVessel.Base = "";
                            deployedVessel.CarryingTechs = 0;
                            InstallVesselsList.Update(deployedVessel.Id, deployedVessel);
                        }
                    }
                    #endregion

                    while (proceed)
                    {
                        List<InstallJobs> jobsToPerform = new List<InstallJobs>();
                        //pick job from the list
                        //substructure first and then device
                        var currentJob = InstallJobsList.GetAll().Where(x => x.Status == Install_Status.NotStarted).OrderBy(i => i.InstallDeviceId).FirstOrDefault();
                        if (currentJob != null)
                        {
                            //make sure sub is completed for device
                            if (currentJob.InstallData.InstallType == InstallType.Device)
                            {
                                //check if sub structure is completed
                                bool currentdeviceSub = InstallJobsList.CheckIfSubCompleted(currentJob.InstallDeviceId, projectCurrentDate);
                                if (!currentdeviceSub)
                                    break;
                            }


                            var total = InstallJobsList.GetAll().ToList();
                            //if no jobs to perform, then installation is complete
                            if (InstallJobsList.GetAll().Where(x => x.Status == Install_Status.Completed).Count() == total.Count)
                                return true;

                            //no vessel = break
                            if (!InstallVesselsList.CheckIfAnyVesselAvailable(projectCurrentDate, currentJob.InstallData.VesselReq))
                            {
                                //trigger lead time if applicable
                                InstallVesselsList.TriggerLeadTimeForVessel(shiftStart, currentJob.InstallData.VesselReq);
                                break;
                            }

                            //check what base is being used
                            var fromBase = InstallBaseList.GetObjByName(currentJob.InstallData.Base);
                            //check if req techs available
                            if (currentJob.InstallData.OperationDuration >= 12)
                            {
                                if (fromBase.NoOfTechs < currentJob.InstallData.NoOftechsReq * 2)
                                    break;
                            }
                            else
                            {
                                if (fromBase.NoOfTechs < currentJob.InstallData.NoOftechsReq)
                                    break;
                            }

                            jobsToPerform.Add(currentJob);
                            if (currentJob.InstallData.Numberofdevicespervessel > 0)
                            {
                                //check if current job is sub or device
                                if (currentJob.InstallData.InstallType == InstallType.SubStructure)
                                {
                                    //check if other sub-struct to complete
                                    var substocomplete = InstallJobsList.GetAll().Where(x => x.Status == Install_Status.NotStarted && x.InstallData.InstallType == InstallType.SubStructure).OrderBy(i => i.InstallDeviceId).ToList();
                                    if (substocomplete.Any())
                                    {
                                        for (int i = 1; i < substocomplete.Count; i++)
                                        {
                                            if (jobsToPerform.Count < currentJob.InstallData.Numberofdevicespervessel)
                                            {
                                                //if there is a diff of tech requirement is higher, just ignore for now
                                                if (jobsToPerform[0].InstallData.NoOftechsReq <= substocomplete[i].InstallData.NoOftechsReq)
                                                    jobsToPerform.Add(substocomplete[i]);
                                            }
                                            else
                                                break;
                                        }
                                    }
                                }
                                else
                                {
                                    //check if other device to complete
                                    var devicestocomplete = InstallJobsList.GetAll().Where(x => x.Status == Install_Status.NotStarted && x.InstallData.InstallType == InstallType.Device).OrderBy(i => i.InstallDeviceId).ToList();
                                    if (devicestocomplete.Any())
                                    {
                                        for (int i = 1; i < devicestocomplete.Count; i++)
                                        {
                                            if (jobsToPerform.Count < currentJob.InstallData.Numberofdevicespervessel)
                                            {
                                                //if there is a diff of tech requirement is higher, just ignore for now
                                                if (jobsToPerform[0].InstallData.NoOftechsReq <= devicestocomplete[i].InstallData.NoOftechsReq)
                                                {
                                                    //check if sub is completed

                                                    if (InstallJobsList.CheckIfSubCompleted(devicestocomplete[i].InstallDeviceId, projectCurrentDate))
                                                        jobsToPerform.Add(devicestocomplete[i]);
                                                }
                                            }
                                            else
                                                break;
                                        }
                                    }
                                }
                            }

                            //perform this job
                            CompInstallJobFeedback _jobStat = CompleteInstallJob(jobsToPerform, shiftStart, shiftEnd, fromBase.NoOfTechs);
                            if (_jobStat != null)
                            {
                                foreach (var _jj in jobsToPerform)
                                {
                                    if (_jobStat.Status != Install_Status.Completed)
                                    {
                                        _jj.Status = Install_Status.ToCompFullRepair;
                                    }
                                    else
                                    {
                                        _jj.Status = _jobStat.Status;

                                    }
                                    InstallJobsList.Update(_jj.Id, _jj);
                                    //record what base is used
                                    if (!_basesUsedInOperationYearly.Contains(fromBase.Basename))
                                        _basesUsedInOperationYearly.Add(fromBase.Basename);

                                    if (!_vesselUsedInOperationYearly.Contains(_jobStat.VesselUsed))
                                        _vesselUsedInOperationYearly.Add(+_jobStat.VesselUsed);
                                }
                            }
                        }
                        else
                            proceed = false;
                    }

                    //perform yearly reports
                    if (projectCurrentDate.Day == 31 && projectCurrentDate.Month == 12 && projectCurrentDate.Hour == 23)
                    {
                        //calculate yearly fixed costs only if active
                        var iyearly = new Installyearly()
                        {
                            T_fuelcost = InstallVesselsList.GetTFuelCost(),
                            BaseCost = InstallBaseList.GetBaseAnnualCost(_basesUsedInOperationYearly),
                            Iteration = currentIteration,
                            TechCost = _reportLogic.CalcTechSalaryCostPeryear(_basesUsedInOperationYearly),
                            T_fixedcost = _reportLogic.CalcFixedVesselCostPeryear(false, 0, _vesselUsedInOperationYearly),
                            Year = yearcounter,
                            Total_install_cost = 0,
                            AdditionalCost = projectCurrentDate.Year == 2000 ? InstStrategyDetails.Installationcost : 0
                        };
                        InstallYearlyResultList.Add(iyearly);
                        yearcounter++;
                        //reset values 
                        _ = InstallVesselsList.ResetFuelHiredDaysCosts();
                        _basesUsedInOperationYearly = new List<string>();
                        _vesselUsedInOperationYearly = new List<int>();

                        if (InstallJobsList.CheckAllTasksCompleted())
                        {
                            UpdateAllStatus(100, projectCurrentDate.ToString(), true);
                            projectCurrentDate = projectEndDate;
                            break;
                        }

                    }
                    projectCurrentDate = projectCurrentDate.AddHours(1);



                };
                //close off all incidents that pending...
                //var getPendingIncidents = SimIncidentsList.GetAll().Where(i => i.SimStageId == simstageId && i.Status == Task_Status.Pending).OrderBy(d => d.IncidentTime).ToList();
                //if (getPendingIncidents.Any())
                //    FinalUpdatePendingIncidents(getPendingIncidents, projectEndDate);

                return true;
            }
            catch (Exception)
            {
                // log.Error(e);// log.Error(e);
            }
            //set simrun status to false 
            return false;
        }
        private bool ProcessSim()
        {
            try
            {
                if (Convert.ToInt32(PreventiveStartEnd.StartMonth) >= 0 && Convert.ToInt32(PreventiveStartEnd.EndMonth) > 0)
                {
                    _pmstart = (new DateTime(2000, Convert.ToInt32(PreventiveStartEnd.StartMonth), 01, 01, 00, 00) - new DateTime(2000, 01, 01, 01, 00, 00)).TotalHours;
                    _pmend = (new DateTime(2000, Convert.ToInt32(PreventiveStartEnd.EndMonth), 01, 01, 00, 00) - new DateTime(2000, 01, 01, 01, 00, 00)).TotalHours;

                    //same month
                    if (_pmstart == _pmend)
                    {
                        _pmend = _pmend + 672;
                    }

                }

                //init
                InitAllList(false);
                UpdateIterationStatusText($"Iteration {currentIteration}/" + ProjectDetails.NoOfIterations);

                bool addIte = SimIterationList.AddSimIterationResult(new SimIterationResult() { Id = currentIteration });

                UpdateAllStatus(10, $"preparing iteration({currentIteration}) simulation data...", false);

                #region add bases
                foreach (var _base in TotalBases.GetBases())
                {
                    var _inbase = new SimBasesDetails()
                    {
                        Annualcost = _base.Annualcost,
                        NoOfTechs = _base.NoOfTechs,
                        OriginalNoOfTechs = _base.NoOfTechs,
                        AnnualsalperTech = _base.AnnualsalperTech,
                        Basename = _base.Basename,
                        Distancetofarm = _base.Distancetofarm
                    };
                    SimBaseList.Add(_inbase);
                }
                #endregion

                #region add vessels

                UpdateAllStatus(11, $"iteration({currentIteration}) lining up vessels...", false);

                int _vId = 1;
                foreach (var _vessel in TotalVessels.GetVessels())
                {
                    bool _rented = !_vessel.Purchased;
                    bool hireasReq = _vessel.Hireasrequired.ToLower() == "yes";

                    for (int j = 0; j < _vessel.Number; j++)
                    {
                        var _simV = new SimVessel
                        {
                            Id = _vId,
                            Available = !_rented ? true : !hireasReq,
                            Rented = _rented,
                            HireAsReq = hireasReq,
                            LeadHrs = hireasReq ? _vessel.VesselLeadtime : 0,
                            OperationFrom = _rented == true ? (!hireasReq ? new DateTime(2000, _vessel.RentalStartMonth, _vessel.RentalStartDay) : DateTime.Now) : DateTime.Now,
                            OperationTo = _rented == true ? (!hireasReq ? new DateTime(2000, _vessel.RentalEndMonth, _vessel.RentalEndDay) : DateTime.Now) : DateTime.Now,
                            VesselType = _vessel.VesselClassif,
                            SimIterationId = currentIteration
                        };
                        SimVesselsList.Add(_simV);
                        _vId++;
                    }
                }
                #endregion

                #region add devices (Turbines)

                UpdateAllStatus(12, $"iteration({currentIteration}) lining up devices...", false);

                //calc PM start month hour and end month hour

                for (int j = 0; j < FarmDetails.NoOfDivices; j++)
                {
                    var _deviceObj = new SimDevice
                    {
                        Id = j + 1,
                        SimStageId = currentIteration
                    };
                    SimDeviceList.Add(_deviceObj);

                    #region create tasks
                    UpdateAllStatus(13, $"iteration({currentIteration}) calculating incidents...", false);
                    GenerateCMTasks(_deviceObj.Id, currentIteration);
                    //create PM tasks only if start and end month are mentioned
                    if (_pmstart >= 0 && _pmend > 0)
                    {
                        GeneratePMTasks(_deviceObj.Id, currentIteration, _pmstart, _pmend);
                    }

                    var testttt = SimIncidentsList.GetAll().Select(x => x.IncidentTime).ToList();

                    #endregion
                }

                #endregion

                if (ProcessSimStages(currentIteration))
                {
                    UpdateAllStatus(75, $"iteration({currentIteration}) preparing results...", false);
                    //set as complete
                    if (_reportLogic.PrepareSimresults(currentIteration))
                    {

                    }
                }
                UpdateAllStatus(100, "Simulation completed.", false);
                return true;
            }
            catch (Exception)
            {
                //close sim and display errors
            }
            return false;
        }
       
        #endregion

        #region check validations cont.

        private bool CheckProjectInputValdation()
        {
            var res = validCheck.CheckProjectValidation();
            if (res.Result)
                return true;
            else
            {

                UpdateErrormessageText(res.Message);
                return false;

            }
        }

        private bool CheckFarmInputValdation()
        {
            var res = validCheck.CheckFarmValidation();
            if (res.Result)
                return true;
            else
            {
                UpdateErrormessageText(res.Message);
                return false;
            }
        }

        private bool CheckResourceInputValdation()
        {
            var res = validCheck.CheckResourceValidation();
            if (res.Result)
                return true;
            else
            {
                UpdateErrormessageText(res.Message);
                return false;
            }
        }

        private bool CheckBasesInputValdation()
        {
            var res = validCheck.CheckBasesValidation();
            if (res.Result)
                return true;
            else
            {
                UpdateErrormessageText(res.Message);
                return false;
            }

        }

        private bool CheckVesselsInputValdation()
        {
            var res = validCheck.CheckVesselsValidation();
            if (res.Result)
                return true;
            else
            {
                UpdateErrormessageText(res.Message);
                return false;
            }
        }

        private bool CheckRepairsInputValdation()
        {

            var res = validCheck.CheckRepairsValidation();
            if (res.Result)
                return true;
            else
            {
                UpdateErrormessageText(res.Message);
                return false;
            }
            //Thread.Sleep(500);
            //return true;
        }

        private bool CheckInstallsInputValdation()
        {
            var res = validCheck.ChekInstallationValidation();
            if (res.Result)
                return true;
            else
            {
                UpdateErrormessageText(res.Message);
                return false;
            }
        }

        private bool CheckPrevMainInputValdation()
        {
            var res = validCheck.CheckPMValidation();
            if (res.Result)
                return true;
            else
            {
                UpdateErrormessageText(res.Message);
                return false;
            }
        }

        private bool CheckCompoInputValdation()
        {
            var res = validCheck.CheckComponentsValidation();
            if (res.Result)
                return true;
            else
            {
                UpdateErrormessageText(res.Message);
                return false;
            }
        }


        #endregion

        #region Crosschecks

        private async Task<bool> checkValidations()
        {
            try
            {
                #region check valdatoins

                statustext.Visible = true;
                UpdateAllStatus(0, "validating inputs...", true);

                #region ProjectValidation
                projectchecklbl.Visible = true;
                projectchecklbl.Text = "checking...";
                UpdateAllStatus(1, null, true);

                Task<bool> projectV = new Task<bool>(CheckProjectInputValdation);
                projectV.Start();
                if (await projectV)
                {
                    projectchecklbl.Text = "OK";
                }
                else
                {
                    projectchecklbl.Text = "Failed";
                    return false;
                }
                #endregion

                #region FarmValidation
                farmchecklbl.Visible = true;
                farmchecklbl.Text = "checking...";
                UpdateAllStatus(2, null, true);
                Task<bool> farmV = new Task<bool>(CheckFarmInputValdation);
                farmV.Start();
                if (await farmV)
                {
                    farmchecklbl.Text = "OK";
                }
                else
                {
                    farmchecklbl.Text = "Failed";
                    return false;
                }
                #endregion

                #region Resource Validation
                resourcechecklbl.Visible = true;
                resourcechecklbl.Text = "checking...";
                UpdateAllStatus(3, null, true);
                Task<bool> resV = new Task<bool>(CheckResourceInputValdation);
                resV.Start();
                if (await resV)
                {
                    resourcechecklbl.Text = "OK";
                }
                else
                {
                    farmchecklbl.Text = "Failed";
                    return false;
                }
                #endregion

                #region Bases Validation
                basechecklbl.Visible = true;
                basechecklbl.Text = "checking...";
                UpdateAllStatus(4, null, true);
                Task<bool> basesV = new Task<bool>(CheckBasesInputValdation);
                basesV.Start();
                if (await basesV)
                {
                    basechecklbl.Text = "OK";
                }
                else
                {
                    basechecklbl.Text = "Failed";
                    return false;
                }
                #endregion

                #region Vessels Validation
                vesselchecklbl.Visible = true;
                vesselchecklbl.Text = "checking...";
                UpdateAllStatus(5, null, true);
                Task<bool> vesselsV = new Task<bool>(CheckVesselsInputValdation);
                vesselsV.Start();
                if (await vesselsV)
                {
                    vesselchecklbl.Text = "OK";
                }
                else
                {
                    vesselchecklbl.Text = "Failed";
                    return false;
                }
                #endregion

                #region preventM Validation
                preventativechecklbl.Visible = true;
                preventativechecklbl.Text = "checking...";
                UpdateAllStatus(7, null, true);
                Task<bool> preM_V = new Task<bool>(CheckPrevMainInputValdation);
                preM_V.Start();
                if (await preM_V)
                {
                    preventativechecklbl.Text = "OK";
                }
                else
                {
                    preventativechecklbl.Text = "Failed";
                    return false;
                }
                #endregion

                #region repairs Validation
                repairchecklbl.Visible = true;
                repairchecklbl.Text = "checking...";
                UpdateAllStatus(8, null, true);
                Task<bool> repairsV = new Task<bool>(CheckRepairsInputValdation);
                repairsV.Start();
                if (await repairsV)
                {
                    repairchecklbl.Text = "OK";
                }
                else
                {
                    repairchecklbl.Text = "Failed";
                    return false;
                }
                #endregion

                #region compo Validation
                componentchecklbl.Visible = true;
                componentchecklbl.Text = "checking...";
                UpdateAllStatus(9, null, true);
                Task<bool> compoV = new Task<bool>(CheckCompoInputValdation);
                compoV.Start();
                if (await compoV)
                {
                    componentchecklbl.Text = "OK";
                }
                else
                {
                    componentchecklbl.Text = "Failed";
                    return false;
                }
                #endregion

                UpdateAllStatus(0, "validation completed...", true);
                return true;
                #endregion
            }
            catch (Exception)
            {

                // log.Error(e);
            }
            return false;
        }

        public bool ProcessSimStages(int simstageId)
        {
            SimJobs = new List<SimJobData>();
            List<VesselDetails> VesselInfo = new List<VesselDetails>();
            projectStartDate = new DateTime(2000, 1, 1, 0, 0, 0);
            projectCurrentDate = new DateTime(2000, 1, 1, 0, 0, 0);
            //yearly reports
            DateTime yearEnd = projectStartDate.AddYears(1);

            try
            {
                UpdateAllStatus(15, "Iteration 1 simulation started...", false);
                //set end date
                projectEndDate = new DateTime(projectStartDate.Year + (ProjectDetails.ProjectLifeTime - 1), 12, 31, 23, 0, 0);

                //for percentage fill up
                var totaldays = (projectEndDate - projectStartDate).TotalDays;

                //add shift if not exists
                CreateDefaultShit(false);
                while (projectCurrentDate <= projectEndDate)
                {
                    if (projectCurrentDate.Year == 2025)
                    {

                    }

                    int _testperc = 15 + (60 - Convert.ToInt32((projectEndDate - projectCurrentDate).TotalDays * 60 / totaldays));
                    UpdateAllStatus(_testperc, projectCurrentDate.ToString(), false);
                    //reset incidents
                    daywiseIncidents = new List<SimIncidents>();
                    //start simulate shift by shift
                    List<Shifts> todaysShifts = CreateShifts(projectCurrentDate);

                    foreach (var shift in todaysShifts)
                    {
                        DateTime shiftStart = projectCurrentDate.Add(shift.Start.TimeOfDay);
                        DateTime shiftEnd = projectCurrentDate.Add(shift.End.TimeOfDay);

                        //remove incidents if required by force
                        if (deletedIncidents.Count > 0)
                        {
                            bool clean = true;
                            foreach (var item in deletedIncidents)
                            {
                                if (!SimIncidentsList.SetDelete(item))
                                    clean = false;
                            }
                            if (clean)
                                deletedIncidents = new List<string>();
                        }


                        //get incidents of this shift
                        //TODO: also order by priority when levels are set
                        var testIncis = SimIncidentsList.GetAll().OrderBy(d => d.IncidentTime).ToList();

                        var getnextIncidents = SimIncidentsList.GetAll().Where(i => i.SimStageId == simstageId && (i.Status != Task_Status.Completed) && i.IncidentTime < shiftStart).OrderBy(d => d.IncidentTime).ThenBy(workstatus => workstatus.Status).ThenBy(t => t.Type).ToList();
                        if (getnextIncidents.Any())
                        {
                            #region Release or ADD Vessel and Techs based on task status
                            foreach (var device in SimDeviceList.GetAll().Where(c => c.OnTaskFrom.HasValue && c.SimStageId == simstageId).ToList())
                            {
                                if (device.OnTaskT0 != null && device.OnTaskT0 < shiftStart)
                                {
                                    device.OnTaskFrom = null;
                                    device.OnTaskT0 = null;
                                    SimDeviceList.Update(device.Id, device);
                                }
                            }
                            foreach (var deployedVessel in SimVesselsList.GetAll().Where(c => c.SimIterationId == simstageId).ToList())
                            {
                                if (deployedVessel.OnTaskT0 != null)
                                {
                                    ReleaseTechsVessels(shiftStart, deployedVessel, true);
                                }
                            }
                            #endregion

                            foreach (var incident in getnextIncidents)
                            {

                                var checkDeviceUnderRepair = SimDeviceList.GetAll().Where(x => x.SimStageId == simstageId && x.Id == incident.SimTurbinesId && x.OnTaskFrom.HasValue).FirstOrDefault();
                                if (checkDeviceUnderRepair == null)
                                {
                                    //firstitme incidents
                                    if (string.IsNullOrEmpty(incident.RootId))
                                    {
                                        var underPendinglist = SimIncidentsList.GetAll().Where(i => i.SimTurbinesId == incident.SimTurbinesId && i.IncidentTime < incident.IncidentTime && i.FinalFixed > incident.IncidentTime).ToList().Count;
                                        //check 1: 
                                        if (underPendinglist > 0)
                                        {
                                            //delete this incident
                                            if (!deletedIncidents.Contains(incident.Id))
                                                deletedIncidents.Add(incident.Id);
                                        }
                                        else
                                            daywiseIncidents.Add(incident);
                                    }
                                    else //incomplete incidents
                                        daywiseIncidents.Add(incident);
                                }
                            }
                            if (daywiseIncidents.Any())
                            {
                                try
                                {
                                    foreach (var inci in daywiseIncidents.ToList())
                                    {
                                        if (inci.Status != Task_Status.Completed)
                                        {
                                            if (deletedIncidents.Contains(inci.Id))
                                                continue;


                                            #region Release or ADD Vessel and Techs based on task status
                                            foreach (var deployedVessel in SimVesselsList.GetAll().Where(c => c.SimIterationId == simstageId).ToList())
                                            {
                                                if (deployedVessel.OnTaskFrom.HasValue)
                                                {
                                                    if (deployedVessel.OnTaskT0 != null)
                                                    {
                                                        ReleaseTechsVessels(shiftStart, deployedVessel, true);
                                                    }
                                                }

                                            }
                                            #endregion
                                            RepairDetails _rep = new RepairDetails();
                                            VesselInfo vess = new VesselInfo();

                                            #region check if vessel and techs are avaialable for this incident
                                            //get repair first
                                            if (inci.Type == TaskType.Corrective)
                                                _rep = TotalRepairs.GetObjByName(inci.Repair);
                                            else
                                            {
                                                var prev = TotalPriventives.GetAllPriventives().Where(x => x.PMCategory == inci.Repair).FirstOrDefault();
                                                _rep = ConvertPMMaintenanceToRepairModel(prev); //TODO: mix both repair and preve task together
                                                if (_rep.Operationlocation == OperationalLocation.Offshore || inci.Stage == Task_Stage.OnshoreStage1)
                                                {
                                                    if (!PMTaskInSeason(shiftStart))
                                                        continue;
                                                }
                                            }

                                            if (_rep == null)
                                                continue;

                                            if ((_rep.Operationlocation == OperationalLocation.Offshore) || (_rep.Operationlocation == OperationalLocation.Onshore && inci.Stage != Task_Stage.OnshoreStage2))
                                            {
                                                //no vessel = break
                                                if (!SimVesselsList.CheckIfAnyVesselAvailable(projectCurrentDate, simstageId, _rep.Vesselrequired))
                                                {
                                                    //trigger lead time if applicable
                                                    SimVesselsList.TriggerLeadTimeForVessel(shiftStart, _rep.Vesselrequired);
                                                    continue;
                                                }

                                                vess = CheckBasicForVesselTechsAvail(_rep, simstageId);
                                                if (!vess.Proceed)
                                                    continue;
                                                else
                                                {
                                                    //make sure shift starts at 7am to 7pm
                                                    if (vess.NightWork.ToLower() == "no" && shiftStart != new DateTime(shiftStart.Year, shiftStart.Month, shiftStart.Day, 7, 0, 0))
                                                        continue;
                                                }
                                            }
                                            #endregion
                                            FinalChecks(inci, shiftStart, shiftEnd, vess);
                                        }


                                    }
                                }
                                catch (Exception)
                                {

                                    // log.Error(e);
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
                var getPendingIncidents = SimIncidentsList.GetAll().Where(i => i.SimStageId == simstageId && i.Status != Task_Status.Completed).OrderBy(d => d.IncidentTime).ToList();
                if (getPendingIncidents.Any())
                    FinalUpdatePendingIncidents(getPendingIncidents, projectEndDate);
                return true;
            }
            catch (Exception)
            {
                // log.Error(e);// log.Error(e);
            }
            //set simrun status to false 
            return false;
        }

        private void ReleaseTechsVessels(DateTime shiftStart, SimVessel deployedVessel, bool ome)
        {
            if (deployedVessel.OnTaskT0 < shiftStart)
            {
                if (ome)
                {
                    //release techs from base
                    var _deployedBase = SimBaseList.GetObjByName(deployedVessel.Base);
                    if (_deployedBase != null)
                    {
                        _deployedBase.NoOfTechs += deployedVessel.CarryingTechs;
                        //update base
                        SimBaseList.Update(_deployedBase.Basename, _deployedBase);
                    }

                    //reset vessel and tech
                    deployedVessel.OnTaskFrom = null;
                    deployedVessel.OnTaskT0 = null;


                    if (deployedVessel.HireAsReq)
                    {
                        // if hiretriggered is false that means hired vessel period is finished
                        if (deployedVessel.HireTriggered)
                            deployedVessel.Available = true;
                        else
                            deployedVessel.Available = false;
                    }
                    else
                        deployedVessel.Available = true;

                    if (!deployedVessel.Rented)
                        deployedVessel.Available = true;


                    deployedVessel.Base = "";
                    deployedVessel.CarryingTechs = 0;
                    SimVesselsList.Update(deployedVessel.Id, deployedVessel);
                }
            }

        }

        private List<Shifts> CreateShifts(DateTime projectCurrentDate)
        {
            List<Shifts> result = new List<Shifts>();
            if (TotalVessels.GetVessels().Where(x => x.NightWork.ToLower() == "yes").FirstOrDefault() != null)
            {
                for (int i = 0; i < 23; i++)
                {
                    var _shift = new Shifts
                    {
                        Start = new DateTime(projectCurrentDate.Year, projectCurrentDate.Month, projectCurrentDate.Day, i, 0, 0),
                        End = new DateTime(projectCurrentDate.Year, projectCurrentDate.Month, projectCurrentDate.Day, i + 1, 0, 0)
                    };
                    result.Add(_shift);
                }
            }
            else
            {
                var _shift = new Shifts
                {
                    Start = new DateTime(projectCurrentDate.Year, projectCurrentDate.Month, projectCurrentDate.Day, 7, 0, 0),
                    End = new DateTime(projectCurrentDate.Year, projectCurrentDate.Month, projectCurrentDate.Day, 19, 0, 0)
                };
                result.Add(_shift);
            }
            return result;
        }

        private bool PMTaskInSeason(DateTime? incidentTime)
        {
            DateTime inciMonth = (DateTime)incidentTime;

            bool result;
            if (Convert.ToInt32(PreventiveStartEnd.StartMonth) <= inciMonth.Month && Convert.ToInt32(PreventiveStartEnd.EndMonth) >= inciMonth.Month)
                result = true;
            else
                result = false;

            //test
            if ((inciMonth.Month > Convert.ToInt32(PreventiveStartEnd.EndMonth) || inciMonth.Month < Convert.ToInt32(PreventiveStartEnd.StartMonth)) && result)
            {

            }
            return result;

        }

        private void CreateDefaultShit(bool fullday)
        {
            //create default shit
            var _shift = new Shifts
            {
                Start = new DateTime(2000, 01, 01, 07, 00, 00),
                End = !fullday ? new DateTime(2000, 01, 01, 19, 00, 00) : new DateTime(2000, 01, 02, 07, 00, 00),
            };
            AllShifts.Add(_shift);
        }

        private void FinalChecks(SimIncidents incident, DateTime shiftStart, DateTime shiftEnd, VesselInfo vess)
        {
            #region Inits
            double repairToPerformDuration = 0, minrepairHoursCanbeperformed = 0;
            string CompOrDeviceName = "";
            //save cross checking list data in _job
            SimJobData _job = new SimJobData();
            SimBasesDetails _base = new SimBasesDetails();
            RepairDetails repairToPerform = null;
            PriventiveDetails prev = null;
            DateTime shiftStartIncaseMidDayRepair = shiftStart;
            double speedLimit = 0;
            TimeSpan journeyTime;
            int reqTechs = 0;
            #endregion

            SimDevice device = SimDeviceList.GetAll().Where(x => x.Id == incident.SimTurbinesId && x.SimStageId == incident.SimStageId).FirstOrDefault();

            #region GET repair, base and Comp
            if (incident.Type == TaskType.Corrective)
                repairToPerform = TotalRepairs.GetObjByName(incident.Repair);
            else
            {
                prev = TotalPriventives.GetAllPriventives().Where(x => x.PMCategory == incident.Repair).FirstOrDefault();
                repairToPerform = ConvertPMMaintenanceToRepairModel(prev); //TODO: mix both repair and preve task together
            }

            if (repairToPerform == null)
                return;

            if (repairToPerform.Operationlocation == OperationalLocation.Offshore)
                repairToPerformDuration = (double)repairToPerform.OperationdurationOffshore;
            else
            {
                if (incident.Stage == Task_Stage.OnshoreStage1 || incident.Stage == Task_Stage.OnshoreStage3)
                    repairToPerformDuration = (double)repairToPerform.OperationdurationOffshore;
                else
                    repairToPerformDuration = (double)repairToPerform.OperatondurationOnshore;

            }

            if (!string.IsNullOrEmpty(repairToPerform.Base))
            {
                _base = SimBaseList.GetObjByName(repairToPerform.Base);
                if (_base == null)
                    return;
            }
            else
                return;


            //component info
            if (incident.Type == TaskType.Corrective)
            {
                var componentObj = TotalComponents.GetObjByName(incident.Component);
                if (componentObj != null)
                    CompOrDeviceName = componentObj.Componentname;
            }
            else
            {
                CompOrDeviceName = "PM " + prev.PMCategory + "On Device " + incident.SimTurbinesId;
            }
            #endregion


            #region test and should be deleted when completed
            if (!string.IsNullOrEmpty(incident.RootId))
            {
                if (SimIncidentsList.GetAll().Where(x => x.RootId == incident.RootId).Count() > 20)
                {
                    int ttt = SimIncidentsList.GetAll().Where(x => x.RootId == incident.RootId).Count();
                }
            }
            #endregion







            _job.SimIncidentsId = incident.Id;

            _job.IncidentTime = incident.IncidentTime;

            #region if repair is onshore maintainance work (stage two), just set as completed
            if (repairToPerform.Operationlocation == OperationalLocation.Onshore && incident.Stage == Task_Stage.OnshoreStage2)
            {
                _job.Completed = true;
                _job.DelayedHours = (double)repairToPerform.OperatondurationOnshore;
                _job.WorkedHours = (double)repairToPerform.OperatondurationOnshore;
                _job.FullRepair = true;
                _job.RepairStarted = incident.IncidentTime;
                _job.RepairEnded = ((DateTime)incident.IncidentTime).AddHours((double)repairToPerform.OperatondurationOnshore);
                _job.DelayedHours = ((DateTime)_job.RepairEnded - (DateTime)incident.IncidentTime).TotalHours;

                incident.IncidentFixed = ((DateTime)incident.IncidentTime).AddHours((double)repairToPerform.OperatondurationOnshore);
                incident.Status = Task_Status.Completed;
                SimIncidentsList.Update(incident.Id, incident);

                AddJobdata(_job);

                //set device is under repair
                device.OnTaskFrom = shiftStart;
                device.OnTaskT0 = ((DateTime)incident.IncidentTime).AddHours((double)repairToPerform.OperatondurationOnshore);
                SimDeviceList.Update(device.Id, device);

                //create new incident to reinstall
                //create new task for stage 3
                var __simInci = new SimIncidents
                {
                    SimComponentId = incident.SimComponentId,
                    Stage = Task_Stage.OnshoreStage3,
                    Repair = incident.Repair,
                    IncidentTime = ((DateTime)incident.IncidentTime).AddHours((double)repairToPerform.OperatondurationOnshore),
                    SimStageId = incident.SimStageId,
                    Status = Task_Status.ToCompFullRepair,
                    Priority = PriorityLevels.Level1,
                    SimTurbinesId = incident.SimTurbinesId,
                    Component = incident.Component,
                    RootId = incident.RootId,
                    Type = incident.Type,
                };
                SimIncidentsList.Add(__simInci);

                return;
            }
            #endregion

            #region normal corrective process of performing job

            if (repairToPerform.Operationlocation == OperationalLocation.Onshore && incident.Stage == Task_Stage.OnshoreStage3)
            {
                repairToPerformDuration = (double)repairToPerform.OperationdurationOffshore;
            }

            try
            {
                if (vess.Proceed)
                {
                    speedLimit = vess.Speed * 1.852;

                    //here find how many hours worked on this incident before
                    if (!string.IsNullOrEmpty(incident.RootId))
                    {
                        var _wokredlist = SimIncidentsList.GetAll().Where(x => x.RootId == incident.RootId && x.WorkedHours > 0).ToList();
                        double _alreadyworkedhours = 0;
                        if (_wokredlist.Any())
                        {
                            foreach (var _wh in _wokredlist)
                            {
                                _alreadyworkedhours += _wh.WorkedHours;
                            }
                            repairToPerformDuration -= _alreadyworkedhours;
                        }
                    }

                    #region Distance
                    double _distanceTOFarm = _base.Distancetofarm;
                    #endregion

                    #region Time to travel 
                    _job.TotalTimeToTravel = (speedLimit > 0 ? ((_distanceTOFarm / speedLimit) * 2) : 0); //2 implies to and fro
                    #endregion

                    #region Timeconstraints
                    //calculate maxworkhouts
                    _job.MaxWorkHours = Math.Abs(_job.HoursAvailInShift - _job.TotalTimeToTravel);

                    TimeSpan _reqworkHours = TimeSpan.FromHours(repairToPerformDuration);

                    double maxsafehoursReq = repairToPerformDuration + _job.TotalTimeToTravel;
                    double hoursToFix = repairToPerformDuration + (_job.TotalTimeToTravel / 2);

                    #endregion

                    #region Shift Info
                    //check vessel and technicians req
                    if (vess.NightWork.ToLower() == "yes")
                        shiftEnd = shiftStart.AddHours(repairToPerformDuration);
                    else
                        shiftEnd = new DateTime(shiftStart.Year, shiftStart.Month, shiftStart.Day, 19, 0, 0);

                    if (incident.IncidentTime > shiftStart)
                    {
                        _job.HoursAvailInShift = (shiftEnd - (DateTime)incident.IncidentTime).TotalHours;
                        _job.IdleHoursInShift = ((DateTime)incident.IncidentTime - shiftStart).TotalHours;
                        shiftStartIncaseMidDayRepair = shiftStart.AddHours(_job.IdleHoursInShift);
                    }
                    else
                    {
                        _job.HoursAvailInShift = (shiftEnd - shiftStart).TotalHours;
                        _job.IdleHoursInShift = 0;
                    }
                    #endregion


                    #region Wave, Wind limits, techs and vessel availability

                    //check vessel and technicians req
                    if (vess.NightWork.ToLower() == "yes")
                    {
                        if (maxsafehoursReq > 12 || Convert.ToInt32(shiftEnd.ToString("HH")) > 19)
                        {
                            if (repairToPerform.NoOfTechs * 2 <= _base.NoOfTechs)
                            {
                                reqTechs = repairToPerform.NoOfTechs * 2;
                            }
                            else
                                return;
                        }
                        else
                        {
                            if (repairToPerform.NoOfTechs <= _base.NoOfTechs)
                            {
                                reqTechs = repairToPerform.NoOfTechs;
                            }
                            else
                                return;
                        }
                    }
                    else
                    {
                        if (repairToPerform.NoOfTechs <= _base.NoOfTechs)
                        {
                            reqTechs = repairToPerform.NoOfTechs;
                        }
                        else
                            return;
                    }


                    double _waveLimit = vess.WaveHeightLimit;
                    var vessDoubleCheck = PickVesselForOM(repairToPerform, maxsafehoursReq);
                    if (!vessDoubleCheck.Proceed)
                        return;

                    #endregion

                    #region WW Check

                    //check WW
                    WWShift ww = CheckWeatherConditions(shiftStartIncaseMidDayRepair, shiftEnd, _waveLimit, maxsafehoursReq, _job.TotalTimeToTravel, vess.NightWork.ToLower() == "yes");

                    if (ww != null)
                    {
                        if (!ww.Result)
                            return;
                        else if (ww.Result && ww.FullRepairHours.CanTravel) //full required hours to fix is doable
                            _job.FullRepair = true;
                        else if (ww.Result && !ww.FullRepairHours.CanTravel && ww.MinRepairHours.CanTravel) //do minimum work is possible
                        {
                            if (vess.NightWork.ToLower() == "yes")
                                return;

                            if (shiftStart != shiftStartIncaseMidDayRepair)
                                return;

                            //return as min repair 
                            minrepairHoursCanbeperformed = Math.Abs(ww.MinRepairHours.Safehours - _job.TotalTimeToTravel);

                            //check pick what is possible to perform
                            if (_job.MaxWorkHours < minrepairHoursCanbeperformed)
                                minrepairHoursCanbeperformed = _job.MaxWorkHours;
                            _job.FullRepair = false;
                            _job.MinRepair = true;
                        }

                    }
                    else
                        return;
                    #endregion

                    #region Check Full or Min repair with other info


                    if (_job.MaxWorkHours > 0)
                    {
                        journeyTime = TimeSpan.FromHours(_distanceTOFarm / speedLimit);

                        if (_job.FullRepair) //above WW will decide full or min repair first
                        {
                            _job.Completed = true;
                            _job.RepairEnded = shiftStartIncaseMidDayRepair.AddHours(_distanceTOFarm / speedLimit).AddHours(repairToPerformDuration);
                            _job.FullRepair = true;
                            _job.DelayedHours = ((DateTime)_job.RepairEnded - (DateTime)incident.IncidentTime).TotalHours;
                            _job.WorkedHours = repairToPerformDuration;
                        }
                        else if (_job.MinRepair)
                        {
                            _job.Completed = true;
                            _job.RepairEnded = shiftStartIncaseMidDayRepair.AddHours(_distanceTOFarm / speedLimit).AddHours(minrepairHoursCanbeperformed);
                            _job.WorkedHours = minrepairHoursCanbeperformed;
                            _job.FullRepair = false;
                        }

                        //jobs data
                        _job.DistanceTravelled = _distanceTOFarm;
                        _job.IncidentTime = incident.IncidentTime;
                        _job.JourneyStart = shiftStartIncaseMidDayRepair;
                        _job.JourneyEnd = shiftStartIncaseMidDayRepair.AddHours(_distanceTOFarm / speedLimit);
                        _job.RepairStarted = shiftStartIncaseMidDayRepair.AddHours(_distanceTOFarm / speedLimit);
                        _job.StandbyTime = ((DateTime)_job.RepairEnded - (DateTime)_job.RepairStarted).TotalHours;
                        _job.OperationalTime = ((DateTime)_job.JourneyEnd - (DateTime)_job.JourneyStart).TotalHours * 2;
                        _job.SimVesselsId = vess.SimVesselsId;

                        //set device is under repair
                        device.OnTaskFrom = shiftStart;
                        device.OnTaskT0 = (DateTime)_job.RepairEnded;
                        SimDeviceList.Update(device.Id, device);

                        //book vessel till job is completed
                        //set vessel as used
                        var usedV = SimVesselsList.GetAll().Where(x => x.Id == vess.SimVesselsId).FirstOrDefault();
                        usedV.Available = false;
                        usedV.Base = _base.Basename;
                        usedV.OnTaskFrom = shiftStartIncaseMidDayRepair;
                        usedV.OnTaskT0 = ((DateTime)_job.RepairEnded).AddHours(_job.TotalTimeToTravel / 2);

                        if (usedV.Rented && usedV.HireAsReq)
                        {
                            //keep the vessel hired if any pending tasks exists
                            //check corrective first
                            if (CheckNearByPeningTasks(((DateTime)_job.RepairEnded).AddHours(_job.TotalTimeToTravel / 2)))
                                usedV.HireTriggered = true;
                            else
                                usedV.HireTriggered = false;

                            double hiredDays = (shiftEnd - shiftStart).TotalDays < 1 ? 1 : (shiftEnd - shiftStart).TotalDays;

                            //init 
                            if (usedV.HiredAsReqByYears == null)
                                usedV.HiredAsReqByYears = new List<HiredAsReqByYear>();
                            //add no of days hired in the years slot
                            var _findYitem = usedV.HiredAsReqByYears.Where(x => x.Year == shiftEnd.Year).FirstOrDefault();
                            if (_findYitem != null)
                                _findYitem.NoOfDaysHired += hiredDays;
                            else
                            {
                                usedV.HiredAsReqByYears.Add(new HiredAsReqByYear() { NoOfDaysHired = hiredDays, Year = shiftEnd.Year });
                            }
                        }
                        SimVesselsList.Update(usedV.Id, usedV);
                    }
                    else
                        return;
                    //perform next shift when activated. For now only one shift
                    #endregion
                }
                else
                    return;

                if (_job.FullRepair)
                {
                    //check if repair is onshore and requires next stage maintenance
                    if (repairToPerform.Operationlocation == OperationalLocation.Onshore && incident.Stage == Task_Stage.OnshoreStage1)
                    {
                        //check what stage is present repair completed
                        //create new task for stage 2
                        var __simInci = new SimIncidents
                        {
                            Type = incident.Type,
                            SimComponentId = incident.SimComponentId,
                            Stage = Task_Stage.OnshoreStage2,
                            Repair = incident.Repair,
                            IncidentTime = ((DateTime)_job.RepairEnded).AddHours(_job.TotalTimeToTravel / 2),
                            SimStageId = incident.SimStageId,
                            Status = Task_Status.NotStarted,
                            Priority = PriorityLevels.Level1,
                            SimTurbinesId = incident.SimTurbinesId,
                            RootId = string.IsNullOrEmpty(incident.RootId) ? incident.Id : incident.RootId,
                            Component = incident.Component
                        };

                        SimIncidentsList.Add(__simInci);
                    }

                    incident.IncidentFixed = (DateTime)_job.RepairEnded;
                    incident.Status = Task_Status.Completed;

                    if (string.IsNullOrEmpty(incident.RootId))
                        incident.FinalFixed = (DateTime)_job.RepairEnded;
                    else
                        SimIncidentsList.SetFinalComplete(incident.RootId, _job.RepairEnded);


                    AddJobdata(_job);
                }
                else //specifiy how many hours still required to complete the task
                {
                    incident.Status = Task_Status.Completed;
                    incident.WorkedHours = _job.WorkedHours;
                    incident.IncidentFixed = (DateTime)_job.RepairEnded;
                    SimIncidentsList.Update(incident.Id, incident);

                    //add new incident to complete full repair
                    var __simInci = new SimIncidents
                    {
                        Type = incident.Type,
                        SimComponentId = incident.SimComponentId,
                        Stage = incident.Stage,
                        Repair = incident.Repair,
                        IncidentTime = (DateTime)_job.RepairEnded,
                        SimStageId = incident.SimStageId,
                        Status = Task_Status.ToCompFullRepair,
                        Priority = PriorityLevels.Level1,
                        SimTurbinesId = incident.SimTurbinesId,
                        RootId = string.IsNullOrEmpty(incident.RootId) ? incident.Id : incident.RootId,
                        Component = incident.Component
                    };
                    SimIncidentsList.Add(__simInci);
                    AddJobdata(_job);
                }
            }
            catch (Exception)
            {
                // log.Error(e);// log.Error(e);
            }
            #endregion


        }

        private bool CheckNearByPeningTasks(DateTime thisshiftEnd)
        {
            //check CM first
            var getCMnextIncidents = SimIncidentsList.GetAll().Where(i => (i.Status != Task_Status.Completed) && i.IncidentTime < thisshiftEnd && i.Type == TaskType.Corrective).OrderBy(d => d.IncidentTime).ThenBy(t => t.Type).ToList();
            if (getCMnextIncidents.Any())
                return true;
            else
            {
                //check PM
                var getPMnextIncidents = SimIncidentsList.GetAll().Where(i => (i.Status != Task_Status.Completed) && i.IncidentTime < thisshiftEnd && i.Type == TaskType.Preventative).OrderBy(d => d.IncidentTime).ThenBy(t => t.Type).ToList();
                if (getPMnextIncidents.Any())
                {
                    //if present shift in season and there are PM tasks pending means vessel should stay
                    if (PMTaskInSeason(thisshiftEnd))
                        return true;
                }
            }
            return false;
        }

        private CompInstallJobFeedback CompleteInstallJob(List<InstallJobs> jobsData, DateTime shiftStart, DateTime shiftEnd, int noOfTechsAvail)
        {
            CompInstallJobFeedback res = new CompInstallJobFeedback();
            try
            {
                if (!jobsData.Any())
                {
                    res.Status = Install_Status.Failed;
                    return res;
                }
                InstallJobs jobData = jobsData[0];

                res.Status = Install_Status.Failed;
                double repairToPerformDuration = 0;
                string CompOrDeviceName = jobData.Job;

                InstallJobData _job = new InstallJobData();
                InstallationDetails repairToPerform = jobData.InstallData;

                #region if repair is empty
                if (repairToPerform == null)
                {
                    _job.WorkedHours = 0;
                    AddInstallJobdata(_job);
                    res.Status = Install_Status.Failed;
                    return res;
                }
                #endregion

                DateTime shiftStartIncaseMidDayRepair = shiftStart;
                //increase time based on actual no of devices to be carried out by vessel
                repairToPerformDuration = repairToPerform.OperationDuration * (jobData.InstallData.Numberofdevicespervessel > 1 ? jobsData.Count : 1);

                _job.InstallId = jobData.Id;
                _job.ShiftStart = shiftStart;

                #region pick vessel based on install
                var vesselInfo = PickVesselForInstall(jobData, repairToPerformDuration);

                if (vesselInfo == null || !vesselInfo.Proceed)
                {
                    _job.WorkedHours = 0;
                    AddInstallJobdata(_job);
                    res.Status = Install_Status.ToCompFullRepair;
                    return res;
                }

                //set vessel as used
                var usedV = InstallVesselsList.GetAll().Where(x => x.Id == vesselInfo.SimVesselsId).FirstOrDefault();

                #endregion


                try
                {
                    double speedLimit = 0, fuelUsageT = 0, fuelUsageS = 0, fuelCost = 0;
                    TimeSpan journeyTime, journeyBtwDevices;

                    #region Shift Info

                    //act start of the shift as starting point if repair is in past, 
                    //else check when repair occurs and then only start the working on it during the shift

                    if (jobData.IncidentTime > shiftStart)
                    {
                        _job.HoursAvailInShift = (shiftEnd - (DateTime)jobData.IncidentTime).TotalHours;
                        _job.IdleHoursInShift = ((DateTime)jobData.IncidentTime - shiftStart).TotalHours;
                        shiftStartIncaseMidDayRepair = shiftStart.AddHours(_job.IdleHoursInShift);
                    }
                    else
                    {
                        _job.HoursAvailInShift = (shiftEnd - shiftStart).TotalHours;
                        _job.IdleHoursInShift = 0;
                    }
                    #endregion

                    #region SpeedLimit
                    //check if farm speed limit is less than vessel speed
                    speedLimit = vesselInfo.Speed;
                    //speed Knots into km: because distance is in km
                    speedLimit *= 1.852;
                    #endregion

                    #region Distance
                    //get selected base
                    var selBase = InstallBaseList.GetObjByName(repairToPerform.Base);

                    if (selBase != null)
                    {
                        //TODO: check if base exists
                    }
                    double _distanceTOFarm = selBase.Distancetofarm;
                    #endregion

                    #region Time to travel 
                    _job.TotalTimeToTravel = (speedLimit > 0 ? ((_distanceTOFarm / speedLimit) * 2) : 0); //2 implies to and fro
                    double maxsafehoursReq = repairToPerformDuration + _job.TotalTimeToTravel;
                    double hoursToFix = repairToPerformDuration + (_job.TotalTimeToTravel / 2);
                    #endregion

                    #region Timeconstraints
                    //calculate maxworkhouts
                    _job.MaxWorkHours = Math.Abs(_job.HoursAvailInShift - _job.TotalTimeToTravel);

                    //here find how many hours worked on this incident before
                    var _wokredlist = InstallJobsDataList.GetAll().Where(x => x.InstallId == _job.InstallId && x.WorkedHours > 0 && x.NoOfAttempts == 0).ToList();

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



                    #endregion

                    #region Wave and Wind limits
                    double _waveLimit = vesselInfo.WaveHeightLimit;
                    #endregion

                    #region check rental period
                    if (!vesselInfo.Purchased && vesselInfo.RentalEnd < shiftStartIncaseMidDayRepair.AddHours(maxsafehoursReq))
                    {
                        //NV
                        _job.WorkedHours = 0;
                        AddInstallJobdata(_job);
                        res.Status = Install_Status.ToCompFullRepair;
                        return res;
                    }
                    #endregion


                    #region WW Check

                    //check WW
                    var ww = CheckWeatherConditionsForInstall(shiftStartIncaseMidDayRepair, shiftEnd, _waveLimit, maxsafehoursReq);
                    if (ww != null)
                    {
                        if (!ww.Result)
                        {
                            //fill WW data for ref
                            AddInstallJobdata(_job);

                            res.Status = Install_Status.ToCompFullRepair;
                            return res;
                        }
                        else
                        {
                            if (repairToPerform.NoOftechsReq <= noOfTechsAvail)
                            {
                                //sigle shift or rotation shift based on hours it takes to fix and tech availability
                                if (maxsafehoursReq <= 12)
                                {
                                    _job.SingleShift = true;
                                }
                                else
                                {
                                    if (repairToPerform.NoOftechsReq * 2 <= noOfTechsAvail)
                                    {
                                        _job.RotationShift = true;
                                    }
                                    else
                                    {
                                        _job.SingleShift = true;
                                    }
                                }

                            }
                            else
                            {
                                _job.WorkedHours = 0;
                                AddInstallJobdata(_job);
                                res.Status = Install_Status.Failed;
                                return res;
                            }
                        }
                    }
                    else
                    {
                        //fill WW data for ref
                        _job.WorkedHours = 0;
                        AddInstallJobdata(_job);
                        res.Status = Install_Status.ToCompFullRepair;
                        return res;
                    }
                    #endregion

                    #region Perform job

                    if (!_job.SingleShift && !_job.RotationShift)
                    {
                        //fill WW data for ref
                        _job.WorkedHours = 0;
                        AddInstallJobdata(_job);
                        res.Status = Install_Status.Failed;
                        return res;
                    }

                    //fuel consumption
                    fuelCost = vesselInfo.FuelCost;
                    fuelUsageS = vesselInfo.FuelUsage;
                    fuelUsageT = vesselInfo.FuelUsage;
                    journeyTime = TimeSpan.FromHours(_distanceTOFarm / speedLimit);
                    journeyBtwDevices = TimeSpan.FromHours(FarmDetails.Distancebetweendevices / speedLimit);

                    int jobIndex = 0;
                    double tfuelcost = 0;
                    foreach (var singleJob in jobsData)
                    {
                        var _performedJob = new InstallJobData();

                        _performedJob.InstallId = singleJob.Id;
                        _performedJob.ShiftStart = shiftStart;
                        _performedJob.Completed = true;
                        //repair start and finish info
                        DateTime RepairStartingPoint = shiftStartIncaseMidDayRepair.AddHours(_distanceTOFarm / speedLimit);

                        if (jobIndex > 0)
                            RepairStartingPoint = ((DateTime)jobsData[jobIndex - 1].CompletedDate).Add(journeyBtwDevices);

                        _performedJob.RepairStarted = RepairStartingPoint;


                        _performedJob.RepairEnded = RepairStartingPoint.AddHours(singleJob.InstallData.OperationDuration);

                        //update to _job so that vessel till is booked
                        _job.RepairEnded = ((DateTime)_performedJob.RepairEnded).AddHours(_distanceTOFarm / speedLimit);
                        singleJob.CompletedDate = _performedJob.RepairEnded;

                        //note starting point
                        if (!singleJob.IncidentTime.HasValue)
                            singleJob.IncidentTime = RepairStartingPoint;

                        _performedJob.JourneyStart = jobIndex == 0 ? shiftStartIncaseMidDayRepair : jobsData[jobIndex - 1].CompletedDate;
                        _performedJob.JourneyEnd = jobIndex == 0 ? shiftStartIncaseMidDayRepair.AddHours(_distanceTOFarm / speedLimit) : ((DateTime)jobsData[jobIndex - 1].CompletedDate).AddHours(FarmDetails.Distancebetweendevices / speedLimit);

                        _performedJob.WorkedHours = ((DateTime)_performedJob.RepairEnded - (DateTime)_performedJob.RepairStarted).TotalHours;


                        if (_job.SingleShift) //above WW will decide full or min repair first
                        {
                            usedV.CarryingTechs = repairToPerform.NoOftechsReq;
                        }
                        else if (_job.RotationShift)
                        {
                            usedV.CarryingTechs = repairToPerform.NoOftechsReq * 2;
                        }

                        _performedJob.OperationalTime = ((DateTime)_performedJob.JourneyEnd - (DateTime)_performedJob.JourneyStart).TotalHours * 2;
                        _performedJob.StandbyTime = _performedJob.WorkedHours;

                        AddInstallJobdata(_performedJob);
                        jobIndex++;
                        //add fuel cost only once
                        // if(tfuelcost == 0)
                        tfuelcost = ((fuelCost * vesselInfo.FuelUsage) * (_performedJob.StandbyTime + _performedJob.OperationalTime));
                    }
                    #endregion
                    usedV.TFuelCost += tfuelcost;
                    usedV.Available = false;
                    usedV.Base = selBase.Basename;
                    usedV.OnTaskFrom = shiftStartIncaseMidDayRepair;
                    usedV.OnTaskT0 = ((DateTime)_job.RepairEnded).AddHours(_job.TotalTimeToTravel / 2);
                    usedV.HireTriggered = false;

                    res.VesselUsed = usedV.Id;

                    if (usedV.Rented && usedV.HireAsReq)
                    {
                        usedV.RentedDaysForHireAsReq += (shiftEnd - shiftEnd).TotalDays > 1 ? (shiftEnd - shiftEnd).TotalDays : 1;
                    }

                    InstallVesselsList.Update(usedV.Id, usedV);

                    res.Status = Install_Status.Completed;

                    return res;
                }
                catch (Exception)
                {

                    // log.Error(e);
                }
            }
            catch (Exception)
            {

                // log.Error(e);
            }
            return null;
        }

        public RepairDetails ConvertPMMaintenanceToRepairModel(PriventiveDetails prev)
        {

            if (prev != null)
            {
                var _rep = new RepairDetails
                {
                    Base = prev.Base,
                    Currentvelocitylimit = prev.CurrentVelocityLimit,
                    NoOfTechs = prev.NoOftechsReq,
                    OperationdurationOffshore = prev.OprDurationOffs,
                    Operationlocation = prev.OperationLOC,
                    OperatondurationOnshore = prev.OprDurationOns,
                    Powerloss = prev.Powerloss,
                    RepairDesc = prev.Taskdescription,
                    RepairName = prev.PMCategory,
                    Sparepart = prev.Sparepart,
                    Vesselrequired = prev.VesselReq,
                    Waveheightlimit = prev.Waveheightlimit,
                    Waveperiodlimit = prev.Waveperiodlimit,
                    Windspeedlimit = prev.Windspeedlimit
                };
                return _rep;
            }
            return null;
        }

        private void AddJobdata(SimJobData _job)
        {
            if (_job != null)
            {
                var __existing = SimJobs.Find(s => s.SimIncidentsId == _job.SimIncidentsId);
                if (__existing != null)
                {
                    if (_job.Completed)
                    {
                        if (_job.FullRepair)
                            __existing.Completed = true;
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
                    SimJobs.Add(_job);

            }
        }

        private void AddInstallJobdata(InstallJobData _job)
        {
            if (_job != null)
            {
                if (!_job.Completed)
                {
                    var __existing = InstallJobsDataList.GetAll().Where(s => s.InstallId == _job.InstallId).FirstOrDefault();
                    if (__existing != null)
                        __existing.NoOfAttempts += 1;
                }
                else
                    InstallJobsDataList.Add(_job);
            }
        }

        private VesselInfo CheckBasicForVesselTechsAvail(RepairDetails repair, int simstageId)
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
                        //check tech available at base
                        //get base

                        //check if vessel avail in sim vessels list
                        var simvesselAvail = SimVesselsList.GetAll().Where(v => v.VesselType == checkVesselpresent.VesselClassif && v.Available == true && v.SimIterationId == simstageId).FirstOrDefault();
                        if (simvesselAvail != null)
                        {
                            bool allgoodWithRentalOrPurchased = true;
                            //check purchased or rental
                            if (simvesselAvail.Rented && !simvesselAvail.HireAsReq)
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
                                result.NightWork = checkVesselpresent.NightWork;
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
            catch (Exception)
            {

                //   log.Error(e);
            }
            return result;
        }

        private VesselInfo PickVesselForOM(RepairDetails _rep, double maxsafehours)
        {
            VesselInfo result = new VesselInfo();
            try
            {
                //check if vessel is available in the list
                var checkVesselpresent = TotalVessels.GetObjByName(_rep.Vesselrequired);
                if (checkVesselpresent != null)
                {
                    //check if vessel avail in sim vessels list
                    var simvesselAvail = SimVesselsList.GetAll().Where(v => v.VesselType == checkVesselpresent.VesselClassif && v.Available == true).FirstOrDefault();
                    if (simvesselAvail != null)
                    {
                        bool allgoodWithRentalOrPurchased = true;
                        //check purchased or rental
                        if (simvesselAvail.Rented && !simvesselAvail.HireAsReq)
                        {
                            DateTime rentStart = new DateTime(Convert.ToInt32(projectCurrentDate.Year.ToString()), Convert.ToInt32(checkVesselpresent.RentalStartMonth.ToString()), Convert.ToInt32(checkVesselpresent.RentalStartDay.ToString()), 0, 0, 0);
                            DateTime rentEnd = new DateTime(Convert.ToInt32(projectCurrentDate.Year.ToString()), Convert.ToInt32(checkVesselpresent.RentalEndMonth.ToString()), Convert.ToInt32(checkVesselpresent.RentalEndDay.ToString()), 0, 0, 0);
                            if (projectCurrentDate >= rentStart && projectCurrentDate.AddHours(maxsafehours) <= rentEnd)
                            {
                                //do nothing as all good
                                result.Purchased = false;
                                result.RentalStart = rentStart;
                                result.RentalEnd = rentEnd;
                            }
                            else
                            {
                                allgoodWithRentalOrPurchased = false;
                                result.FailedReason = FailedReason.OutOfRentalPeriod;
                                result.Comments = $"{result.Comments}{Environment.NewLine} Vessel is out of Rental period";
                                return result;
                            }
                        }

                        if (allgoodWithRentalOrPurchased)
                        {
                            result.Purchased = true;
                            //fill up with more info
                            result.Proceed = true;
                            result.WaveHeightLimit = _rep.Waveheightlimit;
                            result.Speed = checkVesselpresent.Speed;
                            result.SimVesselsId = simvesselAvail.Id;
                            result.FuelUsage = checkVesselpresent.FuelConsumption;
                            result.NightWork = checkVesselpresent.NightWork;
                        }
                    }
                    else
                        result.FailedReason = FailedReason.VesselAvail;
                }
                else
                    result.FailedReason = FailedReason.VesselAvail;
            }
            catch (Exception)
            {

                //   log.Error(e);
            }
            return result;
        }

        private VesselInfo PickVesselForInstall(InstallJobs nextJob, double repairToPerformDuration)
        {
            VesselInfo result = new VesselInfo();
            try
            {
                //check if vessel is available in the list
                var checkVesselpresent = TotalVessels.GetObjByName(nextJob.InstallData.VesselReq);
                if (checkVesselpresent != null)
                {
                    //check techs requirement
                    if (nextJob.InstallData.NoOftechsReq <= checkVesselpresent.TechsCapacity)
                    {
                        //check if vessel avail in sim vessels list
                        var getallVTest = InstallVesselsList.GetAll().ToList();
                        var installvesselAvail = InstallVesselsList.GetAll().Where(v => v.VesselType == checkVesselpresent.VesselClassif && v.Available == true).FirstOrDefault();
                        if (installvesselAvail != null)
                        {
                            bool allgoodWithRentalOrPurchased = true;
                            //check purchased or rental
                            if (installvesselAvail.Rented && !installvesselAvail.HireAsReq)
                            {
                                DateTime rentStart = new DateTime(Convert.ToInt32(projectCurrentDate.Year.ToString()), Convert.ToInt32(checkVesselpresent.RentalStartMonth.ToString()), Convert.ToInt32(checkVesselpresent.RentalStartDay.ToString()), 0, 0, 0);
                                DateTime rentEnd = new DateTime(Convert.ToInt32(projectCurrentDate.Year.ToString()), Convert.ToInt32(checkVesselpresent.RentalEndMonth.ToString()), Convert.ToInt32(checkVesselpresent.RentalEndDay.ToString()), 0, 0, 0);
                                if (projectCurrentDate >= rentStart && projectCurrentDate.AddHours(repairToPerformDuration) <= rentEnd)
                                {
                                    //do nothing as all good
                                    result.Purchased = false;
                                    result.RentalStart = rentStart;
                                    result.RentalEnd = rentEnd;
                                }
                                else
                                {
                                    allgoodWithRentalOrPurchased = false;
                                    result.FailedReason = FailedReason.OutOfRentalPeriod;
                                    result.Comments = $"{result.Comments}{Environment.NewLine} Vessel is out of Rental period";
                                    return result;
                                }
                            }

                            if (allgoodWithRentalOrPurchased)
                            {
                                result.Purchased = true;
                                //fill up with more info
                                result.Proceed = true;
                                result.WaveHeightLimit = nextJob.InstallData.Waveheightlimit;
                                result.Speed = checkVesselpresent.Speed;
                                result.SimVesselsId = installvesselAvail.Id;
                                result.FuelUsage = checkVesselpresent.FuelConsumption;
                                result.NightWork = checkVesselpresent.NightWork;
                                result.FuelCost = checkVesselpresent.FuelCost;
                                result.FuelUsage = checkVesselpresent.FuelConsumption;
                                result.Name = checkVesselpresent.VesselClassif;
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
            catch (Exception)
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
        public WWShift CheckWeatherConditions(DateTime From, DateTime To, double limit, double maxhrs, double minhrs, bool fullShift)
        {
            WWShift result = new WWShift();
            if (fullShift)
                To = From.AddHours(maxhrs);
            try
            {
                DateTime safeStartingPoint = From;
                result.MinRepairHours = new SafeHours();
                result.FullRepairHours = new SafeHours();

                var totalHours = (To - From).TotalHours;
                if (totalHours <= minhrs)
                    return result;

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
                else if (longrunhours > minhrs) //if req hours not acheivable, return how many hours can perform in the shift
                {
                    result.Result = true;
                    //can perform full repair
                    result.MinRepairHours = new SafeHours
                    {
                        CanTravel = true,
                        Safehours = longrunhours <= totalHours ? longrunhours : totalHours,
                        Safestart = safeStartingPoint
                    };
                }
                else
                {
                    result.Result = false;
                }
            }
            catch (Exception)
            {

                //log.Error(e);
            }
            return result;
        }

        public WWShift CheckWeatherConditionsForInstall(DateTime From, DateTime To, double limit, double maxhrs)
        {
            WWShift result = new WWShift();
            try
            {
                To = From.AddHours(maxhrs);
                DateTime safeStartingPoint = From;
                result.MinRepairHours = new SafeHours();
                result.FullRepairHours = new SafeHours();
                var totalHours = maxhrs;
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
                else
                {
                    result.Result = false;
                }
            }
            catch (Exception)
            {

                //log.Error(e);
            }
            return result;
        }

        public String GetTimestamp(DateTime value)
        {
            return value.ToString("yyyyMMddHHmmssffff");
        }

        private void BtnReportstoExcel_Click(object sender, EventArgs e)
        {
            string filePath = Path.Combine(DataFolders.ReportsFolder, ProjectDetails.ProjectName);
            try
            {
                using (var fbd = new FolderBrowserDialog())
                {
                    DialogResult result = fbd.ShowDialog();

                    if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                    {
                        string exportFileName = $"{ProjectDetails.ProjectName}_{GetTimestamp(DateTime.Now)}";
                        filePath = Path.Combine(fbd.SelectedPath, exportFileName);
                    }
                }
                BtnReportstoExcel.Text = "Exporting...";
                BtnReportstoExcel.Enabled = false;

                Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();

                Excel.Workbook xlWorkBook;
                List<Excel.Worksheet> projSheets = new List<Excel.Worksheet>();
                object misValue = System.Reflection.Missing.Value;
                xlWorkBook = xlApp.Workbooks.Add(misValue);

                #region Project sheet
                //create Inst_Yearly_Results_Iteration
                Excel.Worksheet insYResIte;
                insYResIte = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
                insYResIte.Name = "Inst_Yearly_Results";

                insYResIte.Cells[1, 1].Value = "Iteration no.";
                insYResIte.Cells[1, 2].Value = "Year";
                insYResIte.Cells[1, 3].Value = "Base Cost";
                insYResIte.Cells[1, 4].Value = "Additional Cost";
                insYResIte.Cells[1, 5].Value = "Vessel fixed/rental Costs";
                insYResIte.Cells[1, 6].Value = "Tech Costs";
                insYResIte.Cells[1, 7].Value = "Vessel fuel costs";
                insYResIte.Cells[1, 8].Value = "Total Installation Cost";


                int installYcounter = 2;
                foreach (var _installY in InstallYearlyResultList.GetAll())
                {
                    insYResIte.Cells[installYcounter, 1].Value = _installY.Iteration;
                    insYResIte.Cells[installYcounter, 2].Value = _installY.Year;
                    insYResIte.Cells[installYcounter, 3].Value = _installY.BaseCost;
                    insYResIte.Cells[installYcounter, 4].Value = _installY.AdditionalCost;
                    insYResIte.Cells[installYcounter, 5].Value = _installY.T_fixedcost;
                    insYResIte.Cells[installYcounter, 6].Value = _installY.TechCost;
                    insYResIte.Cells[installYcounter, 7].Value = _installY.T_fuelcost;
                    insYResIte.Cells[installYcounter, 8].Value = _installY.AdditionalCost + _installY.BaseCost + _installY.T_fixedcost + _installY.TechCost + _installY.T_fuelcost;
                    installYcounter++;
                }
                projSheets.Add(insYResIte);

                //create Inst_Results_Yearly
                Excel.Worksheet insResYearly;
                insResYearly = (Excel.Worksheet)xlWorkBook.Worksheets.Add(Type.Missing, projSheets[0], Type.Missing, Type.Missing);
                insResYearly.Name = "Inst_Device_Results";
                insResYearly.Cells[1, 1].Value = "Iteration no.";
                insResYearly.Cells[1, 2].Value = "Device Id";
                insResYearly.Cells[1, 3].Value = "Device type";
                insResYearly.Cells[1, 4].Value = "JOB";
                insResYearly.Cells[1, 5].Value = "Start";
                insResYearly.Cells[1, 6].Value = "End";
                insResYearly.Cells[1, 7].Value = "Status";
                insResYearly.Cells[1, 8].Value = "Install time in hrs";


                int installIterationCountrer = 2;
                foreach (var _installIterationDeviceData in InstallDeviceResultList.GetAll())
                {
                    if (_installIterationDeviceData.Status == "Completed")
                    {
                        string _start = Convert.ToDateTime(_installIterationDeviceData.Start).ToLongDateString() + " " + Convert.ToDateTime(_installIterationDeviceData.Start).ToLongTimeString();
                        string _complete = Convert.ToDateTime(_installIterationDeviceData.Complete).ToLongDateString() + " " + Convert.ToDateTime(_installIterationDeviceData.Complete).ToLongTimeString();

                        insResYearly.Cells[installIterationCountrer, 1].Value = _installIterationDeviceData.Iteration;
                        insResYearly.Cells[installIterationCountrer, 2].Value = _installIterationDeviceData.DeviceID;
                        insResYearly.Cells[installIterationCountrer, 3].Value = _installIterationDeviceData.DeviceType;
                        insResYearly.Cells[installIterationCountrer, 4].Value = _installIterationDeviceData.Jobs;
                        insResYearly.Cells[installIterationCountrer, 5].Value = _installIterationDeviceData.Status == "Completed" ? _start : "--";
                        insResYearly.Cells[installIterationCountrer, 6].Value = _installIterationDeviceData.Status == "Completed" ? _complete : "--";
                        insResYearly.Cells[installIterationCountrer, 7].Value = _installIterationDeviceData.Status;
                        insResYearly.Cells[installIterationCountrer, 8].Value = _installIterationDeviceData.InstallTime;

                        installIterationCountrer++;
                    }
                }
                projSheets.Add(insResYearly);


                var fulllist = SimIterationList.GetAllSimIterationResults();
                if (fulllist.Any())
                {
                    int iterationCounter = 1;
                    int cellChange = 2;

                    Excel.Worksheet omResAvg;
                    omResAvg = (Excel.Worksheet)xlWorkBook.Worksheets.Add(Type.Missing, projSheets[1], Type.Missing, Type.Missing);
                    omResAvg.Name = "OM_Results_AverageAnnual";
                    projSheets.Add(omResAvg);

                    Excel.Worksheet _projSheet;
                    //create excel file name : projectName_reports.xlsx
                    _projSheet = (Excel.Worksheet)xlWorkBook.Worksheets.Add(Type.Missing, projSheets[2], Type.Missing, Type.Missing);
                    _projSheet.Name = "OM_Results_Yearly";
                    projSheets.Add(_projSheet);

                    foreach (var _iteration in fulllist)
                    {
                        //OM_Results_Average

                        omResAvg.Cells[1, 1].Value = "Total O&M cost";
                        omResAvg.Cells[1, 2].Value = "Vessel Fuel Costs";
                        omResAvg.Cells[1, 3].Value = "Vessel fixed/rental Costs";
                        omResAvg.Cells[1, 4].Value = "Tech Costs";
                        omResAvg.Cells[1, 5].Value = "Base Costs";
                        omResAvg.Cells[1, 6].Value = "Spare parts/repair Costs";
                        omResAvg.Cells[1, 7].Value = "Additional O&M cost";
                        omResAvg.Cells[1, 8].Value = "Energy Theoretical";
                        omResAvg.Cells[1, 9].Value = "Energy Produced";
                        omResAvg.Cells[1, 10].Value = "Energy-based Availability";
                        omResAvg.Cells[1, 11].Value = "Total hours";
                        omResAvg.Cells[1, 12].Value = "Downtime";
                        omResAvg.Cells[1, 13].Value = "Time-based Availability";
                        omResAvg.Cells[1, 14].Value = "Income";

                        //fill data

                        omResAvg.Cells[iterationCounter + 1, 1].Value = _iteration.Total_omcost;
                        omResAvg.Cells[iterationCounter + 1, 2].Value = _iteration.V_fuelcost;
                        omResAvg.Cells[iterationCounter + 1, 3].Value = _iteration.V_fixedcost;
                        omResAvg.Cells[iterationCounter + 1, 4].Value = _iteration.TechCost;
                        omResAvg.Cells[iterationCounter + 1, 5].Value = _iteration.BaseCost;
                        omResAvg.Cells[iterationCounter + 1, 6].Value = _iteration.Sparepartscost;
                        omResAvg.Cells[iterationCounter + 1, 7].Value = _iteration.AdditionalCost;
                        omResAvg.Cells[iterationCounter + 1, 8].Value = _iteration.E_theoretical;
                        omResAvg.Cells[iterationCounter + 1, 9].Value = _iteration.E_real;
                        omResAvg.Cells[iterationCounter + 1, 10].Value = _iteration.EBA;
                        omResAvg.Cells[iterationCounter + 1, 11].Value = _iteration.T_lifetime;
                        omResAvg.Cells[iterationCounter + 1, 12].Value = _iteration.T_downtime;
                        omResAvg.Cells[iterationCounter + 1, 13].Value = _iteration.TBA;
                        omResAvg.Cells[iterationCounter + 1, 14].Value = _iteration.Income;

                        //OM_Results_Yearly

                        //string cellValue = range.Value.ToString();
                        _projSheet.Cells[1, 1].Value = "Iteration no.";
                        _projSheet.Cells[1, 2].Value = "Year";
                        _projSheet.Cells[1, 3].Value = "Total Annual O&M cost";
                        _projSheet.Cells[1, 4].Value = "Vessel Fuel Costs";
                        _projSheet.Cells[1, 5].Value = "Vessel fixed/rental Costs";
                        _projSheet.Cells[1, 6].Value = "Tech Costs";
                        _projSheet.Cells[1, 7].Value = "Base Costs";
                        _projSheet.Cells[1, 8].Value = "Spare parts/repair Costs";
                        _projSheet.Cells[1, 9].Value = "Additional O&M cost";
                        _projSheet.Cells[1, 10].Value = "Energy Theoretical";
                        _projSheet.Cells[1, 11].Value = "Energy Produced";
                        _projSheet.Cells[1, 12].Value = "Energy-based Availability";
                        _projSheet.Cells[1, 13].Value = "Total hours";
                        _projSheet.Cells[1, 14].Value = "Downtime";
                        _projSheet.Cells[1, 15].Value = "Time-based Availability";
                        _projSheet.Cells[1, 16].Value = "Income";

                        //create sheet 1 with _iteration number
                        var yearsData = SimIterationList.GetAllYearlySimIterationResultsByIterationId(_iteration.Id);

                        foreach (var _yeardata in yearsData)
                        {
                            _projSheet.Cells[cellChange, 1].Value = iterationCounter;
                            _projSheet.Cells[cellChange, 2].Value = _yeardata.Year;
                            _projSheet.Cells[cellChange, 3].Value = _yeardata.Total_omcost;
                            _projSheet.Cells[cellChange, 4].Value = _yeardata.V_fuelcost;
                            _projSheet.Cells[cellChange, 5].Value = _yeardata.V_fixedcost;
                            _projSheet.Cells[cellChange, 6].Value = _yeardata.TechCost;
                            _projSheet.Cells[cellChange, 7].Value = _yeardata.BaseCost;
                            _projSheet.Cells[cellChange, 8].Value = _yeardata.Sparepartscost;
                            _projSheet.Cells[cellChange, 9].Value = _yeardata.AdditionalCost;
                            _projSheet.Cells[cellChange, 10].Value = _yeardata.E_theoretical;
                            _projSheet.Cells[cellChange, 11].Value = _yeardata.E_real;
                            _projSheet.Cells[cellChange, 12].Value = _yeardata.EBA;
                            _projSheet.Cells[cellChange, 13].Value = _yeardata.T_lifetime;
                            _projSheet.Cells[cellChange, 14].Value = _yeardata.T_downtime;
                            _projSheet.Cells[cellChange, 15].Value = _yeardata.TBA;
                            _projSheet.Cells[cellChange, 16].Value = _yeardata.Income;
                            cellChange++;
                        }
                        iterationCounter++;
                    }

                }

                #endregion

                xlWorkBook.SaveAs(filePath, Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);

                xlWorkBook.Close();
                xlApp.Quit();

                foreach (var _s in projSheets)
                {
                    _ = Marshal.ReleaseComObject(_s);
                }
                _ = Marshal.ReleaseComObject(xlWorkBook);
                _ = Marshal.ReleaseComObject(xlApp);
                //End Upload to Excel

                _ = MessageBox.Show("Report exported successfully! : " + filePath);

                //load project files again


            }
            catch (Exception ex)
            {
                //log error
                _ = MessageBox.Show("Error occured while exporting file " + filePath + ".  Error: " + ex.ToString());
            }
            BtnReportstoExcel.Text = "Exported";
            BtnReportstoExcel.Enabled = true;
        }

        private void label3_Click(object sender, EventArgs e)
        {

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
                            //update inci in the main list that it is either completed or to perform ful repair
                            SimIncidentsList.Update(_inc.Id, _inc);
                            //find job that completed incident and add to jobs data
                            var findjobs = SimJobs.FindAll(s => s.SimIncidentsId == _inc.Id && s.Completed == true);
                            if (findjobs.Any())
                            {
                                foreach (var _eachJOb in findjobs)
                                {
                                    SimJobsDataList.Add(_eachJOb);

                                    //job and incident that are completed can be deleted from local lists
                                    if (_eachJOb.Completed)
                                        _ = SimJobs.Remove(_eachJOb);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception)
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
                        Completed = true,
                        DelayedHours = enddate.Subtract((DateTime)_inc.IncidentTime).TotalHours,
                        IncidentTime = _inc.IncidentTime,
                        SimIncidentsId = _inc.Id,
                        RepairEnded = enddate
                    };
                    SimJobs.Add(_job);

                    _inc.IncidentFixed = enddate;
                    _inc.Status = Task_Status.Completed;
                    SimIncidentsList.Update(_inc.Id, _inc);
                }
            }
            catch (Exception)
            {
                //  log.Error(e);
            }
        }
        #endregion
    }
}
