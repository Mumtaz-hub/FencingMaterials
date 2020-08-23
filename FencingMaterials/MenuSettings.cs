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
    public partial class MenuSettings : Form
    {
        public MenuSettings()
        {
            InitializeComponent();
        }

        DataSet dsMain = new DataSet();
        SqlDataAdapter _MainAdapter;

        private void MenuSettings_Load(object sender, EventArgs e)
        {
            displayRecord();
        }

        private void displayRecord()
        {
            _MainAdapter = DBClass.GetAdaptor("Menu_Master");
            dsMain = new DataSet();

            _MainAdapter.Fill(dsMain);
            dsMain.Tables[0].TableName = "Menu_Master";
            dgvMenu.DataSource = null;
            dgvMenu.DataSource = dsMain.Tables["Menu_Master"];

            dgvMenu.Columns["Menu_Code"].ReadOnly = true;
            dgvMenu.Columns["Menu_Code"].HeaderText = "No.";
            dgvMenu.Columns["Menu_Code"].DefaultCellStyle.BackColor = System.Drawing.Color.Gray;
            dgvMenu.Columns["Menu_Code"].Width = 50;

            dgvMenu.Columns["Menu_Name"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvMenu.Columns["Menu_Name"].HeaderText = "Menu Name";

            dgvMenu.Columns["Menu_Group"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvMenu.Columns["Menu_Group"].HeaderText = "Menu Group";

        }

        private void MenuSettings_KeyDown(object sender, KeyEventArgs e)
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

        private void dgvMenu_RowValidated(object sender, DataGridViewCellEventArgs e)
        {
            if (!dsMain.HasChanges()) return;
            SqlCommandBuilder commandBuilder = new SqlCommandBuilder(_MainAdapter);
            _MainAdapter.UpdateCommand = commandBuilder.GetUpdateCommand();
            _MainAdapter.InsertCommand = commandBuilder.GetInsertCommand();
            _MainAdapter.Update(dsMain.Tables["Menu_Master"]);

            dsMain.Tables["Menu_Master"].Clear();
            _MainAdapter = DBClass.GetAdaptor("Menu_Master");
            _MainAdapter.Fill(dsMain.Tables["Menu_Master"]);
        }

        private void dgvMenu_UserDeletedRow(object sender, DataGridViewRowEventArgs e)
        {
            if (!dsMain.HasChanges()) return;
            SqlCommandBuilder commandBuilder = new SqlCommandBuilder(_MainAdapter);
            _MainAdapter.DeleteCommand = commandBuilder.GetDeleteCommand();
            _MainAdapter.Update(dsMain.Tables["Menu_Master"]);
        }
    }
}
