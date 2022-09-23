using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SELKIE.Models;
using SELKIE.Logic;

namespace SELKIE
{
    public partial class OM_Bases : Form
    {
        BasesDetails _baseDetails = new BasesDetails();
        public ValidationCheck validCheck = new ValidationCheck();

        int selIndex = -1;
        public OM_Bases()
        {
            InitializeComponent();
            FillBases();

        }
        private System.Windows.Forms.Form activeForm = null;
        private void openChildForm(System.Windows.Forms.Form childForm)
        {
            if (activeForm != null)
                activeForm.Close();
            activeForm = childForm;
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            dataGridViewBaseDetails.Controls.Add(childForm);
            dataGridViewBaseDetails.Tag = childForm;

            childForm.BringToFront();
            childForm.Show();
        }

        private void buttonAddBases_Click(object sender, EventArgs e)
        {
            openChildForm(new AddBases(-1, this));
           
        }

        
        /*private void initFieldsFromObj()
        {

        textBoxbasename.Text = "";
        textBoxAnnualcost.Text = "";
        textBoxDistancetofarm.Text = "";
        textBoxNumberoftechs.Text = "";
        textBoxAnnualsalarypertech.Text = "";
        selIndex = -1;
        buttonSaveOandMBases.Text = "Save";
        AddNewBtn.Visible = false;

        limitWarningLbl.Text = "";
        limitWarningLbl.Visible = false;
        }*/
    public void FillBases()
        {
        if (TotalBases.GetBases().Count > 0)
        {
        dataGridViewBaseDetails.DataSource = null;
        dataGridViewBaseDetails.Rows.Clear();
        foreach (var item in TotalBases.GetBases())
        {
          dataGridViewBaseDetails.Rows.Add(item.Basename, item.Annualcost, item.Distancetofarm, item.NoOfTechs, item.AnnualsalperTech);
        }
        }
        }

        private void dataGridViewBaseDetails_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {
            selIndex = this.dataGridViewBaseDetails.CurrentRow.Index;
            openChildForm(new AddBases(selIndex, this));
            
        }

