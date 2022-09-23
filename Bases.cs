using SELKIE.Logic;
using SELKIE.Models;
using System;
using System.Windows.Forms;

namespace SELKIE
{
    public partial class Bases : Form
    {
        BasesDetails _baseDetails = new BasesDetails();
        public ValidationCheck validCheck = new ValidationCheck();

        int selIndex = -1;
        public Bases()
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
            dataGridViewBases.Controls.Add(childForm);
            dataGridViewBases.Tag = childForm;

            childForm.BringToFront();
            childForm.Show();
        }

        private void buttonAddBases_Click(object sender, EventArgs e)
        {
            openChildForm(new AddBases(-1, this));
        }
        public void FillBases()
        {
            if (TotalBases.GetBases().Count > 0)
            {
                dataGridViewBases.DataSource = null;
                dataGridViewBases.Rows.Clear();
                foreach (var item in TotalBases.GetBases())
                {
                    _ = dataGridViewBases.Rows.Add("Edit", "Delete", item.Basename, item.Annualcost, item.Distancetofarm, item.NoOfTechs, item.AnnualsalperTech);
                }
            }
            else
            {
                dataGridViewBases.DataSource = null;
                dataGridViewBases.Rows.Clear();
            }
        }

        private void dataGridViewBases_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            selIndex = this.dataGridViewBases.CurrentRow.Index;


            // openChildForm(new AddBases(selIndex, this));
            if (selIndex > -1 && e.RowIndex > -1)
            {
                var _selObj = TotalBases.GetObj(selIndex);
                if (_selObj == null)
                    return;
                //check column index
                int colIndex = this.dataGridViewBases.CurrentCell.ColumnIndex;
                //load
                if (colIndex == 0)
                {
                    //edit
                    openChildForm(new AddBases(selIndex, this));
                }
                else if (colIndex == 1) //delete
                {
                    //delete item from the grid
                    //TODO: add are you sure to delete
                    if (MessageBox.Show("Do you want to remove this row", "Remove row",
                            MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        //Checking delete or not
                        //if(_selObj.Basename == )
                        //
                        if (TotalBases.Delete(_selObj.Basename))
                        {
                            FillBases();
                        }
                        else
                            _ = MessageBox.Show("Failed to delete item.");
                }
            }
        }


    }
}
