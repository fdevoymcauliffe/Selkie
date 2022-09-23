using SELKIE.Logic;
using SELKIE.Models;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace SELKIE
{
    public partial class AddComponent : Form
    {
        public static ComponentDetails _componentDetails = new ComponentDetails();
        public ValidationCheck validCheck = new ValidationCheck();
        int selIndex = -1;
        private readonly Components _compo;

        public AddComponent(int inx, Components compo)
        {
            _compo = compo;
            selIndex = inx;
            InitializeComponent();
            foreach (var item in TotalRepairs.GetAllRepairs())
            {
                _ = comboBoxRepcategory.Items.Add(item.RepairName);
            }
            FillItems();
        }
        private void initFieldsFromObj()
        {
            textBoxComponentname.Text = "";
            textBoxNumberperdevice.Text = "";
            textBoxAnualfailrate.Text = "";
            comboBoxRepcategory.Text = "";
            textBoxSparePart.Text = "";

            selIndex = -1;
            buttonSaveCompdata.Text = "Save";
            CloseCompnetform.Visible = true;

        }
        private void FillItems()
        {
            if (selIndex > -1) //update item
            {
                //get item based on index
                var selObj = TotalComponents.GetObj(selIndex);
                //update with data
                textBoxComponentname.Text = selObj.Componentname;
                textBoxNumberperdevice.Text = selObj.Numberperdevice.ToString();
                textBoxAnualfailrate.Text = selObj.AnnualFailRate.ToString();
                comboBoxRepcategory.Text = selObj.Repair;
                textBoxSparePart.Text = selObj.SpareParts.ToString();

                buttonSaveCompdata.Text = "Update";

            }
            else
            {
                initFieldsFromObj();
            }
        }

        private void buttonSaveCompdata_Click(object sender, EventArgs e)
        {
            try
            {
                //Validation check
                if (CheckAddComponentValidation())
                {

                    //save in temp obj
                    var cmptab = new ComponentDetails();
                    cmptab.Componentname = textBoxComponentname.Text;
                    cmptab.Numberperdevice = Convert.ToInt32(textBoxNumberperdevice.Text);
                    cmptab.AnnualFailRate = Convert.ToDouble(textBoxAnualfailrate.Text);
                    cmptab.Repair = comboBoxRepcategory.Text;
                    cmptab.SpareParts = Convert.ToDouble(textBoxSparePart.Text);


                    //udpating
                    if (selIndex > -1)
                    {
                        TotalComponents.Update(selIndex, cmptab);
                        FillItems();
                        initFieldsFromObj();
                        _compo.FillGrid();
                        this.Close();
                    }
                    else // new
                    {
                        if (TotalComponents.GetAllComponents().Count <= 100)
                        {
                            TotalComponents.Add(cmptab);
                            initFieldsFromObj();
                            _compo.FillGrid();
                            this.Close();
                        }
                        else
                        {
                            limitWarningLbl.Text = "";
                            this.limitWarningLbl.Text = "You reached max limit";
                            limitWarningLbl.Visible = true;
                            //
                        }
                    }
                }
            }
            catch (Exception)
            {
                //log error
            }

        }
        public bool CheckAddComponentValidation()
        {
            bool validcheckresult = true;
            #region Componentname check
            label12.ForeColor = Color.Red;
            if (string.IsNullOrEmpty(textBoxComponentname.Text))
            {
                validcheckresult = false;
            }
            else
            {
                //update obj
                _componentDetails.Componentname = textBoxComponentname.Text;
                label12.ForeColor = Color.Black;
            }
            #endregion

            #region Numberper Device check
            label2.ForeColor = Color.Red;
            int convertedValue1 = validCheck.CheckIfInt(textBoxNumberperdevice.Text);
            if (convertedValue1 > 0)
            {
                //update obj
                _componentDetails.Numberperdevice = convertedValue1;
                label2.ForeColor = Color.Black;
            }
            else
            {
                validcheckresult = false;
            }
            #endregion

            #region Annualfailurerate check
            label3.ForeColor = Color.Red;
            double convertedValue2 = validCheck.CheckIfDouble(textBoxAnualfailrate.Text);
            if (convertedValue2 > 0)
            {
                //update obj
                _componentDetails.AnnualFailRate = convertedValue2;
                label3.ForeColor = Color.Black;
            }
            else
            {
                validcheckresult = false;
            }
            #endregion

            #region Repair Category check
            label5.ForeColor = Color.Red;
            if (string.IsNullOrEmpty(comboBoxRepcategory.Text))
            {
                validcheckresult = false;
            }
            else
            {
                //update obj
                _componentDetails.Repair = comboBoxRepcategory.Text;
                label5.ForeColor = Color.Black;
            }
            #endregion

            #region Spare Part check

            label15.ForeColor = Color.Red;
            double convertedValue4 = validCheck.CheckIfDouble(textBoxSparePart.Text);
            if (convertedValue4 > -1)
            {
                //update obj
                _componentDetails.SpareParts = convertedValue4;
                label15.ForeColor = Color.Black;
            }
            else
            {
                validcheckresult = false;
            }
            #endregion

            return validcheckresult;
        }

        private void CloseCompnetform_Click(object sender, EventArgs e)
        {
            this.Close();

        }
    }
}
