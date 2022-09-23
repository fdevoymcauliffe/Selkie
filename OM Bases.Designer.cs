namespace SELKIE
{
    partial class OM_Bases
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel1 = new System.Windows.Forms.Panel();
            this.buttonAddBases = new System.Windows.Forms.Button();
            this.SaveBaseAlert = new System.Windows.Forms.Label();
            this.lblprojecrdetails = new System.Windows.Forms.Label();
            this.dataGridViewBaseDetails = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewBaseDetails)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.Window;
            this.panel1.Controls.Add(this.buttonAddBases);
            this.panel1.Controls.Add(this.SaveBaseAlert);
            this.panel1.Controls.Add(this.lblprojecrdetails);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1027, 42);
            this.panel1.TabIndex = 3;
            // 
            // buttonAddBases
            // 
            this.buttonAddBases.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonAddBases.BackColor = System.Drawing.SystemColors.HotTrack;
            this.buttonAddBases.FlatAppearance.BorderSize = 0;
            this.buttonAddBases.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonAddBases.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonAddBases.ForeColor = System.Drawing.SystemColors.Control;
            this.buttonAddBases.Location = new System.Drawing.Point(867, 5);
            this.buttonAddBases.Margin = new System.Windows.Forms.Padding(4);
            this.buttonAddBases.Name = "buttonAddBases";
            this.buttonAddBases.Size = new System.Drawing.Size(147, 34);
            this.buttonAddBases.TabIndex = 53;
            this.buttonAddBases.Text = "Add Bases";
            this.buttonAddBases.UseVisualStyleBackColor = false;
            this.buttonAddBases.Click += new System.EventHandler(this.buttonAddBases_Click);
            // 
            // SaveBaseAlert
            // 
            this.SaveBaseAlert.AutoSize = true;
            this.SaveBaseAlert.ForeColor = System.Drawing.Color.Red;
            this.SaveBaseAlert.Location = new System.Drawing.Point(149, 23);
            this.SaveBaseAlert.Name = "SaveBaseAlert";
            this.SaveBaseAlert.Size = new System.Drawing.Size(35, 13);
            this.SaveBaseAlert.TabIndex = 3;
            this.SaveBaseAlert.Text = "label2";
            this.SaveBaseAlert.Visible = false;
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
            // dataGridViewBaseDetails
            // 
            this.dataGridViewBaseDetails.AllowUserToAddRows = false;
            this.dataGridViewBaseDetails.AllowUserToDeleteRows = false;
            this.dataGridViewBaseDetails.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewBaseDetails.BackgroundColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewBaseDetails.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewBaseDetails.ColumnHeadersHeight = 44;
            this.dataGridViewBaseDetails.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Column3,
            this.Column4,
            this.Column5});
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewBaseDetails.DefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridViewBaseDetails.Location = new System.Drawing.Point(12, 61);
            this.dataGridViewBaseDetails.Name = "dataGridViewBaseDetails";
            this.dataGridViewBaseDetails.ReadOnly = true;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewBaseDetails.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            dataGridViewCellStyle5.ForeColor = System.Drawing.Color.Black;
            this.dataGridViewBaseDetails.RowsDefaultCellStyle = dataGridViewCellStyle5;
            this.dataGridViewBaseDetails.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewBaseDetails.Size = new System.Drawing.Size(955, 477);
            this.dataGridViewBaseDetails.TabIndex = 42;
            this.dataGridViewBaseDetails.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewBaseDetails_CellClick_1);
            // 
            // Column1
            // 
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Column1.DefaultCellStyle = dataGridViewCellStyle2;
            this.Column1.FillWeight = 91.16022F;
            this.Column1.HeaderText = "Base name";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            // 
            // Column2
            // 
            this.Column2.FillWeight = 96.73398F;
            this.Column2.HeaderText = "Annual cost";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            // 
            // Column3
            // 
            this.Column3.FillWeight = 101.1446F;
            this.Column3.HeaderText = "Distance to farm (km)";
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            // 
            // Column4
            // 
            this.Column4.FillWeight = 104.979F;
            this.Column4.HeaderText = "Number of technicians";
            this.Column4.Name = "Column4";
            this.Column4.ReadOnly = true;
            // 
            // Column5
            // 
            this.Column5.FillWeight = 105.9822F;
            this.Column5.HeaderText = "Annual salary per technicians";
            this.Column5.Name = "Column5";
            this.Column5.ReadOnly = true;
            // 
            // OM_Bases
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1027, 670);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.dataGridViewBaseDetails);
            this.Name = "OM_Bases";
            this.Text = "OM_Bases";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewBaseDetails)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblprojecrdetails;
        private System.Windows.Forms.Label SaveBaseAlert;
        private System.Windows.Forms.Button buttonAddBases;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
        public System.Windows.Forms.DataGridView dataGridViewBaseDetails;
    }
}