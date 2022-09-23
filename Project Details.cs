using SELKIE.Models;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace SELKIE
{
    public partial class Project_Details : Form
    {
        public Project_Details()
        {
            InitializeComponent();
            initFieldsFromObj();
        }
        private void initFieldsFromObj()
        {
            textBoxProjectname.Text = ProjectDetails.ProjectName;
            textBoxInstallationtime.Text = ProjectDetails.InstallTime.ToString();
            textBoxProjectlifetime.Text = ProjectDetails.ProjectLifeTime.ToString();
            textBoxNoofiterations.Text = ProjectDetails.NoOfIterations.ToString();
            textBoxGridsalerate.Text = ProjectDetails.GridSaleRate.ToString();
            textBoxwakelosses.Text = ProjectDetails.WakeLoss.ToString();
            textBoxTransmission.Text = ProjectDetails.TransLoss.ToString();
            textBoxLostproduction.Text = ProjectDetails.ProdLoss.ToString();
        }

        private void SaveProjDetails_Click(object sender, EventArgs e)
        {
            #region projectName
            labelprojectName.ForeColor = Color.Red;
            if (!string.IsNullOrEmpty(textBoxProjectname.Text))
            {
                //check length
                if (textBoxProjectname.Text.Length >= 1 || textBoxProjectname.Text.Length <= 50)
                {
                    ProjectDetails.ProjectName = textBoxProjectname.Text;
                    labelprojectName.ForeColor = Color.Black;
                }
            }
            #endregion

            #region lost production
            oLossLbl.ForeColor = Color.Red;
            if (double.TryParse(textBoxLostproduction.Text, out double lprod))
            {
                if (lprod >= 0 && lprod <= 100)
                {
                    ProjectDetails.ProdLoss = lprod;
                    oLossLbl.ForeColor = Color.Black;
                }
            }

            #endregion

            #region trans
            tlossLbl.ForeColor = Color.Red;
            if (double.TryParse(textBoxTransmission.Text, out double transLoss))
            {
                if (transLoss >= 0 && transLoss <= 100)
                {
                    ProjectDetails.TransLoss = transLoss;
                    tlossLbl.ForeColor = Color.Black;
                }
            }
            #endregion

            #region wake loss
            wlossLbl.ForeColor = Color.Red;
            if (double.TryParse(textBoxwakelosses.Text, out double wloss))
            {
                if (wloss >= 0 && wloss <= 100)
                {
                    ProjectDetails.WakeLoss = wloss;
                    wlossLbl.ForeColor = Color.Black;
                }
            }
            #endregion

            #region grid rate
            gridLbl.ForeColor = Color.Red;
            _ = double.TryParse(textBoxGridsalerate.Text, out double gs);
            if (gs >= 0)
            {
                ProjectDetails.GridSaleRate = Math.Round(gs, 3);
                gridLbl.ForeColor = Color.Black;
                textBoxGridsalerate.Text = ProjectDetails.GridSaleRate.ToString();
            }
            #endregion

            #region iteratons
            iterLbl.ForeColor = Color.Red;
            _ = int.TryParse(textBoxNoofiterations.Text, out int nit);
            if (nit > 0)
            {
                ProjectDetails.NoOfIterations = nit;
                iterLbl.ForeColor = Color.Black;
            }
            #endregion

            #region lifetime
            pLifeLbl.ForeColor = Color.Red;
            _ = int.TryParse(textBoxProjectlifetime.Text, out int plife);
            if (plife > 0)
            {
                ProjectDetails.ProjectLifeTime = plife;
                pLifeLbl.ForeColor = Color.Black;
            }
            #endregion

            #region installtime
            installLbl.ForeColor = Color.Red;
            _ = int.TryParse(textBoxInstallationtime.Text, out int instime);
            if (instime > 0)
            {
                ProjectDetails.InstallTime = instime;
                installLbl.ForeColor = Color.Black;
            }
            #endregion
        }
    }
}
