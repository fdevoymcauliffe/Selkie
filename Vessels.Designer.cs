namespace SELKIE
{
    partial class Vessels
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel1 = new System.Windows.Forms.Panel();
            this.buttonAddVessel = new System.Windows.Forms.Button();
            this.SaveVesselAlert = new System.Windows.Forms.Label();
            this.lblprojecrdetails = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.dataGridViewVesselDetails = new System.Windows.Forms.DataGridView();
            this.Edit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Delete = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column17 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column18 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column9 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column10 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column11 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column12 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column13 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column16 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Purchased = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewVesselDetails)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.Window;
            this.panel1.Controls.Add(this.buttonAddVessel);
            this.panel1.Controls.Add(this.SaveVesselAlert);
            this.panel1.Controls.Add(this.lblprojecrdetails);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1027, 42);
            this.panel1.TabIndex = 4;
            // 
            // buttonAddVessel
            // 
            this.buttonAddVessel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonAddVessel.BackColor = System.Drawing.SystemColors.HotTrack;
            this.buttonAddVessel.FlatAppearance.BorderSize = 0;
            this.buttonAddVessel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonAddVessel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonAddVessel.ForeColor = System.Drawing.SystemColors.Control;
            this.buttonAddVessel.Location = new System.Drawing.Point(876, 4);
            this.buttonAddVessel.Margin = new System.Windows.Forms.Padding(4);
            this.buttonAddVessel.Name = "buttonAddVessel";
            this.buttonAddVessel.Size = new System.Drawing.Size(147, 34);
            this.buttonAddVessel.TabIndex = 52;
            this.buttonAddVessel.Text = "Add Vessel";
            this.buttonAddVessel.UseVisualStyleBackColor = false;
            this.buttonAddVessel.Click += new System.EventHandler(this.buttonAddVessel_Click);
            // 
            // SaveVesselAlert
            // 
            this.SaveVesselAlert.AutoSize = true;
            this.SaveVesselAlert.ForeColor = System.Drawing.Color.Red;
            this.SaveVesselAlert.Location = new System.Drawing.Point(127, 14);
            this.SaveVesselAlert.Name = "SaveVesselAlert";
            this.SaveVesselAlert.Size = new System.Drawing.Size(35, 13);
            this.SaveVesselAlert.TabIndex = 2;
            this.SaveVesselAlert.Text = "label2";
            this.SaveVesselAlert.Visible = false;
            // 
            // lblprojecrdetails
            // 
            this.lblprojecrdetails.AutoSize = true;
            this.lblprojecrdetails.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblprojecrdetails.ForeColor = System.Drawing.SystemColors.GrayText;
            this.lblprojecrdetails.Location = new System.Drawing.Point(8, 14);
            this.lblprojecrdetails.Name = "lblprojecrdetails";
            this.lblprojecrdetails.Size = new System.Drawing.Size(95, 25);
            this.lblprojecrdetails.TabIndex = 1;
            this.lblprojecrdetails.Text = "Vessels";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.dataGridViewVesselDetails);
            this.panel2.Location = new System.Drawing.Point(13, 63);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(988, 459);
            this.panel2.TabIndex = 5;
            // 
            // dataGridViewVesselDetails
            // 
            this.dataGridViewVesselDetails.AllowUserToAddRows = false;
            this.dataGridViewVesselDetails.AllowUserToDeleteRows = false;
            this.dataGridViewVesselDetails.BackgroundColor = System.Drawing.Color.GhostWhite;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewVesselDetails.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewVesselDetails.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewVesselDetails.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Edit,
            this.Delete,
            this.Column1,
            this.Column2,
            this.Column3,
            this.Column4,
            this.Column5,
            this.Column17,
            this.Column18,
            this.Column6,
            this.Column7,
            this.Column8,
            this.Column9,
            this.Column10,
            this.Column11,
            this.Column12,
            this.Column13,
            this.Column16,
            this.Purchased});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewVesselDetails.DefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridViewVesselDetails.GridColor = System.Drawing.SystemColors.MenuText;
            this.dataGridViewVesselDetails.Location = new System.Drawing.Point(0, 1);
            this.dataGridViewVesselDetails.Name = "dataGridViewVesselDetails";
            this.dataGridViewVesselDetails.ReadOnly = true;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewVesselDetails.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.Black;
            this.dataGridViewVesselDetails.RowsDefaultCellStyle = dataGridViewCellStyle4;
            this.dataGridViewVesselDetails.Size = new System.Drawing.Size(988, 458);
            this.dataGridViewVesselDetails.TabIndex = 40;
            this.dataGridViewVesselDetails.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewVesselDetails_CellClick_1);
            // 
            // Edit
            // 
            this.Edit.HeaderText = "Edit";
            this.Edit.Name = "Edit";
            this.Edit.ReadOnly = true;
            // 
            // Delete
            // 
            this.Delete.HeaderText = "Delete";
            this.Delete.Name = "Delete";
            this.Delete.ReadOnly = true;
            // 
            // Column1
            // 
            this.Column1.HeaderText = "Vessel classification";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            // 
            // Column2
            // 
            this.Column2.HeaderText = "Number";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            // 
            // Column3
            // 
            this.Column3.HeaderText = "Technician capacity";
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            // 
            // Column4
            // 
            this.Column4.HeaderText = "Night work - operates 24/7";
            this.Column4.Name = "Column4";
            this.Column4.ReadOnly = true;
            // 
            // Column5
            // 
            this.Column5.HeaderText = "Annual running cost";
            this.Column5.Name = "Column5";
            this.Column5.ReadOnly = true;
            // 
            // Column17
            // 
            this.Column17.HeaderText = "Hire as required";
            this.Column17.Name = "Column17";
            this.Column17.ReadOnly = true;
            // 
            // Column18
            // 
            this.Column18.HeaderText = "Vessel lead time ";
            this.Column18.Name = "Column18";
            this.Column18.ReadOnly = true;
            // 
            // Column6
            // 
            this.Column6.HeaderText = "Rental start day";
            this.Column6.Name = "Column6";
            this.Column6.ReadOnly = true;
            // 
            // Column7
            // 
            this.Column7.HeaderText = "Rental end day";
            this.Column7.Name = "Column7";
            this.Column7.ReadOnly = true;
            // 
            // Column8
            // 
            this.Column8.HeaderText = "Rental start month";
            this.Column8.Name = "Column8";
            this.Column8.ReadOnly = true;
            // 
            // Column9
            // 
            this.Column9.HeaderText = "Rental end month";
            this.Column9.Name = "Column9";
            this.Column9.ReadOnly = true;
            // 
            // Column10
            // 
            this.Column10.HeaderText = "Daily rental cost";
            this.Column10.Name = "Column10";
            this.Column10.ReadOnly = true;
            // 
            // Column11
            // 
            this.Column11.HeaderText = "Mobilisation cost";
            this.Column11.Name = "Column11";
            this.Column11.ReadOnly = true;
            // 
            // Column12
            // 
            this.Column12.HeaderText = "Fuel consumption";
            this.Column12.Name = "Column12";
            this.Column12.ReadOnly = true;
            // 
            // Column13
            // 
            this.Column13.HeaderText = "Fuel cost";
            this.Column13.Name = "Column13";
            this.Column13.ReadOnly = true;
            // 
            // Column16
            // 
            this.Column16.HeaderText = "Speed";
            this.Column16.Name = "Column16";
            this.Column16.ReadOnly = true;
            // 
            // Purchased
            // 
            this.Purchased.HeaderText = "Purchased";
            this.Purchased.Name = "Purchased";
            this.Purchased.ReadOnly = true;
            // 
            // Vessels
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1027, 670);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "Vessels";
            this.Text = "Vessels";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewVesselDetails)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblprojecrdetails;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label SaveVesselAlert;
        public System.Windows.Forms.DataGridView dataGridViewVesselDetails;
        private System.Windows.Forms.Button buttonAddVessel;
        private System.Windows.Forms.DataGridViewTextBoxColumn Edit;
        private System.Windows.Forms.DataGridViewTextBoxColumn Delete;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column17;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column18;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column6;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column7;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column8;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column9;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column10;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column11;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column12;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column13;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column16;
        private System.Windows.Forms.DataGridViewTextBoxColumn Purchased;
    }
}