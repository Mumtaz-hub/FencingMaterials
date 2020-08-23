namespace FencingMaterials
{
    partial class Report
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Report));
            this.pnlMain = new System.Windows.Forms.Panel();
            this.pnlRight = new System.Windows.Forms.Panel();
            this.pnlGrid = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.dgvGrid = new System.Windows.Forms.DataGridView();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnExportToExcel = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.rbAC = new System.Windows.Forms.RadioButton();
            this.rbNonAc = new System.Windows.Forms.RadioButton();
            this.grpBillDate = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtToBillDate = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtFromBillDate = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.btnOk = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.rbDineIn = new System.Windows.Forms.RadioButton();
            this.rbPickup = new System.Windows.Forms.RadioButton();
            this.rbDelivery = new System.Windows.Forms.RadioButton();
            this.rbAll = new System.Windows.Forms.RadioButton();
            this.grpBillNo = new System.Windows.Forms.GroupBox();
            this.txtToBillNo = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.txtFromBillNo = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rbSummary = new System.Windows.Forms.RadioButton();
            this.rbDetails = new System.Windows.Forms.RadioButton();
            this.pnlLeft = new System.Windows.Forms.Panel();
            this.rbMonthWiseDetails = new System.Windows.Forms.RadioButton();
            this.label15 = new System.Windows.Forms.Label();
            this.rbItemDetails = new System.Windows.Forms.RadioButton();
            this.rbBillDatewiseDetails = new System.Windows.Forms.RadioButton();
            this.rbBillNowiseDetails = new System.Windows.Forms.RadioButton();
            this.pnlMain.SuspendLayout();
            this.pnlRight.SuspendLayout();
            this.pnlGrid.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvGrid)).BeginInit();
            this.panel2.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.grpBillDate.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.grpBillNo.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.pnlLeft.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlMain
            // 
            this.pnlMain.Controls.Add(this.pnlRight);
            this.pnlMain.Controls.Add(this.pnlLeft);
            resources.ApplyResources(this.pnlMain, "pnlMain");
            this.pnlMain.Name = "pnlMain";
            // 
            // pnlRight
            // 
            this.pnlRight.BackColor = System.Drawing.SystemColors.Control;
            this.pnlRight.Controls.Add(this.pnlGrid);
            this.pnlRight.Controls.Add(this.groupBox2);
            this.pnlRight.Controls.Add(this.grpBillDate);
            this.pnlRight.Controls.Add(this.btnOk);
            this.pnlRight.Controls.Add(this.groupBox3);
            this.pnlRight.Controls.Add(this.grpBillNo);
            this.pnlRight.Controls.Add(this.groupBox1);
            resources.ApplyResources(this.pnlRight, "pnlRight");
            this.pnlRight.Name = "pnlRight";
            // 
            // pnlGrid
            // 
            this.pnlGrid.Controls.Add(this.panel3);
            this.pnlGrid.Controls.Add(this.panel2);
            resources.ApplyResources(this.pnlGrid, "pnlGrid");
            this.pnlGrid.Name = "pnlGrid";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.dgvGrid);
            resources.ApplyResources(this.panel3, "panel3");
            this.panel3.Name = "panel3";
            // 
            // dgvGrid
            // 
            this.dgvGrid.AllowUserToAddRows = false;
            this.dgvGrid.AllowUserToDeleteRows = false;
            this.dgvGrid.AllowUserToResizeRows = false;
            this.dgvGrid.BackgroundColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Verdana", 11.25F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvGrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            resources.ApplyResources(this.dgvGrid, "dgvGrid");
            this.dgvGrid.Name = "dgvGrid";
            this.dgvGrid.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgvGrid_KeyDown);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnClose);
            this.panel2.Controls.Add(this.btnExportToExcel);
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.Name = "panel2";
            // 
            // btnClose
            // 
            resources.ApplyResources(this.btnClose, "btnClose");
            this.btnClose.BackColor = System.Drawing.SystemColors.Control;
            this.btnClose.Name = "btnClose";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnExportToExcel
            // 
            resources.ApplyResources(this.btnExportToExcel, "btnExportToExcel");
            this.btnExportToExcel.BackColor = System.Drawing.SystemColors.Control;
            this.btnExportToExcel.Name = "btnExportToExcel";
            this.btnExportToExcel.UseVisualStyleBackColor = false;
            this.btnExportToExcel.Click += new System.EventHandler(this.btnExportToExcel_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.rbAC);
            this.groupBox2.Controls.Add(this.rbNonAc);
            this.groupBox2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.ForeColor = System.Drawing.Color.Blue;
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // rbAC
            // 
            resources.ApplyResources(this.rbAC, "rbAC");
            this.rbAC.ForeColor = System.Drawing.Color.Black;
            this.rbAC.Name = "rbAC";
            this.rbAC.UseVisualStyleBackColor = true;
            // 
            // rbNonAc
            // 
            resources.ApplyResources(this.rbNonAc, "rbNonAc");
            this.rbNonAc.Checked = true;
            this.rbNonAc.ForeColor = System.Drawing.Color.Black;
            this.rbNonAc.Name = "rbNonAc";
            this.rbNonAc.TabStop = true;
            this.rbNonAc.UseVisualStyleBackColor = true;
            // 
            // grpBillDate
            // 
            this.grpBillDate.Controls.Add(this.label1);
            this.grpBillDate.Controls.Add(this.txtToBillDate);
            this.grpBillDate.Controls.Add(this.label9);
            this.grpBillDate.Controls.Add(this.txtFromBillDate);
            this.grpBillDate.Controls.Add(this.label10);
            this.grpBillDate.FlatStyle = System.Windows.Forms.FlatStyle.System;
            resources.ApplyResources(this.grpBillDate, "grpBillDate");
            this.grpBillDate.ForeColor = System.Drawing.Color.Blue;
            this.grpBillDate.Name = "grpBillDate";
            this.grpBillDate.TabStop = false;
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Name = "label1";
            // 
            // txtToBillDate
            // 
            resources.ApplyResources(this.txtToBillDate, "txtToBillDate");
            this.txtToBillDate.Name = "txtToBillDate";
            // 
            // label9
            // 
            resources.ApplyResources(this.label9, "label9");
            this.label9.ForeColor = System.Drawing.Color.Black;
            this.label9.Name = "label9";
            // 
            // txtFromBillDate
            // 
            resources.ApplyResources(this.txtFromBillDate, "txtFromBillDate");
            this.txtFromBillDate.Name = "txtFromBillDate";
            this.txtFromBillDate.TextChanged += new System.EventHandler(this.txtFromBillDate_TextChanged);
            // 
            // label10
            // 
            resources.ApplyResources(this.label10, "label10");
            this.label10.ForeColor = System.Drawing.Color.Black;
            this.label10.Name = "label10";
            // 
            // btnOk
            // 
            this.btnOk.FlatAppearance.MouseDownBackColor = System.Drawing.Color.DodgerBlue;
            this.btnOk.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DodgerBlue;
            resources.ApplyResources(this.btnOk, "btnOk");
            this.btnOk.ForeColor = System.Drawing.Color.Firebrick;
            this.btnOk.Name = "btnOk";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.rbDineIn);
            this.groupBox3.Controls.Add(this.rbPickup);
            this.groupBox3.Controls.Add(this.rbDelivery);
            this.groupBox3.Controls.Add(this.rbAll);
            this.groupBox3.FlatStyle = System.Windows.Forms.FlatStyle.System;
            resources.ApplyResources(this.groupBox3, "groupBox3");
            this.groupBox3.ForeColor = System.Drawing.Color.Blue;
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.TabStop = false;
            // 
            // rbDineIn
            // 
            resources.ApplyResources(this.rbDineIn, "rbDineIn");
            this.rbDineIn.ForeColor = System.Drawing.Color.Black;
            this.rbDineIn.Name = "rbDineIn";
            this.rbDineIn.UseVisualStyleBackColor = true;
            // 
            // rbPickup
            // 
            resources.ApplyResources(this.rbPickup, "rbPickup");
            this.rbPickup.ForeColor = System.Drawing.Color.Black;
            this.rbPickup.Name = "rbPickup";
            this.rbPickup.UseVisualStyleBackColor = true;
            // 
            // rbDelivery
            // 
            resources.ApplyResources(this.rbDelivery, "rbDelivery");
            this.rbDelivery.ForeColor = System.Drawing.Color.Black;
            this.rbDelivery.Name = "rbDelivery";
            this.rbDelivery.UseVisualStyleBackColor = true;
            // 
            // rbAll
            // 
            resources.ApplyResources(this.rbAll, "rbAll");
            this.rbAll.Checked = true;
            this.rbAll.ForeColor = System.Drawing.Color.Black;
            this.rbAll.Name = "rbAll";
            this.rbAll.TabStop = true;
            this.rbAll.UseVisualStyleBackColor = true;
            // 
            // grpBillNo
            // 
            this.grpBillNo.Controls.Add(this.txtToBillNo);
            this.grpBillNo.Controls.Add(this.label11);
            this.grpBillNo.Controls.Add(this.txtFromBillNo);
            this.grpBillNo.Controls.Add(this.label12);
            this.grpBillNo.FlatStyle = System.Windows.Forms.FlatStyle.System;
            resources.ApplyResources(this.grpBillNo, "grpBillNo");
            this.grpBillNo.ForeColor = System.Drawing.Color.Blue;
            this.grpBillNo.Name = "grpBillNo";
            this.grpBillNo.TabStop = false;
            // 
            // txtToBillNo
            // 
            resources.ApplyResources(this.txtToBillNo, "txtToBillNo");
            this.txtToBillNo.Name = "txtToBillNo";
            // 
            // label11
            // 
            resources.ApplyResources(this.label11, "label11");
            this.label11.ForeColor = System.Drawing.Color.Black;
            this.label11.Name = "label11";
            // 
            // txtFromBillNo
            // 
            resources.ApplyResources(this.txtFromBillNo, "txtFromBillNo");
            this.txtFromBillNo.Name = "txtFromBillNo";
            // 
            // label12
            // 
            resources.ApplyResources(this.label12, "label12");
            this.label12.ForeColor = System.Drawing.Color.Black;
            this.label12.Name = "label12";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rbSummary);
            this.groupBox1.Controls.Add(this.rbDetails);
            this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.ForeColor = System.Drawing.Color.Blue;
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // rbSummary
            // 
            resources.ApplyResources(this.rbSummary, "rbSummary");
            this.rbSummary.ForeColor = System.Drawing.Color.Black;
            this.rbSummary.Name = "rbSummary";
            this.rbSummary.UseVisualStyleBackColor = true;
            // 
            // rbDetails
            // 
            resources.ApplyResources(this.rbDetails, "rbDetails");
            this.rbDetails.Checked = true;
            this.rbDetails.ForeColor = System.Drawing.Color.Black;
            this.rbDetails.Name = "rbDetails";
            this.rbDetails.TabStop = true;
            this.rbDetails.UseVisualStyleBackColor = true;
            // 
            // pnlLeft
            // 
            this.pnlLeft.BackColor = System.Drawing.SystemColors.Control;
            this.pnlLeft.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlLeft.Controls.Add(this.rbMonthWiseDetails);
            this.pnlLeft.Controls.Add(this.label15);
            this.pnlLeft.Controls.Add(this.rbItemDetails);
            this.pnlLeft.Controls.Add(this.rbBillDatewiseDetails);
            this.pnlLeft.Controls.Add(this.rbBillNowiseDetails);
            resources.ApplyResources(this.pnlLeft, "pnlLeft");
            this.pnlLeft.Name = "pnlLeft";
            // 
            // rbMonthWiseDetails
            // 
            resources.ApplyResources(this.rbMonthWiseDetails, "rbMonthWiseDetails");
            this.rbMonthWiseDetails.Name = "rbMonthWiseDetails";
            this.rbMonthWiseDetails.UseVisualStyleBackColor = true;
            // 
            // label15
            // 
            resources.ApplyResources(this.label15, "label15");
            this.label15.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.label15.Name = "label15";
            // 
            // rbItemDetails
            // 
            resources.ApplyResources(this.rbItemDetails, "rbItemDetails");
            this.rbItemDetails.Name = "rbItemDetails";
            this.rbItemDetails.UseVisualStyleBackColor = true;
            // 
            // rbBillDatewiseDetails
            // 
            resources.ApplyResources(this.rbBillDatewiseDetails, "rbBillDatewiseDetails");
            this.rbBillDatewiseDetails.Name = "rbBillDatewiseDetails";
            this.rbBillDatewiseDetails.UseVisualStyleBackColor = true;
            // 
            // rbBillNowiseDetails
            // 
            resources.ApplyResources(this.rbBillNowiseDetails, "rbBillNowiseDetails");
            this.rbBillNowiseDetails.Checked = true;
            this.rbBillNowiseDetails.Name = "rbBillNowiseDetails";
            this.rbBillNowiseDetails.TabStop = true;
            this.rbBillNowiseDetails.UseVisualStyleBackColor = true;
            // 
            // Report
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ControlBox = false;
            this.Controls.Add(this.pnlMain);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.KeyPreview = true;
            this.Name = "Report";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.Report_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Report_KeyDown);
            this.pnlMain.ResumeLayout(false);
            this.pnlRight.ResumeLayout(false);
            this.pnlGrid.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvGrid)).EndInit();
            this.panel2.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.grpBillDate.ResumeLayout(false);
            this.grpBillDate.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.grpBillNo.ResumeLayout(false);
            this.grpBillNo.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.pnlLeft.ResumeLayout(false);
            this.pnlLeft.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlMain;
        private System.Windows.Forms.Panel pnlLeft;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.RadioButton rbItemDetails;
        private System.Windows.Forms.RadioButton rbBillDatewiseDetails;
        private System.Windows.Forms.RadioButton rbBillNowiseDetails;
        private System.Windows.Forms.Panel pnlRight;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.GroupBox grpBillNo;
        private System.Windows.Forms.TextBox txtToBillNo;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtFromBillNo;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rbSummary;
        private System.Windows.Forms.RadioButton rbDetails;
        private System.Windows.Forms.GroupBox grpBillDate;
        private System.Windows.Forms.TextBox txtToBillDate;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtFromBillDate;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.RadioButton rbDineIn;
        private System.Windows.Forms.RadioButton rbPickup;
        private System.Windows.Forms.RadioButton rbDelivery;
        private System.Windows.Forms.RadioButton rbAll;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton rbNonAc;
        private System.Windows.Forms.RadioButton rbAC;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton rbMonthWiseDetails;
        private System.Windows.Forms.Panel pnlGrid;
        private System.Windows.Forms.DataGridView dgvGrid;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel2;
        internal System.Windows.Forms.Button btnExportToExcel;
        internal System.Windows.Forms.Button btnClose;
    }
}