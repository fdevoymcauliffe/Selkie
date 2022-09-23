using SELKIE.Logic;
using SELKIE.Models;
using System;
using System.Windows.Forms;

namespace SELKIE
{
    public partial class Vessels : Form
    {
        public static VesselDetails VesselsTab = new VesselDetails();

        public ValidationCheck validCheck = new ValidationCheck();
        int selIndex = -1;
        public Vessels()
        {
            InitializeComponent();
            FillVsls();
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
            dataGridViewVesselDetails.Controls.Add(childForm);
            dataGridViewVesselDetails.Tag = childForm;

            childForm.BringToFront();
            childForm.Show();
        }

        public void FillVsls()
        {
            if (TotalVessels.GetVessels().Count > 0)
            {
                dataGridViewVesselDetails.DataSource = null;
                dataGridViewVesselDetails.Rows.Clear();
                foreach (var item in TotalVessels.GetVessels())
                {
                    _ = dataGridViewVesselDetails.Rows.Add("Edit", "Delete", item.VesselClassif, item.Number, item.TechsCapacity,
                        item.NightWork, item.AnnualrunningCost, item.Hireasrequired, item.VesselLeadtime, item.RentalStartDay, item.RentalEndDay, item.RentalStartMonth,
                        item.RentalEndMonth, item.DailyRentalCost, item.MobilizationCost, item.FuelConsumption, item.FuelCost,
                        item.Speed, item.Purchased);

                }
            }
            else
            {
                dataGridViewVesselDetails.DataSource = null;
                dataGridViewVesselDetails.Rows.Clear();
            }
        }

        private void radioButtonPurchase_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void radioButtonRented_CheckedChanged(object sender, EventArgs e)
        {


        }



        private void dataGridViewVesselDetails_CellClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void AddNewBtn_Click(object sender, EventArgs e)
        {
            // initFieldsFromObj();
        }

        private void buttonAddVessel_Click(object sender, EventArgs e)
        {
            openChildForm(new AddVessels(-1, this));
        }

        private void dataGridViewVesselDetails_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {
            selIndex = this.dataGridViewVesselDetails.CurrentRow.Index;
            //openChildForm(new AddVessels(selIndex, this));
            if (selIndex > -1 && e.RowIndex > -1)
            {
                var _selObj = TotalVessels.GetObj(selIndex);
                if (_selObj == null)
                    return;

                //check column index
                int colIndex = this.dataGridViewVesselDetails.CurrentCell.ColumnIndex;
                //load
                if (colIndex == 0)
                {
                    //edit
                    openChildForm(new AddVessels(selIndex, this));
                }
                else if (colIndex == 1) //delete
                {
                    //delete item from the grid
                    //TODO: add are you sure to delete
                    if (MessageBox.Show("Do you want to remove this row", "Remove row",
                            MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        if (TotalVessels.Delete(_selObj.VesselClassif))
                        {
                            FillVsls();
                        }

                        else
                            _ = MessageBox.Show("Failed to delete item.");
                }
            }
        }


    }
}

