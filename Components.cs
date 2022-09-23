using SELKIE.Logic;
using SELKIE.Models;
using System;
using System.Windows.Forms;

namespace SELKIE
{
    public partial class Components : Form
    {
        public static ComponentDetails _componentDetails = new ComponentDetails();

        public ValidationCheck validCheck = new ValidationCheck();
        int selIndex = -1;
        public Components()
        {
            InitializeComponent();
            FillGrid();
            selIndex = -1;
            /* foreach (var item in TotalRepairs.GetAllRepairs())
             {
                 comboBoxRepcategory.Items.Add(item.RepairName);
             }*/
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
            dataGridViewCMP.Controls.Add(childForm);
            dataGridViewCMP.Tag = childForm;

            childForm.BringToFront();
            childForm.Show();
        }
        private void buttonAddComponents_Click(object sender, EventArgs e)
        {
            // dataGridViewCMP.Hide();
            openChildForm(new AddComponent(-1, this));
        }

        public void FillGrid()
        {
            if (TotalComponents.GetAllComponents().Count > 0)
            {
                dataGridViewCMP.DataSource = null;
                dataGridViewCMP.Rows.Clear();
                foreach (var item in TotalComponents.GetAllComponents())
                {
                    _ = dataGridViewCMP.Rows.Add("Edit", "Delete", item.Componentname, item.Numberperdevice, item.AnnualFailRate,
                        item.Repair, item.SpareParts);
                }
            }
            else
            {
                dataGridViewCMP.DataSource = null;
                dataGridViewCMP.Rows.Clear();
            }
        }



        private void dataGridViewCMP_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {
            selIndex = this.dataGridViewCMP.CurrentRow.Index;
            //openChildForm(new AddComponent(selIndex, this));
            if (selIndex > -1 && e.RowIndex > -1)
            {
                var _selObj = TotalComponents.GetObj(selIndex);
                if (_selObj == null)
                    return;

                //check column index
                int colIndex = this.dataGridViewCMP.CurrentCell.ColumnIndex;
                //load
                if (colIndex == 0)
                {
                    //edit
                    openChildForm(new AddComponent(selIndex, this));
                }
                else if (colIndex == 1) //delete
                {
                    //delete item from the grid
                    //Are you sure to delete
                    if (MessageBox.Show("Do you want to remove this row", "Remove row",
                            MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        if (TotalComponents.Delete(_selObj.Componentname))
                        {
                            FillGrid();
                        }

                        else
                            _ = MessageBox.Show("Failed to delete item.");
                }
            }
        }
    }
}
