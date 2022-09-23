using SELKIE.Enums;
using SELKIE.Logic;
using SELKIE.Models;
using System;
using System.Windows.Forms;

namespace SELKIE
{

    public partial class Repairs : Form
    {
        public static RepairDetails repairDetails = new RepairDetails();
        public ValidationCheck validCheck = new ValidationCheck();
        int selIndex = -1;

        public Repairs()
        {
            InitializeComponent();

            foreach (var item in TotalBases.GetBases())
            {
                _ = comboBoxBasetype.Items.Add(item.Basename);
            }
            foreach (var item in TotalVessels.GetVessels())
            {
                _ = comboBoxVesselrequire.Items.Add(item.VesselClassif);
            }
            FillGrid();
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
            dataGridViewADDRep.Controls.Add(childForm);
            dataGridViewADDRep.Tag = childForm;

            childForm.BringToFront();
            childForm.Show();
        }

        public void FillGrid()
        {
            dataGridViewADDRep.DataSource = null;
            dataGridViewADDRep.Rows.Clear();
            if (TotalRepairs.GetAllRepairs().Count > 0)
            {
                var _totalRepairs = TotalRepairs.GetAllRepairs();
                if (_totalRepairs.Count > 0)
                {
                    foreach (var item in TotalRepairs.GetAllRepairs())
                    {
                        _ = dataGridViewADDRep.Rows.Add("Edit", "Delete", item.RepairName, item.RepairDesc, item.NoOfTechs, item.Vesselrequired,
                            item.Base, item.Operationlocation, item.OperationdurationOffshore, item.OperatondurationOnshore,
                            item.Waveheightlimit, item.Waveperiodlimit, item.Windspeedlimit, item.Currentvelocitylimit,
                            item.Powerloss);
                    }
                }
            }
        }

        private void buttonAddRepairs_Click(object sender, EventArgs e)
        {
            //dataGridViewADDRep.Hide();
            openChildForm(new AddRepairs(-1, this));
        }

        /* private void buttonSaveRepdata_Click(object sender, EventArgs e)
         {
             //save in temp obj
             var Reptab = new RepairDetails
             {
                 RepairName = textBoxAddRepairCategory.Text,
                 RepairDesc = textBoxTaskDescription.Text,
                 NoOfTechs = Convert.ToInt32(textBoxNumberofTEchs.Text),
                 Vesselrequired = comboBoxVesselrequire.Text,
                 Base = comboBoxBasetype.Text,
                 Operationlocation = comboBoxOprloc.Text,
                 OperationdurationOffshore = Convert.ToInt32(textBoxDurationoffshore.Text),
                 OperatondurationOnshore = Convert.ToInt32(textBoxDurationOnshore.Text),
                 Waveheightlimit = Convert.ToInt32(textBoxWaveheightOffs.Text),
                 Waveperiodlimit = Convert.ToInt32(textBoxWaveperiodLimitOffs.Text),
                 Windspeedlimit = Convert.ToInt32(textBoxwindspeedLimitrepair.Text),
                 Currentvelocitylimit = Convert.ToInt32(textBoxCurrentVelocity.Text),
                 Powerloss = Convert.ToInt32(textBoxPowerloss.Text)
             };
            // dataGridViewADDRep.Show();


             //udpating
             if (selIndex > -1)
             {
                 TotalRepairs.Update(selIndex, Reptab);
                 FillRepairs();
                 initFieldsFromObj();
             }
             else // new
             {
                 if (TotalRepairs.GetAllRepairs().Count <= 20)
                 {
                     TotalRepairs.Add(Reptab);
                     FillRepairs();
                     initFieldsFromObj();
                 }
                 else
                 {
                     limitWarningLbl.Text = "";
                     this.limitWarningLbl.Text = "You reached max limit";
                     limitWarningLbl.Visible = true;
                 }
             }
         }*/

