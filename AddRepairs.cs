using SELKIE.Enums;
using SELKIE.Logic;
using SELKIE.Models;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace SELKIE
{
    public partial class AddRepairs : Form
    {
        public static RepairDetails repairDetails = new RepairDetails();
        public ValidationCheck validCheck = new ValidationCheck();
        int selIndex = -1;
        private readonly Repairs _repairs;

        public AddRepairs(int inx, Repairs repairs)
        {
            _repairs = repairs;
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

            textBoxAddRepairCategory.Text = "";
            textBoxTaskDescription.Text = "";
            textBoxNumberofTEchs.Text = "";
            comboBoxVesselrequire.Text = "";
            comboBoxBasetype.Text = "";
            comboBoxOprloc.Text = OperationalLocation.Onshore.ToString();
            textBoxDurationoffshore.Text = "";
            textBoxDurationOnshore.Text = "";
            textBoxWaveheightOffs.Text = "";
            textBoxWaveperiodLimitOffs.Text = "";
            textBoxwindspeedLimitrepair.Text = "";
            textBoxCurrentVelocity.Text = "";
            textBoxPowerloss.Text = "";
            selIndex = -1;
            buttonSaveRepdata.Text = "Save";
            CloseRepairs.Visible = true;

        }
        private void FillItems()
        {

            if (selIndex > -1) //update item
            {
                //get item based on index
                var selObj = TotalRepairs.GetObj(selIndex);
                //update with data
                textBoxAddRepairCategory.Text = selObj.RepairName;
                textBoxTaskDescription.Text = selObj.RepairDesc;
                textBoxNumberofTEchs.Text = selObj.NoOfTechs.ToString();
                comboBoxVesselrequire.Text = selObj.Vesselrequired;
                comboBoxBasetype.Text = selObj.Base;
                comboBoxOprloc.Text = selObj.Operationlocation.ToString();
                textBoxDurationoffshore.Text = selObj.OperationdurationOffshore.ToString();
                textBoxDurationOnshore.Text = selObj.OperatondurationOnshore.ToString();
                textBoxWaveheightOffs.Text = selObj.Waveheightlimit.ToString();
                textBoxWaveperiodLimitOffs.Text = selObj.Waveperiodlimit.ToString();
                textBoxwindspeedLimitrepair.Text = selObj.Windspeedlimit.ToString();
                textBoxCurrentVelocity.Text = selObj.Currentvelocitylimit.ToString();
                textBoxPowerloss.Text = selObj.Powerloss.ToString();

                buttonSaveRepdata.Text = "Update";

            }
            else
            {
                initFieldsFromObj();
            }
        }

        private void buttonSaveRepdata_Click(object sender, EventArgs e)
        {
            try
            {
                //validation check
                if (CheckAddRepairValidation())
                {
                    OperationalLocation _locsel;
                    _ = Enum.TryParse(comboBoxOprloc.Text, out _locsel);
                    //save in temp obj

                    var Reptab = new RepairDetails
                    {
                        RepairName = textBoxAddRepairCategory.Text,
                        RepairDesc = textBoxTaskDescription.Text,
                        NoOfTechs = Convert.ToInt32(textBoxNumberofTEchs.Text),
                        Vesselrequired = comboBoxVesselrequire.Text,
                        Base = comboBoxBasetype.Text,
                        Operationlocation = _locsel,
                        OperationdurationOffshore = Convert.ToDouble(textBoxDurationoffshore.Text),
                        OperatondurationOnshore = Convert.ToDouble(textBoxDurationOnshore.Text),
                        Waveheightlimit = Convert.ToDouble(textBoxWaveheightOffs.Text),
                        Waveperiodlimit = Convert.ToDouble(textBoxWaveperiodLimitOffs.Text),
                        Windspeedlimit = Convert.ToDouble(textBoxwindspeedLimitrepair.Text),
                        Currentvelocitylimit = Convert.ToDouble(textBoxCurrentVelocity.Text),
                        Powerloss = Convert.ToDouble(textBoxPowerloss.Text)
                    };

                    //udpating
                    if (selIndex > -1)
                    {
                        TotalRepairs.Update(selIndex, Reptab);
                        FillItems();
                        initFieldsFromObj();
                        _repairs.FillGrid();
                        this.Close();
                    }
                    else // new
                    {
                        if (TotalRepairs.GetAllRepairs().Count <= 100)
                        {
                            TotalRepairs.Add(Reptab);
                            FillItems();
                            initFieldsFromObj();
                            _repairs.FillGrid();
                            this.Close();
                        }
                    }
                }
            }
            catch (Exception)
            {
                //log error
            }

        }
        private bool CheckAddRepairValidation()
        {
            bool validcheckresult = true;
            #region Repaircaegory check
            label8.ForeColor = Color.Red;
            if (!string.IsNullOrEmpty(textBoxAddRepairCategory.Text))
            {
                label8.ForeColor = Color.Black;
                repairDetails.RepairName = textBoxAddRepairCategory.Text;
            }
            else
                validcheckresult = false;
            #endregion

            #region Numberof techs Required check
            label3.ForeColor = Color.Red;
            int convertedValue1 = validCheck.CheckIfInt(textBoxNumberofTEchs.Text);
            if (convertedValue1 > 0)
            {
                //update obj
                repairDetails.NoOfTechs = convertedValue1;
                label3.ForeColor = Color.Black;
            }
            else
                validcheckresult = false;
            #endregion

            #region Vessel Required check
            label5.ForeColor = Color.Red;
            if (string.IsNullOrEmpty(comboBoxVesselrequire.Text))
            {
                validcheckresult = false;
            }
            else
            {
                //update obj
                repairDetails.Vesselrequired = comboBoxVesselrequire.Text;
                label5.ForeColor = Color.Black;
            }
            #endregion

            #region Base check
            label26.ForeColor = Color.Red;
            if (string.IsNullOrEmpty(comboBoxBasetype.Text))
            {
                validcheckresult = false;
            }

            else
            {
                //update obj
                repairDetails.Base = comboBoxBasetype.Text;
                label26.ForeColor = Color.Black;
            }
            #endregion

            #region Opeartion Location check
            label7.ForeColor = Color.Red;
            if (string.IsNullOrEmpty(comboBoxOprloc.Text))
            {
                validcheckresult = false;
            }

            else
            {
                OperationalLocation _locsel;
                _ = Enum.TryParse(comboBoxOprloc.Text, out _locsel);
                //update obj
                repairDetails.Operationlocation = _locsel;
                label7.ForeColor = Color.Black;
            }
            #endregion

            #region Operationduration Offshore check
            label10.ForeColor = Color.Red;
            double convertedValue5 = validCheck.CheckIfDouble(textBoxDurationoffshore.Text);
            if (convertedValue5 >= 0)
            {
                //update obj
                repairDetails.OperationdurationOffshore = convertedValue5;
                label10.ForeColor = Color.Black;
            }
            else
            {
                validcheckresult = false;
            }
            #endregion

            #region Operationduration Onshore check
            label15.ForeColor = Color.Red;
            if (repairDetails.Operationlocation == OperationalLocation.Onshore)
            {
                double convertedValue6 = validCheck.CheckIfDouble(textBoxDurationOnshore.Text);
                if (convertedValue6 >= 0)
                {
                    //update obj
                    repairDetails.OperatondurationOnshore = convertedValue6;
                    label15.ForeColor = Color.Black;
                }
                else
                {
                    validcheckresult = false;
                }
            }
            #endregion

            #region Waveheight Limit check
            label9.ForeColor = Color.Red;
            double convertedValue7 = validCheck.CheckIfDouble(textBoxWaveheightOffs.Text);
            if (convertedValue7 >= 0)
            {
                //update obj
                repairDetails.Waveheightlimit = convertedValue7;
                label9.ForeColor = Color.Black;
            }
            else
            {
                validcheckresult = false;
            }

            #endregion

            #region Waveperiod Limit check
            label11.ForeColor = Color.Red;
            double convertedValue8 = validCheck.CheckIfDouble(textBoxWaveperiodLimitOffs.Text);
            if (convertedValue8 >= 0)
            {
                //update obj
                repairDetails.Waveperiodlimit = convertedValue8;
                label11.ForeColor = Color.Black;
            }
            else
            {
                validcheckresult = false;
            }
            #endregion

            #region Windspeed Limit check
            label13.ForeColor = Color.Red;
            double convertedValue9 = validCheck.CheckIfDouble(textBoxwindspeedLimitrepair.Text);
            if (convertedValue9 >= 0)
            {
                //update obj
                repairDetails.Windspeedlimit = convertedValue9;
                label13.ForeColor = Color.Black;
            }
            else
            {
                validcheckresult = false;
            }

            #endregion

            #region Currentvelocity check
            label12.ForeColor = Color.Red;
            double convertedValue10 = validCheck.CheckIfDouble(textBoxCurrentVelocity.Text);
            if (convertedValue10 > 0)
            {
                //update obj
                repairDetails.Currentvelocitylimit = convertedValue10;
                label12.ForeColor = Color.Black;
            }
            else
            {
                validcheckresult = false;
            }

            #endregion

            #region Powrloss check
            label14.ForeColor = Color.Red;
            double convertedValue11 = validCheck.CheckIfDouble(textBoxPowerloss.Text);
            if (convertedValue11 >= 0)
            {
                //update obj
                repairDetails.Powerloss = convertedValue11;
                label14.ForeColor = Color.Black;
            }
            else
            {
                validcheckresult = false;
            }
            #endregion

            return validcheckresult;
        }

        private void CloseRepairs_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void comboBoxOprloc_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxOprloc.SelectedItem.ToString() == "Offshore")
            {
                textBoxDurationOnshore.Hide();
                label15.Hide();
                textBoxDurationOnshore.Text = "0";
            }
            else
            {
                textBoxDurationOnshore.Show();
                label15.Show();
            }

        }


    }
}
