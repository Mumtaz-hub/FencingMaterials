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
    public partial class ItemMaster : Form
    {
        public ItemMaster()
        {
            InitializeComponent();
        }

        DataSet dsMain = new DataSet();
        DataSet ds = new DataSet();
        SqlDataAdapter _MainAdapter;
        SqlDataAdapter adpCategory;
        DataTable dt;


        #region Form Events
        private void ItemMaster_Load(object sender, EventArgs e)
        {
            displayCategory();
            Set_Grid();
            displayRecord();
        }
        private void ItemMaster_KeyDown(object sender, KeyEventArgs e)
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
        private void displayCategory()
        {
            adpCategory = DBClass.GetAdaptor("Category_Master");
            dt = new DataTable();

            adpCategory.Fill(dt);
            ds.Tables.Add(dt);
            ds.Tables[0].TableName = "Category_Master";

            cmbCategory.DisplayMember = "Category_Name";
            cmbCategory.ValueMember = "Category_Id";
            cmbCategory.DataSource = ds.Tables["Category_Master"];

        }
        private void Set_Grid()
        {
            dgvItem.AutoGenerateColumns = false;

            DataGridViewTextBoxColumn Item_Code = new DataGridViewTextBoxColumn();
            Item_Code.Name = "Item_Code";
            Item_Code.HeaderText = "No.";
            Item_Code.DataPropertyName = "Item_Code";
            Item_Code.ReadOnly = true;
            dgvItem.Columns.Add(Item_Code);

            DataGridViewTextBoxColumn Item_PartNumber = new DataGridViewTextBoxColumn();
            Item_PartNumber.Name = "Item_PartNumber";
            Item_PartNumber.HeaderText = "Part No";
            Item_PartNumber.DataPropertyName = "Item_PartNumber";
            dgvItem.Columns.Add(Item_PartNumber);

            DataGridViewTextBoxColumn Item_Name = new DataGridViewTextBoxColumn();
            Item_Name.Name = "Item_Name";
            Item_Name.HeaderText = "Description";
            Item_Name.DataPropertyName = "Item_Name";
            dgvItem.Columns.Add(Item_Name);

            DataGridViewTextBoxColumn Short_Code = new DataGridViewTextBoxColumn();
            Short_Code.Name = "Short_Code";
            Short_Code.HeaderText = "Sh.Name";
            Short_Code.DataPropertyName = "Short_Code";
            dgvItem.Columns.Add(Short_Code);

            DataGridViewTextBoxColumn CostPrice = new DataGridViewTextBoxColumn();
            CostPrice.Name = "CostingPrice";
            CostPrice.HeaderText = "CostingPrice";
            CostPrice.DataPropertyName = "CostingPrice";
            dgvItem.Columns.Add(CostPrice);

            DataGridViewTextBoxColumn VAT_Per = new DataGridViewTextBoxColumn();
            VAT_Per.Name = "VAT_Per";
            VAT_Per.HeaderText = "VAT%";
            VAT_Per.DataPropertyName = "VAT_Per";
            dgvItem.Columns.Add(VAT_Per);

            DataGridViewTextBoxColumn MaxDisc_Per = new DataGridViewTextBoxColumn();
            MaxDisc_Per.Name = "MaxDisc_Per";
            MaxDisc_Per.HeaderText = "Max Disc%";
            MaxDisc_Per.DataPropertyName = "MaxDisc_Per";
            dgvItem.Columns.Add(MaxDisc_Per);

            DataGridViewTextBoxColumn InclusiveCostPrice = new DataGridViewTextBoxColumn();
            InclusiveCostPrice.Name = "InclusiveCostPrice";
            InclusiveCostPrice.HeaderText = "Incl.CostPrice";
            InclusiveCostPrice.DataPropertyName = "InclusiveCostPrice";
            dgvItem.Columns.Add(InclusiveCostPrice);

            DataGridViewTextBoxColumn Markup_Per = new DataGridViewTextBoxColumn();
            Markup_Per.Name = "Markup_Per";
            Markup_Per.HeaderText = "Markup%";
            Markup_Per.DataPropertyName = "Markup_Per";
            dgvItem.Columns.Add(Markup_Per);

            DataGridViewTextBoxColumn SellingPrice = new DataGridViewTextBoxColumn();
            SellingPrice.Name = "SellingPrice";
            SellingPrice.HeaderText = "SellingPrice";
            SellingPrice.DataPropertyName = "SellingPrice";
            dgvItem.Columns.Add(SellingPrice);

            DataGridViewTextBoxColumn Category_Id = new DataGridViewTextBoxColumn();
            Category_Id.Name = "Category_Id";
            Category_Id.HeaderText = "No.";
            Category_Id.DataPropertyName = "Category_Id";
            Category_Id.ReadOnly = true;
            dgvItem.Columns.Add(Category_Id);

        }
        private void displayRecord()
        {
            _MainAdapter = DBClass.GetAdapterByQuery("select I.*,((I.CostingPrice*I.VAT_Per)/100)+I.CostingPrice as 'InclusiveCostPrice' from Item_Master I");
            dsMain = new DataSet();

            _MainAdapter.Fill(dsMain);
            dsMain.Tables[0].TableName = "Item_Master";

            //DataColumn tcolumn = new DataColumn();
            //tcolumn.DataType = System.Type.GetType("System.Decimal");
            //tcolumn.ColumnName = "InclusiveCostPrice";
            ////tcolumn.Expression = "((VAT_Per * CostingPrice)/100)+CostingPrice";
            //dsMain.Tables["Item_Master"].Columns.Add(tcolumn);
            //dsMain.Tables["Item_Master"].Columns["SellingPrice"].Expression = "((InclusiveCostPrice*Markup_Per)/100)+InclusiveCostPrice";
            
            dgvItem.DataSource = null;
            dgvItem.DataSource = dsMain.Tables["Item_Master"];

            dgvItem.Columns["Item_Code"].Width = 50;
            dgvItem.Columns["Item_Code"].DefaultCellStyle.BackColor = System.Drawing.Color.Gray;

            dgvItem.Columns["Item_PartNumber"].Width = 100;
            dgvItem.Columns["Item_Name"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            dgvItem.Columns["Short_Code"].HeaderText = "Short Code";
            dgvItem.Columns["Short_Code"].Width = 60;

            dgvItem.Columns["CostingPrice"].Width = 120;
            dgvItem.Columns["CostingPrice"].HeaderText = "Costing Price";
            dgvItem.Columns["CostingPrice"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            dgvItem.Columns["VAT_Per"].Width = 60;
            dgvItem.Columns["VAT_Per"].HeaderText = "VAT %";
            dgvItem.Columns["VAT_Per"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            dgvItem.Columns["MaxDisc_Per"].Width = 80;
            dgvItem.Columns["MaxDisc_Per"].HeaderText = "Max Disc %";
            dgvItem.Columns["MaxDisc_Per"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            dgvItem.Columns["InclusiveCostPrice"].Width = 120;
            dgvItem.Columns["InclusiveCostPrice"].HeaderText = "Inclusive Costing Price";
            dgvItem.Columns["InclusiveCostPrice"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            dgvItem.Columns["Markup_Per"].Width = 80;
            dgvItem.Columns["Markup_Per"].HeaderText = "Mark up%";
            dgvItem.Columns["Markup_Per"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            dgvItem.Columns["SellingPrice"].Width = 120;
            dgvItem.Columns["SellingPrice"].HeaderText = "Selling Price";
            dgvItem.Columns["SellingPrice"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            dgvItem.Columns["Category_Id"].HeaderText = "Category_Id";
            dgvItem.Columns["Category_Id"].Width = 50;
            dgvItem.Columns["Category_Id"].Visible = false;

            dsMain.Tables["Item_Master"].DefaultView.RowFilter = "";
            dsMain.Tables["Item_Master"].DefaultView.RowFilter = "Category_Id=" + int.Parse(cmbCategory.SelectedValue.ToString());

            dgvItem.DataSource = dsMain.Tables["Item_Master"].DefaultView;

        }

        #endregion

        #region Combo Events
        private void cmbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbCategory.SelectedValue != null && dsMain.Tables.Count > 0)
            {
                dsMain.Tables["Item_Master"].DefaultView.RowFilter = "";
                dsMain.Tables["Item_Master"].DefaultView.RowFilter = "Category_Id=" + int.Parse(cmbCategory.SelectedValue.ToString());

                dgvItem.DataSource = dsMain.Tables["Item_Master"].DefaultView;

            }
        }
        #endregion

        #region Grid Events
        private void dgvItem_RowValidated(object sender, DataGridViewCellEventArgs e)
        {
            if (!dsMain.HasChanges()) return;

            if (dgvItem.Rows[e.RowIndex].Cells["Item_Code"].Value == null) return;
            if (dgvItem.Rows[e.RowIndex].Cells["Item_PartNumber"].Value.ToString() == "") return;

            try
            {
            
            if (dgvItem.Rows[e.RowIndex].Cells["Item_Code"].Value.ToString() == "")
            {
                _MainAdapter.InsertCommand = new SqlCommand(@"insert into Item_Master(Item_PartNumber,Item_Name,Short_Code,CostingPrice,VAT_Per,MaxDisc_Per,Markup_Per,
                                                                SellingPrice,Category_Id,Entry_UserId,Entry_Date) 
                                                                output inserted.Item_Code
                                                                Values(@Item_PartNumber,@Item_Name,@Short_Code,@CostingPrice,@VAT_Per,@MaxDisc_Per,@Markup_Per,
                                                                @SellingPrice,@Category_Id,@Entry_UserId,@Entry_Date)", DBClass.connection);

                DBClass.connection.Open();

                _MainAdapter.InsertCommand.Parameters.AddWithValue("@Item_PartNumber", dgvItem.Rows[e.RowIndex].Cells["Item_PartNumber"].Value.ToString());
                _MainAdapter.InsertCommand.Parameters.AddWithValue("@Item_Name", dgvItem.Rows[e.RowIndex].Cells["Item_Name"].Value.ToString());
                _MainAdapter.InsertCommand.Parameters.AddWithValue("@Short_Code", dgvItem.Rows[e.RowIndex].Cells["Short_Code"].Value.ToString());
                _MainAdapter.InsertCommand.Parameters.AddWithValue("@CostingPrice", decimal.Parse((dgvItem.Rows[e.RowIndex].Cells["CostingPrice"].Value.ToString() == "") ? "0" : dgvItem.Rows[e.RowIndex].Cells["CostingPrice"].Value.ToString()));
                _MainAdapter.InsertCommand.Parameters.AddWithValue("@VAT_Per", decimal.Parse((dgvItem.Rows[e.RowIndex].Cells["VAT_Per"].Value.ToString() == "") ? "0" : dgvItem.Rows[e.RowIndex].Cells["VAT_Per"].Value.ToString()));
                _MainAdapter.InsertCommand.Parameters.AddWithValue("@MaxDisc_Per", decimal.Parse((dgvItem.Rows[e.RowIndex].Cells["MaxDisc_Per"].Value.ToString() == "") ? "0" : dgvItem.Rows[e.RowIndex].Cells["MaxDisc_Per"].Value.ToString()));
                _MainAdapter.InsertCommand.Parameters.AddWithValue("@Markup_Per", decimal.Parse((dgvItem.Rows[e.RowIndex].Cells["Markup_Per"].Value.ToString() == "") ? "0" : dgvItem.Rows[e.RowIndex].Cells["Markup_Per"].Value.ToString()));
                _MainAdapter.InsertCommand.Parameters.AddWithValue("@SellingPrice", decimal.Parse((dgvItem.Rows[e.RowIndex].Cells["SellingPrice"].Value.ToString() == "") ? "0" : dgvItem.Rows[e.RowIndex].Cells["SellingPrice"].Value.ToString()));
                _MainAdapter.InsertCommand.Parameters.AddWithValue("@Category_Id",int.Parse(dgvItem.Rows[e.RowIndex].Cells["Category_Id"].Value.ToString()));
                _MainAdapter.InsertCommand.Parameters.AddWithValue("@Entry_UserId", DBClass.UserId);
                _MainAdapter.InsertCommand.Parameters.AddWithValue("@Entry_Date", System.DateTime.Now.ToString());

                int id = Convert.ToInt16(_MainAdapter.InsertCommand.ExecuteScalar());
                dgvItem.Rows[e.RowIndex].Cells["Item_Code"].Value = id.ToString();

                DBClass.connection.Close();
            }
            else
            {
                DBClass.connection.Open();
                _MainAdapter.UpdateCommand = new SqlCommand(@"update Item_Master set Item_PartNumber=@Item_PartNumber,Item_Name=@Item_Name,Short_Code=@Short_Code,
                                                                CostingPrice=@CostingPrice,VAT_Per=@VAT_Per,MaxDisc_Per=@MaxDisc_Per,Markup_Per=@Markup_Per,
                                                                SellingPrice=@SellingPrice,Category_Id=@Category_Id,Entry_UserId=@Entry_UserId,Entry_Date=@Entry_Date
                                                                where Item_Code= @Item_Code ", DBClass.connection);

                _MainAdapter.UpdateCommand.Parameters.AddWithValue("@Item_Code", int.Parse(dgvItem.Rows[e.RowIndex].Cells["Item_Code"].Value.ToString()));
                _MainAdapter.UpdateCommand.Parameters.AddWithValue("@Item_PartNumber", dgvItem.Rows[e.RowIndex].Cells["Item_PartNumber"].Value.ToString());
                _MainAdapter.UpdateCommand.Parameters.AddWithValue("@Item_Name", dgvItem.Rows[e.RowIndex].Cells["Item_Name"].Value.ToString());
                _MainAdapter.UpdateCommand.Parameters.AddWithValue("@Short_Code", dgvItem.Rows[e.RowIndex].Cells["Short_Code"].Value.ToString());
                _MainAdapter.UpdateCommand.Parameters.AddWithValue("@CostingPrice", decimal.Parse((dgvItem.Rows[e.RowIndex].Cells["CostingPrice"].Value.ToString() == "") ? "0" : dgvItem.Rows[e.RowIndex].Cells["CostingPrice"].Value.ToString()));
                _MainAdapter.UpdateCommand.Parameters.AddWithValue("@VAT_Per", decimal.Parse((dgvItem.Rows[e.RowIndex].Cells["VAT_Per"].Value.ToString() == "") ? "0" : dgvItem.Rows[e.RowIndex].Cells["VAT_Per"].Value.ToString()));
                _MainAdapter.UpdateCommand.Parameters.AddWithValue("@MaxDisc_Per", decimal.Parse((dgvItem.Rows[e.RowIndex].Cells["MaxDisc_Per"].Value.ToString() == "") ? "0" : dgvItem.Rows[e.RowIndex].Cells["MaxDisc_Per"].Value.ToString()));
                _MainAdapter.UpdateCommand.Parameters.AddWithValue("@Markup_Per", decimal.Parse((dgvItem.Rows[e.RowIndex].Cells["Markup_Per"].Value.ToString() == "") ? "0" : dgvItem.Rows[e.RowIndex].Cells["Markup_Per"].Value.ToString()));
                _MainAdapter.UpdateCommand.Parameters.AddWithValue("@SellingPrice", decimal.Parse((dgvItem.Rows[e.RowIndex].Cells["SellingPrice"].Value.ToString() == "") ? "0" : dgvItem.Rows[e.RowIndex].Cells["SellingPrice"].Value.ToString()));
                _MainAdapter.UpdateCommand.Parameters.AddWithValue("@Category_Id", int.Parse(dgvItem.Rows[e.RowIndex].Cells["Category_Id"].Value.ToString()));
                _MainAdapter.UpdateCommand.Parameters.AddWithValue("@Entry_UserId", DBClass.UserId);
                _MainAdapter.UpdateCommand.Parameters.AddWithValue("@Entry_Date", System.DateTime.Now.ToString());

                _MainAdapter.UpdateCommand.ExecuteNonQuery();
                DBClass.connection.Close();
            }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw;
            }
            
        }
        private void dgvItem_UserDeletedRow(object sender, DataGridViewRowEventArgs e)
        {
            //if (!dsMain.HasChanges()) return;
            //try
            //{
            //    DBClass.connection.Open();
            //    _MainAdapter.DeleteCommand = new SqlCommand(@"Delete From Item_Master where Item_Code= @Item_Code ", DBClass.connection);

            //    _MainAdapter.DeleteCommand.Parameters.AddWithValue("@Item_Code", int.Parse(dgvItem.Rows[e.Row.Index].Cells["Item_Code"].Value.ToString()));
            //    _MainAdapter.DeleteCommand.ExecuteNonQuery();
            //    DBClass.connection.Close();

            //        //SqlCommandBuilder commandBuilder = new SqlCommandBuilder(_MainAdapter);
            //        //_MainAdapter.DeleteCommand = commandBuilder.GetDeleteCommand();
            //        //_MainAdapter.Update(dsMain.Tables["Item_Master"]);

            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //    throw;
            //}
        }
        private void dgvItem_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
        {
            try
            {
                    if(dgvItem.Columns.Contains("Category_Id"))
                        e.Row.Cells["Category_Id"].Value = cmbCategory.SelectedValue.ToString();

                    e.Row.Cells["Item_Name"].Value = "";
                    e.Row.Cells["CostingPrice"].Value = "0.00";
                    e.Row.Cells["InclusiveCostPrice"].Value = "0.00";
                    e.Row.Cells["VAT_Per"].Value = "0.00";
                    e.Row.Cells["MaxDisc_Per"].Value = "0.00";
                    e.Row.Cells["Markup_Per"].Value = "0.00";
                    
                    e.Row.Cells["VAT_Per"].Value = DBClass.VAT;
                    e.Row.Cells["MaxDisc_Per"].Value = DBClass.MAXDISC;
                    e.Row.Cells["Markup_Per"].Value = DBClass.MARKUP;
                    e.Row.Cells["SellingPrice"].Value = "0.00";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw;
            }
        }
        private void dgvItem_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
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
                    _MainAdapter.DeleteCommand = new SqlCommand(@"Delete From Item_Master where Item_Code= @Item_Code ", DBClass.connection);
                    _MainAdapter.DeleteCommand.Parameters.AddWithValue("@Item_Code", int.Parse(dgvItem.Rows[e.Row.Index].Cells["Item_Code"].Value.ToString()));
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
        private void dgvItem_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvItem.Columns[e.ColumnIndex].Name.ToUpper() == "SELLINGPRICE" || dgvItem.Columns[e.ColumnIndex].Name.ToUpper() == "INCLUSIVECOSTPRICE"
                    || dgvItem.Columns[e.ColumnIndex].Name.ToUpper() == "COSTINGPRICE" || dgvItem.Columns[e.ColumnIndex].Name.ToUpper() == "VAT_PER"
                || dgvItem.Columns[e.ColumnIndex].Name.ToUpper() == "MARKUP_PER" || dgvItem.Columns[e.ColumnIndex].Name.ToUpper() == "MAXDISC_PER")
            {
                e.CellStyle.Format = "N2";
            }
        }
        private void dgvItem_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            //if (!dsMain.HasChanges()) return;
            try
            {

                if (dgvItem.Columns[e.ColumnIndex].Name.ToUpper() == "COSTINGPRICE" || dgvItem.Columns[e.ColumnIndex].Name.ToUpper() == "VAT_PER"
                    || dgvItem.Columns[e.ColumnIndex].Name.ToUpper() == "MARKUP_PER" || dgvItem.Columns[e.ColumnIndex].Name.ToUpper() == "SELLINGPRICE")
                {
                    decimal CostPrice = Convert.ToDecimal((dgvItem.CurrentRow.Cells["COSTINGPRICE"].Value is DBNull) ? 0 : dgvItem.CurrentRow.Cells["COSTINGPRICE"].Value);
                    decimal VATPer = Convert.ToDecimal((dgvItem.CurrentRow.Cells["VAT_PER"].Value is DBNull) ? 0 : dgvItem.CurrentRow.Cells["VAT_PER"].Value);
                    decimal MarkupPer = Convert.ToDecimal((dgvItem.CurrentRow.Cells["Markup_Per"].Value is DBNull) ? 0 : dgvItem.CurrentRow.Cells["Markup_Per"].Value);
                    decimal InclusiveCostPrice = Convert.ToDecimal((dgvItem.CurrentRow.Cells["InclusiveCostPrice"].Value is DBNull) ? 0 : dgvItem.CurrentRow.Cells["InclusiveCostPrice"].Value);
                    decimal SellingPrice = Convert.ToDecimal((dgvItem.CurrentRow.Cells["SellingPrice"].Value is DBNull) ? 0 : dgvItem.CurrentRow.Cells["SellingPrice"].Value);

                    InclusiveCostPrice = ((CostPrice * VATPer) / 100) + CostPrice;
                    dgvItem.CurrentRow.Cells["InclusiveCostPrice"].Value = InclusiveCostPrice;

                    if (dgvItem.Columns[e.ColumnIndex].Name.ToUpper() == "SELLINGPRICE")
                    {
                        if (InclusiveCostPrice.ToString() != "" && SellingPrice.ToString() != "" && InclusiveCostPrice != 0)
                        {
                            dgvItem.CurrentRow.Cells["Markup_Per"].Value = Math.Round(((SellingPrice - InclusiveCostPrice) * 100) / InclusiveCostPrice, 2);
                        }
                    }
                    else
                    {
                        dgvItem.CurrentRow.Cells["SellingPrice"].Value = ((InclusiveCostPrice * MarkupPer) / 100) + InclusiveCostPrice;
                    }
                }

            //if (dgvItem.Columns[e.ColumnIndex].Name.ToUpper() == "COSTINGPRICE" || dgvItem.Columns[e.ColumnIndex].Name.ToUpper() == "VAT_PER")
            //{
            //    decimal cell1 = Convert.ToDecimal((dgvItem.CurrentRow.Cells["COSTINGPRICE"].Value is DBNull) ? 0 : dgvItem.CurrentRow.Cells["COSTINGPRICE"].Value);
            //    decimal cell2 = Convert.ToDecimal((dgvItem.CurrentRow.Cells["VAT_PER"].Value is DBNull) ? 0 : dgvItem.CurrentRow.Cells["VAT_PER"].Value);
            //    decimal MarkupPer = Convert.ToDecimal((dgvItem.CurrentRow.Cells["Markup_Per"].Value is DBNull) ? 0 : dgvItem.CurrentRow.Cells["Markup_Per"].Value);
            //    decimal InclusiveCostPrice = 0;

            //    if (cell1.ToString() != "" && cell2.ToString() != "")
            //    {
            //        InclusiveCostPrice = ((cell1 * cell2) / 100) + cell1;
            //        dgvItem.CurrentRow.Cells["InclusiveCostPrice"].Value = InclusiveCostPrice;

            //        if (MarkupPer.ToString() != "" && InclusiveCostPrice.ToString() != "")
            //        {
            //            dgvItem.CurrentRow.Cells["SellingPrice"].Value = ((InclusiveCostPrice * MarkupPer) / 100) + InclusiveCostPrice;
            //        }
            //    }
            //}
            //else if (dgvItem.Columns[e.ColumnIndex].Name.ToUpper() == "MARKUP_PER")
            //{
            //    //((InclusiveCostPrice*Markup_Per)/100)+InclusiveCostPrice
            //    decimal cell1 = Convert.ToDecimal((dgvItem.CurrentRow.Cells["InclusiveCostPrice"].Value is DBNull) ? 0 : dgvItem.CurrentRow.Cells["InclusiveCostPrice"].Value);
            //    decimal cell2 = Convert.ToDecimal((dgvItem.CurrentRow.Cells["Markup_Per"].Value is DBNull) ? 0 : dgvItem.CurrentRow.Cells["Markup_Per"].Value);

            //    if (cell1.ToString() != "" && cell2.ToString() != "")
            //    {
            //        dgvItem.CurrentRow.Cells["SellingPrice"].Value = ((cell1 * cell2) / 100) + cell1;
            //    }
            //}
            //else if (dgvItem.Columns[e.ColumnIndex].Name.ToUpper() == "SELLINGPRICE")
            //{
            //    decimal cell1 = Convert.ToDecimal((dgvItem.CurrentRow.Cells["InclusiveCostPrice"].Value is DBNull) ? 0 : dgvItem.CurrentRow.Cells["InclusiveCostPrice"].Value);
            //    decimal cell2 = Convert.ToDecimal((dgvItem.CurrentRow.Cells["SellingPrice"].Value is DBNull) ? 0 : dgvItem.CurrentRow.Cells["SellingPrice"].Value);
            //    if (cell1.ToString() != "" && cell2.ToString() != "" && cell1 != 0)
            //    {
            //        dgvItem.CurrentRow.Cells["Markup_Per"].Value = Math.Round(((cell2 - cell1) * 100) / cell1,2);
            //    }
            //}

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion
    } 
}
