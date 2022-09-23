using SELKIE.Enums;
using SELKIE.Logic;
using SELKIE.Models;
using SELKIE.SimModels;
using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace SELKIE
{
    public partial class Dashboard : Form
    {
        Form1 frm1 = new Form1();
        Bases _bases = new Bases();
        Vessels _vessels = new Vessels();
        Installation_Strategy _installStrategy = new Installation_Strategy();
        PM_Maintenance _pmMaintenance = new PM_Maintenance();
        Repairs _repairs = new Repairs();
        Components _components = new Components();
        Project_Details _projetsDetails = new Project_Details();
        Farm_Details _farmDetails = new Farm_Details();
        Resource _resource = new Resource();
        delegate void SetStatusText(string _updatetext);
        delegate void SetProjectprogress(int percentage);
        delegate void SetStausText(string _exporttext);
        public ValidationCheck validCheck = new ValidationCheck();
        public Dashboard()
        {
            InitializeComponent();
            FillTableWithProjects();
            // progressBar2.Visible = false;
            // DelProject();

        }
        public void FillTableWithProjects()
        {
            //fill grid with files from folder
            if (Directory.Exists(DataFolders.ProjectsFolder))
            {
                projectsDataGrid.DataSource = null;
                projectsDataGrid.Rows.Clear();

                var projects = Directory.GetFiles(DataFolders.ProjectsFolder);
                int _sno = 1;
                foreach (var pfile in projects)
                {
                    _ = projectsDataGrid.Rows.Add(_sno.ToString(), Path.GetFileName(pfile), "Load", "Delete");
                    _sno++;
                }
            }
        }

        private void BtnExporttoExcel_Click(object sender, EventArgs e)
        {
            bool errorsFound = false;
            string errorInfo = "";
            string filePath = "";
            try
            {
                BtnExporttoExcel.Text = "Exporting...";
                BtnExporttoExcel.Enabled = false;

                Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();

                Excel.Workbook xlWorkBook;
                Excel.Worksheet projSheet;
                object misValue = System.Reflection.Missing.Value;
                xlWorkBook = xlApp.Workbooks.Add(misValue);
                int _sheetOrder = 1;
                projSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(_sheetOrder);
                projSheet.Name = "Project details";
                ExportProjectStatusText("Exporting project...");
                #region Project sheet
                //check project validation
                var res = validCheck.CheckProjectValidation();
                if (res.Result)
                {
                    projSheet.Cells[1, 1].Value = "Project name";
                    projSheet.Cells[1, 2].Value = ProjectDetails.ProjectName;
                    projSheet.Cells[2, 1].Value = "Installationtime";
                    projSheet.Cells[2, 2].Value = ProjectDetails.InstallTime;
                    projSheet.Cells[3, 1].Value = "Project life time";
                    projSheet.Cells[3, 2].Value = ProjectDetails.ProjectLifeTime;
                    projSheet.Cells[4, 1].Value = "Noofiterations";
                    projSheet.Cells[4, 2].Value = ProjectDetails.NoOfIterations;
                    projSheet.Cells[5, 1].Value = "Gridsalerate";
                    projSheet.Cells[5, 2].Value = ProjectDetails.GridSaleRate;
                    projSheet.Cells[6, 1].Value = "Wake losses";
                    projSheet.Cells[6, 2].Value = ProjectDetails.WakeLoss;
                    projSheet.Cells[7, 1].Value = "Transmission";
                    projSheet.Cells[7, 2].Value = ProjectDetails.TransLoss;
                    projSheet.Cells[8, 1].Value = "Other Loses";
                    projSheet.Cells[8, 2].Value = ProjectDetails.ProdLoss;
                }
                else
                { errorsFound = true; errorInfo = "please check " + res.Message; return; }
                #endregion


                Excel.Sheets worksheets = xlWorkBook.Worksheets;
                var farmSheet = (Excel.Worksheet)worksheets.Add(Type.Missing, worksheets[_sheetOrder], Type.Missing, Type.Missing);
                farmSheet.Name = "Farm details";

                #region farm details
                var farmcheck = validCheck.CheckFarmValidation();
                if (farmcheck.Result)
                {
                    farmSheet.Cells[1, 1].Value = "Manufacturer name";
                    farmSheet.Cells[1, 2].Value = FarmDetails.ManufatureName;
                    farmSheet.Cells[2, 1].Value = "Total number";
                    farmSheet.Cells[2, 2].Value = FarmDetails.NoOfDivices;
                    farmSheet.Cells[3, 1].Value = "Technology type";
                    farmSheet.Cells[3, 2].Value = FarmDetails.TechType;
                    farmSheet.Cells[4, 1].Value = "Select power curve";
                    farmSheet.Cells[4, 2].Value = FarmDetails.PowerCurve;
                    farmSheet.Cells[5, 1].Value = "Sub structre name";
                    farmSheet.Cells[5, 2].Value = FarmDetails.Substructrename;
                    farmSheet.Cells[6, 1].Value = "Sub structre type";
                    farmSheet.Cells[6, 2].Value = FarmDetails.Substructretype;
                    farmSheet.Cells[7, 1].Value = "Disatnce between devices";
                    farmSheet.Cells[7, 2].Value = FarmDetails.Distancebetweendevices;

                    farmSheet.Cells[10, 1].Value = "Measurement height";
                    farmSheet.Cells[10, 2].Value = FarmDetails.Tidal_height;
                    farmSheet.Cells[11, 1].Value = "Hub height";
                    farmSheet.Cells[11, 2].Value = FarmDetails.Tidal_hubHeight;

                }
                else
                { errorsFound = true; errorInfo = "please check " + farmcheck.Message; return; }
                #endregion

                // xlNewSheet.Cells[1, 1] = "New sheet content";



                //********Resource***********************

                _sheetOrder++;
                var resourceSheet = (Excel.Worksheet)worksheets.Add(Type.Missing, worksheets[_sheetOrder], Type.Missing, Type.Missing);
                resourceSheet.Name = "Resource data";
                // xlNewSheet.Cells[1, 1] = "New sheet content";

                #region resource details
                var rescheck = validCheck.CheckResourceValidation();
                if (rescheck.Result)
                {
                    resourceSheet.Cells[1, 1].Value = "Location name";
                    resourceSheet.Cells[1, 2].Value = ResourceDetails.Locationname;
                    resourceSheet.Cells[2, 1].Value = "Water depth";
                    resourceSheet.Cells[3, 1].Value = "Resource data mesurement height";
                    resourceSheet.Cells[4, 1].Value = "Specyfy metocian data";
                    resourceSheet.Cells[2, 2].Value = ResourceDetails.Waterdepth;
                    resourceSheet.Cells[3, 2].Value = ResourceDetails.Mesurementheight;
                    resourceSheet.Cells[4, 2].Value = ResourceDetails.Specifymetociandata;
                }
                else
                { errorsFound = true; errorInfo = "please check " + rescheck.Message; return; }
                #endregion
                _sheetOrder++;
                var xlNewSheet2 = (Excel.Worksheet)worksheets.Add(Type.Missing, worksheets[_sheetOrder], Type.Missing, Type.Missing);
                xlNewSheet2.Name = "O&M Base";


                #region base details
                var basecheck = validCheck.CheckBasesValidation();
                if (basecheck.Result)
                {
                    // storing header part in Excel  
                    for (int i = 1; i < _bases.dataGridViewBases.Columns.Count + 1; i++)
                    {
                        xlNewSheet2.Cells[1, i] = _bases.dataGridViewBases.Columns[i - 1].HeaderText;
                    }
                    // storing Each row and column value to excel sheet  
                    for (int i = 0; i < _bases.dataGridViewBases.Rows.Count; i++)
                    {
                        for (int j = 0; j < _bases.dataGridViewBases.Columns.Count; j++)
                        {
                            xlNewSheet2.Cells[i + 2, j + 1] = _bases.dataGridViewBases.Rows[i].Cells[j].Value.ToString();
                        }

                    }
                }
                else
                { errorsFound = true; errorInfo = "please check " + basecheck.Message; return; }
                #endregion
                _sheetOrder++;
                var xlNewSheet3 = (Excel.Worksheet)worksheets.Add(Type.Missing, worksheets[_sheetOrder], Type.Missing, Type.Missing);
                xlNewSheet3.Name = "Vessel Data";

                #region vessel details
                var vesselcheck = validCheck.CheckVesselsValidation();
                if (vesselcheck.Result)
                {
                    // storing header part in Excel  
                    for (int i = 1; i < _vessels.dataGridViewVesselDetails.Columns.Count + 1; i++)
                    {
                        xlNewSheet3.Cells[1, i] = _vessels.dataGridViewVesselDetails.Columns[i - 1].HeaderText;
                    }
                    // storing Each row and column value to excel sheet  
                    for (int i = 0; i < _vessels.dataGridViewVesselDetails.Rows.Count; i++)
                    {
                        for (int j = 0; j < _vessels.dataGridViewVesselDetails.Columns.Count; j++)
                        {
                            xlNewSheet3.Cells[i + 2, j + 1] = _vessels.dataGridViewVesselDetails.Rows[i].Cells[j].Value.ToString();
                        }
                    }
                }
                else
                { errorsFound = true; errorInfo = "please check " + vesselcheck.Message; return; }
                #endregion

                //Installation Strategy


                _sheetOrder++;
                var xlNewSheet4 = (Excel.Worksheet)worksheets.Add(Type.Missing, worksheets[_sheetOrder], Type.Missing, Type.Missing);
                xlNewSheet4.Name = "Installation Strategy";

                #region install details
                var installcheck = validCheck.ChekInstallationValidation();
                if (installcheck.Result)
                {
                    xlNewSheet4.Cells[1, 1].Value = "Installation start month";
                    xlNewSheet4.Cells[2, 1].Value = "Project commissioning";
                    xlNewSheet4.Cells[3, 1].Value = "Installation - Project management";
                    xlNewSheet4.Cells[4, 1].Value = "Installation - Contingency and Insurance";
                    xlNewSheet4.Cells[5, 1].Value = "Additional installation cost";
                    xlNewSheet4.Cells[1, 2].Value = InstStrategyDetails.Instalstartmonth;
                    xlNewSheet4.Cells[2, 2].Value = InstStrategyDetails.ProjectCommissioning;
                    xlNewSheet4.Cells[3, 2].Value = InstStrategyDetails.Projectmanagement;
                    xlNewSheet4.Cells[4, 2].Value = InstStrategyDetails.InstallationContingency;
                    xlNewSheet4.Cells[5, 2].Value = InstStrategyDetails.Installationcost;
                }
                else
                { errorsFound = true; errorInfo = "please check " + installcheck.Message; return; }
                _sheetOrder++;
                var xlNewSheet5 = (Excel.Worksheet)worksheets.Add(Type.Missing, worksheets[_sheetOrder], Type.Missing, Type.Missing);
                xlNewSheet5.Name = "Installattion Data";
                // storing header part in Excel  
                for (int i = 1; i < _installStrategy.dataGridViewInstask.Columns.Count + 1; i++)
                {
                    xlNewSheet5.Cells[1, i] = _installStrategy.dataGridViewInstask.Columns[i - 1].HeaderText;
                }
                // storing Each row and column value to excel sheet  
                for (int i = 0; i < _installStrategy.dataGridViewInstask.Rows.Count; i++)
                {
                    for (int j = 0; j < _installStrategy.dataGridViewInstask.Columns.Count; j++)
                    {
                        xlNewSheet5.Cells[i + 2, j + 1] = _installStrategy.dataGridViewInstask.Rows[i].Cells[j].Value.ToString();
                    }
                }
                #endregion

                //PM
                _sheetOrder++;
                var xlNewSheet6 = (Excel.Worksheet)worksheets.Add(Type.Missing, worksheets[_sheetOrder], Type.Missing, Type.Missing);
                xlNewSheet6.Name = "Preventive Maintenance";

                #region Preventive details
                var prevcheck = validCheck.CheckPMValidation();
                if (prevcheck.Result)
                {
                    xlNewSheet6.Cells[1, 1].Value = "PM O&&M Start month";
                    xlNewSheet6.Cells[2, 1].Value = "PM O&&M End month";
                    xlNewSheet6.Cells[3, 1].Value = "Additional annual O&&M cost";
                    xlNewSheet6.Cells[1, 2].Value = PreventiveStartEnd.StartMonth;
                    xlNewSheet6.Cells[2, 2].Value = PreventiveStartEnd.EndMonth;
                    xlNewSheet6.Cells[3, 2].Value = PreventiveStartEnd.AdditionalAnnualCost;
                }
                else
                { errorsFound = true; errorInfo = "please check " + prevcheck.Message; return; }

                _sheetOrder++;
                var xlNewSheet7 = (Excel.Worksheet)worksheets.Add(Type.Missing, worksheets[_sheetOrder], Type.Missing, Type.Missing);
                xlNewSheet7.Name = "Preventive Data";

                // storing header part in Excel  
                for (int i = 1; i < _pmMaintenance.dataGridViewPMTask.Columns.Count + 1; i++)
                {
                    xlNewSheet7.Cells[1, i] = _pmMaintenance.dataGridViewPMTask.Columns[i - 1].HeaderText;
                }
                // storing Each row and column value to excel sheet  
                for (int i = 0; i < _pmMaintenance.dataGridViewPMTask.Rows.Count; i++)
                {
                    for (int j = 0; j < _pmMaintenance.dataGridViewPMTask.Columns.Count; j++)
                    {
                        xlNewSheet7.Cells[i + 2, j + 1] = _pmMaintenance.dataGridViewPMTask.Rows[i].Cells[j].Value.ToString();
                    }
                }
                #endregion
                _sheetOrder++;
                var xlNewSheet8 = (Excel.Worksheet)worksheets.Add(Type.Missing, worksheets[_sheetOrder], Type.Missing, Type.Missing);
                xlNewSheet8.Name = "Repairs";

                #region repair details
                var repaircheck = validCheck.CheckRepairsValidation();
                if (repaircheck.Result)
                {
                    for (int i = 1; i < _repairs.dataGridViewADDRep.Columns.Count + 1; i++)
                    {
                        xlNewSheet8.Cells[1, i] = _repairs.dataGridViewADDRep.Columns[i - 1].HeaderText;
                    }
                    // storing Each row and column value to excel sheet  
                    for (int i = 0; i < _repairs.dataGridViewADDRep.Rows.Count; i++)
                    {
                        for (int j = 0; j < _repairs.dataGridViewADDRep.Columns.Count; j++)
                        {
                            xlNewSheet8.Cells[i + 2, j + 1] = _repairs.dataGridViewADDRep.Rows[i].Cells[j].Value.ToString();
                        }
                    }
                }
                else
                { errorsFound = true; errorInfo = "please check " + repaircheck.Message; return; }
                #endregion
                _sheetOrder++;
                var xlNewSheet9 = (Excel.Worksheet)worksheets.Add(Type.Missing, worksheets[_sheetOrder], Type.Missing, Type.Missing);
                xlNewSheet9.Name = "Components";

                #region component details
                var compcheck = validCheck.CheckComponentsValidation();
                if (compcheck.Result)
                {
                    for (int i = 1; i < _components.dataGridViewCMP.Columns.Count + 1; i++)
                    {
                        xlNewSheet9.Cells[1, i] = _components.dataGridViewCMP.Columns[i - 1].HeaderText;
                    }
                    // storing Each row and column value to excel sheet  
                    for (int i = 0; i < _components.dataGridViewCMP.Rows.Count; i++)
                    {
                        for (int j = 0; j < _components.dataGridViewCMP.Columns.Count; j++)
                        {
                            xlNewSheet9.Cells[i + 2, j + 1] = _components.dataGridViewCMP.Rows[i].Cells[j].Value.ToString();
                        }
                    }
                }
                else
                { errorsFound = true; errorInfo = "please check " + compcheck.Message; return; }
                #endregion

                string filename = labelExport.Text;
                filePath = Path.Combine(DataFolders.ProjectsFolder, ProjectDetails.ProjectName);
                xlWorkBook.SaveAs(filePath, Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlExclusive, true, misValue, misValue, misValue, misValue);

                xlWorkBook.Close();
                xlApp.Quit();

                _ = Marshal.ReleaseComObject(projSheet);
                _ = Marshal.ReleaseComObject(farmSheet);
                _ = Marshal.ReleaseComObject(resourceSheet);
                _ = Marshal.ReleaseComObject(xlNewSheet2);
                _ = Marshal.ReleaseComObject(xlNewSheet3);
                _ = Marshal.ReleaseComObject(xlNewSheet4);
                _ = Marshal.ReleaseComObject(xlNewSheet5);
                _ = Marshal.ReleaseComObject(xlNewSheet6);
                _ = Marshal.ReleaseComObject(xlNewSheet7);
                _ = Marshal.ReleaseComObject(xlNewSheet8);
                _ = Marshal.ReleaseComObject(xlNewSheet9);

                _ = Marshal.ReleaseComObject(xlWorkBook);
                _ = Marshal.ReleaseComObject(xlApp);
                //End Upload to Excel
                FillTableWithProjects();
                _ = MessageBox.Show("Project exported successfully!");

                //load project files again
                ExportProjectStatusText("Project exported in to excel....");

            }
            catch (Exception ex)
            {
                //log error
                _ = MessageBox.Show("Error occured while exporting file " + filePath + ".  Error: " + ex.ToString());
            }
            finally
            {
                if (errorsFound)
                    _ = MessageBox.Show("Export failed! " + errorInfo);

                BtnExporttoExcel.Text = "Save Project";
                BtnExporttoExcel.Enabled = true;
            }
        }

        private bool LoadProject()
        {
            if (string.IsNullOrEmpty(loadStatus.Text) && string.IsNullOrEmpty(DataFolders.ProjectsFolder))
                return false;

            bool errorFound = false, closeWB = false;
            string _filename = loadStatus.Text;
            string filePath = Path.Combine(DataFolders.ProjectsFolder, _filename);
            Excel.Application xlApp = new Excel.Application();
            Excel.Workbook xlWorkBook;
            Excel.Worksheet Projectsheet, Farmsheet, Resourcesheet, Vesselssheet, Installationsheet, Basessheet, Installationdatasheet, Preventivessheet, Preventivesdatasheet, Repairssheet, Comoponentssheet;
            xlWorkBook = xlApp.Workbooks.Open(filePath);

            Projectsheet = xlWorkBook.Worksheets["Project details"];
            Farmsheet = xlWorkBook.Worksheets["Farm details"];
            Resourcesheet = xlWorkBook.Worksheets["Resource data"];
            Vesselssheet = xlWorkBook.Worksheets["Vessel Data"];
            Installationsheet = xlWorkBook.Worksheets["Installation Strategy"];
            Basessheet = xlWorkBook.Worksheets["O&M Base"];
            Installationdatasheet = xlWorkBook.Worksheets["Installattion Data"];
            Preventivessheet = xlWorkBook.Worksheets["Preventive Maintenance"];
            Preventivesdatasheet = xlWorkBook.Worksheets["Preventive Data"];
            Repairssheet = xlWorkBook.Worksheets["Repairs"];
            Comoponentssheet = xlWorkBook.Worksheets["Components"];

            try
            {
                CleanData();
                try
                {
                    closeWB = true;
                    //Load Project
                    #region Load Project

                    UpdatePrjectStatusText("loading project details....");
                    ProjectDetails.ProjectName = !string.IsNullOrEmpty(Projectsheet.Cells[1, 2].Text) ? Projectsheet.Cells[1, 2].Text : "";
                    ProjectDetails.InstallTime = int.TryParse(Projectsheet.Cells[2, 2].Text, out int installT) ? installT : 0;
                    ProjectDetails.ProjectLifeTime = int.TryParse(Projectsheet.Cells[3, 2].Text, out int projLife) ? projLife : 0;
                    ProjectDetails.NoOfIterations = int.TryParse(Projectsheet.Cells[4, 2].Text, out int noOfIter) ? noOfIter : 0;
                    ProjectDetails.GridSaleRate = double.TryParse(Projectsheet.Cells[5, 2].Text, out double gridSale) ? gridSale : 0;
                    ProjectDetails.WakeLoss = double.TryParse(Projectsheet.Cells[6, 2].Text, out double wakeL) ? wakeL : 0;
                    ProjectDetails.TransLoss = double.TryParse(Projectsheet.Cells[7, 2].Text, out double transL) ? transL : 0;
                    ProjectDetails.ProdLoss = double.TryParse(Projectsheet.Cells[8, 2].Text, out double prodL) ? prodL : 0;

                    var res = validCheck.CheckProjectValidation();
                    if (!res.Result)
                        errorFound = true;

                    #endregion

                    //Loading Farm Details
                    #region Loading Farm
                    if (!errorFound)
                    {
                        UpdatePrjectStatusText("loading farm details....");

                        FarmDetails.ManufatureName = !string.IsNullOrEmpty(Farmsheet.Cells[1, 2].Text) ? Farmsheet.Cells[1, 2].Text : "";
                        FarmDetails.NoOfDivices = int.TryParse(Farmsheet.Cells[2, 2].Text, out int noOfDs) ? noOfDs : 0;
                        FarmDetails.TechType = !string.IsNullOrEmpty(Farmsheet.Cells[3, 2].Text) ? Farmsheet.Cells[3, 2].Text : "";
                        FarmDetails.PowerCurve = !string.IsNullOrEmpty(Farmsheet.Cells[4, 2].Text) ? Farmsheet.Cells[4, 2].Text : "";
                        FarmDetails.Substructrename = !string.IsNullOrEmpty(Farmsheet.Cells[5, 2].Text) ? Farmsheet.Cells[5, 2].Text : "";
                        FarmDetails.Substructretype = !string.IsNullOrEmpty(Farmsheet.Cells[6, 2].Text) ? Farmsheet.Cells[6, 2].Text : "";
                        FarmDetails.Distancebetweendevices = double.TryParse(Farmsheet.Cells[7, 2].Text, out double dBDs) ? dBDs : 0;

                        FarmDetails.Tidal_height = double.TryParse(Farmsheet.Cells[10, 2].Text, out double th) ? th : 0;
                        FarmDetails.Tidal_hubHeight = double.TryParse(Farmsheet.Cells[11, 2].Text, out double thh) ? thh : 0;



                        var fcheck = validCheck.CheckFarmValidation();
                        if (!fcheck.Result)
                            errorFound = true;
                        if (!fcheck.FileExists)
                            FarmDetails.PowerCurve = "";
                    }

                    #endregion

                    //Loading Resource Details
                    #region Loading Resource
                    if (!errorFound)
                    {
                        UpdatePrjectStatusText("loading resource details....");
                        ResourceDetails.Locationname = !string.IsNullOrEmpty(Resourcesheet.Cells[1, 2].Text) ? Resourcesheet.Cells[1, 2].Text : "";
                        ResourceDetails.Waterdepth = double.TryParse(Resourcesheet.Cells[2, 2].Text, out double wDepth) ? wDepth : 0;
                        ResourceDetails.Mesurementheight = double.TryParse(Resourcesheet.Cells[3, 2].Text, out double mH) ? mH : 0;
                        ResourceDetails.Specifymetociandata = !string.IsNullOrEmpty(Resourcesheet.Cells[4, 2].Text) ? Resourcesheet.Cells[4, 2].Text : "";

                        var rcheck = validCheck.CheckResourceValidation();
                        if (!rcheck.Result)
                            errorFound = true;
                        if (!rcheck.FileExists)
                            ResourceDetails.Specifymetociandata = "";

                    }
                    #endregion

                    //Load Bases datagrid values
                    #region Load Bases
                    if (!errorFound)
                    {
                        UpdatePrjectStatusText("loading bases....");
                        for (int i = 2; i < 5; i++)
                        {
                            //check if base has name first
                            if (!string.IsNullOrEmpty(Basessheet.Cells[i, 3].Text) && !string.IsNullOrEmpty(Basessheet.Cells[i, 1].Text))
                            {
                                //save in temp obj
                                var btab = new BasesDetails
                                {
                                    Basename = Basessheet.Cells[i, 3].Text,
                                    Annualcost = double.TryParse(Basessheet.Cells[i, 4].Text, out double acost) ? acost : 0,
                                    Distancetofarm = double.TryParse(Basessheet.Cells[i, 5].Text, out double dF) ? dF : 0,
                                    NoOfTechs = int.TryParse(Basessheet.Cells[i, 6].Text, out int nT) ? nT : 0,
                                    AnnualsalperTech = double.TryParse(Basessheet.Cells[i, 7].Text, out double aS) ? aS : 0
                                };
                                TotalBases.AddBase(btab);
                            }
                        }

                        var bcheck = validCheck.CheckBasesValidation();
                        if (!bcheck.Result)
                            errorFound = true;
                    }
                    #endregion

                    #region Load Vessels
                    if (!errorFound)
                    {
                        UpdatePrjectStatusText("loading vessels....");

                        for (int i = 2; i < 15; i++)
                        {
                            if (!string.IsNullOrEmpty(Vesselssheet.Cells[i, 1].Text))
                            {
                                if (!string.IsNullOrEmpty(Vesselssheet.Cells[i, 3].Text))
                                {
                                    bool.TryParse(Vesselssheet.Cells[i, 19].Text, out bool pur);
                                    //save in temp obj
                                    var vtab = new VesselDetails()
                                    {
                                        VesselClassif = Vesselssheet.Cells[i, 3].Text,
                                        Number = int.TryParse(Vesselssheet.Cells[i, 4].Text, out int num) ? num : 0,
                                        TechsCapacity = int.TryParse(Vesselssheet.Cells[i, 5].Text, out int tcap) ? tcap : 0,
                                        NightWork = Vesselssheet.Cells[i, 6].Text,
                                        //purchase properties
                                        AnnualrunningCost = double.TryParse(Vesselssheet.Cells[i, 7].Text, out double acost) ? acost : 0,
                                        Hireasrequired = Vesselssheet.Cells[i, 8].Text,
                                        VesselLeadtime = double.TryParse(Vesselssheet.Cells[i, 9].Text, out double vlead) ? vlead : 0,
                                        //rent properties
                                        RentalStartDay = int.TryParse(Vesselssheet.Cells[i, 10].Text, out int rsd) ? rsd : 0,
                                        RentalEndDay = int.TryParse(Vesselssheet.Cells[i, 11].Text, out int red) ? red : 0,
                                        RentalStartMonth = int.TryParse(Vesselssheet.Cells[i, 12].Text, out int rsm) ? rsm : 0,
                                        RentalEndMonth = int.TryParse(Vesselssheet.Cells[i, 13].Text, out int rem) ? rem : 0,
                                        DailyRentalCost = double.TryParse(Vesselssheet.Cells[i, 14].Text, out double drc) ? drc : 0,
                                        MobilizationCost = double.TryParse(Vesselssheet.Cells[i, 15].Text, out double mcost) ? mcost : 0,
                                        FuelConsumption = double.TryParse(Vesselssheet.Cells[i, 16].Text, out double fc) ? fc : 0,
                                        FuelCost = double.TryParse(Vesselssheet.Cells[i, 17].Text, out double fcost) ? fcost : 0,
                                        Speed = double.TryParse(Vesselssheet.Cells[i, 18].Text, out double speed) ? speed : 0,
                                        Purchased = pur
                                    };
                                    TotalVessels.AddVessel(vtab);
                                }

                            }
                        }

                        var vcheck = validCheck.CheckVesselsValidation();
                        if (!vcheck.Result)
                            errorFound = true;

                    }

                    #endregion

                    #region Installation strategy
                    if (!errorFound)
                    {
                        UpdatePrjectStatusText("loading installation strategy....");
                        InstStrategyDetails.Instalstartmonth = Installationsheet.Cells[1, 2].Text;
                        InstStrategyDetails.ProjectCommissioning = double.TryParse(Installationsheet.Cells[2, 2].Text, out double ism) ? ism : 0;
                        InstStrategyDetails.Projectmanagement = double.TryParse(Installationsheet.Cells[3, 2].Text, out double pm) ? pm : 0;
                        InstStrategyDetails.InstallationContingency = double.TryParse(Installationsheet.Cells[4, 2].Text, out double ic) ? ic : 0;
                        InstStrategyDetails.Installationcost = double.TryParse(Installationsheet.Cells[5, 2].Text, out double icost) ? icost : 0;
                    }
                    UpdatePrjectStatusText("loading installation data....");
                    for (int i = 2; i < 4; i++)
                    {
                        if (!string.IsNullOrEmpty(Installationdatasheet.Cells[i, 1].Text))
                        {
                            //save in temp obj
                            var Itab = new InstallationDetails
                            {
                                Taskname = Installationdatasheet.Cells[i, 3].Text,
                                TaskDescription = Installationdatasheet.Cells[i, 4].Text,
                                NoOftechsReq = int.TryParse(Installationdatasheet.Cells[i, 5].Text, out int ntechs) ? ntechs : 0,
                                VesselReq = Installationdatasheet.Cells[i, 6].Text,
                                Numberofdevicespervessel = int.TryParse(Installationdatasheet.Cells[i, 7].Text, out int ndpv) ? ndpv : 0,
                                Base = Installationdatasheet.Cells[i, 8].Text,
                                OperationDuration = double.TryParse(Installationdatasheet.Cells[i, 9].Text, out double od) ? od : 0,
                                Waveheightlimit = double.TryParse(Installationdatasheet.Cells[i, 10].Text, out double wh) ? wh : 0,
                                Waveperiodlimit = double.TryParse(Installationdatasheet.Cells[i, 11].Text, out double wp) ? wp : 0,
                                Windspeedlimit = double.TryParse(Installationdatasheet.Cells[i, 12].Text, out double wpl) ? wpl : 0,
                                Currentvelocitylimit = double.TryParse(Installationdatasheet.Cells[i, 13].Text, out double cl) ? cl : 0,
                                InstallType = i == 2 ? InstallType.Device : InstallType.SubStructure
                            };
                            TotalInstallations.Add(Itab);
                        }
                    }

                    var icheck = validCheck.ChekInstallationValidation();
                    if (!icheck.Result)
                        errorFound = true;

                    #endregion

                    #region Preventives
                    if (!errorFound)
                    {
                        PreventiveStartEnd.StartMonth = (Preventivessheet.Cells[1, 2].Text);
                        PreventiveStartEnd.EndMonth = (Preventivessheet.Cells[2, 2].Text);
                        PreventiveStartEnd.AdditionalAnnualCost = double.TryParse(Preventivessheet.Cells[3, 2].Text, out double aac) ? aac : 0;
                    }
                    UpdatePrjectStatusText("loading preventive data....");
                    for (int i = 2; i < 11; i++)
                    {
                        if (!string.IsNullOrEmpty(Preventivesdatasheet.Cells[i, 1].Text))
                        {
                            //save in temp obj
                            var PMtab = new PriventiveDetails
                            {
                                PMCategory = Preventivesdatasheet.Cells[i, 3].Text,
                                Taskdescription = Preventivesdatasheet.Cells[i, 4].Text,
                                NoOftechsReq = int.TryParse(Preventivesdatasheet.Cells[i, 5].Text, out int ntr) ? ntr : 0,
                                VesselReq = Preventivesdatasheet.Cells[i, 6].Text,
                                Base = Preventivesdatasheet.Cells[i, 7].Text,
                                Frequency = double.TryParse(Preventivesdatasheet.Cells[i, 8].Text, out double f) ? f : 0,
                                OperationLOC = !string.IsNullOrEmpty(Preventivesdatasheet.Cells[i, 9].Text) ? GetValueFromDescription<OperationalLocation>(Preventivesdatasheet.Cells[i, 9].Text) : OperationalLocation.Onshore,
                                OprDurationOffs = double.TryParse(Preventivesdatasheet.Cells[i, 10].Text, out double odo) ? odo : 0,
                                OprDurationOns = double.TryParse(Preventivesdatasheet.Cells[i, 11].Text, out double odon) ? odon : 0,
                                Waveheightlimit = double.TryParse(Preventivesdatasheet.Cells[i, 12].Text, out double whl) ? whl : 0,
                                Waveperiodlimit = double.TryParse(Preventivesdatasheet.Cells[i, 13].Text, out double wpl) ? wpl : 0,
                                Windspeedlimit = double.TryParse(Preventivesdatasheet.Cells[i, 14].Text, out double wsl) ? wsl : 0,
                                CurrentVelocityLimit = double.TryParse(Preventivesdatasheet.Cells[i, 15].Text, out double cvl) ? cvl : 0,
                                Powerloss = double.TryParse(Preventivesdatasheet.Cells[i, 16].Text, out double pl) ? pl : 0,
                                Sparepart = double.TryParse(Preventivesdatasheet.Cells[i, 17].Text, out double sp) ? sp : 0
                            };
                            TotalPriventives.Add(PMtab);
                        }
                    }

                    var pmcheck = validCheck.CheckPMValidation();
                    if (!pmcheck.Result)
                        errorFound = true;

                    #endregion

                    #region Repairs grid data
                    if (!errorFound)
                    {
                        UpdatePrjectStatusText("loading repairs....");
                        for (int i = 2; i < 100; i++)
                        {
                            if (!string.IsNullOrEmpty(Repairssheet.Cells[i, 1].Text))
                            {
                                //save in temp obj
                                var Reptab = new RepairDetails
                                {
                                    RepairName = Repairssheet.Cells[i, 3].Text,
                                    RepairDesc = Repairssheet.Cells[i, 4].Text,
                                    NoOfTechs = int.TryParse(Repairssheet.Cells[i, 5].Text, out int ntechs) ? ntechs : 0,
                                    Vesselrequired = Repairssheet.Cells[i, 6].Text,
                                    Base = Repairssheet.Cells[i, 7].Text,
                                    Operationlocation = !string.IsNullOrEmpty(Repairssheet.Cells[i, 8].Text) ? GetValueFromDescription<OperationalLocation>(Repairssheet.Cells[i, 8].Text) : OperationalLocation.Onshore,
                                    OperationdurationOffshore = double.TryParse(Repairssheet.Cells[i, 9].Text, out double odo) ? odo : 0,
                                    OperatondurationOnshore = double.TryParse(Repairssheet.Cells[i, 10].Text, out double odon) ? odon : 0,
                                    Waveheightlimit = double.TryParse(Repairssheet.Cells[i, 11].Text, out double whl) ? whl : 0,
                                    Waveperiodlimit = double.TryParse(Repairssheet.Cells[i, 12].Text, out double wpl) ? wpl : 0,
                                    Windspeedlimit = double.TryParse(Repairssheet.Cells[i, 13].Text, out double wsl) ? wsl : 0,
                                    Currentvelocitylimit = double.TryParse(Repairssheet.Cells[i, 14].Text, out double cvl) ? cvl : 0,
                                    Powerloss = double.TryParse(Repairssheet.Cells[i, 15].Text, out double pl) ? pl : 0
                                };
                                TotalRepairs.Add(Reptab);
                            }
                        }
                    }

                    var repcheck = validCheck.CheckRepairsValidation();
                    if (!repcheck.Result)
                        errorFound = true;

                    #endregion

                    #region Load Components grid
                    if (!errorFound)
                    {
                        UpdatePrjectStatusText("loading components....");
                        for (int i = 2; i < 101; i++)
                        {
                            if (!string.IsNullOrEmpty(Comoponentssheet.Cells[i, 1].Text))
                            {
                                //save in temp obj
                                var cmptab = new ComponentDetails()
                                {
                                    Componentname = Comoponentssheet.Cells[i, 3].Text,
                                    Numberperdevice = int.TryParse(Comoponentssheet.Cells[i, 4].Text, out int npd) ? npd : 0,
                                    AnnualFailRate = double.TryParse(Comoponentssheet.Cells[i, 5].Text, out double afr) ? afr : 0,
                                    Repair = Comoponentssheet.Cells[i, 6].Text,
                                    SpareParts = double.TryParse(Comoponentssheet.Cells[i, 7].Text, out double sp) ? sp : 0
                                };
                                TotalComponents.Add(cmptab);
                            }
                        }
                    }
                    #endregion

                    var compcheck = validCheck.CheckComponentsValidation();
                    if (!compcheck.Result)
                        errorFound = true;
                    UpdatePrjectStatusText("Loading completed successfully!");

                    if (errorFound)
                        return false;
                    else
                        return true;
                }
                catch (Exception ex)
                {
                    _ = MessageBox.Show("Error occured while loading file " + filePath + ".  Error: " + ex.ToString());
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                if (closeWB)
                {
                    if (xlWorkBook != null)
                    {
                        xlWorkBook.Close();
                        xlApp.Quit();
                        if (Projectsheet != null)
                            _ = Marshal.ReleaseComObject(Projectsheet);
                        if (Farmsheet != null)
                            _ = Marshal.ReleaseComObject(Farmsheet);
                        if (Resourcesheet != null)
                            _ = Marshal.ReleaseComObject(Resourcesheet);
                        if (Basessheet != null)
                            _ = Marshal.ReleaseComObject(Basessheet);
                        if (Vesselssheet != null)
                            _ = Marshal.ReleaseComObject(Vesselssheet);
                        if (Installationsheet != null)
                            _ = Marshal.ReleaseComObject(Installationsheet);
                        if (Installationdatasheet != null)
                            _ = Marshal.ReleaseComObject(Installationdatasheet);
                        if (Preventivessheet != null)
                            _ = Marshal.ReleaseComObject(Preventivessheet);
                        if (Preventivesdatasheet != null)
                            _ = Marshal.ReleaseComObject(Preventivesdatasheet);
                        if (Repairssheet != null)
                            _ = Marshal.ReleaseComObject(Repairssheet);
                        if (Comoponentssheet != null)
                            _ = Marshal.ReleaseComObject(Comoponentssheet);

                        _ = Marshal.ReleaseComObject(xlWorkBook);
                        _ = Marshal.ReleaseComObject(xlApp);
                    }
                }
            }
        }

        private bool DelProject()
        {

            string filename = loadStatus.Text;
            string filePath = Path.Combine(DataFolders.ProjectsFolder, filename);
            try
            {
                UpdatePrjectStatusText("project deletion in process....");
                File.Delete(filePath);
                UpdatePrjectStatusText("project deleted successfully!");

                return true;
            }
            catch (IOException)
            {
                _ = MessageBox.Show("File being used by other process. Please try again later.");
            }
            catch (Exception)
            {
                //log
                _ = MessageBox.Show("Error occured while deleting file. Please try again.");
            }
            return false;
        }
        private async void projectsDataGrid_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {
            projectsDataGrid.Enabled = false;
            int cellIndex = this.projectsDataGrid.CurrentRow.Index;
            if (cellIndex > -1)
            {
                if (projectsDataGrid.CurrentRow.Cells[1].Value == null)
                    return;
                string filename = projectsDataGrid.CurrentRow.Cells[1].Value.ToString();
                loadStatus.Text = filename;
                loadStatus.Visible = true;

                //check column index
                int colIndex = this.projectsDataGrid.CurrentCell.ColumnIndex;
                //load
                if (colIndex == 2)
                {
                    Task<bool> _loadingProj = new Task<bool>(LoadProject);
                    _loadingProj.Start();
                    if (await _loadingProj)
                        loadStatus.Text = projectsDataGrid.CurrentRow.Cells[1].Value + " project loaded successfully!";
                    else
                    {
                        //show exact error
                        loadStatus.Text = projectsDataGrid.CurrentRow.Cells[1].Value + " project FAILED to load.";
                    }
                }
                else if (colIndex == 3) //delete
                {

                    //delete file from folder
                    //get file name to delete
                    Task<bool> _delProj = new Task<bool>(DelProject);

                    if (MessageBox.Show("Do you want to remove this row", "Remove row",
                           MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        //user said yes
                        _delProj.Start();
                        if (await _delProj)
                        {
                            //pass
                            loadStatus.Text = projectsDataGrid.CurrentRow.Cells[1].Value + " project deleted successfully!";
                            FillTableWithProjects();
                        }
                        else
                        {
                            //fail
                            loadStatus.Text = projectsDataGrid.CurrentRow.Cells[1].Value + " project FAILED to Delete.";
                        }
                    }
                    else
                    {
                        //user said no
                    }
                }
            }
            projectsDataGrid.Enabled = true;
        }
        public void ExportProjectStatusText(string _exporttext)
        {
            if (this.labelExport.InvokeRequired)
            {
                SetStatusText s = new SetStatusText(ExportProjectStatusText);
                _ = this.Invoke(s, new object[] { _exporttext });
            }
            else
            {
                labelExport.Text = _exporttext;
            }
        }
        public void UpdatePrjectStatusText(string _updatetext)
        {
            if (this.loadStatus.InvokeRequired)
            {
                SetStatusText s = new SetStatusText(UpdatePrjectStatusText);
                _ = this.Invoke(s, new object[] { _updatetext });
            }
            else
            {
                loadStatus.Text = _updatetext;
            }
        }
        public void UpdatePrjectProgress(int percentage)
        {
            if (this.progressBar2.InvokeRequired)
            {
                SetProjectprogress s = new SetProjectprogress(UpdatePrjectProgress);
                _ = this.Invoke(s, new object[] { percentage });
            }
            else
            {
                progressBar2.Value = percentage;
            }
        }

        public T GetValueFromDescription<T>(string description) where T : Enum
        {
            foreach (var field in typeof(T).GetFields())
            {
                if (Attribute.GetCustomAttribute(field,
                typeof(DescriptionAttribute)) is DescriptionAttribute attribute)
                {
                    if (attribute.Description == description)
                        return (T)field.GetValue(null);
                }
                else
                {
                    if (field.Name == description)
                        return (T)field.GetValue(null);
                }
            }

            throw new ArgumentException("Not found.", nameof(description));
            // Or return default(T);
        }

        private void newprojectBtn_Click(object sender, EventArgs e)
        {
            CleanData();
            this.Close();

            //newprojectBtn_Click(frm1.buttonProjectDetails, e);


        }

        private void CleanData()
        {
            #region fill data ProjectDetails
            ProjectDetails.ProjectName = "";
            ProjectDetails.GridSaleRate = 0;
            ProjectDetails.InstallTime = 0;
            ProjectDetails.ProjectLifeTime = 0;
            ProjectDetails.NoOfIterations = 0;
            ProjectDetails.WakeLoss = 0;
            ProjectDetails.TransLoss = 0;
            ProjectDetails.ProdLoss = 0;
            #endregion

            #region fill farm details
            FarmDetails.ManufatureName = "";
            FarmDetails.NoOfDivices = 0;
            FarmDetails.Substructretype = "";
            FarmDetails.Substructrename = "";
            FarmDetails.PowerCurve = "";
            FarmDetails.Distancebetweendevices = 0;
            FarmDetails.TechType = "";

            PowerCurveData.Reset();
            #endregion

            #region resource
            ResourceDetails.Locationname = "";
            ResourceDetails.Waterdepth = 0;
            ResourceDetails.Mesurementheight = 0;
            ResourceDetails.Specifymetociandata = "";
            _ = WeatherYearlyData.Reset();
            #endregion

            #region bases
            TotalBases.InitList();
            #endregion

            #region vessels
            TotalVessels.Init();
            #endregion

            #region installStrat
            InstStrategyDetails.Instalstartmonth = "0";
            InstStrategyDetails.InstallationContingency = 0;
            InstStrategyDetails.Installationcost = 0;
            InstStrategyDetails.ProjectCommissioning = 0;
            InstStrategyDetails.Projectmanagement = 0;

            TotalInstallations.Init();

            #endregion

            #region PM
            TotalPriventives.Init();
            PreventiveStartEnd.AdditionalAnnualCost = 0;
            PreventiveStartEnd.StartMonth = "1";
            PreventiveStartEnd.EndMonth = "1";

            #endregion

            #region repairs
            TotalRepairs.Init();





            #endregion

            #region components
            TotalComponents.Init();
            #endregion
        }
        /* public void UpdateThreadedText(Label control, string text)
         {
             Action action = () => control.Text = text;
             control.Invoke(action);
         }*/
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            for (int i = 1; i <= 100; i++)
            {
                Thread.Sleep(70);
                backgroundWorker1.WorkerReportsProgress = true;
                backgroundWorker1.ReportProgress(i);
            }
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            // progressBar2.Value = e.ProgressPercentage;
            /*label3Load.Text = e.ProgressPercentage.ToString() + "%";
            if(label3Load.Text == "100%")
            {
                label4Success.Text = "Imporeted successfully...";
                #region All text fields
                ProjectDetails.ProjectName = "";
                ProjectDetails.GridSaleRate = 0;
                ProjectDetails.InstallTime = 0;
                ProjectDetails.ProjectLifeTime = 0;
                ProjectDetails.NoOfIterations = 0;
                ProjectDetails.WakeLoss = 0;
                ProjectDetails.TransLoss = 0;
                ProjectDetails.ProdLoss = 0;
                #endregion
            }*/

        }
        public void UpdateAllStatus(int percentage, bool prgSt)
        {
            if (!prgSt)
            {
                if (percentage > 0)
                    UpdatePrjectProgress(percentage);

            }
            else
            {
                if (percentage > 0)
                    UpdatePrjectProgress(percentage);

            }

        }

        private void BtnExporttoExcel_MouseHover(object sender, EventArgs e)
        {
            this.toolTip1.SetToolTip(BtnExporttoExcel, "Export current project to excel.");
        }

        private void newprojectBtn_MouseHover(object sender, EventArgs e)
        {
            this.toolTip1.SetToolTip(newprojectBtn, "This clears information loaded and set to clean start.");
        }




    }


}
