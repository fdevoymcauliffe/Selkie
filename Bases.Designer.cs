namespace SELKIE
{
    partial class Bases
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel1 = new System.Windows.Forms.Panel();
            this.limitWarningLbl = new System.Windows.Forms.Label();
            this.buttonAddBases = new System.Windows.Forms.Button();
            this.lblprojecrdetails = new System.Windows.Forms.Label();
            this.dataGridViewBases = new System.Windows.Forms.DataGridView();
            this.Edit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Delete = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column9 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewBases)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.Window;
            this.panel1.Controls.Add(this.limitWarningLbl);
            this.panel1.Controls.Add(this.buttonAddBases);
            this.panel1.Controls.Add(this.lblprojecrdetails);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1027, 42);
            this.panel1.TabIndex = 6;
            // 
            // limitWarningLbl
            // 
            this.limitWarningLbl.AutoSize = true;
            this.limitWarningLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.limitWarningLbl.ForeColor = System.Drawing.Color.DarkRed;
            this.limitWarningLbl.Location = new System.Drawing.Point(322, 7);
            this.limitWarningLbl.Name = "limitWarningLbl";
            this.limitWarningLbl.Size = new System.Drawing.Size(77, 33);
            this.limitWarningLbl.TabIndex = 52;
            this.limitWarningLbl.Text = "label";
            this.limitWarningLbl.Visible = false;
            // 
            // buttonAddBases
            // 
            this.buttonAddBases.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonAddBases.BackColor = System.Drawing.SystemColors.HotTrack;
            this.buttonAddBases.FlatAppearance.BorderSize = 0;
            this.buttonAddBases.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonAddBases.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonAddBases.ForeColor = System.Drawing.SystemColors.Control;
            this.buttonAddBases.Location = new System.Drawing.Point(867, 4);
            this.buttonAddBases.Margin = new System.Windows.Forms.Padding(4);
            this.buttonAddBases.Name = "buttonAddBases";
            this.buttonAddBases.Size = new System.Drawing.Size(147, 34);
            this.buttonAddBases.TabIndex = 51;
            this.buttonAddBases.Text = "Add Bases";
            this.buttonAddBases.UseVisualStyleBackColor = false;
            this.buttonAddBases.Click += new System.EventHandler(this.buttonAddBases_Click);
            // 
            // lblprojecrdetails
            // 
            this.lblprojecrdetails.AutoSize = true;
            this.lblprojecrdetails.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblprojecrdetails.ForeColor = System.Drawing.SystemColors.GrayText;
            this.lblprojecrdetails.Location = new System.Drawing.Point(8, 14);
            this.lblprojecrdetails.Name = "lblprojecrdetails";
            this.lblprojecrdetails.Size = new System.Drawing.Size(77, 25);
            this.lblprojecrdetails.TabIndex = 1;
            this.lblprojecrdetails.Text = "Bases";
            // 
            // dataGridViewBases
            // 
            this.dataGridViewBases.AllowUserToAddRows = false;
            this.dataGridViewBases.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Black;
            this.dataGridViewBases.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewBases.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewBases.BackgroundColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewBases.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridViewBases.ColumnHeadersHeight = 44;
            this.dataGridViewBases.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Edit,
            this.Delete,
            this.Column2,
            this.Column3,
            this.Column4,
            this.Column8,
            this.Column9});
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewBases.DefaultCellStyle = dataGridViewCellStyle5;
            this.dataGridViewBases.Location = new System.Drawing.Point(36, 97);
            this.dataGridViewBases.Name = "dataGridViewBases";
            this.dataGridViewBases.ReadOnly = true;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewBases.RowHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.dataGridViewBases.RowHeadersWidth = 25;
            dataGridViewCellStyle7.ForeColor = System.Drawing.Color.Black;
            this.dataGridViewBases.RowsDefaultCellStyle = dataGridViewCellStyle7;
            this.dataGridViewBases.RowTemplate.Height = 25;
            this.dataGridViewBases.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewBases.Size = new System.Drawing.Size(955, 477);
            this.dataGridViewBases.TabIndex = 236;
            this.dataGridViewBases.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewBases_CellClick);
            // 
            // Edit
            // 
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
            this.Edit.DefaultCellStyle = dataGridViewCellStyle3;
            this.Edit.HeaderText = "Edit";
            this.Edit.Name = "Edit";
            this.Edit.ReadOnly = true;
            // 
            // Delete
            // 
            this.Delete.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Delete.HeaderText = "Delete";
            this.Delete.Name = "Delete";
            this.Delete.ReadOnly = true;
            this.Delete.Width = 75;
            // 
            // Column2
            // 
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Column2.DefaultCellStyle = dataGridViewCellStyle4;
            this.Column2.HeaderText = "Base name";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            // 
            // Column3
            // 
            this.Column3.HeaderText = "Annual cost";
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            // 
            // Column4
            // 
            this.Column4.HeaderText = "Distance to farm (km)";
            this.Column4.Name = "Column4";
            this.Column4.ReadOnly = true;
            // 
            // Column8
            // 
            this.Column8.HeaderText = "Number of technicians";
            this.Column8.Name = "Column8";
            this.Column8.ReadOnly = true;
            // 
            // Column9
            // 
            this.Column9.HeaderText = "Annual salary per technicians";
            this.Column9.Name = "Column9";
            this.Column9.ReadOnly = true;
            // 
            // Bases
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1027, 670);
            this.Controls.Add(this.dataGridViewBases);
            this.Controls.Add(this.panel1);
            this.Name = "Bases";
            this.Text = "Bases";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewBases)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        public System.Windows.Forms.Label limitWarningLbl;
        private System.Windows.Forms.Button buttonAddBases;
        private System.Windows.Forms.Label lblprojecrdetails;
        public System.Windows.Forms.DataGridView dataGridViewBases;
        private System.Windows.Forms.DataGridViewTextBoxColumn Edit;
        private System.Windows.Forms.DataGridViewTextBoxColumn Delete;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column8;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column9;
    }
}