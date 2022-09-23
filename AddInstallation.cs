using SELKIE.Enums;
using SELKIE.Logic;
using SELKIE.Models;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace SELKIE
{
    public partial class AddInstallation : Form
    {
        public static InstallationDetails _InstallationDetails = new InstallationDetails();
        public ValidationCheck validCheck = new ValidationCheck();
        int selIndex = -1;
        private readonly Installation_Strategy inst;

        public AddInstallation(int inx, Installation_Strategy inst)
        {
            selIndex = inx;
            this.inst = inst;
            InitializeComponent();
            foreach (var item in TotalBases.GetBases())
            {
                _ = comboBoxBase.Items.Add(item.Basename);
            }
            foreach (var item in TotalVessels.GetVessels())
            {
                _ = comboBoxVesselrequired.Items.Add(item.VesselClassif);
            }

            FillItems();
        }
        private void initFieldsFromObj()
        {

            //Second PanelData
            textBoxTaskNmae.Text = "";
            textBoxTaskDescription.Text = "";
            textBoxNumberofTEchs.Text = "";
            comboBoxVesselrequired.Text = "";
            textBoxNoOfdevicespervessel.Text = "";
            comboBoxBase.Text = "";
            textBoxOpeartionDuration.Text = "";
            textBoxWaveheightlimit.Text = "";
            textBoxWaveperiodLimit.Text = "";
            textBoxwindspeedLimitrepair.Text = "";
            textBoxCurrentVelocity.Text = "";
            selIndex = -1;
            buttonSaveInstallDt.Text = "Save";
            CloseInstallationForm.Visible = true;
            DeviceOrSubLbl.Text = "-" + (TotalInstallations.GetInstalls().Count == 0 ? InstallType.Device.ToString() : InstallType.SubStructure.ToString());

        }
        private void FillItems()
        {
            if (selIndex > -1) //update item
            {
                //get item based on index
                var selObj = TotalInstallations.GetObj(selIndex);
                //update with data
                comboBoxVesselrequired.Text = selObj.VesselReq;
                textBoxTaskNmae.Text = selObj.Taskname;
                textBoxTaskDescription.Text = selObj.TaskDescription;
                textBoxNumberofTEchs.Text = selObj.NoOftechsReq.ToString();
                textBoxNoOfdevicespervessel.Text = selObj.Numberofdevicespervessel.ToString();
                comboBoxBase.Text = selObj.Base;
                textBoxOpeartionDuration.Text = selObj.OperationDuration.ToString();
                textBoxWaveheightlimit.Text = selObj.Waveheightlimit.ToString();
                textBoxWaveperiodLimit.Text = selObj.Waveperiodlimit.ToString();
                textBoxwindspeedLimitrepair.Text = selObj.Windspeedlimit.ToString();
                textBoxCurrentVelocity.Text = selObj.Currentvelocitylimit.ToString();
                DeviceOrSubLbl.Text = selObj.InstallType.ToString();
                buttonSaveInstallDt.Text = "Update";

            }
            else
            {
                initFieldsFromObj();
            }
        }

        private void buttonSaveInstallDt_Click(object sender, EventArgs e)
        {
            try
            {
                var testt = TotalInstallations.GetInstalls();

                //validation check
                if (CheckInstallationValidation())
                {
                    //save in temp obj
                    var Itab = new InstallationDetails
                    {
                        Taskname = textBoxTaskNmae.Text,
                        TaskDescription = textBoxTaskDescription.Text,
                        NoOftechsReq = Convert.ToInt32(textBoxNumberofTEchs.Text),
                        VesselReq = comboBoxVesselrequired.Text,
                        Numberofdevicespervessel = Convert.ToInt32(textBoxNoOfdevicespervessel.Text),
                        Base = comboBoxBase.Text,
                        OperationDuration = Convert.ToDouble(textBoxOpeartionDuration.Text),
                        Waveheightlimit = Convert.ToDouble(textBoxWaveheightlimit.Text),
                        Waveperiodlimit = Convert.ToDouble(textBoxWaveperiodLimit.Text),
                        Windspeedlimit = Convert.ToDouble(textBoxwindspeedLimitrepair.Text),
                        Currentvelocitylimit = Convert.ToDouble(textBoxCurrentVelocity.Text),
                        //InstallType = TotalInstallations.GetInstalls().Count == 0 ? InstallType.Device : InstallType.SubStructure,
                    };
                    // dataGridViewInstask.Show();

                    //udpating
                    if (selIndex > -1)
                    {
                        TotalInstallations.Update(selIndex, Itab);
                        FillItems();
                        initFieldsFromObj();
                        inst.FillInstDt();
                        this.Close();
                    }
                    else // new
                    {
                        if (TotalInstallations.GetDevicesOnly().Count <= 1)
                        {
                            Itab.InstallType = TotalInstallations.GetInstalls().Count == 0 ? InstallType.Device : InstallType.SubStructure;
                            TotalInstallations.Add(Itab);
                            FillItems();
                            initFieldsFromObj();
                            inst.FillInstDt();
                            this.Close();
                        }
                        else
                        {
                            limitWarningLbl.Text = "";
                            this.limitWarningLbl.Text = "You reached max limit";
                            limitWarningLbl.Visible = true;
                        }

                    }
                }
            }
            catch (Exception)
            {
                //log error
            }
        }
        private bool CheckInstallationValidation()
        {
            bool validcheckresult = true;

            #region Taskname check
            label12.ForeColor = Color.Red;
            if (!string.IsNullOrEmpty(textBoxTaskNmae.Text))
            {
                //update obj
                _InstallationDetails.Taskname = textBoxTaskNmae.Text;
                label12.ForeColor = Color.Black;
            }
            else
                validcheckresult = false;
            #endregion

            #region NumberofTechs Reqquired check
            label10.ForeColor = Color.Red;
            int convertedValue1 = validCheck.CheckIfInt(textBoxNumberofTEchs.Text);
            if (convertedValue1 > 0)
            {
                //update obj
                _InstallationDetails.NoOftechsReq = convertedValue1;
                label10.ForeColor = Color.Black;
            }
            else
                validcheckresult = false;
            #endregion

            #region Vessel Required check
            label9.ForeColor = Color.Red;
            if (!string.IsNullOrEmpty(comboBoxVesselrequired.Text))
            {
                //update obj
                _InstallationDetails.VesselReq = comboBoxVesselrequired.Text;
                label9.ForeColor = Color.Black;
            }
            else
                validcheckresult = false;
            #endregion

            #region NumberperVessel Required check
            label14.ForeColor = Color.Red;
            int convertedValue3 = validCheck.CheckIfInt(textBoxNoOfdevicespervessel.Text);
            if (convertedValue3 > 0)
            {
                //update obj
                _InstallationDetails.Numberofdevicespervessel = convertedValue3;
                label14.ForeColor = Color.Black;
            }
            else
                validcheckresult = false;
            #endregion

            #region Base check
            label13.ForeColor = Color.Red;
            if (!string.IsNullOrEmpty(comboBoxBase.Text))
            {
                //update obj
                _InstallationDetails.Base = comboBoxBase.Text;
                label13.ForeColor = Color.Black;
            }
            else
                validcheckresult = false;
            #endregion

            #region Operationduration check
            label26.ForeColor = Color.Red;
            double convertedValue5 = validCheck.CheckIfDouble(textBoxOpeartionDuration.Text);
            if (convertedValue5 >= 0)
            {
                //update obj
                _InstallationDetails.OperationDuration = convertedValue5;
                label26.ForeColor = Color.Black;
            }
            else
                validcheckresult = false;
            #endregion

            #region Waveheightlimit Operation check
            label24.ForeColor = Color.Red;
            Double convertedValue6 = validCheck.CheckIfDouble(textBoxWaveheightlimit.Text);
            if (convertedValue6 >= 0)
            {
                //update obj
                _InstallationDetails.Waveheightlimit = convertedValue6;
                label24.ForeColor = Color.Black;
            }
            else
                validcheckresult = false;
            #endregion

            #region Waveperiodlimit Opeartion check
            label21.ForeColor = Color.Red;
            Double convertedValue7 = validCheck.CheckIfDouble(textBoxWaveperiodLimit.Text);
            if (convertedValue7 >= 0)
            {
                //update obj
                _InstallationDetails.Waveperiodlimit = convertedValue7;
                label21.ForeColor = Color.Black;
            }
            else
                validcheckresult = false;
            #endregion

            #region Windspeedlimit Operation check
            label15.ForeColor = Color.Red;
            Double convertedValue8 = validCheck.CheckIfDouble(textBoxwindspeedLimitrepair.Text);
            if (convertedValue8 >= 0)
            {
                //update obj
                _InstallationDetails.Windspeedlimit = convertedValue8;
                label15.ForeColor = Color.Black;
            }
            else
                validcheckresult = false;
            #endregion

            #region Current Velocity check
            label19.ForeColor = Color.Red;
            Double convertedValue9 = validCheck.CheckIfDouble(textBoxCurrentVelocity.Text);
            if (convertedValue9 > 0)
            {
                //update obj
                _InstallationDetails.Currentvelocitylimit = convertedValue9;
                label19.ForeColor = Color.Black;
            }
            else
                validcheckresult = false;
            #endregion
            return validcheckresult;
        }
        private void CloseInstallationForm_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
