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
    public partial class ItemCategory : Form
    {
        public ItemCategory()
        {
            InitializeComponent();
        }

        DataSet dsMain = new DataSet();
        SqlDataAdapter _MainAdapter;
        int GrpCode=0;
        #region Form Events
        private void ItemCategory_Load(object sender, EventArgs e)
        {
            Set_Grid();
            displayRecord();
        }
        private void ItemCategory_KeyDown(object sender, KeyEventArgs e)
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
        #endregion

        #region Display Methods
        private void displayRecord()
        {
            _MainAdapter = DBClass.GetAdaptor("Category_Master");
            dsMain = new DataSet();

            _MainAdapter.Fill(dsMain);
            dsMain.Tables[0].TableName = "Category_Master";

            dgvCategory.DataSource = null;
            dgvCategory.DataSource = dsMain.Tables["Category_Master"];

            dgvCategory.Columns["Category_Id"].ReadOnly = true;
            dgvCategory.Columns["Category_Id"].HeaderText = "No.";
            //dgvCategory.Columns["Category_Id"].DefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(1,78,130);
            dgvCategory.Columns["Category_Id"].Width = 50;

            dgvCategory.Columns["Category_Name"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvCategory.Columns["Category_Name"].HeaderText = "Name";

            if (rbResidential.Checked)
            {
                GrpCode = DBClass.GetIdByQuery("Select Grp_Code from Group_Master where Grp_Name like '%residential%'");
            }
            else if (rbAgriculture.Checked)
            {
                GrpCode = DBClass.GetIdByQuery("Select Grp_Code from Group_Master where Grp_Name like '%agriculture%'");
            }

            dsMain.Tables["Category_Master"].DefaultView.RowFilter = "";
            dsMain.Tables["Category_Master"].DefaultView.RowFilter = "Grp_Code=" + GrpCode;
            dgvCategory.DataSource = dsMain.Tables["Category_Master"].DefaultView;

        }
        private void Set_Grid()
        {
            dgvCategory.AutoGenerateColumns = false;

            DataGridViewTextBoxColumn Category_Id = new DataGridViewTextBoxColumn();
            Category_Id.Name = "Category_Id";
            Category_Id.HeaderText = "No.";
            Category_Id.DataPropertyName = "Category_Id";
            Category_Id.ReadOnly = true;
            dgvCategory.Columns.Add(Category_Id);

            DataGridViewTextBoxColumn Category_Name = new DataGridViewTextBoxColumn();
            Category_Name.Name = "Category_Name";
            Category_Name.HeaderText = "Name";
            Category_Name.DataPropertyName = "Category_Name";
            dgvCategory.Columns.Add(Category_Name);

            DataGridViewTextBoxColumn Grp_Code = new DataGridViewTextBoxColumn();
            Grp_Code.Name = "Grp_Code";
            Grp_Code.HeaderText = "Grp_Code";
            Grp_Code.DataPropertyName = "Grp_Code";
            dgvCategory.Columns.Add(Grp_Code);
        }
        #endregion

        #region Grid Events
        private void dgvCategory_RowValidated(object sender, DataGridViewCellEventArgs e)
        {
           // if (!dsMain.HasChanges()) return;

           // if (dgvCategory.Rows[e.RowIndex].Cells["Category_Id"].Value == null) return;
            //DataRow[] row= dsMain.Tables["Category_Master"].Select("Category_Id=" + int.Parse(dgvCategory.Rows[e.RowIndex].Cells["Category_Id"].Value.ToString()));
            //GrpCode = 2;
            try
            {
                if (dgvCategory.Rows[e.RowIndex].Cells["Category_Name"].Value != null && GrpCode!=0)
                {
                    if (dgvCategory.Rows[e.RowIndex].Cells["Category_Id"].Value.ToString() == "" && dgvCategory.Rows[e.RowIndex].Cells["Category_Name"].Value.ToString() != "")
                    {
                        _MainAdapter.InsertCommand = new SqlCommand(@"insert into Category_Master(Category_Name,Grp_Code,Entry_UserId,Entry_Date) 
                                                                output inserted.Category_Id
                                                                Values(@Category_Name,@Grp_Code,@Entry_UserId,@Entry_Date)", DBClass.connection);

                        DBClass.connection.Open();

                        _MainAdapter.InsertCommand.Parameters.AddWithValue("@Category_Name", dgvCategory.Rows[e.RowIndex].Cells["Category_Name"].Value.ToString());
                        _MainAdapter.InsertCommand.Parameters.AddWithValue("@Grp_Code", GrpCode);
                        _MainAdapter.InsertCommand.Parameters.AddWithValue("@Entry_UserId", DBClass.UserId);
                        _MainAdapter.InsertCommand.Parameters.AddWithValue("@Entry_Date", System.DateTime.Now.ToString());

                        int id = Convert.ToInt16(_MainAdapter.InsertCommand.ExecuteScalar());
                        dgvCategory.Rows[e.RowIndex].Cells["Category_Id"].Value = id.ToString();
                        DBClass.connection.Close();

                    }
                    else
                    {
                        DBClass.connection.Open();
                        _MainAdapter.UpdateCommand = new SqlCommand(@"update Category_Master set Category_Name=@Category_Name,Grp_Code=@Grp_Code,Entry_UserId=@Entry_UserId,Entry_Date=@Entry_Date
                                                                where Category_Id= @Category_Id ", DBClass.connection);

                        _MainAdapter.UpdateCommand.Parameters.AddWithValue("@Category_Id", int.Parse(dgvCategory.Rows[e.RowIndex].Cells["Category_Id"].Value.ToString()));
                        _MainAdapter.UpdateCommand.Parameters.AddWithValue("@Category_Name", dgvCategory.Rows[e.RowIndex].Cells["Category_Name"].Value.ToString());
                        _MainAdapter.UpdateCommand.Parameters.AddWithValue("@Grp_Code", GrpCode);
                        _MainAdapter.UpdateCommand.Parameters.AddWithValue("@Entry_UserId", DBClass.UserId);
                        _MainAdapter.UpdateCommand.Parameters.AddWithValue("@Entry_Date", System.DateTime.Now.ToString());

                        _MainAdapter.UpdateCommand.ExecuteNonQuery();
                        DBClass.connection.Close();
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw;
            }

            //SqlCommandBuilder commandBuilder = new SqlCommandBuilder(_MainAdapter);
            //_MainAdapter.UpdateCommand = commandBuilder.GetUpdateCommand();
            //_MainAdapter.InsertCommand = commandBuilder.GetInsertCommand();
            //_MainAdapter.Update(dsMain.Tables["Category_Master"]);

            //dsMain.Tables["Category_Master"].Clear();
            //_MainAdapter = DBClass.GetAdaptor("Category_Master");
            //_MainAdapter.Fill(dsMain.Tables["Category_Master"]);
        }
        private void dgvCategory_UserDeletedRow(object sender, DataGridViewRowEventArgs e)
        {
            //if (!dsMain.HasChanges()) return;
            //try
            //{
            //    SqlCommandBuilder commandBuilder = new SqlCommandBuilder(_MainAdapter);
            //    _MainAdapter.DeleteCommand = commandBuilder.GetDeleteCommand();
            //    _MainAdapter.Update(dsMain.Tables["Category_Master"]);
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //    throw;
            //}
        }
        private void dgvCategory_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            if (MessageBox.Show("Are you Sure to delete this Category ? ", "Delete Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.No)
            {
                e.Cancel = true;
            }
            else
            {
                try
                {
                    DBClass.connection.Open();
                    _MainAdapter.DeleteCommand = new SqlCommand(@"Delete From Category_Master where Category_Id= @Category_Id ", DBClass.connection);
                    _MainAdapter.DeleteCommand.Parameters.AddWithValue("@Category_Id", int.Parse(dgvCategory.Rows[e.Row.Index].Cells["Category_Id"].Value.ToString()));
                    _MainAdapter.DeleteCommand.ExecuteNonQuery();
                    DBClass.connection.Close();

                    //SqlCommandBuilder commandBuilder = new SqlCommandBuilder(_MainAdapter);
                    //_MainAdapter.DeleteCommand = commandBuilder.GetDeleteCommand();
                    //_MainAdapter.Update(dsMain.Tables["Item_Master"]);

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    throw;
                }
            }
        }
        #endregion

        #region Filter Grid
        private void FilterGrid()
        {
            if (rbResidential.Checked)
            {
                GrpCode = DBClass.GetIdByQuery("Select Grp_Code from Group_Master where Grp_Name like '%residential%'");
            }
            else if (rbAgriculture.Checked)
            {
                GrpCode = DBClass.GetIdByQuery("Select Grp_Code from Group_Master where Grp_Name like '%agriculture%'");
            }

            dsMain.Tables["Category_Master"].DefaultView.RowFilter = "";
            dsMain.Tables["Category_Master"].DefaultView.RowFilter = "Grp_Code=" + GrpCode;

            dgvCategory.DataSource = dsMain.Tables["Category_Master"].DefaultView;
        }
        private void rbResidential_CheckedChanged(object sender, EventArgs e)
        {
            FilterGrid();
        }
        private void rbAgriculture_CheckedChanged(object sender, EventArgs e)
        {
            FilterGrid();
        }
        #endregion

        private void dgvCategory_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
        {
            if ((dgvCategory.Rows.Count != 0))
            {
                e.Row.Cells["Grp_Code"].Value = GrpCode;
            }
        }
    }
}
