namespace SELKIE
{
    partial class Dashboard
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.newprojectBtn = new System.Windows.Forms.Button();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.label3Load = new System.Windows.Forms.Label();
            this.label4Success = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.BtnExporttoExcel = new System.Windows.Forms.Button();
            this.progressBar2 = new System.Windows.Forms.ProgressBar();
            this.labelExport = new System.Windows.Forms.Label();
            this.loadStatus = new System.Windows.Forms.Label();
            this.projectsDataGrid = new System.Windows.Forms.DataGridView();
            this.Sno = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Project = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Load = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Delete = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.projectsDataGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.Window;
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1027, 42);
            this.panel1.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.GrayText;
            this.label1.Location = new System.Drawing.Point(12, 56);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(126, 25);
            this.label1.TabIndex = 10;
            this.label1.Text = "Dashboard";
            // 
            // newprojectBtn
            // 
            this.newprojectBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.newprojectBtn.BackColor = System.Drawing.SystemColors.HotTrack;
            this.newprojectBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.newprojectBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.newprojectBtn.ForeColor = System.Drawing.SystemColors.Control;
            this.newprojectBtn.Location = new System.Drawing.Point(746, 56);
            this.newprojectBtn.Name = "newprojectBtn";
            this.newprojectBtn.Size = new System.Drawing.Size(268, 31);
            this.newprojectBtn.TabIndex = 6;
            this.newprojectBtn.Text = "Create Own";
            this.newprojectBtn.UseVisualStyleBackColor = false;
            this.newprojectBtn.Click += new System.EventHandler(this.newprojectBtn_Click);
            this.newprojectBtn.MouseHover += new System.EventHandler(this.newprojectBtn_MouseHover);
            // 
            // label3Load
            // 
            this.label3Load.AutoSize = true;
            this.label3Load.Location = new System.Drawing.Point(198, 341);
            this.label3Load.Name = "label3Load";
            this.label3Load.Size = new System.Drawing.Size(0, 13);
            this.label3Load.TabIndex = 17;
            // 
            // label4Success
            // 
            this.label4Success.AutoSize = true;
            this.label4Success.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4Success.Location = new System.Drawing.Point(809, 365);
            this.label4Success.Name = "label4Success";
            this.label4Success.Size = new System.Drawing.Size(0, 18);
            this.label4Success.TabIndex = 18;
            this.label4Success.Visible = false;
            // 
            // BtnExporttoExcel
            // 
            this.BtnExporttoExcel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnExporttoExcel.BackColor = System.Drawing.SystemColors.HotTrack;
            this.BtnExporttoExcel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnExporttoExcel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnExporttoExcel.ForeColor = System.Drawing.SystemColors.Control;
            this.BtnExporttoExcel.Location = new System.Drawing.Point(144, 107);
            this.BtnExporttoExcel.Name = "BtnExporttoExcel";
            this.BtnExporttoExcel.Size = new System.Drawing.Size(166, 33);
            this.BtnExporttoExcel.TabIndex = 60;
            this.BtnExporttoExcel.Text = "Save Project";
            this.BtnExporttoExcel.UseVisualStyleBackColor = false;
            this.BtnExporttoExcel.Click += new System.EventHandler(this.BtnExporttoExcel_Click);
            // 
            // progressBar2
            // 
            this.progressBar2.Location = new System.Drawing.Point(144, 568);
            this.progressBar2.Name = "progressBar2";
            this.progressBar2.Size = new System.Drawing.Size(632, 23);
            this.progressBar2.TabIndex = 64;
            this.progressBar2.Visible = false;
            // 
            // labelExport
            // 
            this.labelExport.AutoSize = true;
            this.labelExport.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelExport.Location = new System.Drawing.Point(141, 523);
            this.labelExport.Name = "labelExport";
            this.labelExport.Size = new System.Drawing.Size(110, 18);
            this.labelExport.TabIndex = 61;
            this.labelExport.Text = "exportStatusLbl";
            this.labelExport.Visible = false;
            // 
            // loadStatus
            // 
            this.loadStatus.AutoSize = true;
            this.loadStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.loadStatus.Location = new System.Drawing.Point(141, 465);
            this.loadStatus.Name = "loadStatus";
            this.loadStatus.Size = new System.Drawing.Size(111, 18);
            this.loadStatus.TabIndex = 63;
            this.loadStatus.Text = "importStatusLbl";
            this.loadStatus.Visible = false;
            // 
            // projectsDataGrid
            // 
            this.projectsDataGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.projectsDataGrid.BackgroundColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.projectsDataGrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.projectsDataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.projectsDataGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Sno,
            this.Project,
            this.Load,
            this.Delete});
            this.projectsDataGrid.Location = new System.Drawing.Point(144, 146);
            this.projectsDataGrid.Name = "projectsDataGrid";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.projectsDataGrid.RowHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.projectsDataGrid.Size = new System.Drawing.Size(632, 316);
            this.projectsDataGrid.TabIndex = 62;
            this.projectsDataGrid.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.projectsDataGrid_CellClick_1);
            // 
            // Sno
            // 
            this.Sno.HeaderText = "No";
            this.Sno.Name = "Sno";
            this.Sno.ReadOnly = true;
            // 
            // Project
            // 
            this.Project.HeaderText = "Project";
            this.Project.Name = "Project";
            this.Project.ReadOnly = true;
            // 
            // Load
            // 
            this.Load.HeaderText = "Load";
            this.Load.Name = "Load";
            this.Load.ReadOnly = true;
            // 
            // Delete
            // 
            this.Delete.HeaderText = "Delete";
            this.Delete.Name = "Delete";
            this.Delete.ReadOnly = true;
            // 
            // Dashboard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1027, 670);
            this.Controls.Add(this.BtnExporttoExcel);
            this.Controls.Add(this.progressBar2);
            this.Controls.Add(this.labelExport);
            this.Controls.Add(this.loadStatus);
            this.Controls.Add(this.projectsDataGrid);
            this.Controls.Add(this.label4Success);
            this.Controls.Add(this.label3Load);
            this.Controls.Add(this.newprojectBtn);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.panel1);
            this.Name = "Dashboard";
            this.Text = "Dashboard";
            ((System.ComponentModel.ISupportInitialize)(this.projectsDataGrid)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button newprojectBtn;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Label label3Load;
        private System.Windows.Forms.Label label4Success;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button BtnExporttoExcel;
        private System.Windows.Forms.ProgressBar progressBar2;
        private System.Windows.Forms.Label labelExport;
        private System.Windows.Forms.Label loadStatus;
        private System.Windows.Forms.DataGridView projectsDataGrid;
        private System.Windows.Forms.DataGridViewTextBoxColumn Sno;
        private System.Windows.Forms.DataGridViewTextBoxColumn Project;
        private System.Windows.Forms.DataGridViewTextBoxColumn Load;
        private System.Windows.Forms.DataGridViewTextBoxColumn Delete;
    }
}