using SELKIE.Logic;
using SELKIE.Models;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace SELKIE
{
    public partial class PM_Maintenance : Form
    {
        public static PriventiveDetails priventiveDetails = new PriventiveDetails();
        public ValidationCheck validCheck = new ValidationCheck();
        int selIndex = -1;
        public PM_Maintenance()
        {
            InitializeComponent();
            initFieldsFromObj();
            FillPMdt();
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
            dataGridViewPMTask.Controls.Add(childForm);
            dataGridViewPMTask.Tag = childForm;

            childForm.BringToFront();
            childForm.Show();
        }
        private void initFieldsFromObj()
        {
            comboBoxPMStartMonth.Text = PreventiveStartEnd.StartMonth;
            comboBoxPMEndmonth.Text = PreventiveStartEnd.EndMonth;


            textBoxAnnualOandMCost.Text = PreventiveStartEnd.AdditionalAnnualCost.ToString();

        }
        public void FillPMdt()
        {
            if (TotalPriventives.GetAllPriventives().Count > 0)
            {
                dataGridViewPMTask.DataSource = null;
                dataGridViewPMTask.Rows.Clear();
                var totalP = TotalPriventives.GetAllPriventives();
                foreach (var item in totalP)
                {
                    _ = dataGridViewPMTask.Rows.Add("Edit", "Delete", item.PMCategory, item.Taskdescription, item.NoOftechsReq, item.VesselReq,
                        item.Base, item.Frequency, item.OperationLOC, item.OprDurationOffs, item.OprDurationOns,
                        item.Waveheightlimit, item.Waveperiodlimit, item.Windspeedlimit, item.CurrentVelocityLimit,
                        item.Powerloss, item.Sparepart);
                }
            }
            else
            {
                dataGridViewPMTask.DataSource = null;
                dataGridViewPMTask.Rows.Clear();
            }
        }
        private void buttonPreventiveData_Click(object sender, EventArgs e)
        {
            //dataGridViewPMTask.Hide();
            openChildForm(new AddPreventivesMaintenance(-1, this));
        }
        private void CheckMonthValidation()
        {
            //end month
            int _endmonth = validCheck.CheckIfInt(comboBoxPMEndmonth.Text);
            if (_endmonth == -1)
                _endmonth = 1;
            int _startmonth = validCheck.CheckIfInt(comboBoxPMStartMonth.Text);
            if (_startmonth == -1)
                _startmonth = 1;

            if (_endmonth < _startmonth)
                _endmonth = _startmonth;

            PreventiveStartEnd.StartMonth = _startmonth.ToString();
            PreventiveStartEnd.EndMonth = _endmonth.ToString();

            comboBoxPMEndmonth.Text = _endmonth.ToString();
            comboBoxPMStartMonth.Text = _startmonth.ToString();


            label6.ForeColor = Color.Red;
            int convertedValue = validCheck.CheckIfInt(textBoxAnnualOandMCost.Text);
            if (convertedValue >= 0)
            {
                //update obj
                PreventiveStartEnd.AdditionalAnnualCost = convertedValue;
                label6.ForeColor = Color.Black;
            }
        }
        private void dataGridViewPMTask_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {
            selIndex = this.dataGridViewPMTask.CurrentRow.Index;
            //openChildForm(new AddPreventivesMaintenance(selIndex, this));
            if (selIndex > -1 && e.RowIndex > -1)
            {
                var _selObj = TotalPriventives.GetObj(selIndex);
                if (_selObj == null)
                    return;

                //check column index
                int colIndex = this.dataGridViewPMTask.CurrentCell.ColumnIndex;
                //load
                if (colIndex == 0)
                {
                    //edit
                    openChildForm(new AddPreventivesMaintenance(selIndex, this));
                }
                else if (colIndex == 1) //delete
                {
                    //delete item from the grid
                    //Are you sure to delete

                    if (MessageBox.Show("Do you want to remove this row", "Remove row",
                            MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        if (TotalPriventives.Delete(_selObj.PMCategory))
                        {
                            FillPMdt();
                        }
                        else
                            _ = MessageBox.Show("Failed to delete item.");
                    }

                }
            }

        }
        private void pmsave_Click(object sender, EventArgs e)
        {
            CheckMonthValidation();
        }
    }
}

