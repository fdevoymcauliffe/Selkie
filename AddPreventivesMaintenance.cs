using SELKIE.Enums;
using SELKIE.Logic;
using SELKIE.Models;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace SELKIE
{
    public partial class AddPreventivesMaintenance : Form
    {
        public static PriventiveDetails _priventiveDetails = new PriventiveDetails();
        public ValidationCheck validCheck = new ValidationCheck();
        int selIndex = -1;
        private readonly PM_Maintenance _PM_Maintenance;
        public AddPreventivesMaintenance(int inx, PM_Maintenance PM_Maintenance)
        {
            _PM_Maintenance = PM_Maintenance;
            selIndex = inx;
            InitializeComponent();

            foreach (var item in TotalBases.GetBases())
            {
                _ = comboBoxBasetype.Items.Add(item.Basename);
            }
            foreach (var item in TotalVessels.GetVessels())
            {
                _ = comboBoxVesselrequire.Items.Add(item.VesselClassif);
            }
            FillItems();
        }
        private void initFieldsFromObj()
        {
            //Preventive data data fields
            textBoxPMcategory.Text = "";
            textBoxTaskDescription.Text = "";
            textBoxNumberofTEchs.Text = "";
            comboBoxVesselrequire.Text = "";
            comboBoxBasetype.Text = "";
            textBoxFrequency.Text = "";
            comboBoxOprloc.Text = OperationalLocation.Onshore.ToString();
            textBoxOPrDurationoffshore.Text = "";
            textBoxDurationOnshore.Text = "";
            textBoxWaveheightOffs.Text = "";
            textBoxWaveperiodLimitOffs.Text = "";
            textBoxwindspeedLimitrepair.Text = "";
            textBoxCurrentVelocity.Text = "";
            textBoxPowerloss.Text = "";
            textBoxSparePart.Text = "";
            selIndex = -1;
            buttonSavePreventivedata.Text = "Save";
            ClosePreventiveForm.Visible = true;
        }
        private void FillItems()
        {
            if (selIndex > -1) //update item
            {
                //get item based on index
                var selObj = TotalPriventives.GetObj(selIndex);
                //update with data
                textBoxPMcategory.Text = selObj.PMCategory;
                textBoxTaskDescription.Text = selObj.Taskdescription;
                textBoxNumberofTEchs.Text = selObj.NoOftechsReq.ToString();
                comboBoxVesselrequire.Text = selObj.VesselReq;
                comboBoxBasetype.Text = selObj.Base;
                textBoxFrequency.Text = selObj.Frequency.ToString();
                comboBoxOprloc.Text = selObj.OperationLOC.ToString();
                textBoxOPrDurationoffshore.Text = selObj.OprDurationOffs.ToString();
                textBoxDurationOnshore.Text = selObj.OprDurationOns.ToString();
                textBoxWaveheightOffs.Text = selObj.Waveheightlimit.ToString();
                textBoxWaveperiodLimitOffs.Text = selObj.Waveperiodlimit.ToString();
                textBoxwindspeedLimitrepair.Text = selObj.Windspeedlimit.ToString();
                textBoxCurrentVelocity.Text = selObj.CurrentVelocityLimit.ToString();
                textBoxPowerloss.Text = selObj.Powerloss.ToString();
                textBoxSparePart.Text = selObj.Sparepart.ToString();

                buttonSavePreventivedata.Text = "Update";

            }
            else
            {
                initFieldsFromObj();
            }
        }

        private void buttonSavePreventivedata_Click(object sender, EventArgs e)
        {
            try
            {
                if (checkPMdataValidation())
                {
                    OperationalLocation _locsel;
                    _ = Enum.TryParse(comboBoxOprloc.Text, out _locsel);
                    //save in temp obj
                    var PMtab = new PriventiveDetails
                    {
                        PMCategory = textBoxPMcategory.Text,
                        Taskdescription = textBoxTaskDescription.Text,
                        NoOftechsReq = Convert.ToInt32(textBoxNumberofTEchs.Text),
                        VesselReq = comboBoxVesselrequire.Text,
                        Base = comboBoxBasetype.Text,
                        Frequency = Convert.ToDouble(textBoxFrequency.Text),
                        OperationLOC = _locsel,
                        OprDurationOffs = Convert.ToDouble(textBoxOPrDurationoffshore.Text),
                        OprDurationOns = Convert.ToDouble(textBoxDurationOnshore.Text),
                        Waveheightlimit = Convert.ToDouble(textBoxWaveheightOffs.Text),
                        Waveperiodlimit = Convert.ToDouble(textBoxWaveperiodLimitOffs.Text),
                        Windspeedlimit = Convert.ToDouble(textBoxwindspeedLimitrepair.Text),
                        CurrentVelocityLimit = Convert.ToDouble(textBoxCurrentVelocity.Text),
                        Powerloss = Convert.ToDouble(textBoxPowerloss.Text),
                        Sparepart = Convert.ToDouble(textBoxSparePart.Text)
                    };
                    //dataGridViewPMTask.Show();

                    //udpating
                    if (selIndex > -1)
                    {
                        TotalPriventives.Update(selIndex, PMtab);
                        FillItems();
                        initFieldsFromObj();
                        //update main grid new updates
                        _PM_Maintenance.FillPMdt();
                        //retrun to main grid
                        this.Close();
                    }
                    else // new
                    {
                        if (TotalPriventives.GetAllPriventives().Count <= 1)
                        {
                            TotalPriventives.Add(PMtab);
                            FillItems();
                            initFieldsFromObj();
                            _PM_Maintenance.FillPMdt();
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

                //MessageBox.Show("Fill the all required fields...!");
            }

        }
        private bool checkPMdataValidation()
        {
            bool validcheckresult = true;
            #region PMCategory check
            label10.ForeColor = Color.Red;
            if (!string.IsNullOrEmpty(textBoxPMcategory.Text))
            {
                _priventiveDetails.PMCategory = textBoxPMcategory.Text;
                label10.ForeColor = Color.Black;
            }
            else
                validcheckresult = false;
            #endregion

            #region Numberoftechs Required check
            label8.ForeColor = Color.Red;
            int convertedValue1 = validCheck.CheckIfInt(textBoxNumberofTEchs.Text);
            if (convertedValue1 > 0)
            {
                //update obj
                _priventiveDetails.NoOftechsReq = convertedValue1;
                label8.ForeColor = Color.Black;
            }
            else
                validcheckresult = false;
            #endregion

            #region Vesselrequired check
            label7.ForeColor = Color.Red;
            if (!string.IsNullOrEmpty(comboBoxVesselrequire.Text))
            {
                //update obj
                _priventiveDetails.VesselReq = comboBoxVesselrequire.Text;
                label7.ForeColor = Color.Black;
            }
            else
                validcheckresult = false;
            #endregion

            #region Base check
            label5.ForeColor = Color.Red;
            if (string.IsNullOrEmpty(comboBoxBasetype.Text))
                validcheckresult = false;
            else
            {
                //update obj
                _priventiveDetails.Base = comboBoxBasetype.Text;
                label5.ForeColor = Color.Black;
            }
            #endregion

            #region Frequency check
            label29.ForeColor = Color.Red;
            double convertedValue4 = validCheck.CheckIfDouble(textBoxFrequency.Text);
            if (convertedValue4 >= 0)
            {
                //update obj
                _priventiveDetails.Frequency = convertedValue4;
                label29.ForeColor = Color.Black;
            }
            else
                validcheckresult = false;
            #endregion

            #region Opeartionlocation check
            label27.ForeColor = Color.Red;
            if (string.IsNullOrEmpty(comboBoxOprloc.Text))
            {
                validcheckresult = false;
            }
            else
            {
                OperationalLocation _locsel;
                _ = Enum.TryParse(comboBoxOprloc.Text, out _locsel);
                //update obj
                _priventiveDetails.OperationLOC = _locsel;
                label27.ForeColor = Color.Black;
            }
            #endregion

            #region Operationduration Offshore check
            label26.ForeColor = Color.Red;
            double convertedValue6 = validCheck.CheckIfDouble(textBoxOPrDurationoffshore.Text);
            if (convertedValue6 >= 0)
            {
                //update obj
                _priventiveDetails.OprDurationOffs = convertedValue6;
                label26.ForeColor = Color.Black;
            }
            else
                validcheckresult = false;
            #endregion

            #region Operationduration Onshore check
            label15.ForeColor = Color.Red;
            double convertedValue7 = validCheck.CheckIfDouble(textBoxDurationOnshore.Text);
            if (convertedValue7 >= 0)
            {
                //update obj
                _priventiveDetails.OprDurationOns = convertedValue7;
                label15.ForeColor = Color.Black;
            }
            else
                validcheckresult = false;
            #endregion

            #region Waveheight Limit check
            label14.ForeColor = Color.Red;
            double convertedValue8 = validCheck.CheckIfDouble(textBoxWaveheightOffs.Text);
            if (convertedValue8 >= 0)
            {
                //update obj
                _priventiveDetails.Waveheightlimit = convertedValue8;
                label14.ForeColor = Color.Black;
            }
            else
                validcheckresult = false;
            #endregion

            #region Waveperiod Limit check
            label11.ForeColor = Color.Red;
            double convertedValue9 = validCheck.CheckIfDouble(textBoxWaveperiodLimitOffs.Text);
            if (convertedValue9 >= 0)
            {
                //update obj
                _priventiveDetails.Waveperiodlimit = convertedValue9;
                label11.ForeColor = Color.Black;
            }
            else
                validcheckresult = false;
            #endregion

            #region Windspeed Limit check
            label13.ForeColor = Color.Red;
            double convertedValue10 = validCheck.CheckIfDouble(textBoxwindspeedLimitrepair.Text);
            if (convertedValue10 >= 0)
            {
                //update obj
                _priventiveDetails.Windspeedlimit = convertedValue10;
                label13.ForeColor = Color.Black;
            }
            else
                validcheckresult = false;
            #endregion

            #region Currentvelocitylimit perPM check
            label12.ForeColor = Color.Red;
            double convertedValue11 = validCheck.CheckIfDouble(textBoxCurrentVelocity.Text);
            if (convertedValue11 > 0)
            {
                //update obj
                _priventiveDetails.CurrentVelocityLimit = convertedValue11;
                label12.ForeColor = Color.Black;
            }
            else
                validcheckresult = false;
            #endregion

            #region Powerloss check
            label34.ForeColor = Color.Red;
            double convertedValue12 = validCheck.CheckIfDouble(textBoxPowerloss.Text);
            if (convertedValue12 >= 0)
            {
                //update obj
                _priventiveDetails.Powerloss = convertedValue12;
                label34.ForeColor = Color.Black;
            }
            else
                validcheckresult = false;
            #endregion

            #region Sparepart check
            label33.ForeColor = Color.Red;
            double convertedValue13 = validCheck.CheckIfDouble(textBoxSparePart.Text);
            if (convertedValue13 >= 0)
            {
                //update obj
                _priventiveDetails.Sparepart = convertedValue13;
                label33.ForeColor = Color.Black;
            }
            else
                validcheckresult = false;
            #endregion

            return validcheckresult;
        }

        private void ClosePreventiveForm_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void comboBoxOprloc_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxOprloc.SelectedItem.ToString() == "Offshore")
            {
                label15.Hide();
                textBoxDurationOnshore.Hide();
            }
            else
            {
                label15.Show();
                textBoxDurationOnshore.Show();
            }
        }



    }
}
