using SELKIE.Logic;
using SELKIE.Models;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace SELKIE
{
    public partial class AddBases : Form
    {
        BasesDetails _baseDetails = new BasesDetails();
        Bases _bse = new Bases();
        public ValidationCheck validCheck = new ValidationCheck();
        int selIndex = -1;
        private readonly Bases _bases;
        public AddBases(int inx, Bases bases)
        {
            _bases = bases;
            selIndex = inx;
            InitializeComponent();
            FillItems();
        }
        private void initFieldsFromObj()
        {

            textBoxbasename.Text = "";
            textBoxAnnualcost.Text = "";
            textBoxDistancetofarm.Text = "";
            textBoxNumberoftechs.Text = "";
            textBoxAnnualsalarypertech.Text = "";
            selIndex = -1;
            buttonSaveOandMBases.Text = "Save";
            CancelBaseForm.Visible = true;

        }
        private void FillItems()
        {
            if (selIndex > -1) //update item
            {
                //get item based on index
                var selObj = TotalBases.GetObj(selIndex);
                //update with data
                textBoxbasename.Text = selObj.Basename;
                textBoxAnnualcost.Text = selObj.Annualcost.ToString();
                textBoxDistancetofarm.Text = selObj.Distancetofarm.ToString();
                textBoxNumberoftechs.Text = selObj.NoOfTechs.ToString();
                textBoxAnnualsalarypertech.Text = selObj.AnnualsalperTech.ToString();
                buttonSaveOandMBases.Text = "Update";

            }
            else
            {
                initFieldsFromObj();
            }
        }

        private void buttonSaveOandMBases_Click(object sender, EventArgs e)
        {
            try
            {
                //validation check
                if (CheckBasesValidation())
                {
                    //save in temp obj
                    var btab = new BasesDetails
                    {
                        Basename = textBoxbasename.Text,
                        Annualcost = Convert.ToDouble(textBoxAnnualcost.Text),
                        Distancetofarm = Convert.ToDouble(textBoxDistancetofarm.Text),
                        NoOfTechs = Convert.ToInt32(textBoxNumberoftechs.Text),
                        AnnualsalperTech = Convert.ToDouble(textBoxAnnualsalarypertech.Text)
                    };

                    //udpating
                    if (selIndex > -1)
                    {
                        TotalBases.UpdateBase(selIndex, btab);
                        FillItems();
                        initFieldsFromObj();
                        _bases.FillBases();
                        this.Close();
                    }
                    else // new
                    {
                        if (TotalBases.GetBases().Count <= 2)
                        {
                            TotalBases.AddBase(btab);
                            FillItems();
                            initFieldsFromObj();
                            _bases.FillBases();
                            this.Close();


                        }
                        else
                        {
                            limitWarningLbl.Text = "";
                            this.limitWarningLbl.Text = "You reached maximum limit";
                            limitWarningLbl.Visible = true;
                        }
                    }
                }
            }
            catch (Exception)
            {
                //log issue
            }

        }
        public bool CheckBasesValidation()
        {
            bool validcheckresult = true;
            #region Basename check
            bnameLbl.ForeColor = Color.Red;
            if (!string.IsNullOrEmpty(textBoxbasename.Text))
            {
                //check length condition
                if (textBoxbasename.Text.Length >= 1 && textBoxbasename.Text.Length <= 30)
                {
                    _baseDetails.Basename = textBoxbasename.Text;
                    bnameLbl.ForeColor = Color.Black;
                }
                else
                    validcheckresult = false;
            }
            else
                validcheckresult = false;
            #endregion

            #region Annualcost check
            labelAnnualcost.ForeColor = Color.Red;
            double convertedValue1 = validCheck.CheckIfDouble(textBoxAnnualcost.Text);
            if (convertedValue1 >= 0)
            {
                //update obj
                _baseDetails.Annualcost = convertedValue1;
                labelAnnualcost.ForeColor = Color.Black;
            }
            else
            {
                validcheckresult = false;
            }
            #endregion

            #region Distanceto Farm check
            labelDistancetofarm.ForeColor = Color.Red;
            double convertedValue2 = validCheck.CheckIfDouble(textBoxDistancetofarm.Text);
            if (convertedValue2 > 0)
            {
                //update obj
                _baseDetails.Distancetofarm = convertedValue2;
                labelDistancetofarm.ForeColor = Color.Black;
            }
            else
            {
                validcheckresult = false;
            }
            #endregion

            #region Numberof Techs check
            labelTotalnumberoftechniciansatonshorebase.ForeColor = Color.Red;
            int convertedValue3 = validCheck.CheckIfInt(textBoxNumberoftechs.Text);
            if (convertedValue3 > 0)
            {
                //update obj
                _baseDetails.NoOfTechs = convertedValue3;
                labelTotalnumberoftechniciansatonshorebase.ForeColor = Color.Black;
            }
            else
            {
                validcheckresult = false;
            }
            #endregion

            #region Annualsalary Pertech check
            labelTechnicianannualsalary.ForeColor = Color.Red;
            double convertedValue4 = validCheck.CheckIfDouble(textBoxAnnualsalarypertech.Text);
            if (convertedValue4 >= 0)
            {
                //update obj
                _baseDetails.AnnualsalperTech = convertedValue4;
                labelTechnicianannualsalary.ForeColor = Color.Black;
            }
            else
            {
                validcheckresult = false;
            }
            #endregion

            return validcheckresult;
        }


        private void CancelBaseForm_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }

}



