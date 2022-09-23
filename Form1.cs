using SELKIE.Entities;
using SELKIE.Models;
using System;
using System.IO;
using System.Windows.Forms;

namespace SELKIE
{
    public partial class Form1 : Form
    {
        delegate void ShowProjectTab();
        public Form1()
        {
            InitializeComponent();
            customizeDesing();
            CreateDataFolders();
        }

        private void CreateDataFolders()
        {
            try
            {
                //create folders 
                //create main folder Selkie
                string folder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                string selkieFolder = Path.Combine(folder, "Selkie");
                _ = Directory.CreateDirectory(selkieFolder);

                //create WW
                string wwFolder = Path.Combine(selkieFolder, "WW");
                _ = Directory.CreateDirectory(wwFolder);
                DataFolders.WWFolder = wwFolder;
                //create Projects
                string projectsFolder = Path.Combine(selkieFolder, "Projects");
                _ = Directory.CreateDirectory(projectsFolder);
                DataFolders.ProjectsFolder = projectsFolder;
                //create PC
                string pcFolder = Path.Combine(selkieFolder, "PC");
                _ = Directory.CreateDirectory(pcFolder);
                DataFolders.PCFolder = pcFolder;
                //create reports folder
                string rFolder = Path.Combine(selkieFolder, "Reports");
                _ = Directory.CreateDirectory(rFolder);
                DataFolders.ReportsFolder = rFolder;
                //create usermanul folder
                string umFolder = Path.Combine(selkieFolder, "Usermanual");
                _ = Directory.CreateDirectory(umFolder);
                DataFolders.Usermanual = umFolder;


            }
            catch (Exception)
            {
                _ = MessageBox.Show("Data folders failed to create or get access. This will limit export and import");
            }
        }

        private void customizeDesing()
        {
            panelBaseSetupDropdown.Visible = false;
            Correctivepanel.Visible = false;

        }
        private void hideSubMenu()
        {
            if (panelBaseSetupDropdown.Visible == true)
                panelBaseSetupDropdown.Visible = false;
            if (Correctivepanel.Visible == true)
                Correctivepanel.Visible = false;

        }
        private void showSubMenu(Panel subMenu)
        {
            if (subMenu.Visible == false)
            {
                hideSubMenu();
                subMenu.Visible = true;
            }
            else
                subMenu.Visible = false;
        }
        /* private void hideSubMenu2()
         {
             if (Correctivepanel.Visible == true)
                 Correctivepanel.Visible = false;
         }
         private void showSubMenu2(Panel subMenu)
         {
             if (subMenu.Visible == false)
             {
                 hideSubMenu();
                 subMenu.Visible = true;
             }
             else
                 subMenu.Visible = false;
         }*/
        private void buttonBaseSetup_Click(object sender, EventArgs e)
        {
            showSubMenu(panelBaseSetupDropdown);

        }
        private void CorrectivemntSubmenu_Click(object sender, EventArgs e)
        {
            showSubMenu(Correctivepanel);
        }
        private System.Windows.Forms.Form activeForm = null;
        public void openChildForm(System.Windows.Forms.Form childForm)
        {
            if (activeForm != null)
                activeForm.Close();
            activeForm = childForm;
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            panelBody.Controls.Add(childForm);
            panelBody.Tag = childForm;
            childForm.BringToFront();
            childForm.Show();
        }

        private bool IsProjectLocked()
        {
            if (ProjectSettings.ProjectLock)
            {
                _ = MessageBox.Show("Project locked!");
                return true;
            }
            else
                return false;
        }

        private void buttonProjectDetails_Click(object sender, EventArgs e)
        {
            ShowProjectPage();
        }




        public void ShowProjectPage()
        {
            if (IsProjectLocked())
                return;
            panelProjects.Visible = false;
            openChildForm(new Project_Details());
        }

        public void TriggerProjectTab()
        {
            ShowProjectTab d = new ShowProjectTab(ShowProjectPage);
            _ = this.Invoke(d);
        }

        private void buttonFarmDetails_Click(object sender, EventArgs e)
        {
            if (IsProjectLocked())
                return;
            panelProjects.Visible = false;
            openChildForm(new Farm_Details());
        }

        private void buttonResource_Click(object sender, EventArgs e)
        {
            if (IsProjectLocked())
                return;
            panelProjects.Visible = false;
            openChildForm(new Resource());
        }

        private void buttonOMBases_Click(object sender, EventArgs e)
        {
            if (IsProjectLocked())
                return;
            panelProjects.Visible = false;
            openChildForm(new Bases());
        }

        private void buttonVessels_Click(object sender, EventArgs e)
        {
            if (IsProjectLocked())
                return;
            panelProjects.Visible = false;
            openChildForm(new Vessels());
        }

        private void buttonInstallation_Click(object sender, EventArgs e)
        {
            if (IsProjectLocked())
                return;
            panelProjects.Visible = false;
            openChildForm(new Installation_Strategy());
        }

        private void buttonPreventive_Click(object sender, EventArgs e)
        {
            if (IsProjectLocked())
                return;
            panelProjects.Visible = false;
            openChildForm(new PM_Maintenance());
        }

        private void buttonRepairs_Click(object sender, EventArgs e)
        {
            if (IsProjectLocked())
                return;
            panelProjects.Visible = false;
            openChildForm(new Repairs());
        }

        private void buttonComponent_Click(object sender, EventArgs e)
        {
            if (IsProjectLocked())
                return;
            panelProjects.Visible = false;
            openChildForm(new Components());
        }

        private void buttonDasboard_Click(object sender, EventArgs e)
        {
            if (IsProjectLocked())
                return;
            openChildForm(new Dashboard());
        }

        private void BtnSimLink_Click(object sender, EventArgs e)
        {
            if (IsProjectLocked())
                return;
            panelProjects.Visible = false;
            openChildForm(new SimForm());
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (IsProjectLocked())
                return;
            panelProjects.Visible = false;
            openChildForm(new Usermanual());
        }
    }
}
