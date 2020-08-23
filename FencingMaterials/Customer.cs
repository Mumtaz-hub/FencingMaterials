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
    public partial class Customer : Form
    {
        public Customer()
        {
            InitializeComponent();
        }

        SqlDataAdapter _MainAdapter;
        DataTable _dt = new DataTable();
        DataSet dsMain = new DataSet();

        private void Customer_Load(object sender, EventArgs e)
        {
            FillGrid();
        }

        private void Customer_KeyDown(object sender, KeyEventArgs e)
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

        private void btnShow_Click(object sender, EventArgs e)
        {
            RefreshGrid();
        }

        public void RefreshGrid()
        {
            string strCriteria = " 1=1 ";
            //string strCriteria = "";
            if (txtSearchMobile.Text != "")
            {
                strCriteria += " and Phone LIKE '%" + txtSearchMobile.Text + "%'";
            }

            if (txtSearchName.Text != "")
            {
                strCriteria += " and Name LIKE '%" + txtSearchName.Text + "%'";
            }

            if (txtSearchAddress.Text != "")
            {
                strCriteria += " and Address LIKE '%" + txtSearchAddress.Text + "%'";
            }

            dsMain.Tables["Customer"].DefaultView.RowFilter = "";
            dsMain.Tables["Customer"].DefaultView.RowFilter = strCriteria;

            dgvView.DataSource = dsMain.Tables["Customer"].DefaultView.ToTable();
            
        }
        public void FillGrid()
        {

            string Sql = @"select distinct Customer_Phone as 'Phone',Customer_Name as 'Name',Customer_Address as 'Address' 
                            from customer_master Order By Customer_Name";

            if (dsMain.Tables.Contains("Customer"))
                dsMain.Tables["Customer"].Clear();

            _MainAdapter = DBClass.GetAdapterByQuery(Sql);
            _MainAdapter.Fill(_dt);
            dsMain.Tables.Add(_dt);
            dsMain.Tables[0].TableName = "Customer";

            if (dgvView.DataSource == null)
                dgvView.DataSource = dsMain.Tables["Customer"];

            dgvView.Columns["Phone"].Width = 150;
            dgvView.Columns["Phone"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            dgvView.Columns["Name"].Width = 230;
            dgvView.Columns["Name"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            dgvView.Columns["Address"].Width = 130;
            dgvView.Columns["Address"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgvView.Columns["Address"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            
            dgvView.ReadOnly = true;
        }
    }
}
