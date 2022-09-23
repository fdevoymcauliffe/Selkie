using SELKIE.Logic;
using SELKIE.Models;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace SELKIE
{
    public partial class AddVessels : Form
    {
        public static VesselDetails VesselsTab = new VesselDetails();

        public ValidationCheck validCheck = new ValidationCheck();
        int selIndex = -1;
        private readonly Vessels vess;

        public AddVessels(int inx, Vessels vess)
        {
            selIndex = inx;
            this.vess = vess;
            InitializeComponent();
            FillItems();
        }
        private void initFieldsFromObj()
        {
            textBoxVesselClassification.Text = "";
            textBoxNumberofvessels.Text = "1";
            textBoxTechnicianCapacity.Text = "1";
            comboBoxNightWork.Text = "No";
            textBoxAnnualRunningCost.Text = "0";

            textBoxRentalStartDay.Text = "1";
            textBoxRentalSartMonth.Text = "1";

            textBoxRentalEndDay.Text = "1";
            textBoxRentalEndMonth.Text = "2";

            textBoxDailyRentalCost.Text = "0";
            textBoxMobilizationCost.Text = "0";
            textBoxFuelConsumption.Text = "0";
            textBoxFuelCost.Text = "0";
            textBoxSpeedknots.Text = "1";
            comboBoxHireasrequired.Text = "No";
            textBoxVesselleadtime.Text = "0";
            selIndex = -1;
            buttonSaveVesselType2.Text = "Save";
            CancelVessel.Visible = true;
            radioButtonPurchase.Checked = true;
            limitWarningLbl.Text = "";
            limitWarningLbl.Visible = false;
        }
        private void FillItems()
        {
            try
            {
                if (selIndex > -1) //update item
                {
                    //get item based on index
                    var selObj = TotalVessels.GetObj(selIndex);
                    //update with data
                    textBoxVesselClassification.Text = selObj.VesselClassif;
                    textBoxNumberofvessels.Text = selObj.Number.ToString();
                    textBoxTechnicianCapacity.Text = selObj.TechsCapacity.ToString();
                    comboBoxNightWork.Text = selObj.NightWork;
                    textBoxAnnualRunningCost.Text = selObj.AnnualrunningCost.ToString();
                    textBoxRentalStartDay.Text = selObj.RentalStartDay.ToString();
                    textBoxRentalEndDay.Text = selObj.RentalEndDay.ToString();
                    textBoxRentalSartMonth.Text = selObj.RentalStartMonth.ToString();
                    textBoxRentalEndMonth.Text = selObj.RentalEndMonth.ToString();
                    textBoxDailyRentalCost.Text = selObj.DailyRentalCost.ToString();
                    textBoxMobilizationCost.Text = selObj.MobilizationCost.ToString();
                    textBoxFuelConsumption.Text = selObj.FuelConsumption.ToString();
                    textBoxFuelCost.Text = selObj.FuelCost.ToString();
                    textBoxSpeedknots.Text = selObj.Speed.ToString();
                    comboBoxHireasrequired.Text = selObj.Hireasrequired;
                    textBoxVesselleadtime.Text = selObj.VesselLeadtime.ToString();
                    buttonSaveVesselType2.Text = "Update";
                    radioButtonRented.Checked = !selObj.Purchased;
                    radioButtonPurchase.Checked = selObj.Purchased;
                }
                else
                {
                    initFieldsFromObj();
                }
            }
            catch (Exception)
            {
                // log.Error(e);
            }

        }
        private void buttonSaveVesselType2_Click(object sender, EventArgs e)
        {
            try
            {
                //validation check
                if (CheckVesselValidation())
                {
                    //pass - save object
                    var vtab = new VesselDetails();
                    vtab.VesselClassif = textBoxVesselClassification.Text;
                    vtab.Number = Convert.ToInt32(textBoxNumberofvessels.Text);
                    vtab.TechsCapacity = Convert.ToInt32(textBoxTechnicianCapacity.Text);
                    vtab.NightWork = comboBoxNightWork.Text;
                    //purchase properties
                    vtab.AnnualrunningCost = radioButtonPurchase.Checked ? Convert.ToDouble(textBoxAnnualRunningCost.Text) : 0;
                    //rent properties
                    vtab.RentalStartDay = radioButtonRented.Checked ? Convert.ToInt32(textBoxRentalStartDay.Text) : 0;
                    vtab.RentalEndDay = radioButtonRented.Checked ? Convert.ToInt32(textBoxRentalEndDay.Text) : 0;
                    vtab.RentalStartMonth = radioButtonRented.Checked ? Convert.ToInt32(textBoxRentalSartMonth.Text) : 0;
                    vtab.RentalEndMonth = radioButtonRented.Checked ? Convert.ToInt32(textBoxRentalEndMonth.Text) : 0;
                    vtab.DailyRentalCost = radioButtonRented.Checked ? Convert.ToDouble(textBoxDailyRentalCost.Text) : 0;
                    vtab.MobilizationCost = radioButtonRented.Checked ? Convert.ToDouble(textBoxMobilizationCost.Text) : 0;
                    vtab.FuelConsumption = Convert.ToDouble(textBoxFuelConsumption.Text);
                    vtab.FuelCost = Convert.ToDouble(textBoxFuelCost.Text);
                    vtab.Speed = Convert.ToDouble(textBoxSpeedknots.Text);
                    vtab.Hireasrequired = comboBoxHireasrequired.Text;
                    vtab.VesselLeadtime = radioButtonRented.Checked ? Convert.ToDouble(textBoxVesselleadtime.Text) : 0;

                    vtab.Purchased = radioButtonPurchase.Checked ? true : false;


                    //udpating
                    if (selIndex > -1)
                    {
                        TotalVessels.UpdateVessel(selIndex, vtab);
                        FillItems();
                        initFieldsFromObj();
                        vess.FillVsls();
                        this.Close();
                    }
                    else // new
                    {
                        if (TotalVessels.GetVessels().Count <= 5)
                        {
                            TotalVessels.AddVessel(vtab);
                            FillItems();
                            initFieldsFromObj();
                            vess.FillVsls();
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
                else //fail - show errors
                {
                    // SaveVesselAlert.Text = "Please check validations and try again.";
                }
            }
            catch (Exception)
            {

                // MessageBox.Show("Fill the all required fields...!");
            }
        }
        private bool CheckVesselValidation()
        {
            bool validcheckresult = true;
            //check all fields one by one

            #region Vessel classification check
            vnameLbl.ForeColor = Color.Red;
            if (!string.IsNullOrEmpty(textBoxVesselClassification.Text))
            {
                if (textBoxVesselClassification.Text.Length >= 1 && textBoxVesselClassification.Text.Length <= 50)
                {
                    //update obj
                    VesselsTab.VesselClassif = textBoxVesselClassification.Text;
                    vnameLbl.ForeColor = Color.Black;
                }
                else
                    validcheckresult = false;
            }
            else
                validcheckresult = false;
            #endregion

            #region Number of Vessels check
            vnumLbl.ForeColor = Color.Red;
            int convertedValue2 = validCheck.CheckIfInt(textBoxNumberofvessels.Text);
            if (convertedValue2 > 0)
            {
                //update obj
                VesselsTab.Number = convertedValue2;
                vnumLbl.ForeColor = Color.Black;
            }
            else
                validcheckresult = false;
            #endregion

            #region TechnicianCapacity
            vtechLbl.ForeColor = Color.Red;
            int convertedValue3 = validCheck.CheckIfInt(textBoxTechnicianCapacity.Text);
            if (convertedValue3 > 0)
            {
                //update obj
                VesselsTab.TechsCapacity = convertedValue3;
                vtechLbl.ForeColor = Color.Black;
            }
            else
                validcheckresult = false;
            #endregion

            #region NightWork check
            lbl247.ForeColor = Color.Red;
            if (!string.IsNullOrEmpty(comboBoxNightWork.Text))
            {
                //update obj
                VesselsTab.NightWork = comboBoxNightWork.Text;
                lbl247.ForeColor = Color.Black;
            }
            else
                validcheckresult = false;
            #endregion

            #region Fuelconsumption check
            labelFuelConsumption.ForeColor = Color.Red;
            double convertedValue12 = validCheck.CheckIfDouble(textBoxFuelConsumption.Text);
            if (convertedValue12 >= 0)
            {
                //update obj
                VesselsTab.FuelConsumption = convertedValue12;
                labelFuelConsumption.ForeColor = Color.Black;
            }
            else
                validcheckresult = false;
            #endregion

            #region Fuelcost check
            labelFuelcost.ForeColor = Color.Red;
            double convertedValue13 = validCheck.CheckIfDouble(textBoxFuelCost.Text);
            if (convertedValue13 >= 0)
            {
                //update obj
                VesselsTab.FuelCost = convertedValue13;
                labelFuelcost.ForeColor = Color.Black;
            }
            else
                validcheckresult = false;
            #endregion

            #region Speedknots check
            labelSpeedknots.ForeColor = Color.Red;
            double convertedValue14 = validCheck.CheckIfDouble(textBoxSpeedknots.Text);
            if (convertedValue14 > 0)
            {
                //update obj
                VesselsTab.Speed = convertedValue14;
                labelSpeedknots.ForeColor = Color.Black;
            }
            else
                validcheckresult = false;
            #endregion


            if (!VesselsTab.Purchased)
            {
                #region Hireas Required check
                hasreqLbl.ForeColor = Color.Red;
                if (!string.IsNullOrEmpty(comboBoxHireasrequired.Text))
                {
                    //update obj
                    VesselsTab.Hireasrequired = comboBoxHireasrequired.Text;
                    hasreqLbl.ForeColor = Color.Black;
                }
                else
                    validcheckresult = false;
                #endregion



                if (VesselsTab.Hireasrequired.ToLower() == "yes")
                {
                    #region Vessellead Time Check
                    leadtimeLbl.ForeColor = Color.Red;
                    double convertedValue16 = validCheck.CheckIfDouble(textBoxVesselleadtime.Text);
                    if (convertedValue16 >= 0)
                    {
                        //update obj
                        VesselsTab.VesselLeadtime = convertedValue16;
                        leadtimeLbl.ForeColor = Color.Black;
                    }
                    else
                        validcheckresult = false;
                    #endregion
                }
                else
                {
                    #region Rentalstart Day check
                    rsdLbl.ForeColor = Color.Red;
                    int convertedValue6 = validCheck.CheckIfInt(textBoxRentalStartDay.Text);
                    if (convertedValue6 > 0 && convertedValue6 <= 31)
                    {
                        VesselsTab.RentalStartDay = convertedValue6;
                        rsdLbl.ForeColor = Color.Black;
                    }
                    else
                        validcheckresult = false;
                    #endregion

                    #region RentalStart Month Check
                    rsmLbl.ForeColor = Color.Red;
                    int convertedValue7 = validCheck.CheckIfInt(textBoxRentalSartMonth.Text);
                    if (convertedValue7 > 0 && convertedValue7 <= 12)
                    {
                        //update obj
                        VesselsTab.RentalStartMonth = convertedValue7;
                        rsmLbl.ForeColor = Color.Black;

                    }
                    else
                        validcheckresult = false;
                    #endregion

                    #region Rentalend Day check
                    redLbl.ForeColor = Color.Red;
                    int convertedValue8 = validCheck.CheckIfInt(textBoxRentalEndDay.Text);
                    if (convertedValue8 > 0 && convertedValue8 <= 31)
                    {
                        //update obj
                        VesselsTab.RentalEndDay = convertedValue8;
                        // VesselsTab.RentalEndDay = convertedValue8;
                        redLbl.ForeColor = Color.Black;
                    }
                    else
                        validcheckresult = false;
                    #endregion

                    #region Rentalend Month check
                    remLbl.ForeColor = Color.Red;
                    int convertedValue9 = validCheck.CheckIfInt(textBoxRentalEndMonth.Text);
                    if (convertedValue9 > 0 && convertedValue9 <= 12)
                    {
                        //update obj
                        VesselsTab.RentalEndMonth = convertedValue9;
                        remLbl.ForeColor = Color.Black;
                    }
                    else
                        validcheckresult = false;
                    #endregion

                    if (!CheckFromToDateValidation())
                        validcheckresult = false;
                }

                #region Dailyrental Cost check
                drcLbl.ForeColor = Color.Red;
                double convertedValue10 = validCheck.CheckIfDouble(textBoxDailyRentalCost.Text);
                if (convertedValue10 >= 0)
                {
                    //update obj
                    VesselsTab.DailyRentalCost = convertedValue10;
                    drcLbl.ForeColor = Color.Black;
                }
                else
                    validcheckresult = false;
                #endregion
                #region Mobilization Cost check
                mobcLbl.ForeColor = Color.Red;
                double convertedValue11 = validCheck.CheckIfDouble(textBoxMobilizationCost.Text);
                if (convertedValue11 >= 0)
                {
                    //update obj
                    VesselsTab.MobilizationCost = convertedValue11;
                    mobcLbl.ForeColor = Color.Black;
                }
                else
                    validcheckresult = false;
                #endregion
            }
            else
            {
                #region Annualrunning Cost chek
                anruncostLbl.ForeColor = Color.Red;
                double convertedValue5 = validCheck.CheckIfDouble(textBoxAnnualRunningCost.Text);
                if (convertedValue5 >= 0)
                {
                    //update obj
                    VesselsTab.AnnualrunningCost = convertedValue5;
                    anruncostLbl.ForeColor = Color.Black;
                }
                else
                    validcheckresult = false;
                #endregion
            }
            return validcheckresult;
        }
        private bool CheckFromToDateValidation()
        {

            if (!radioButtonPurchase.Checked)
            {
                //check date validation
                //1. check if all values entered
                if (VesselsTab.RentalStartMonth > 0 && VesselsTab.RentalEndMonth > 0 && VesselsTab.RentalStartDay > 0 && VesselsTab.RentalEndDay > 0)
                {
                    DateTime startDate, endDate;
                    bool testStartDateValid = DateTime.TryParse($"2000/{VesselsTab.RentalStartMonth}/{VesselsTab.RentalStartDay}", out _);
                    bool testEndDateValid = DateTime.TryParse($"2000/{VesselsTab.RentalEndMonth}/{VesselsTab.RentalEndDay}", out _);

                    if (testStartDateValid && testEndDateValid)
                    {
                        startDate = new DateTime(2000, VesselsTab.RentalStartMonth, VesselsTab.RentalStartDay);
                        endDate = new DateTime(2000, VesselsTab.RentalEndMonth, VesselsTab.RentalEndDay);
                        if (endDate > startDate)
                            return true;

                        else
                        {
                            _ = MessageBox.Show("Check dates");
                            return false;
                        }
                    }
                    else
                    {
                        _ = MessageBox.Show("Check dates");
                        return false;
                    }
                }
                else
                    return false;


            }


            return true;
        }
        private void AddNewBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void radioButtonPurchase_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonPurchase.Checked)
            {
                panelPurchasedCTV.Visible = true;
                VesselsTab.Purchased = true;
                panelrentedCTV.Visible = false;
            }
        }
        private void radioButtonRented_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonRented.Checked)
            {
                panelrentedCTV.Visible = true;
                VesselsTab.Purchased = false;
                panelPurchasedCTV.Visible = false;
            }
        }
        private void comboBoxHireasrequired_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxHireasrequired.SelectedItem.ToString() == "Yes")
            {
                panelRentalDates.Hide();
                vleadPanel.Visible = true;
            }
            else
            {
                panelRentalDates.Show();
                vleadPanel.Visible = false;
            }

        }
    }
}