        private void dataGridViewADDRep_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            selIndex = this.dataGridViewADDRep.CurrentRow.Index;
            if (selIndex > -1 && e.RowIndex > -1)
            {
                var _selObj = TotalRepairs.GetObj(selIndex);
                if (_selObj == null)
                    return;

                //check column index
                int colIndex = this.dataGridViewADDRep.CurrentCell.ColumnIndex;
                //load
                if (colIndex == 0)
                {
                    //edit
                    openChildForm(new AddRepairs(selIndex, this));
                }
                else if (colIndex == 1) //delete
                {
                    //delete item from the grid
                    //Are you sure to delete
                    if (MessageBox.Show("Do you wan to remove this row", "Remove row",
                            MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        if (TotalRepairs.Delete(_selObj.RepairName))
                            FillGrid();
                        else
                            _ = MessageBox.Show("Failed to delete item.");
                }
            }
        }

        private void AddNewBtn_Click(object sender, EventArgs e)
        {
            //initFieldsFromObj();
        }

        private void textBoxAddRepairCategory_TextChanged(object sender, EventArgs e)
        {
            RepairCategoryValidTest.Text = "Enter value here.";
            RepairCategoryValidTest.Visible = false;

            int convertedValue = validCheck.CheckIfInt(textBoxAddRepairCategory.Text);
            if (convertedValue > 0)
            {
                //mean entered value is int
                //updte obj value to 0 if valdation failed
                repairDetails.RepairName = "";

                //show error message to user
                RepairCategoryValidTest.Text = "Enter string only.";
                RepairCategoryValidTest.Visible = true;
            }

            else
            {
                //update obj
                repairDetails.RepairName = textBoxAddRepairCategory.Text;
            }
        }

        private void textBoxNumberofTEchs_TextChanged(object sender, EventArgs e)
        {
            NumberOfTechsReqValidTest.Text = "Enter value here.";
            NumberOfTechsReqValidTest.Visible = false;
            int convertedValue = validCheck.CheckIfInt(textBoxNumberofTEchs.Text);
            if (convertedValue > 0)
            {
                //update obj
                repairDetails.NoOfTechs = convertedValue;
            }
            else
            {
                //updte obj value to 0 if valdation failed
                repairDetails.NoOfTechs = 0;

                //show error message to user
                NumberOfTechsReqValidTest.Text = "Invlalid data entry.";
                NumberOfTechsReqValidTest.Visible = true;
            }
        }

        private void comboBoxVesselrequire_TextChanged(object sender, EventArgs e)
        {
            VesselRequiredValidTest.Text = "Enter value here.";
            VesselRequiredValidTest.Visible = false;

            int convertedValue = validCheck.CheckIfInt(comboBoxVesselrequire.Text);
            if (convertedValue > 0)
            {
                //mean entered value is int
                //updte obj value to 0 if valdation failed
                repairDetails.Vesselrequired = "";

                //show error message to user
                VesselRequiredValidTest.Text = "Enter string only.";
                VesselRequiredValidTest.Visible = true;
            }

            else
            {
                //update obj
                repairDetails.Vesselrequired = comboBoxVesselrequire.Text;
            }
        }

        private void comboBoxBasetype_TextChanged(object sender, EventArgs e)
        {
            BaseValidTest.Text = "Enter value here.";
            BaseValidTest.Visible = false;

            int convertedValue = validCheck.CheckIfInt(comboBoxBasetype.Text);
            if (convertedValue > 0)
            {
                //mean entered value is int
                //updte obj value to 0 if valdation failed
                repairDetails.Base = "";

                //show error message to user
                BaseValidTest.Text = "Enter string only.";
                BaseValidTest.Visible = true;
            }

            else
            {
                //update obj
                repairDetails.Base = comboBoxBasetype.Text;
            }
        }

        private void comboBoxOprloc_TextChanged(object sender, EventArgs e)
        {
            OperationLocationValidTest.Text = "Enter value here.";
            OperationLocationValidTest.Visible = false;

            int convertedValue = validCheck.CheckIfInt(comboBoxOprloc.Text);
            if (convertedValue > 0)
            {
                //mean entered value is int
                //updte obj value to 0 if valdation failed
                repairDetails.Operationlocation = Enums.OperationalLocation.Onshore;

                //show error message to user
                OperationLocationValidTest.Text = "Enter string only.";
                OperationLocationValidTest.Visible = true;
            }

            else
            {
                //update obj
                OperationalLocation _locsel;
                _ = Enum.TryParse(comboBoxOprloc.Text, out _locsel);

                repairDetails.Operationlocation = _locsel;
            }
        }

        private void textBoxDurationOnshore_TextChanged(object sender, EventArgs e)
        {
            OperDurationOnshoreValidTest.Text = "Enter value here.";
            OperDurationOnshoreValidTest.Visible = false;
            double convertedValue = validCheck.CheckIfDouble(textBoxDurationOnshore.Text);
            if (convertedValue > 0)
            {
                //update obj
                repairDetails.OperatondurationOnshore = convertedValue;
            }
            else
            {
                //updte obj value to 0 if valdation failed
                repairDetails.OperatondurationOnshore = 0;

                //show error message to user
                OperDurationOnshoreValidTest.Text = "Invlalid data entry.";
                OperDurationOnshoreValidTest.Visible = true;
            }
        }

        private void textBoxWaveheightOffs_TextChanged(object sender, EventArgs e)
        {
            WaveHeightValidTest.Text = "Enter value here.";
            WaveHeightValidTest.Visible = false;
            double convertedValue = validCheck.CheckIfDouble(textBoxWaveheightOffs.Text);
            if (convertedValue > 0)
            {
                //update obj
                repairDetails.Waveheightlimit = convertedValue;
            }
            else
            {
                //updte obj value to 0 if valdation failed
                repairDetails.Waveheightlimit = 0;

                //show error message to user
                WaveHeightValidTest.Text = "Invlalid data entry.";
                WaveHeightValidTest.Visible = true;
            }
        }

        private void textBoxWaveperiodLimitOffs_TextChanged(object sender, EventArgs e)
        {
            WavePeriodValidTest.Text = "Enter value here.";
            WavePeriodValidTest.Visible = false;
            double convertedValue = validCheck.CheckIfDouble(textBoxWaveperiodLimitOffs.Text);
            if (convertedValue > 0)
            {
                //update obj
                repairDetails.Waveperiodlimit = convertedValue;
            }
            else
            {
                //updte obj value to 0 if valdation failed
                repairDetails.Waveperiodlimit = 0;

                //show error message to user
                WavePeriodValidTest.Text = "Invlalid data entry.";
                WavePeriodValidTest.Visible = true;
            }
        }

        private void textBoxwindspeedLimitrepair_TextChanged(object sender, EventArgs e)
        {
            WindSpeedValidTest.Text = "Enter value here.";
            WindSpeedValidTest.Visible = false;
            double convertedValue = validCheck.CheckIfDouble(textBoxwindspeedLimitrepair.Text);
            if (convertedValue > 0)
            {
                //update obj
                repairDetails.Windspeedlimit = convertedValue;
            }
            else
            {
                //updte obj value to 0 if valdation failed
                repairDetails.Windspeedlimit = 0;

                //show error message to user
                WindSpeedValidTest.Text = "Invlalid data entry.";
                WindSpeedValidTest.Visible = true;
            }
        }

        private void textBoxCurrentVelocity_TextChanged(object sender, EventArgs e)
        {
            CurrentVelocityValidTest.Text = "Enter value here.";
            CurrentVelocityValidTest.Visible = false;
            double convertedValue = validCheck.CheckIfDouble(textBoxCurrentVelocity.Text);
            if (convertedValue > 0)
            {
                //update obj
                repairDetails.Currentvelocitylimit = convertedValue;
            }
            else
            {
                //updte obj value to 0 if valdation failed
                repairDetails.Currentvelocitylimit = 0;

                //show error message to user
                CurrentVelocityValidTest.Text = "Invlalid data entry.";
                CurrentVelocityValidTest.Visible = true;
            }
        }

        private void textBoxPowerloss_TextChanged(object sender, EventArgs e)
        {
            PowerLossValidTest.Text = "Enter value here.";
            PowerLossValidTest.Visible = false;
            double convertedValue = validCheck.CheckIfDouble(textBoxPowerloss.Text);
            if (convertedValue > 0)
            {
                //update obj
                repairDetails.Powerloss = convertedValue;
            }
            else
            {
                //updte obj value to 0 if valdation failed
                repairDetails.Powerloss = 0;

                //show error message to user
                PowerLossValidTest.Text = "Invlalid data entry.";
                PowerLossValidTest.Visible = true;
            }
        }

        private void textBoxDurationoffshore_TextChanged(object sender, EventArgs e)
        {
            OperDurationOffshoreValidTest.Text = "Enter value here.";
            OperDurationOffshoreValidTest.Visible = false;
            double convertedValue = validCheck.CheckIfDouble(textBoxDurationoffshore.Text);
            if (convertedValue > 0)
            {
                //update obj
                repairDetails.OperationdurationOffshore = convertedValue;
            }
            else
            {
                //updte obj value to 0 if valdation failed
                repairDetails.OperationdurationOffshore = 0;

                //show error message to user
                OperDurationOffshoreValidTest.Text = "Invlalid data entry.";
                OperDurationOffshoreValidTest.Visible = true;
            }
        }

        private void CloseRepairsForm_Click(object sender, EventArgs e)
        {
            dataGridViewADDRep.Show();
        }
    }
}
