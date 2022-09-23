namespace SELKIE
{
    partial class SimForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblprojecrdetails = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnSimStart = new System.Windows.Forms.Button();
            this.BtnReportstoExcel = new System.Windows.Forms.Button();
            this.projLockGrp = new System.Windows.Forms.GroupBox();
            this.projLockLbl = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.projectchecklbl = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.farmchecklbl = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.resourcechecklbl = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.basechecklbl = new System.Windows.Forms.Label();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.vesselchecklbl = new System.Windows.Forms.Label();
            this.installchecklbl = new System.Windows.Forms.Label();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.preventativechecklbl = new System.Windows.Forms.Label();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.repairchecklbl = new System.Windows.Forms.Label();
            this.groupBox9 = new System.Windows.Forms.GroupBox();
            this.componentchecklbl = new System.Windows.Forms.Label();
            this.simstratedTime = new System.Windows.Forms.Label();
            this.simendedTime = new System.Windows.Forms.Label();
            this.totalSimProgressBar = new System.Windows.Forms.ProgressBar();
            this.statustext = new System.Windows.Forms.Label();
            this.installstatustext = new System.Windows.Forms.Label();
            this.installprogressBar = new System.Windows.Forms.ProgressBar();
            this.insEndedTime = new System.Windows.Forms.Label();
            this.insStratedTime = new System.Windows.Forms.Label();
            this.Errormessage = new System.Windows.Forms.Label();
            this.IterationLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.projLockGrp.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.groupBox8.SuspendLayout();
            this.groupBox9.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblprojecrdetails
            // 
            this.lblprojecrdetails.AutoSize = true;
            this.lblprojecrdetails.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblprojecrdetails.ForeColor = System.Drawing.SystemColors.GrayText;
            this.lblprojecrdetails.Location = new System.Drawing.Point(8, 14);
            this.lblprojecrdetails.Name = "lblprojecrdetails";
            this.lblprojecrdetails.Size = new System.Drawing.Size(122, 25);
            this.lblprojecrdetails.TabIndex = 1;
            this.lblprojecrdetails.Text = "Simulation";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.Window;
            this.panel1.Controls.Add(this.btnSimStart);
            this.panel1.Controls.Add(this.lblprojecrdetails);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1027, 42);
            this.panel1.TabIndex = 43;
            // 
            // btnSimStart
            // 
            this.btnSimStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSimStart.BackColor = System.Drawing.SystemColors.Highlight;
            this.btnSimStart.FlatAppearance.BorderSize = 0;
            this.btnSimStart.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSimStart.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSimStart.ForeColor = System.Drawing.SystemColors.Control;
            this.btnSimStart.Location = new System.Drawing.Point(870, 11);
            this.btnSimStart.Name = "btnSimStart";
            this.btnSimStart.Size = new System.Drawing.Size(147, 25);
            this.btnSimStart.TabIndex = 56;
            this.btnSimStart.Text = "Start Simulation";
            this.btnSimStart.UseVisualStyleBackColor = false;
            this.btnSimStart.Click += new System.EventHandler(this.btnSimStart_Click);
            // 
            // BtnReportstoExcel
            // 
            this.BtnReportstoExcel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnReportstoExcel.BackColor = System.Drawing.SystemColors.HotTrack;
            this.BtnReportstoExcel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnReportstoExcel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnReportstoExcel.ForeColor = System.Drawing.SystemColors.Control;
            this.BtnReportstoExcel.Location = new System.Drawing.Point(819, 577);
            this.BtnReportstoExcel.Name = "BtnReportstoExcel";
            this.BtnReportstoExcel.Size = new System.Drawing.Size(166, 59);
            this.BtnReportstoExcel.TabIndex = 59;
            this.BtnReportstoExcel.Text = "Export Reports";
            this.BtnReportstoExcel.UseVisualStyleBackColor = false;
            this.BtnReportstoExcel.Visible = false;
            this.BtnReportstoExcel.Click += new System.EventHandler(this.BtnReportstoExcel_Click);
            // 
            // projLockGrp
            // 
            this.projLockGrp.Controls.Add(this.projLockLbl);
            this.projLockGrp.Location = new System.Drawing.Point(57, 109);
            this.projLockGrp.Name = "projLockGrp";
            this.projLockGrp.Size = new System.Drawing.Size(149, 73);
            this.projLockGrp.TabIndex = 59;
            this.projLockGrp.TabStop = false;
            this.projLockGrp.Text = "Project lock Status";
            // 
            // projLockLbl
            // 
            this.projLockLbl.AutoSize = true;
            this.projLockLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.projLockLbl.Location = new System.Drawing.Point(6, 20);
            this.projLockLbl.Name = "projLockLbl";
            this.projLockLbl.Size = new System.Drawing.Size(115, 39);
            this.projLockLbl.TabIndex = 0;
            this.projLockLbl.Text = "label1";
            this.projLockLbl.Visible = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.projectchecklbl);
            this.groupBox1.Location = new System.Drawing.Point(212, 109);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(149, 73);
            this.groupBox1.TabIndex = 60;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Project details check";
            // 
            // projectchecklbl
            // 
            this.projectchecklbl.AutoSize = true;
            this.projectchecklbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.projectchecklbl.Location = new System.Drawing.Point(6, 20);
            this.projectchecklbl.Name = "projectchecklbl";
            this.projectchecklbl.Size = new System.Drawing.Size(115, 39);
            this.projectchecklbl.TabIndex = 0;
            this.projectchecklbl.Text = "label1";
            this.projectchecklbl.Visible = false;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.farmchecklbl);
            this.groupBox2.Location = new System.Drawing.Point(368, 109);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(149, 73);
            this.groupBox2.TabIndex = 61;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Farm details check";
            // 
            // farmchecklbl
            // 
            this.farmchecklbl.AutoSize = true;
            this.farmchecklbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.farmchecklbl.Location = new System.Drawing.Point(6, 20);
            this.farmchecklbl.Name = "farmchecklbl";
            this.farmchecklbl.Size = new System.Drawing.Size(115, 39);
            this.farmchecklbl.TabIndex = 0;
            this.farmchecklbl.Text = "label2";
            this.farmchecklbl.Visible = false;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.resourcechecklbl);
            this.groupBox3.Location = new System.Drawing.Point(523, 109);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(149, 73);
            this.groupBox3.TabIndex = 62;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Resource check";
            // 
            // resourcechecklbl
            // 
            this.resourcechecklbl.AutoSize = true;
            this.resourcechecklbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.resourcechecklbl.Location = new System.Drawing.Point(6, 20);
            this.resourcechecklbl.Name = "resourcechecklbl";
            this.resourcechecklbl.Size = new System.Drawing.Size(115, 39);
            this.resourcechecklbl.TabIndex = 0;
            this.resourcechecklbl.Text = "label3";
            this.resourcechecklbl.Visible = false;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.basechecklbl);
            this.groupBox4.Location = new System.Drawing.Point(678, 109);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(149, 73);
            this.groupBox4.TabIndex = 63;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Base(s) check";
            // 
            // basechecklbl
            // 
            this.basechecklbl.AutoSize = true;
            this.basechecklbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.basechecklbl.Location = new System.Drawing.Point(6, 20);
            this.basechecklbl.Name = "basechecklbl";
            this.basechecklbl.Size = new System.Drawing.Size(115, 39);
            this.basechecklbl.TabIndex = 0;
            this.basechecklbl.Text = "label4";
            this.basechecklbl.Visible = false;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.vesselchecklbl);
            this.groupBox5.Location = new System.Drawing.Point(833, 109);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(149, 73);
            this.groupBox5.TabIndex = 63;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Vessel(s) check";
            // 
            // vesselchecklbl
            // 
            this.vesselchecklbl.AutoSize = true;
            this.vesselchecklbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.vesselchecklbl.Location = new System.Drawing.Point(6, 20);
            this.vesselchecklbl.Name = "vesselchecklbl";
            this.vesselchecklbl.Size = new System.Drawing.Size(115, 39);
            this.vesselchecklbl.TabIndex = 0;
            this.vesselchecklbl.Text = "label5";
            this.vesselchecklbl.Visible = false;
            // 
            // installchecklbl
            // 
            this.installchecklbl.AutoSize = true;
            this.installchecklbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.installchecklbl.Location = new System.Drawing.Point(6, 20);
            this.installchecklbl.Name = "installchecklbl";
            this.installchecklbl.Size = new System.Drawing.Size(115, 39);
            this.installchecklbl.TabIndex = 0;
            this.installchecklbl.Text = "label1";
            this.installchecklbl.Visible = false;
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.installchecklbl);
            this.groupBox6.Location = new System.Drawing.Point(523, 188);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(459, 73);
            this.groupBox6.TabIndex = 60;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Installation check";
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.preventativechecklbl);
            this.groupBox7.Location = new System.Drawing.Point(56, 188);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(149, 73);
            this.groupBox7.TabIndex = 61;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "PM check";
            // 
            // preventativechecklbl
            // 
            this.preventativechecklbl.AutoSize = true;
            this.preventativechecklbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.preventativechecklbl.Location = new System.Drawing.Point(6, 20);
            this.preventativechecklbl.Name = "preventativechecklbl";
            this.preventativechecklbl.Size = new System.Drawing.Size(115, 39);
            this.preventativechecklbl.TabIndex = 0;
            this.preventativechecklbl.Text = "label1";
            this.preventativechecklbl.Visible = false;
            // 
            // groupBox8
            // 
            this.groupBox8.Controls.Add(this.repairchecklbl);
            this.groupBox8.Location = new System.Drawing.Point(212, 188);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size(149, 73);
            this.groupBox8.TabIndex = 64;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = "Repair(s) check";
            // 
            // repairchecklbl
            // 
            this.repairchecklbl.AutoSize = true;
            this.repairchecklbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.repairchecklbl.Location = new System.Drawing.Point(6, 20);
            this.repairchecklbl.Name = "repairchecklbl";
            this.repairchecklbl.Size = new System.Drawing.Size(115, 39);
            this.repairchecklbl.TabIndex = 0;
            this.repairchecklbl.Text = "label1";
            this.repairchecklbl.Visible = false;
            // 
            // groupBox9
            // 
            this.groupBox9.Controls.Add(this.componentchecklbl);
            this.groupBox9.Location = new System.Drawing.Point(369, 188);
            this.groupBox9.Name = "groupBox9";
            this.groupBox9.Size = new System.Drawing.Size(149, 73);
            this.groupBox9.TabIndex = 65;
            this.groupBox9.TabStop = false;
            this.groupBox9.Text = "Component(s) check";
            // 
            // componentchecklbl
            // 
            this.componentchecklbl.AutoSize = true;
            this.componentchecklbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.componentchecklbl.Location = new System.Drawing.Point(6, 20);
            this.componentchecklbl.Name = "componentchecklbl";
            this.componentchecklbl.Size = new System.Drawing.Size(115, 39);
            this.componentchecklbl.TabIndex = 0;
            this.componentchecklbl.Text = "label1";
            this.componentchecklbl.Visible = false;
            // 
            // simstratedTime
            // 
            this.simstratedTime.AutoSize = true;
            this.simstratedTime.Location = new System.Drawing.Point(62, 491);
            this.simstratedTime.Name = "simstratedTime";
            this.simstratedTime.Size = new System.Drawing.Size(75, 13);
            this.simstratedTime.TabIndex = 70;
            this.simstratedTime.Text = "Sim Start Time";
            this.simstratedTime.Visible = false;
            // 
            // simendedTime
            // 
            this.simendedTime.AutoSize = true;
            this.simendedTime.Location = new System.Drawing.Point(520, 491);
            this.simendedTime.Name = "simendedTime";
            this.simendedTime.Size = new System.Drawing.Size(72, 13);
            this.simendedTime.TabIndex = 71;
            this.simendedTime.Text = "Sim End Time";
            this.simendedTime.Visible = false;
            // 
            // totalSimProgressBar
            // 
            this.totalSimProgressBar.Location = new System.Drawing.Point(57, 506);
            this.totalSimProgressBar.Margin = new System.Windows.Forms.Padding(2);
            this.totalSimProgressBar.Name = "totalSimProgressBar";
            this.totalSimProgressBar.Size = new System.Drawing.Size(926, 42);
            this.totalSimProgressBar.TabIndex = 66;
            // 
            // statustext
            // 
            this.statustext.AutoSize = true;
            this.statustext.Location = new System.Drawing.Point(57, 554);
            this.statustext.Name = "statustext";
            this.statustext.Size = new System.Drawing.Size(91, 13);
            this.statustext.TabIndex = 67;
            this.statustext.Text = "O&&M progress bar";
            // 
            // installstatustext
            // 
            this.installstatustext.AutoSize = true;
            this.installstatustext.Location = new System.Drawing.Point(59, 394);
            this.installstatustext.Name = "installstatustext";
            this.installstatustext.Size = new System.Drawing.Size(118, 13);
            this.installstatustext.TabIndex = 73;
            this.installstatustext.Text = "Installation progress bar";
            // 
            // installprogressBar
            // 
            this.installprogressBar.Location = new System.Drawing.Point(59, 346);
            this.installprogressBar.Margin = new System.Windows.Forms.Padding(2);
            this.installprogressBar.Name = "installprogressBar";
            this.installprogressBar.Size = new System.Drawing.Size(926, 42);
            this.installprogressBar.TabIndex = 72;
            // 
            // insEndedTime
            // 
            this.insEndedTime.AutoSize = true;
            this.insEndedTime.Location = new System.Drawing.Point(520, 331);
            this.insEndedTime.Name = "insEndedTime";
            this.insEndedTime.Size = new System.Drawing.Size(80, 13);
            this.insEndedTime.TabIndex = 75;
            this.insEndedTime.Text = "Instal End Time";
            this.insEndedTime.Visible = false;
            // 
            // insStratedTime
            // 
            this.insStratedTime.AutoSize = true;
            this.insStratedTime.Location = new System.Drawing.Point(62, 329);
            this.insStratedTime.Name = "insStratedTime";
            this.insStratedTime.Size = new System.Drawing.Size(85, 13);
            this.insStratedTime.TabIndex = 74;
            this.insStratedTime.Text = "Install Start Time";
            this.insStratedTime.Visible = false;
            // 
            // Errormessage
            // 
            this.Errormessage.AutoSize = true;
            this.Errormessage.ForeColor = System.Drawing.Color.DarkRed;
            this.Errormessage.Location = new System.Drawing.Point(178, 577);
            this.Errormessage.Name = "Errormessage";
            this.Errormessage.Size = new System.Drawing.Size(10, 13);
            this.Errormessage.TabIndex = 76;
            this.Errormessage.Text = " ";
            // 
            // IterationLabel
            // 
            this.IterationLabel.AutoSize = true;
            this.IterationLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IterationLabel.Location = new System.Drawing.Point(841, 465);
            this.IterationLabel.Name = "IterationLabel";
            this.IterationLabel.Size = new System.Drawing.Size(0, 25);
            this.IterationLabel.TabIndex = 77;
            this.IterationLabel.Click += new System.EventHandler(this.label3_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.label1.Location = new System.Drawing.Point(51, 291);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(145, 25);
            this.label1.TabIndex = 78;
            this.label1.Text = "Install Tasks";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.label2.Location = new System.Drawing.Point(51, 451);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(373, 25);
            this.label2.TabIndex = 79;
            this.label2.Text = "Operation and Maintenance Tasks";
            // 
            // SimForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1027, 670);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.BtnReportstoExcel);
            this.Controls.Add(this.IterationLabel);
            this.Controls.Add(this.Errormessage);
            this.Controls.Add(this.insEndedTime);
            this.Controls.Add(this.insStratedTime);
            this.Controls.Add(this.installstatustext);
            this.Controls.Add(this.installprogressBar);
            this.Controls.Add(this.simendedTime);
            this.Controls.Add(this.simstratedTime);
            this.Controls.Add(this.statustext);
            this.Controls.Add(this.totalSimProgressBar);
            this.Controls.Add(this.groupBox9);
            this.Controls.Add(this.groupBox8);
            this.Controls.Add(this.groupBox7);
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.projLockGrp);
            this.Controls.Add(this.panel1);
            this.Name = "SimForm";
            this.Text = "SimForm";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.projLockGrp.ResumeLayout(false);
            this.projLockGrp.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            this.groupBox8.ResumeLayout(false);
            this.groupBox8.PerformLayout();
            this.groupBox9.ResumeLayout(false);
            this.groupBox9.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lblprojecrdetails;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnSimStart;
        private System.Windows.Forms.GroupBox projLockGrp;
        private System.Windows.Forms.Label projLockLbl;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label projectchecklbl;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label farmchecklbl;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label resourcechecklbl;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label basechecklbl;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Label vesselchecklbl;
        private System.Windows.Forms.Label installchecklbl;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.Label preventativechecklbl;
        private System.Windows.Forms.GroupBox groupBox8;
        private System.Windows.Forms.Label repairchecklbl;
        private System.Windows.Forms.GroupBox groupBox9;
        private System.Windows.Forms.Label componentchecklbl;
        private System.Windows.Forms.Label simstratedTime;
        private System.Windows.Forms.Label simendedTime;
        private System.Windows.Forms.ProgressBar totalSimProgressBar;
        private System.Windows.Forms.Label statustext;
        private System.Windows.Forms.Label installstatustext;
        private System.Windows.Forms.ProgressBar installprogressBar;
        private System.Windows.Forms.Label insEndedTime;
        private System.Windows.Forms.Label insStratedTime;
        private System.Windows.Forms.Label Errormessage;
        private System.Windows.Forms.Label IterationLabel;
        private System.Windows.Forms.Button BtnReportstoExcel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}