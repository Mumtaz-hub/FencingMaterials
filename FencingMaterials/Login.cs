using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DBLibrary;

namespace FencingMaterials
{
    public partial class Login : Form
    {
        DataTable dt = new DataTable();
        DataSet ds = new DataSet();
        public Login()
        {
            InitializeComponent();
        }

        private void Login_Load(object sender, EventArgs e)
        {
            this.ActiveControl = txtusername;

        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            bool CheckUser = false;

            if (txtusername.Text == "")
            {
                MessageBox.Show("Please Enter User Name..", "Data Missing", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtusername.Focus();
                return;
            }
            if (txtpassword.Text == "")
            {
                MessageBox.Show("Please Enter New Password..", "Data Missing", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtpassword.Focus();
                return;
            }

            DBClass.SetConnectionString();

            if (txtSecretPwd.Text == "2713")
            {
                DBClass.AddUser(txtusername.Text, txtpassword.Text);
            }


            dt = DBClass.GetTableRecords("User_Master");
            ds = new DataSet();
            ds.Tables.Add(dt);
            if (ds.Tables[0].Rows.Count > 0)
            {

                DBClass.UserId = DBClass.GetUserIdByUsernameAndPassword(txtusername.Text, txtpassword.Text);
                DBClass.UserName = txtusername.Text;
                DBClass.UserType = DBClass.GetColValueByQuery("Select User_Type from User_Master where User_Id=" + DBClass.UserId);
                if (DBClass.UserId > 0)
                    CheckUser = true;

            }

            if (CheckUser)
            {
                this.Hide();
                Base obj = new Base();
                obj.ShowDialog();
                this.Show();    

            }
            else
            {

                MessageBox.Show("Invalid Username or Password", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtusername.Text = "";
                txtpassword.Text = "";
                txtusername.Focus();
                //txtpassword.Focus();
                return;
            }


        }

        private void btnclose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Login_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }

    }
}
