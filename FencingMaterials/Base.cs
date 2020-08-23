using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using DBLibrary;

namespace FencingMaterials
{
    public partial class Base : Form
    {
        public Base()
        {
            InitializeComponent();
        }

        SqlDataAdapter _MainAdapter;
        DataTable _dt = new DataTable();
        DataSet dsMain = new DataSet();

        public enum Btn
        {
            Configuration = 1,
            Customer = 2,
            EstimateCost = 3,
            Report = 4,
            Settings = 5,
            BackupRestore=6,
            Logout = 7
        }
        private void DispayButtonOnUserRight()
        {
            if (DBClass.UserType == "USER")
            {
                DataTable dtMenu = new DataTable();
                DataTable dtUright = new DataTable();
                dtMenu = DBClass.GetTableRecords("Menu_Master");

                dtUright = DBClass.GetTableByQuery("Select * from User_Rights where User_Id=" + DBClass.UserId);

                bool FlagItemConfig = false;
                bool FlagSettings = false;
                bool FlagEstimate = false;
                if (dtUright.Rows.Count > 0)
                {
                    for (int row = 0; row < dtUright.Rows.Count; row++)
                    {
                        int Uright = int.Parse(dtUright.Rows[row]["U_Right"].ToString());
                        int MCode = int.Parse(dtUright.Rows[row]["Menu_Code"].ToString());

                        if (dtMenu.Rows.Count > 0)
                        {
                            switch (MCode)
                            {
                                
                                case 2:
                                    if (Uright == 1)
                                    {
                                        btnCategory.Visible = true;
                                        FlagItemConfig = true;
                                    }
                                    else
                                        btnCategory.Visible = false;
                                    break;
                                case 3:
                                    if (Uright == 1)
                                    {
                                        btnItemDetails.Visible = true;
                                        FlagItemConfig = true;
                                    }
                                    else
                                        btnItemDetails.Visible = false;
                                    break;
                                case 4:
                                    if (Uright == 1)
                                        btnCustomer.Visible = true;
                                    else
                                        btnCustomer.Visible = false;
                                    break;
                                case 5:
                                    if (Uright == 1)
                                    {
                                        btnMenuSettings.Visible = true;
                                        FlagSettings = true;
                                    }
                                    else
                                        btnMenuSettings.Visible = false;
                                    break;
                                case 6:
                                    if (Uright == 1)
                                    {
                                        btnUserSettings.Visible = true;
                                        FlagSettings = true;
                                    }
                                    else
                                        btnUserSettings.Visible = false;
                                    break;
                                case 8:
                                    if (Uright == 1)
                                        btnReport.Visible = true;
                                    else
                                        btnReport.Visible = false;
                                    break;
                                case 9:
                                    if (Uright == 1)
                                        btnBackupRestore.Visible = true;
                                    else
                                        btnBackupRestore.Visible = false;
                                    break;
                                case 10:
                                    if (Uright == 1)
                                    {
                                        btnNewDaimondMeshEstimate.Visible = true;
                                        FlagEstimate = true;
                                    }
                                    else
                                        btnNewDaimondMeshEstimate.Visible = false;
                                    break;
                                case 11:
                                    if (Uright == 1)
                                    {
                                        btnAddNewFieldfence.Visible = true;
                                        FlagEstimate = true;
                                    }
                                    else
                                        btnAddNewFieldfence.Visible = false;
                                    break;
                                case 12:
                                    if (Uright == 1)
                                    {
                                        btnShowRecords.Visible = true;
                                        FlagEstimate = true;
                                    }
                                    else
                                        btnShowRecords.Visible = false;
                                    break;
                            }
                        }
                    }

                    btnConfiguration.Visible = FlagItemConfig;
                    btnSettings.Visible = FlagSettings;
                    btnEstimateCost.Visible = FlagEstimate;
                }
            }
        }
        private void Base_Load(object sender, EventArgs e)
        {
            DispayButtonOnUserRight();
            ///For Hiding Taskbar
            //FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;

            lblDate.Text = System.DateTime.Now.ToShortDateString();

            //SelectedButtonNew((int)Btn.EstimateCost);
            //this.pnlMiddle.Controls.Clear();
            //ItemCategory obj = new ItemCategory();
            //obj.TopLevel = false;
            //obj.AutoScroll = true;
            //this.pnlMiddle.Controls.Add(obj);
            //obj.Show();
            //lblPageHead.Text = "Item Category";

            //FrmLogin obj=new FrmLogin;
            //if (Login.Username == "A")
            //    btnTransfer.Visible = true;
            //else
            //    btnTransfer.Visible = false;

            pnlLeftConfig.Visible = false;
            pnlLeftSettings.Visible = false;
            pnlLeftEstimate.Visible = false;
        }
        private void Base_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.pnlMiddle.Controls.Clear();
            }
        }
        private void btnLogout_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void SelectedButtonNew(int selected)
        {
            btnConfiguration.BackColor = System.Drawing.Color.White;
            btnConfiguration.ForeColor = System.Drawing.Color.FromArgb(1,78,130);
            btnCustomer.BackColor = System.Drawing.Color.White;
            btnCustomer.ForeColor = System.Drawing.Color.FromArgb(1, 78, 130);
            btnEstimateCost.BackColor = System.Drawing.Color.White;
            btnEstimateCost.ForeColor = System.Drawing.Color.FromArgb(1, 78, 130);
            btnReport.BackColor = System.Drawing.Color.White;
            btnReport.ForeColor = System.Drawing.Color.FromArgb(1, 78, 130);
            btnSettings.BackColor = System.Drawing.Color.White;
            btnSettings.ForeColor = System.Drawing.Color.FromArgb(1, 78, 130);
            btnBackupRestore.BackColor = System.Drawing.Color.White;
            btnBackupRestore.ForeColor = System.Drawing.Color.FromArgb(1, 78, 130);
            btnLogout.BackColor = System.Drawing.Color.White;
            btnLogout.ForeColor = System.Drawing.Color.FromArgb(1, 78, 130);

            switch (selected)
            {
                case (int)Btn.Configuration:
                    btnConfiguration.BackColor = System.Drawing.Color.FromArgb(1, 78, 130);
                    btnConfiguration.ForeColor = System.Drawing.Color.White;
                    break;
                case (int)Btn.Customer:
                    btnCustomer.BackColor = System.Drawing.Color.FromArgb(1, 78, 130);
                    btnCustomer.ForeColor = System.Drawing.Color.White;
                    break;

                case (int)Btn.EstimateCost:
                    btnEstimateCost.BackColor = System.Drawing.Color.FromArgb(1, 78, 130);
                    btnEstimateCost.ForeColor = System.Drawing.Color.White;
                    break;
                case (int)Btn.Report:
                    btnReport.BackColor = System.Drawing.Color.FromArgb(1, 78, 130);
                    btnReport.ForeColor = System.Drawing.Color.White;
                    break;
                case (int)Btn.Settings:
                    btnSettings.BackColor = System.Drawing.Color.FromArgb(1, 78, 130);
                    btnSettings.ForeColor = System.Drawing.Color.White;
                    break;
                case (int)Btn.BackupRestore:
                    btnBackupRestore.BackColor = System.Drawing.Color.FromArgb(1, 78, 130);
                    btnBackupRestore.ForeColor = System.Drawing.Color.White;
                    break;
                case (int)Btn.Logout:
                    btnLogout.BackColor = System.Drawing.Color.FromArgb(1, 78, 130);
                    btnLogout.ForeColor = System.Drawing.Color.White;
                    break;
            }
        }

        private void btnCategory_Click(object sender, EventArgs e)
        {
            pnlLeftConfig.Visible = false;
            pnlLeftSettings.Visible = false;
            pnlLeftEstimate.Visible = false;

            SelectedButtonNew((int)Btn.Configuration);
            this.pnlMiddle.Controls.Clear();
            ItemCategory obj = new ItemCategory();
            obj.TopLevel = false;
            obj.AutoScroll = true;
            this.pnlMiddle.Controls.Add(obj);
            obj.Show();
            lblPageHead.Text = "Item Category";
        }

        private void btnConfiguration_Click(object sender, EventArgs e)
        {

            if (pnlLeftConfig.Visible == true)
            {
                pnlLeftConfig.Visible = false;
                pnlLeftSettings.Visible = false;
                pnlLeftEstimate.Visible = false;
            }
            else
            {
                pnlLeftConfig.Visible = true;
                pnlLeftSettings.Visible = false;
                pnlLeftEstimate.Visible = false;
            }

            this.pnlMiddle.Controls.Clear();
            SelectedButtonNew((int)Btn.Configuration);
            
        }

        private void btnItemDetails_Click(object sender, EventArgs e)
        {
            pnlLeftConfig.Visible = false;
            pnlLeftSettings.Visible = false;
            pnlLeftEstimate.Visible = false;

            SelectedButtonNew((int)Btn.Configuration);
            this.pnlMiddle.Controls.Clear();
            ItemMaster obj = new ItemMaster();
            obj.TopLevel = false;
            obj.AutoScroll = true;
            this.pnlMiddle.Controls.Add(obj);
            obj.Show();
            lblPageHead.Text = "Item Details";
        }

        private void btnEstimateCost_Click(object sender, EventArgs e)
        {
            this.pnlMiddle.Controls.Clear();
            SelectedButtonNew((int)Btn.EstimateCost);

            if (pnlLeftEstimate.Visible == true)
            {
                pnlLeftConfig.Visible = false;
                pnlLeftSettings.Visible = false;
                pnlLeftEstimate.Visible = false;
            }
            else
            {
                pnlLeftConfig.Visible = false;
                pnlLeftSettings.Visible = false;
                pnlLeftEstimate.Visible = true;
            }

            
        }

        private void btnUserSettings_Click(object sender, EventArgs e)
        {
            pnlLeftConfig.Visible = false;
            pnlLeftSettings.Visible = false;
            pnlLeftEstimate.Visible = false;

            SelectedButtonNew((int)Btn.Settings);
            this.pnlMiddle.Controls.Clear();
            frmUserSettings obj = new frmUserSettings();
            obj.TopLevel = false;
            obj.AutoScroll = true;
            this.pnlMiddle.Controls.Add(obj);
            obj.Show();
            lblPageHead.Text = "User Settings";
        }

        private void btnMenuSettings_Click(object sender, EventArgs e)
        {
            pnlLeftConfig.Visible = false;
            pnlLeftSettings.Visible = false;
            pnlLeftEstimate.Visible = false;

            SelectedButtonNew((int)Btn.Settings);
            this.pnlMiddle.Controls.Clear();
            MenuSettings obj = new MenuSettings();
            obj.TopLevel = false;
            obj.AutoScroll = true;
            this.pnlMiddle.Controls.Add(obj);
            obj.Show();
            lblPageHead.Text = "Menu Settings";
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            this.pnlMiddle.Controls.Clear();
            SelectedButtonNew((int)Btn.Settings);

            if (pnlLeftSettings.Visible == true)
            {
                pnlLeftConfig.Visible = false;
                pnlLeftSettings.Visible = false;
                pnlLeftEstimate.Visible = false;
            }
            else
            {
                pnlLeftConfig.Visible = false;
                pnlLeftSettings.Visible = true;
                pnlLeftEstimate.Visible = false;
            }
        }

        private void btnNewDaimondMeshEstimate_Click(object sender, EventArgs e)
        {
            pnlLeftConfig.Visible = false;
            pnlLeftSettings.Visible = false;
            pnlLeftEstimate.Visible = false;

            SelectedButtonNew((int)Btn.EstimateCost);
            this.pnlMiddle.Controls.Clear();
            EstimateCost obj = new EstimateCost();
            obj.mainForm = this;
            obj.TopLevel = false;
            obj.AutoScroll = true;
            this.pnlMiddle.Controls.Add(obj);
            obj.Show();
            lblPageHead.Text = "Estimate Cost";
        }

        private void btnAddNewFieldfence_Click(object sender, EventArgs e)
        {
            pnlLeftConfig.Visible = false;
            pnlLeftSettings.Visible = false;
            pnlLeftEstimate.Visible = false;

            SelectedButtonNew((int)Btn.EstimateCost);
            this.pnlMiddle.Controls.Clear();
            EstimateCost obj = new EstimateCost();
            obj.mainForm = this;
            obj.TopLevel = false;
            obj.AutoScroll = true;
            this.pnlMiddle.Controls.Add(obj);
            obj.Show();
            lblPageHead.Text = "Estimate Cost";
        }

        private void btnShowRecords_Click(object sender, EventArgs e)
        {
            pnlLeftConfig.Visible = false;
            pnlLeftSettings.Visible = false;
            pnlLeftEstimate.Visible = false;

            SelectedButtonNew((int)Btn.EstimateCost);
            this.pnlMiddle.Controls.Clear();
            FinalEstimationEntry obj = new FinalEstimationEntry();
            //obj.mainForm = this;
            obj.TopLevel = false;
            obj.AutoScroll = true;
            this.pnlMiddle.Controls.Add(obj);
            obj.Show();
            lblPageHead.Text = "Estimation Records";
        }

        private void btnBackupRestore_Click(object sender, EventArgs e)
        {
            pnlLeftConfig.Visible = false;
            pnlLeftSettings.Visible = false;
            pnlLeftEstimate.Visible = false;

            SelectedButtonNew((int)Btn.BackupRestore);
            this.pnlMiddle.Controls.Clear();
            BackupRestore obj = new BackupRestore();
            obj.TopLevel = false;
            obj.AutoScroll = true;
            this.pnlMiddle.Controls.Add(obj);
            obj.Show();
            lblPageHead.Text = "Backup / Restore";
        }

        private void btnCustomer_Click(object sender, EventArgs e)
        {
            pnlLeftConfig.Visible = false;
            pnlLeftSettings.Visible = false;
            pnlLeftEstimate.Visible = false;

            SelectedButtonNew((int)Btn.Customer);
            this.pnlMiddle.Controls.Clear();
            Customer obj = new Customer();
            obj.TopLevel = false;
            obj.AutoScroll = true;
            this.pnlMiddle.Controls.Add(obj);
            obj.Show();
            lblPageHead.Text = "Customer Details";
        }

        private void btnReport_Click(object sender, EventArgs e)
        {
            pnlLeftConfig.Visible = false;
            pnlLeftSettings.Visible = false;
            pnlLeftEstimate.Visible = false;

            SelectedButtonNew((int)Btn.Report);
            this.pnlMiddle.Controls.Clear();
            Report obj = new Report();
            obj.TopLevel = false;
            obj.AutoScroll = true;
            this.pnlMiddle.Controls.Add(obj);
            obj.Show();
            lblPageHead.Text = "Report";
        }
    }
}
