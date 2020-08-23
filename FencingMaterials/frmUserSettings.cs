using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DBLibrary;
using System.Data.SqlClient;

namespace FencingMaterials
{
    public partial class frmUserSettings : Form
    {
        #region Declaration
        bool chkRights;
        int Menu_Code;
        DataSet dsMain = new DataSet();
        #endregion

        #region Common Function
        private void ClearAll()
        {
            txtUserId.Text = "";
            txtUserName.Text = "";
            txtOldPassword.Text = "";
            txtNewPassword.Text = "";
            txtConfirmPassword.Text = "";
            txtUserId.Enabled = true;
            txtUserName.Enabled = true;
            cboUserGroup.Enabled = true;
            chkSalesPerson.Checked = false;
        }
        private bool RequiredField()
        {
            try
            {
            if (txtUserId.Text.Trim() == "")
            {
                MessageBox.Show("Please Enter User ID ..", "Data Missing", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtUserId.Focus();
            }
            else if (cboUserGroup.Text.Trim() == "")
            {
                MessageBox.Show("Please Enter User Group..", "Data Missing", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                cboUserGroup.Focus();
            }
            else if (txtUserName.Text.Trim() == "")
            {
                MessageBox.Show("Please Enter User Name..", "Data Missing", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtUserName.Focus();
            }
            else if (txtNewPassword.Text.Trim() == "")
            {
                MessageBox.Show("Please Enter New Password..", "Data Missing", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtNewPassword.Focus();
            }
            else if (txtConfirmPassword.Text.Trim() == "")
            {
                MessageBox.Show("Please Enter New Password..", "Data Missing", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtConfirmPassword.Focus();
            }
            else if (txtConfirmPassword.Text.Trim() != txtNewPassword.Text.Trim())
            {
                MessageBox.Show("New Password and Confirm Password does not match..", "Data Missing", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtNewPassword.Focus();
                txtNewPassword.Text = "";
                txtConfirmPassword.Text = "";
            }
            else
            {
                return true;
            }
            return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }
        private void GetUserName()
        {
            try
            {
                DataRow[] row = dsMain.Tables["USER_MASTER"].Select("User_Id='" + txtUserId.Text.Trim() + "'");
                if (row.Length != 0)
                {
                    txtUserName.Text = Convert.ToString(row[0]["User_Name"]);
                    cboUserGroup.Text = Convert.ToString(row[0]["User_Type"]);
                    chkSalesPerson.Checked = Convert.ToBoolean(row[0]["User_Type"]);
                }
                else
                {
                    txtUserId.Text = "";
                    cboUserGroup.Text = "";
                    chkSalesPerson.Checked = false;
                    txtUserId.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
        }
        private void CheckDupID()
        {
            try
            {
                DataRow[] row = dsMain.Tables["USER_MASTER"].Select("User_Id=" + txtUserId.Text.Trim() + "");
                if (row.Length != 0)
                {
                    MessageBox.Show("This ID is already Exist,please Enter another one", "ID Exist", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtUserId.Text = "";
                    txtUserId.Focus();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void CheckDupUserName()
        {
            try
            {
                DataRow[] row = dsMain.Tables["USER_MASTER"].Select("User_Name='" + txtUserName.Text.Trim() + "'");
                if (row.Length != 0)
                {
                    MessageBox.Show("This User Name is already Exist,please Enter another one", "ID Exist", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtUserName.Text = "";
                    txtUserName.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void LockUser()
        {
            try
            {
                
                txtUserId.Text = DBClass.UserId.ToString(); //loginUserId;
                txtUserName.Text = DBClass.UserName;
                cboUserGroup.Text =DBClass.UserType;

                if (DBClass.UserType.ToUpper() == "ADMIN")
                {
                    txtUserId.Enabled = false;
                    txtUserName.Enabled = false;
                    cboUserGroup.SelectedIndex = 0;
                    cboUserGroup.Enabled = false;
                    chkSalesPerson.Enabled = false;
                }
                else
                {
                    txtUserId.Enabled = false;
                    txtUserName.Enabled = false;
                    cboUserGroup.Enabled = false;
                    txtOldPassword.Text = "";
                    txtNewPassword.Text = "";
                    txtConfirmPassword.Text = "";
                    btn_NewUser.Visible = false;
                    chkSalesPerson.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void CheckPassword()
        {
            try
            {
                string StrCriteria = " User_Id='" + txtUserId.Text.Trim() + "' AND User_Name='" + txtUserName.Text.Trim() + "'";
                
                if (DBClass.UserType.ToUpper()!="ADMIN")
                {
                    DataRow[] row = dsMain.Tables["USER_MASTER"].Select(StrCriteria);
                    if (row.Length!=0)
                    {
                        string Password = Convert.ToString(row[0]["PASS"]);
                        string DcodePass="";
                        DcodePass = Security.ECODE_DECODE(Password.ToUpper(), "D");
                        if (txtOldPassword.Text.Trim().ToUpper() == DcodePass.ToUpper())
                        {
                            return;
                        }
                        else
                        {
                            MessageBox.Show("Incorrect Password.Please retype it", "Invalid", MessageBoxButtons.OK);
                            txtOldPassword.Text = "";
                            txtOldPassword.Focus();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void SelectedTab()
        {
            try
            {
                if (TabControl1.SelectedTab.Name.ToString() == "tabUserSettings")
                {
                    cboAllUser.Items.Clear();
                    DataRow[] row = dsMain.Tables["USER_MASTER"].Select("User_Type='USER'");
                    foreach (DataRow rw in row)
                    {
                        cboAllUser.Items.Add(rw["User_Id"].ToString() + "-" + rw["User_Name"].ToString());
                    }
                    ShowMenu();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void SaveNewUSer()
        {
            try
            {
                string encodePass = Security.ECODE_DECODE((txtNewPassword.Text).Trim(), "E");
                string StrCriteria = " User_Id='" + txtUserId.Text.Trim() + "' AND User_Name='" + txtUserName.Text.Trim() + "'";
                DataRow[] row = dsMain.Tables["USER_MASTER"].Select(StrCriteria);
                if (RequiredField())
                {
                    if (row.Length != 0)
                    {
                        int rowIndex = dsMain.Tables["USER_MASTER"].Rows.IndexOf(row[0]);
                        //dsMain.Tables["USER_MASTER"].Rows[rowIndex]["User_Id"] = txtUserId.Text;
                        //dsMain.Tables["USER_MASTER"].Rows[rowIndex]["User_Name"] = txtUserName.Text;
                        dsMain.Tables["USER_MASTER"].Rows[rowIndex]["User_Password"] = encodePass;
                        dsMain.Tables["USER_MASTER"].Rows[rowIndex]["IsSalesPerson"] = chkSalesPerson.Checked;
                        //dsMain.Tables["USER_MASTER"].Rows[rowIndex]["User_Type"] = cboUserGroup.Text;

                        SqlCommandBuilder commandBuilder = new SqlCommandBuilder(_MainAdapter);
                        _MainAdapter.UpdateCommand = commandBuilder.GetUpdateCommand();
                        _MainAdapter.InsertCommand = commandBuilder.GetInsertCommand();
                        _MainAdapter.Update(dsMain.Tables["USER_MASTER"]);

                        dsMain.Tables["USER_MASTER"].Clear();
                        _MainAdapter = DBClass.GetAdaptor("USER_MASTER");
                        _MainAdapter.Fill(dsMain.Tables["USER_MASTER"]);

                        MessageBox.Show("User Password updated", "Data Updated", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        
                        dsMain.Tables["USER_MASTER"].Rows.Add(txtUserId.Text, txtUserName.Text, encodePass, cboUserGroup.Text,"",(chkSalesPerson.Checked)?1:0);

                        SqlCommandBuilder commandBuilder = new SqlCommandBuilder(_MainAdapter);
                        _MainAdapter.UpdateCommand = commandBuilder.GetUpdateCommand();
                        _MainAdapter.InsertCommand = commandBuilder.GetInsertCommand();
                        _MainAdapter.Update(dsMain.Tables["USER_MASTER"]);

                        dsMain.Tables["USER_MASTER"].Clear();
                        _MainAdapter = DBClass.GetAdaptor("USER_MASTER");
                        _MainAdapter.Fill(dsMain.Tables["USER_MASTER"]);

                        MessageBox.Show("New User Sucessfully Inserted", "Data Inserted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                btn_NewUser.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void changeUserPassword()
        {
            if (txtUserNewPass.Text.Trim() == "")
            {
                MessageBox.Show("Please Enter New Password..", "Data Missing", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtUserNewPass.Focus();
                return;
            }
            else if (txtUserConfirmPass.Text.Trim() == "")
            {
                MessageBox.Show("Please Enter New Password..", "Data Missing", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtUserConfirmPass.Focus();
                return;
            }
            else if (txtUserConfirmPass.Text.Trim() != txtUserNewPass.Text.Trim())
            {
                MessageBox.Show("New Password and Confirm Password does not match..", "Data Missing", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtUserNewPass.Focus();
                txtUserNewPass.Text = "";
                txtUserConfirmPass.Text = "";
                return;
            }
            

            try
            {
                string encodePass =Security.ECODE_DECODE((txtUserNewPass.Text).Trim(), "E");
                if (txtUserNewPass.Text != "" && txtUserConfirmPass.Text != "" && (txtUserNewPass.Text.Trim() == txtUserConfirmPass.Text.Trim()))
                {
                    string userid = Get_ID((cboAllUser.Text).Trim());
                    string StrCriteria = " User_Id=" + userid + "";
                    DataRow[] row = dsMain.Tables["USER_MASTER"].Select(StrCriteria);
                    //if (RequiredField())
                    //{
                        if (row.Length != 0)
                        {
                            int rowIndex = dsMain.Tables["USER_MASTER"].Rows.IndexOf(row[0]);
                            //dsMain.Tables["USER_MASTER"].Rows[rowIndex]["User_Id"] = userid;
                            //dsMain.Tables["USER_MASTER"].Rows[rowIndex]["User_Name"] = txtUserName.Text;
                            dsMain.Tables["USER_MASTER"].Rows[rowIndex]["User_Password"] = encodePass;
                            //dsMain.Tables["USER_MASTER"].Rows[rowIndex]["User_Type"] = cboUserGroup.Text;

                            SqlCommandBuilder commandBuilder = new SqlCommandBuilder(_MainAdapter);
                            _MainAdapter.UpdateCommand = commandBuilder.GetUpdateCommand();
                            _MainAdapter.InsertCommand = commandBuilder.GetInsertCommand();
                            _MainAdapter.Update(dsMain.Tables["USER_MASTER"]);

                            dsMain.Tables["USER_MASTER"].Clear();
                            _MainAdapter = DBClass.GetAdaptor("USER_MASTER");
                            _MainAdapter.Fill(dsMain.Tables["USER_MASTER"]);

                            MessageBox.Show("User Password updated", "Data Updated", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    //}
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void ShowMenu()
        {
            DataGridViewCheckBoxColumn rightscheck=new DataGridViewCheckBoxColumn();
            rightscheck.Name = "U_RIGHT";
            rightscheck.HeaderText = "Right";

            menu_Grid.Columns.Clear();
            menu_Grid.Rows.Clear();

            menu_Grid.Columns.Add("Menu_Code","Code");
            menu_Grid.Columns.Add("Menu_Name","Menu Name");
            menu_Grid.Columns.Add(rightscheck);

            menu_Grid.Columns["Menu_Code"].ReadOnly = true;
            menu_Grid.Columns["Menu_Name"].ReadOnly = true;

            menu_Grid.Columns["Menu_Code"].ValueType = typeof(System.Int32);
            menu_Grid.Columns["Menu_Code"].Width = 100;
            menu_Grid.Columns["Menu_Name"].Width = 156;
            menu_Grid.Columns["U_RIGHT"].Width = 100;

            if (dsMain.Tables["Menu_Master"].Rows.Count>0)
            {
                for (int i = 0; i < dsMain.Tables["Menu_Master"].Rows.Count; i++)
                {
                    menu_Grid.Rows.Add();
                    menu_Grid.Rows[i].Cells["Menu_Code"].Value = dsMain.Tables["Menu_Master"].Rows[i]["Menu_Code"].ToString();
                    menu_Grid.Rows[i].Cells["Menu_Name"].Value = dsMain.Tables["Menu_Master"].Rows[i]["Menu_Name"].ToString();
                }
            }
        }
        private void SaveMenuDetail(DataGridViewCellEventArgs e)
        {
            try
            {
                Menu_Code = Convert.ToInt16(menu_Grid.Rows[e.RowIndex].Cells["Menu_Code"].Value.ToString());
                string User_ID = Get_ID((cboAllUser.Text).Trim());

                string StrCriteria = " User_Id=" + User_ID + " AND Menu_Code=" + Menu_Code;
                DataRow[] row = dsMain.Tables["USER_RIGHTS"].Select(StrCriteria);

                //if (chkRights)
                {
                    //bool rights=;
                    if (row.Length != 0)
                    {
                        int rowIndex = dsMain.Tables["USER_RIGHTS"].Rows.IndexOf(row[0]);
                        dsMain.Tables["USER_RIGHTS"].Rows[rowIndex]["User_Id"] = User_ID;
                        dsMain.Tables["USER_RIGHTS"].Rows[rowIndex]["Menu_Code"] = Menu_Code;
                        dsMain.Tables["USER_RIGHTS"].Rows[rowIndex]["U_RIGHT"] = (chkRights)?1:0;

                        SqlCommandBuilder commandBuilder = new SqlCommandBuilder(_MainDtAdapter);
                        _MainDtAdapter.UpdateCommand = commandBuilder.GetUpdateCommand();
                        _MainDtAdapter.InsertCommand = commandBuilder.GetInsertCommand();
                        _MainDtAdapter.Update(dsMain.Tables["USER_RIGHTS"]);

                        dsMain.Tables["USER_RIGHTS"].Clear();
                        _MainDtAdapter = DBClass.GetAdaptor("USER_RIGHTS");
                        _MainDtAdapter.Fill(dsMain.Tables["USER_RIGHTS"]);
                    }
                    else
                    {
                        dsMain.Tables["USER_RIGHTS"].Rows.Add(User_ID, Menu_Code, (chkRights) ? 1 : 0);
                        SqlCommandBuilder commandBuilder = new SqlCommandBuilder(_MainDtAdapter);
                        _MainDtAdapter.UpdateCommand = commandBuilder.GetUpdateCommand();
                        _MainDtAdapter.InsertCommand = commandBuilder.GetInsertCommand();
                        _MainDtAdapter.Update(dsMain.Tables["USER_RIGHTS"]);

                        dsMain.Tables["USER_RIGHTS"].Clear();
                        _MainDtAdapter = DBClass.GetAdaptor("USER_RIGHTS");
                        _MainDtAdapter.Fill(dsMain.Tables["USER_RIGHTS"]);
                    }
                }
                /*else
                {
                    dsMain.Tables["USER_RIGHTS"].Rows.Remove(row[0]);
                    SqlCommandBuilder commandBuilder = new SqlCommandBuilder(_MainDtAdapter);
                    _MainDtAdapter.UpdateCommand = commandBuilder.GetUpdateCommand();
                    _MainDtAdapter.Update(dsMain.Tables["USER_RIGHTS"]);

                    dsMain.Tables["USER_RIGHTS"].Clear();
                    _MainDtAdapter = DBClass.GetAdaptor("USER_RIGHTS");
                    _MainDtAdapter.Fill(dsMain.Tables["USER_RIGHTS"]);
                }
                */
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void SaveMenu()
        {
            bool chkRight;
            try
            {
                for (int i = 0; i < menu_Grid.Rows.Count; i++)
                {
                    Menu_Code = Convert.ToInt16(menu_Grid.Rows[i].Cells["Menu_Code"].Value.ToString());
                    chkRight = Convert.ToBoolean(menu_Grid.Rows[i].Cells[2].Value);
                    string User_ID = Get_ID((cboAllUser.Text).Trim());

                    string StrCriteria = " User_Id=" + User_ID + " AND Menu_Code=" + Menu_Code;
                    DataRow[] row = dsMain.Tables["USER_RIGHTS"].Select(StrCriteria);

                    //if (chkRights)
                    {
                        //bool rights=;
                        if (row.Length != 0)
                        {
                            int rowIndex = dsMain.Tables["USER_RIGHTS"].Rows.IndexOf(row[0]);
                            dsMain.Tables["USER_RIGHTS"].Rows[rowIndex]["User_Id"] = User_ID;
                            dsMain.Tables["USER_RIGHTS"].Rows[rowIndex]["Menu_Code"] = Menu_Code;
                            dsMain.Tables["USER_RIGHTS"].Rows[rowIndex]["U_RIGHT"] = (chkRight) ? 1 : 0;

                            SqlCommandBuilder commandBuilder = new SqlCommandBuilder(_MainDtAdapter);
                            _MainDtAdapter.UpdateCommand = commandBuilder.GetUpdateCommand();
                            _MainDtAdapter.InsertCommand = commandBuilder.GetInsertCommand();
                            _MainDtAdapter.Update(dsMain.Tables["USER_RIGHTS"]);

                            dsMain.Tables["USER_RIGHTS"].Clear();
                            _MainDtAdapter = DBClass.GetAdaptor("USER_RIGHTS");
                            _MainDtAdapter.Fill(dsMain.Tables["USER_RIGHTS"]);
                        }
                        else
                        {
                            dsMain.Tables["USER_RIGHTS"].Rows.Add(User_ID, Menu_Code, (chkRights) ? 1 : 0);
                            SqlCommandBuilder commandBuilder = new SqlCommandBuilder(_MainDtAdapter);
                            _MainDtAdapter.UpdateCommand = commandBuilder.GetUpdateCommand();
                            _MainDtAdapter.InsertCommand = commandBuilder.GetInsertCommand();
                            _MainDtAdapter.Update(dsMain.Tables["USER_RIGHTS"]);

                            dsMain.Tables["USER_RIGHTS"].Clear();
                            _MainDtAdapter = DBClass.GetAdaptor("USER_RIGHTS");
                            _MainDtAdapter.Fill(dsMain.Tables["USER_RIGHTS"]);
                        }
                    }

                    /*else
                    {
                        dsMain.Tables["USER_RIGHTS"].Rows.Remove(row[0]);
                        SqlCommandBuilder commandBuilder = new SqlCommandBuilder(_MainDtAdapter);
                        _MainDtAdapter.UpdateCommand = commandBuilder.GetUpdateCommand();
                        _MainDtAdapter.Update(dsMain.Tables["USER_RIGHTS"]);

                        dsMain.Tables["USER_RIGHTS"].Clear();
                        _MainDtAdapter = DBClass.GetAdaptor("USER_RIGHTS");
                        _MainDtAdapter.Fill(dsMain.Tables["USER_RIGHTS"]);
                    }
                    */
                }
                MessageBox.Show("Record Save successfully");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private string Get_ID(string name)
        {
            string code=name.Substring(0,name.IndexOf("-",0));
            return code;
        }
        private void DisplayUserRights()
        {
            try
            {
                if (cboAllUser.Text.Length>0)
                {
                    menu_Grid.Rows.Clear();
                    string User_ID = Get_ID((cboAllUser.Text).Trim());

                    if (dsMain.Tables["Menu_Master"].Rows.Count > 0)
                    {
                        for (int i = 0; i < dsMain.Tables["Menu_Master"].Rows.Count; i++)
                        {
                            menu_Grid.Rows.Add();
                            menu_Grid.Rows[i].Cells["Menu_Code"].Value = dsMain.Tables["Menu_Master"].Rows[i]["Menu_Code"].ToString();
                            menu_Grid.Rows[i].Cells["Menu_Name"].Value = dsMain.Tables["Menu_Master"].Rows[i]["Menu_Name"].ToString();

                            string Menu_Code = Convert.ToString(menu_Grid.Rows[i].Cells["Menu_Code"].Value);
                            string StrCriteria = " User_Id=" + User_ID + " AND Menu_Code=" + Menu_Code;
                            DataRow[] row = dsMain.Tables["USER_RIGHTS"].Select(StrCriteria);
                            if (row.Length!=0)
                            {
                                foreach (DataRow rw in row)
                                {
                                    menu_Grid.Rows[i].Cells["U_RIGHT"].Value = Convert.ToInt32(rw["U_RIGHT"]);
                                }
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region Form Events
        public frmUserSettings()
        {
            InitializeComponent();
        }
        private void frmUserSettings_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{TAB}");
            }
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }

        SqlDataAdapter _MainAdapter;
        SqlDataAdapter _MainDtAdapter;

        private void frmUserSettings_Load(object sender, EventArgs e)
        {
            ClearAll();
            DataTable dt = new DataTable();
            
            _MainAdapter = DBClass.GetAdaptor("USER_MASTER");
            _MainAdapter.Fill(dsMain);
            dsMain.Tables[0].TableName = "USER_MASTER";

            _MainDtAdapter = DBClass.GetAdaptor("USER_RIGHTS");
            _MainDtAdapter.Fill(dsMain);
            dsMain.Tables[1].TableName = "USER_RIGHTS";

            dt = DBClass.GetTableRecords("MENU_MASTER");
            dsMain.Tables.Add(dt);
            dsMain.Tables[2].TableName = "MENU_MASTER";

            cboUserGroup.Items.Clear();
            cboUserGroup.Items.Add("Admin");
            cboUserGroup.Items.Add("User");

            if (DBClass.UserType.ToUpper() == "ADMIN")
            {
                txtOldPassword.Enabled = false;
                LockUser();
                
            }
            else
            {
                TabControl1.Controls.Remove(TabControl1.TabPages["tabUserSettings"]);
                cboUserGroup.DropDownStyle = ComboBoxStyle.DropDown;
                LockUser();
            }
        }
        #endregion

        #region Control Events
        private void btn_Close_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void txtUserId_Validated(object sender, EventArgs e)
        {
            //CheckDupID();
        }
        private void btn_NewUser_Click(object sender, EventArgs e)
        {
            ClearAll();
            cboUserGroup.Items.Clear();
            cboUserGroup.Items.Add("Admin");
            cboUserGroup.Items.Add("User");
            txtUserId.Text = DBClass.GetIdByQuery("Select max(User_Id)+1 from User_Master").ToString();
            txtUserName.Focus();
            btn_NewUser.Enabled = false;
            btn_ChangePasword.Text = "Save";
            chkSalesPerson.Enabled = true;
        }
        private void txtOldPassword_Validated(object sender, EventArgs e)
        {
            if (txtOldPassword.Text.Length>0)
            {
                CheckPassword();
            }
        }
        private void txtUserName_Validated(object sender, EventArgs e)
        {
            if (txtUserName.Text.Length>0)
            {
                CheckDupUserName();
            }
        }
        private void btn_Display_Click(object sender, EventArgs e)
        {
            if (btn_Display.Text.Trim().ToUpper() == "DISPLAY")
            {
                DisplayUserRights();
                btn_Display.Text = "Save";
                cboAllUser.Enabled = false;
                txtUserNewPass.Enabled = false;
                txtUserConfirmPass.Enabled = false;
                btn_chaneUserPass.Enabled = false;
            }
            else if (btn_Display.Text.Trim().ToUpper() == "SAVE")
            {
                SaveMenu();
                btn_Display.Text = "Display";
                cboAllUser.Enabled = true;
                txtUserNewPass.Enabled = true;
                txtUserConfirmPass.Enabled = true;
                btn_chaneUserPass.Enabled = true;
            }
        }

        private void btn_chaneUserPass_Click(object sender, EventArgs e)
        {
            changeUserPassword();
        }
        private void btn_ChangePasword_Click(object sender, EventArgs e)
        {
            SaveNewUSer();
        }
        private void TabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectedTab();
        }
        #endregion

        #region Grid Events
        private void menu_Grid_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (!(e.ColumnIndex == menu_Grid.Columns["U_RIGHT"].Index))
            {
                return;
            }
            else
            {
                chkRights = Convert.ToBoolean(menu_Grid.Rows[e.RowIndex].Cells[2].Value);
                SaveMenuDetail(e);
            }
        }
        #endregion
    }
}