        /*private void buttonSaveOandMBases_Click(object sender, EventArgs e)
        {
        //validation check
        if (CheckBasesValidation())
        {
        //save in temp obj
        var btab = new BasesDetails
        {
          Annualcost = Convert.ToInt32(textBoxAnnualcost.Text),
          AnnualsalperTech = Convert.ToInt32(textBoxAnnualcost.Text),
          Basename = textBoxbasename.Text,
          Distancetofarm = Convert.ToInt32(textBoxDistancetofarm.Text),
          NoOfTechs = Convert.ToInt32(textBoxNumberoftechs.Text)
        };

        //udpating
        if (selIndex > -1)
        {
          TotalBases.UpdateBase(selIndex, btab);
          FillBases();
          initFieldsFromObj();
        }
        else // new
        {
          if (TotalBases.GetBases().Count <= 2)
          {
              TotalBases.AddBase(btab);
              FillBases();
              initFieldsFromObj();
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
        SaveBaseAlert.Text = "Please check validations and try again.";
        }

        }
        public bool CheckBasesValidation()
        {
        bool validcheckresult = true;
        #region Basename check
        int convertedValue = validCheck.CheckIfInt(textBoxbasename.Text);
        if (convertedValue > 0)
        {
        //mean entered value is int
        //updte obj value to 0 if valdation failed
        _baseDetails.Basename = "";

        //show error message to user
        BasenameValidTest.Text = "Enter string only.";
        BasenameValidTest.Visible = true;
        validcheckresult = false;
        }
        else
        {
        //string

        //check length condition
        if (textBoxbasename.Text.Length < 1 || textBoxbasename.Text.Length > 30)
        {
          BasenameValidTest.Text = "String length validation occured (5-10 chars).";
          BasenameValidTest.Visible = true;
          validcheckresult = false;
        }
        else
        {
          //update obj
          _baseDetails.Basename = textBoxbasename.Text;
        }
        }
        #endregion

        #region Annualcost check
        int convertedValue1 = validCheck.CheckIfInt(textBoxAnnualcost.Text);
        if (convertedValue1 >= 0)
        {
        //update obj
        _baseDetails.Annualcost = convertedValue1;
        }
        else
        {
        //updte obj value to 0 if valdation failed
        _baseDetails.Annualcost = 0;

        //show error message to user
        AnnualCostValidTest.Text = "Invlalid data entry.";
        AnnualCostValidTest.Visible = true;
        }
        #endregion

        #region Distanceto Farm check
        double convertedValue2 = validCheck.CheckIfDouble(textBoxDistancetofarm.Text);
        if (convertedValue2 > 0)
        {
        //update obj
        _baseDetails.Distancetofarm = convertedValue2;
        }
        else
        {
        //updte obj value to 0 if valdation failed
        _baseDetails.Distancetofarm = 0;

        //show error message to user
        DistancetofarmValidTest.Text = "Invlalid data entry.";
        DistancetofarmValidTest.Visible = true;
        }
        #endregion

        #region Numberof Techs check
        int convertedValue3 = validCheck.CheckIfInt(textBoxNumberoftechs.Text);
        if (convertedValue3 > 0)
        {
        //update obj
        _baseDetails.NoOfTechs = convertedValue3;
        }
        else
        {
        //updte obj value to 0 if valdation failed
        _baseDetails.NoOfTechs = 0;

        //show error message to user
        NumberOfTechsValidTest.Text = "Invlalid data entry.";
        NumberOfTechsValidTest.Visible = true;
        }
        #endregion

        #region Annualsalary Pertech check
        int convertedValue4 = validCheck.CheckIfInt(textBoxAnnualsalarypertech.Text);
        if (convertedValue4 >= 0)
        {
        //update obj
        _baseDetails.AnnualsalperTech = convertedValue4;
        }
        else
        {
        //updte obj value to 0 if valdation failed
        _baseDetails.AnnualsalperTech = 0;

        //show error message to user
        AnnualSalperTechValidTest.Text = "Invlalid data entry.";
        AnnualSalperTechValidTest.Visible = true;
        }
        #endregion

        return validcheckresult;
        }

        private void dataGridViewBaseDetails_CellClick(object sender, DataGridViewCellEventArgs e)
        {
        textBoxbasename.Text = this.dataGridViewBaseDetails.CurrentRow.Cells[0].Value.ToString();
        textBoxAnnualcost.Text = this.dataGridViewBaseDetails.CurrentRow.Cells[1].Value.ToString();
        textBoxDistancetofarm.Text = this.dataGridViewBaseDetails.CurrentRow.Cells[2].Value.ToString();
        textBoxNumberoftechs.Text = this.dataGridViewBaseDetails.CurrentRow.Cells[3].Value.ToString();
        textBoxAnnualsalarypertech.Text = this.dataGridViewBaseDetails.CurrentRow.Cells[4].Value.ToString();

        //color change to show user of what row is selected
        //TODO: uma to udpate colors
        this.dataGridViewBaseDetails.CurrentRow.DefaultCellStyle.SelectionBackColor = Color.Blue;

        selIndex = this.dataGridViewBaseDetails.CurrentRow.Index;
        buttonSaveOandMBases.Text = "Update";
        AddNewBtn.Visible = true;
        }

        private void AddNewBtn_Click(object sender, EventArgs e)
        {
        initFieldsFromObj();
        }

        private void textBoxbasename_TextChanged(object sender, EventArgs e)
        {

        BasenameValidTest.Text = "Enter value here.";
        BasenameValidTest.Visible = false;

        CheckBasesValidation();
        }

        private void textBoxAnnualcost_TextChanged(object sender, EventArgs e)
        {

        AnnualCostValidTest.Text = "Enter value here.";
        AnnualCostValidTest.Visible = false;
        CheckBasesValidation();

        }

        private void textBoxDistancetofarm_TextChanged(object sender, EventArgs e)
        {

        DistancetofarmValidTest.Text = "Enter value here.";
        DistancetofarmValidTest.Visible = false;
        CheckBasesValidation();

        }

        private void textBoxNumberoftechs_TextChanged(object sender, EventArgs e)
        {

        NumberOfTechsValidTest.Text = "Enter value here.";
        NumberOfTechsValidTest.Visible = false;
        CheckBasesValidation();

        }

        private void textBoxAnnualsalarypertech_TextChanged(object sender, EventArgs e)
        {

        AnnualSalperTechValidTest.Text = "Enter value here.";
        AnnualSalperTechValidTest.Visible = false;

        CheckBasesValidation();
        }*/

    }
}

