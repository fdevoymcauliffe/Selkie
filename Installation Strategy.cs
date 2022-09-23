using SELKIE.Logic;
using SELKIE.Models;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace SELKIE
{
    public partial class Installation_Strategy : Form
    {
        public static InstallationDetails InstallationDetails = new InstallationDetails();
        public ValidationCheck validCheck = new ValidationCheck();
        int selIndex = -1;
        public Installation_Strategy()
        {
            InitializeComponent();
            FillInstDt();
            initFieldsFromObj();
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
            dataGridViewInstask.Controls.Add(childForm);
            dataGridViewInstask.Tag = childForm;

            childForm.BringToFront();
            childForm.Show();
        }
        private void initFieldsFromObj()
        {
            comboBoxInstallStartMonth.Text = InstStrategyDetails.Instalstartmonth;
            textBoxAdditionalcost.Text = InstStrategyDetails.Installationcost.ToString();
            //Second PanelData

        }
        public void FillInstDt()
        {
            if (TotalInstallations.GetInstalls().Count > 0)
            {
                dataGridViewInstask.DataSource = null;
                dataGridViewInstask.Rows.Clear();
                foreach (var item in TotalInstallations.GetInstalls())
                {
                    _ = dataGridViewInstask.Rows.Add("Edit", item.Taskname, item.TaskDescription, item.InstallType, item.NoOftechsReq,
                        item.VesselReq, item.Numberofdevicespervessel, item.Base, item.OperationDuration,
                        item.Waveheightlimit, item.Waveperiodlimit, item.Windspeedlimit, item.Currentvelocitylimit);
                }
                if (TotalInstallations.GetInstalls().Count == 2)
                    buttonAddInstallation.Visible = false;
                else if (TotalInstallations.GetInstalls().Count == 1)
                {
                    buttonAddInstallation.Visible = true;
                    buttonAddInstallation.Text = "Add Substructure";
                }
            }
            else
            {
                dataGridViewInstask.DataSource = null;
                dataGridViewInstask.Rows.Clear();
                buttonAddInstallation.Visible = true;
                buttonAddInstallation.Text = "Add Device";
            }
        }

        private void buttonAddInstallation_Click(object sender, EventArgs e)
        {
            //dataGridViewInstask.Hide();
            openChildForm(new AddInstallation(-1, this));
        }

        private void comboBoxInstallStartMonth_TextChanged(object sender, EventArgs e)
        {
            label4.ForeColor = Color.Red;
            int convertedValue = validCheck.CheckIfInt(comboBoxInstallStartMonth.Text);
            if (convertedValue >= 0 && convertedValue <= 12)
            {
                InstStrategyDetails.Instalstartmonth = comboBoxInstallStartMonth.Text;
                label4.ForeColor = Color.Black;
            }
        }

        private void textBoxAdditionalcost_TextChanged(object sender, EventArgs e)
        {
            label7.ForeColor = Color.Red;
            double convertedValue = validCheck.CheckIfDouble(textBoxAdditionalcost.Text);
            if (convertedValue >= 0)
            {
                InstStrategyDetails.Installationcost = convertedValue;
                label7.ForeColor = Color.Black;
            }
        }

        private void dataGridViewInstask_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            selIndex = this.dataGridViewInstask.CurrentRow.Index;
            if (selIndex > -1)
            {
                var _selObj = TotalInstallations.GetObj(selIndex);
                if (_selObj == null)
                    return;

                //check column index
                int colIndex = this.dataGridViewInstask.CurrentCell.ColumnIndex;
                //load
                if (colIndex == 0)
                {
                    //edit
                    openChildForm(new AddInstallation(selIndex, this));
                }
            }
        }

    }
}
