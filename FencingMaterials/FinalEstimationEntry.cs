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
    public partial class FinalEstimationEntry : Form
    {
        string FormType;
        public FinalEstimationEntry(string strFormType="ShowRecord")
        {
            InitializeComponent();
            FormType = strFormType;
        }

        #region Common Variables
        // Common Variables///////////////////////

        DataSet dsForCombo = new DataSet();
        DataTable _DataTable;
        public DataSet dsMain = new DataSet();
        DataSet ds = new DataSet();
        SqlDataAdapter _MainAdapter;
        SqlDataAdapter _MainSubAdapter;
        SqlDataAdapter _MainDTAdapter;
        SqlDataAdapter _ItemsAdapter;
        SqlDataAdapter _CustomerAdapter;
        DataTable dt;
        int MBillNo;
        int IntRowIndex = 0;
        #endregion

        #region Form Load
        private void FinalEstimationEntry_Load(object sender, EventArgs e)
        {
            AutoComplete_textbox();
            Set_EstimationGrid();
            //if (FormType == "ShowRecord")
            {
                DisplayRecord();
            }

            Enable_Disable_Controls(true);
            if (FormType == "NewRecord")
            {
                NewRecord();
            }
            

        }
        private void FinalEstimationEntry_KeyDown(object sender, KeyEventArgs e)
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

        #region Fill AutoTextbox
        private void AutoComplete_textbox()
        {
            //Customer Name
            _DataTable = new DataTable();

            _CustomerAdapter = DBClass.GetAdapterByQuery("Select * from Customer_Master");
            _CustomerAdapter.Fill(_DataTable);
            dsForCombo.Tables.Add(_DataTable);
            dsForCombo.Tables[0].TableName = "Customer_Master";

            AutoCompleteStringCollection strCustomerName = new AutoCompleteStringCollection();
            for (int i = 0; i < dsForCombo.Tables["Customer_Master"].Rows.Count; i++)
            {
                strCustomerName.Add(dsForCombo.Tables["Customer_Master"].Rows[i]["Customer_Name"].ToString());
            }

            txtCustomerName.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txtCustomerName.AutoCompleteCustomSource = strCustomerName;
            txtCustomerName.AutoCompleteMode = AutoCompleteMode.SuggestAppend;

            _DataTable = DBClass.GetTableByQuery("Select * from User_Master where IsSalesPerson=1");

            AutoCompleteStringCollection strUserName = new AutoCompleteStringCollection();
            for (int i = 0; i < _DataTable.Rows.Count; i++)
            {
                strUserName.Add(_DataTable.Rows[i]["User_Name"].ToString());
            }

            txtUserName.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txtUserName.AutoCompleteCustomSource = strUserName;
            txtUserName.AutoCompleteMode = AutoCompleteMode.SuggestAppend;

            _DataTable = DBClass.GetTableRecords("Item_Master");
            dsForCombo.Tables.Add(_DataTable);
            dsForCombo.Tables[1].TableName = "Item_Master";

        }
        #endregion

        #region Fill Records
        private void DisplayRecord()
        {
            _MainAdapter = DBClass.GetAdaptor("Trans_Master");
            _DataTable = new DataTable();
            _MainAdapter.Fill(_DataTable);

            dsMain.Tables.Add(_DataTable);
            dsMain.Tables[0].TableName = "Trans_Master";

            _MainDTAdapter = DBClass.GetAdaptor("Trans_Details");
            _DataTable = new DataTable();
            _MainDTAdapter.Fill(_DataTable);

            dsMain.Tables.Add(_DataTable);
            dsMain.Tables[1].TableName = "Trans_Details";

            _MainSubAdapter = DBClass.GetAdaptor("Trans_SubMaster");
            _DataTable = new DataTable();
            _MainSubAdapter.Fill(_DataTable);

            dsMain.Tables.Add(_DataTable);
            dsMain.Tables[2].TableName = "Trans_SubMaster";

            if (FormType != "NewRecord")
            {
                Set_EstimationGrid();

                FillRecord(dsMain.Tables["Trans_Master"].Rows.Count - 1);
            }
        }
        private void FillRecord(int Index)
        {
            int rowIndex;
            if (MBillNo > 0)
            {
                DataRow[] row = dsMain.Tables["Trans_Master"].Select("Bill_No=" + MBillNo);
                if (row.Length != 0)
                {
                    rowIndex = dsMain.Tables["Trans_Master"].Rows.IndexOf(row[0]);
                    Index = rowIndex;
                }
            }

            if (dsMain.Tables["Trans_Master"].Rows.Count > 0)
            {
                txtNumber.Text = dsMain.Tables["Trans_Master"].Rows[Index]["Estimation_No"].ToString();
                txtDate.Text = Convert.ToDateTime(dsMain.Tables["Trans_Master"].Rows[Index]["Trans_Date"].ToString()).ToShortDateString();
                txtReference.Text = dsMain.Tables["Trans_Master"].Rows[Index]["Refrence"].ToString();
                txtUserName.Text = dsMain.Tables["Trans_Master"].Rows[Index]["SalesPerson"].ToString();
                txtCustomerCode.Text = dsMain.Tables["Trans_Master"].Rows[Index]["Customer_Code"].ToString();
                txtDelivery.Text = dsMain.Tables["Trans_Master"].Rows[Index]["Delivery_Address"].ToString();
                txtTotalAmt.Text = dsMain.Tables["Trans_Master"].Rows[Index]["Total_Amt"].ToString();
                txtVATAmt.Text = dsMain.Tables["Trans_Master"].Rows[Index]["VAT_Amt"].ToString();
                txtDiscountAmt.Text = dsMain.Tables["Trans_Master"].Rows[Index]["DISC_Amt"].ToString();
                txtNetAmt.Text = dsMain.Tables["Trans_Master"].Rows[Index]["Net_Amt"].ToString();

                if (txtCustomerCode.Text != "")
                {
                    dsForCombo.Tables["Customer_Master"].DefaultView.RowFilter = "";
                    dsForCombo.Tables["Customer_Master"].DefaultView.RowFilter = "Customer_Code=" + int.Parse(txtCustomerCode.Text) + "";

                    DataTable DT = new DataTable();
                    DT = dsForCombo.Tables["Customer_Master"].DefaultView.ToTable();
                    if (DT.Rows.Count > 0)
                    {
                        txtCustomerCode.Text = DT.Rows[0]["Customer_Code"].ToString();
                        txtAdders.Text = DT.Rows[0]["Customer_Address"].ToString();
                        txtDelivery.Text = DT.Rows[0]["Customer_Delivery"].ToString();
                        txtContactNo.Text = DT.Rows[0]["Customer_Phone"].ToString();
                        txtWhatsapp.Text = DT.Rows[0]["Customer_Whatsapp"].ToString();
                        txtIdentityNo.Text = DT.Rows[0]["Customer_IdentityNo"].ToString();
                    }
                }

                DisplaySubDetails(txtNumber.Text);
                DisplayDetails(txtNumber.Text);
            }
        }
        private void DisplaySubDetails(string TransId)
        {
            dsMain.Tables["Trans_SubMaster"].DefaultView.RowFilter = "";
            dsMain.Tables["Trans_SubMaster"].DefaultView.RowFilter = " Trans_Id=" + int.Parse(TransId);
            DataTable dt = new DataTable();
            dt = dsMain.Tables["Trans_SubMaster"].DefaultView.ToTable();
            if (dt.Rows.Count > 0)
            {
                lblTransSubId.Text = dt.Rows[0]["Trans_SubId"].ToString();
                lblFenceType.Text = dt.Rows[0]["Fence_Type"].ToString();
                lblSecurity.Text = dt.Rows[0]["Security_Type"].ToString();
                txtPlotHeight.Text = dt.Rows[0]["Plot_Height"].ToString();
                txtPlotWidth.Text = dt.Rows[0]["Plot_Width"].ToString();
                txtTotalArea.Text = dt.Rows[0]["Total_Area"].ToString();
                txtTotalCorner.Text = dt.Rows[0]["Total_Corner"].ToString();
                txtRollSize.Text = dt.Rows[0]["DM_Roll"].ToString();
                txtHeightOfFence.Text = dt.Rows[0]["Height_of_Fence"].ToString();
                txtTotalGate.Text = dt.Rows[0]["Total_Gate"].ToString();
                txtTotalGateArea.Text = dt.Rows[0]["Gate_Area"].ToString();
                txtStdY.Text = dt.Rows[0]["StandardY"].ToString();
                txtDropper.Text = dt.Rows[0]["Dropper"].ToString();
            }
        }
        private void DisplayDetails(string TransId)
        {
            dsMain.Tables["Trans_Details"].DefaultView.RowFilter = "";
            dsMain.Tables["Trans_Details"].DefaultView.RowFilter = " Trans_Id=" + int.Parse(TransId);

            dgvEstimateGrid.Rows.Clear();
            DataTable dt = new DataTable();
            dt = dsMain.Tables["Trans_Details"].DefaultView.ToTable();
            if (dt.Rows.Count > 0)
            {
                for (int row = 0; row < dt.Rows.Count; row++)
                {
                    string ItemName = "";
                    string ItemPartNumber = "";
                    string ItemCode = dt.Rows[row]["Item_Code"].ToString();
                    decimal MaxDiscPer = 0;
                    dsForCombo.Tables["Item_Master"].DefaultView.RowFilter = "";
                    dsForCombo.Tables["Item_Master"].DefaultView.RowFilter = "Item_Code=" + int.Parse(ItemCode);

                    DataTable dtItem = new DataTable();
                    dtItem = dsForCombo.Tables["Item_Master"].DefaultView.ToTable();
                    if (dtItem.Rows.Count > 0)
                    {
                        ItemName = dtItem.Rows[0]["Item_Name"].ToString();
                        ItemPartNumber = dtItem.Rows[0]["Item_PartNumber"].ToString();
                        MaxDiscPer = Convert.ToDecimal(dtItem.Rows[0]["MaxDisc_Per"].ToString());
                    }

                    decimal ExclusiveAmt = decimal.Parse(dt.Rows[row]["Item_Qty"].ToString()) * decimal.Parse(dt.Rows[row]["SellingPrice"].ToString());
                    decimal VATAmt = (ExclusiveAmt * decimal.Parse(dt.Rows[row]["VAT_Per"].ToString()))/100;
                    decimal InclusiveAmt = ExclusiveAmt + VATAmt;
                    decimal DiscAmt = (InclusiveAmt * decimal.Parse(dt.Rows[row]["DISC_Per"].ToString())) / 100;
                    decimal NetAmt = ExclusiveAmt + VATAmt - DiscAmt;
                    dgvEstimateGrid.Rows.Add(dt.Rows[row]["Details_Id"].ToString(), dt.Rows[row]["Trans_Id"].ToString(), ItemCode, ItemPartNumber, ItemName, dt.Rows[row]["Additional_Info"].ToString(), dt.Rows[row]["Item_Qty"].ToString(), dt.Rows[row]["SellingPrice"].ToString(), ExclusiveAmt, dt.Rows[row]["VAT_Per"].ToString(),VATAmt,InclusiveAmt, MaxDiscPer, dt.Rows[row]["Disc_Per"].ToString(),DiscAmt, NetAmt);
                }
            }

            CalculateTotal();
        }
        private void CalculateTotal()
        {
            //dsMain.Tables["Trans_Details"].DefaultView.RowFilter = "";
            //dsMain.Tables["Trans_Details"].DefaultView.RowFilter = " Bill_No=" + int.Parse(txtBillNo.Text);

            if (dgvEstimateGrid.Rows.Count >= 1)
            {
                //DataTable DT = new DataTable();
                //DT = dsMain.Tables["Trans_Details"].DefaultView.ToTable();
                //DataTable dt = new DataTable();
                //DT = (DataTable)dgvEstimateGrid.DataSource;
                //DT.Columns["ExclusiveAmount"].DataType = typeof(decimal);
                //DT.Columns["VATAmount"].DataType = typeof(decimal);
                //DT.Columns["DiscAmount"].DataType = typeof(decimal);

                DataTable DT = new DataTable();
                foreach (DataGridViewColumn col in dgvEstimateGrid.Columns)
                {
                    DT.Columns.Add(col.Name);
                }

                foreach (DataGridViewRow row in dgvEstimateGrid.Rows)
                {
                    DataRow dRow = dt.NewRow();
                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        dRow[cell.ColumnIndex] = cell.Value;
                    }
                    DT.Rows.Add(dRow);
                }

                if (DT.Rows.Count > 0)
                {

                    decimal TotalAmt = 0, VATAmt = 0, DiscAmt = 0;
                    foreach (DataRow dr in DT.Rows)
                    {
                        TotalAmt += Convert.ToDecimal(dr["ExclusiveAmount"]);
                        VATAmt += Convert.ToDecimal(dr["VATAmount"]);
                        DiscAmt += Convert.ToDecimal(dr["DiscAmount"]);
                    }

                    
                    txtTotalAmt.Text = TotalAmt.ToString();
                    txtVATAmt.Text = VATAmt.ToString();
                    txtDiscountAmt.Text = DiscAmt.ToString();

                    decimal NetAmt = TotalAmt + Convert.ToDecimal(VATAmt) - Convert.ToDecimal(DiscAmt);
                    txtNetAmt.Text = NetAmt.ToString();
                }
                else
                {
                    txtTotalAmt.Text = "0";
                    txtNetAmt.Text = "0";

                }
            }
            else
            {
                txtTotalAmt.Text = "0";
                txtNetAmt.Text = "0";
            }
        }
        #endregion

        #region Transaction Functions
        private void Enable_Disable_Controls(bool Val)
        {
            btn_New.Enabled = Val;
            btn_Edit.Enabled = Val;
            btn_Delete.Enabled = Val;
            btn_Clear.Enabled = Val;
            btn_Save.Enabled = !Val;
            btn_Clear.Enabled = !Val;
        }
        private void ClearRecord()
        {
            ///Trans Master
            txtNumber.Text = "";
            txtDate.Text = "";
            txtReference.Text = "";
            txtUserName.Text = "";
            txtuserId.Text = "";

            txtCustomerCode.Text = "";
            txtCustomerName.Text = "";
            txtAdders.Text = "";
            txtDelivery.Text = "";
            txtContactNo.Text = "";
            txtWhatsapp.Text = "";
            txtIdentityNo.Text = "";


            txtTotalAmt.Text = "0";
            txtDiscountAmt.Text = "0";
            txtVATAmt.Text = "0";
            txtNetAmt.Text = "0";

            //Trans Sub Master
            lblSecurity.Text = "";
            lblFenceType.Text = "";
            lblTransSubId.Text = "";
            txtPlotHeight.Text = "";
            txtPlotWidth.Text = "";
            txtTotalArea.Text = "";
            txtTotalCorner.Text = "";
            txtRollSize.Text = "";
            txtHeightOfFence.Text = "";
            txtTotalGate.Text = "";
            txtTotalGateArea.Text = "";
            txtStdY.Text = "";
            txtDropper.Text = "";
        }
        private void NewRecord()
        {
            if (FormType != "NewRecord")
            {
                ClearRecord();
            }

            int maxTransId = DBClass.GetIdByQuery("SELECT IDENT_CURRENT('Trans_Master')+1");
            txtNumber.Text = maxTransId.ToString();
            txtDate.Text = System.DateTime.Now.ToShortDateString();

            int maxTransSubid = DBClass.GetIdByQuery("SELECT IDENT_CURRENT('Trans_SubMaster')+1");
            lblTransSubId.Text = maxTransSubid.ToString();

            dsMain.Tables["Trans_Details"].DefaultView.RowFilter = "";
            dsMain.Tables["Trans_Details"].DefaultView.RowFilter = " Trans_Id=" + int.Parse(txtNumber.Text);
            //dgvEstimateGrid.Rows.Clear();
            if (dgvEstimateGrid.Rows.Count > 0)
            {
                CalculateTotal();
            }
            Enable_Disable_Controls(false);
            txtCustomerName.Focus();
        }
        private void DeleteRecord()
        {
            if (MessageBox.Show("Are you Sure to delete this record ? ", "Delete Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
            {
                DataTable dt = DBClass.GetTableByQuery("Select count(*) from Trans_Details where Trans_Id=" + Convert.ToInt16(txtNumber.Text));
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0][0].ToString() != "0")
                    {
                        MessageBox.Show("Details Exist of this Estimation,Can't be delete!");
                        return;
                    }
                }
                DBClass.connection.Open();

                _MainDTAdapter.DeleteCommand = new SqlCommand(@"Delete From Trans_Details where Trans_Id=@Trans_Id ", DBClass.connection);
                _MainDTAdapter.DeleteCommand.Parameters.AddWithValue("@Trans_Id", txtNumber.Text);
                _MainDTAdapter.DeleteCommand.ExecuteNonQuery();

                dsMain.Tables["Trans_Details"].Clear();
                _MainDTAdapter.Fill(dsMain.Tables["Trans_Details"]);

                _MainSubAdapter.DeleteCommand = new SqlCommand(@"Delete From Trans_SubMaster where Trans_Id=@Trans_Id ", DBClass.connection);
                _MainSubAdapter.DeleteCommand.Parameters.AddWithValue("@Trans_Id", txtNumber.Text);
                _MainSubAdapter.DeleteCommand.ExecuteNonQuery();

                dsMain.Tables["Trans_SubMaster"].Clear();
                _MainSubAdapter.Fill(dsMain.Tables["Trans_SubMaster"]);

                _MainAdapter.DeleteCommand = new SqlCommand(@"Delete From Trans_Master where Trans_Id= @Trans_Id ", DBClass.connection);
                _MainAdapter.DeleteCommand.Parameters.AddWithValue("@Trans_Id", txtNumber.Text);
                _MainAdapter.DeleteCommand.ExecuteNonQuery();

                dsMain.Tables["Trans_Master"].Clear();
                _MainAdapter.Fill(dsMain.Tables["Trans_Master"]);

                DBClass.connection.Close();

                if (dsMain.Tables["Trans_Master"].Rows.Count > 0)
                    FillRecord(dsMain.Tables["Trans_Master"].Rows.Count - 1);
                else
                    ClearRecord();
            }
        }
        private void SaveRecord()
        {
            ///////////Save Trans_Master

            DataRow[] row = dsMain.Tables["Trans_Master"].Select("Trans_Id=" + int.Parse(txtNumber.Text));
            if (row.Length == 0)
            {
                _MainAdapter.InsertCommand = new SqlCommand(@"insert into Trans_Master(Trans_Date,Estimation_No,Refrence,SalesPerson,Customer_Code,Delivery_Address,Total_Amt,VAT_Amt,DISC_Amt,Net_Amt,IsPrint,Entry_UserId,Entry_Date) 
                                                              output inserted.Trans_Id
                                                              Values(@Trans_Date,@Estimation_No,@Refrence,@SalesPerson,@Customer_Code,@Delivery_Address,@Total_Amt,@VAT_Amt,@DISC_Amt,@Net_Amt,@IsPrint,@Entry_UserId,@Entry_Date)", DBClass.connection);
                DBClass.connection.Open();

                _MainAdapter.InsertCommand.Parameters.AddWithValue("@Trans_Date", txtDate.Text);
                _MainAdapter.InsertCommand.Parameters.AddWithValue("@Estimation_No", txtNumber.Text);
                _MainAdapter.InsertCommand.Parameters.AddWithValue("@Refrence", txtReference.Text);
                _MainAdapter.InsertCommand.Parameters.AddWithValue("@SalesPerson", txtUserName.Text);
                _MainAdapter.InsertCommand.Parameters.AddWithValue("@Customer_Code", int.Parse((txtCustomerCode.Text == "") ? "0" : txtCustomerCode.Text));
                _MainAdapter.InsertCommand.Parameters.AddWithValue("@Delivery_Address", txtDelivery.Text);

                string TotalAmt = (txtTotalAmt.Text == "") ? "0" : txtTotalAmt.Text;
                _MainAdapter.InsertCommand.Parameters.AddWithValue("@Total_Amt", Convert.ToDecimal(TotalAmt));
                string VATAmt = (txtVATAmt.Text == "") ? "0" : txtVATAmt.Text;
                _MainAdapter.InsertCommand.Parameters.AddWithValue("@VAT_Amt", Convert.ToDecimal(VATAmt));
                string DISCAmt = (txtDiscountAmt.Text == "") ? "0" : txtDiscountAmt.Text;
                _MainAdapter.InsertCommand.Parameters.AddWithValue("@DISC_Amt", decimal.Parse(DISCAmt));
                string NetAmt = (txtNetAmt.Text == "") ? "0" : txtNetAmt.Text;
                _MainAdapter.InsertCommand.Parameters.AddWithValue("@Net_Amt", decimal.Parse(NetAmt));
                _MainAdapter.InsertCommand.Parameters.AddWithValue("@IsPrint", 0);
                _MainAdapter.InsertCommand.Parameters.AddWithValue("@Entry_UserId", DBClass.UserId);
                _MainAdapter.InsertCommand.Parameters.AddWithValue("@Entry_Date", System.DateTime.Now);

                int id = (int)_MainAdapter.InsertCommand.ExecuteScalar();

                txtNumber.Text = id.ToString();
                DBClass.connection.Close();
            }
            else
            {
                DBClass.connection.Open();
                _MainAdapter.UpdateCommand = new SqlCommand(@"update Trans_Master set Trans_Date=@Trans_Date,Estimation_No=@Estimation_No,Refrence=@Refrence,SalesPerson=@SalesPerson,
                                                                Customer_Code=@Customer_Code,Delivery_Address=@Delivery_Address,Total_Amt=@Total_Amt,VAT_Amt=@VAT_Amt,DISC_Amt=@DISC_Amt,
                                                                Net_Amt=@Net_Amt,Entry_UserId=@Entry_UserId,Entry_Date=@Entry_Date 
                                                                where Trans_Id= @Trans_Id ", DBClass.connection);

                _MainAdapter.UpdateCommand.Parameters.AddWithValue("@Trans_Id", txtNumber.Text);
                _MainAdapter.UpdateCommand.Parameters.AddWithValue("@Trans_Date", txtDate.Text);
                _MainAdapter.UpdateCommand.Parameters.AddWithValue("@Estimation_No", txtNumber.Text);
                _MainAdapter.UpdateCommand.Parameters.AddWithValue("@Refrence", txtReference.Text);
                _MainAdapter.UpdateCommand.Parameters.AddWithValue("@SalesPerson", txtUserName.Text);
                _MainAdapter.UpdateCommand.Parameters.AddWithValue("@Customer_Code", int.Parse((txtCustomerCode.Text == "") ? "0" : txtCustomerCode.Text));
                _MainAdapter.UpdateCommand.Parameters.AddWithValue("@Delivery_Address", txtDelivery.Text);

                string TAmt = (txtTotalAmt.Text == "") ? "0" : txtTotalAmt.Text;
                _MainAdapter.UpdateCommand.Parameters.AddWithValue("@Total_Amt", decimal.Parse(TAmt));

                string VATAmt = (txtVATAmt.Text == "") ? "0" : txtVATAmt.Text;
                _MainAdapter.UpdateCommand.Parameters.AddWithValue("@VAT_Amt", decimal.Parse(VATAmt));

                string DiscAmt = (txtDiscountAmt.Text == "") ? "0" : txtDiscountAmt.Text;
                _MainAdapter.UpdateCommand.Parameters.AddWithValue("@DISC_Amt", Convert.ToDecimal(DiscAmt));

                string NetAmt = (txtNetAmt.Text == "") ? "0" : txtNetAmt.Text;
                _MainAdapter.UpdateCommand.Parameters.AddWithValue("@Net_Amt", decimal.Parse(NetAmt));

                _MainAdapter.UpdateCommand.Parameters.AddWithValue("@Entry_UserId", DBClass.UserId);
                _MainAdapter.UpdateCommand.Parameters.AddWithValue("@Entry_Date", System.DateTime.Now);

                _MainAdapter.UpdateCommand.ExecuteNonQuery();
                DBClass.connection.Close();
            }

            dsMain.Tables["Trans_Master"].Clear();
            _MainAdapter.Fill(dsMain.Tables["Trans_Master"]);

            ////////////////Save Trans Sub Master
            DataRow[] rw = dsMain.Tables["Trans_SubMaster"].Select("Trans_Id=" + int.Parse(txtNumber.Text));
            if (rw.Length == 0)
            {
                _MainSubAdapter.InsertCommand = new SqlCommand(@"insert into Trans_SubMaster(Trans_Id,Fence_Type,Security_Type,Plot_Height,Plot_Width,Total_Area,Total_Corner,DM_Roll,Height_of_Fence,Total_Gate,StandardY,Dropper,Gate_Area,Entry_UserId,Entry_Date) 
                                                              output inserted.Trans_SubId
                                                              Values(@Trans_Id,@Fence_Type,@Security_Type,@Plot_Height,@Plot_Width,@Total_Area,@Total_Corner,@DM_Roll,@Height_of_Fence,@Total_Gate,@StandardY,@Dropper,@Gate_Area,@Entry_UserId,@Entry_Date)", DBClass.connection);
                DBClass.connection.Open();

                _MainSubAdapter.InsertCommand.Parameters.AddWithValue("@Trans_Id", txtNumber.Text);
                _MainSubAdapter.InsertCommand.Parameters.AddWithValue("@Fence_Type", lblFenceType.Text);
                _MainSubAdapter.InsertCommand.Parameters.AddWithValue("@Security_Type", lblSecurity.Text);
                _MainSubAdapter.InsertCommand.Parameters.AddWithValue("@Plot_Height", Convert.ToInt16((txtPlotHeight.Text == "") ? "0" : txtPlotHeight.Text));
                _MainSubAdapter.InsertCommand.Parameters.AddWithValue("@Plot_Width", Convert.ToInt16((txtPlotWidth.Text == "") ? "0" : txtPlotWidth.Text));

                string TotalArea = (txtTotalArea.Text == "") ? "0" : txtTotalArea.Text;
                _MainSubAdapter.InsertCommand.Parameters.AddWithValue("@Total_Area", Convert.ToDecimal(TotalArea));

                string TotalCorner = (txtTotalCorner.Text == "") ? "0" : txtTotalCorner.Text;
                _MainSubAdapter.InsertCommand.Parameters.AddWithValue("@Total_Corner", Convert.ToInt16(TotalCorner));

                _MainSubAdapter.InsertCommand.Parameters.AddWithValue("@DM_Roll", txtRollSize.Text);
                _MainSubAdapter.InsertCommand.Parameters.AddWithValue("@Height_of_Fence", txtHeightOfFence.Text);

                string TotalGate = (txtTotalGate.Text == "") ? "0" : txtTotalGate.Text;
                _MainSubAdapter.InsertCommand.Parameters.AddWithValue("@Total_Gate", Convert.ToInt16(TotalGate));

                _MainSubAdapter.InsertCommand.Parameters.AddWithValue("@StandardY", txtStdY.Text);
                _MainSubAdapter.InsertCommand.Parameters.AddWithValue("@Dropper", txtDropper.Text);

                string TGateArea = (txtTotalGateArea.Text == "") ? "0" : txtTotalGateArea.Text;
                _MainSubAdapter.InsertCommand.Parameters.AddWithValue("@Gate_Area", decimal.Parse(TGateArea));

                _MainSubAdapter.InsertCommand.Parameters.AddWithValue("@Entry_UserId", DBClass.UserId);
                _MainSubAdapter.InsertCommand.Parameters.AddWithValue("@Entry_Date", System.DateTime.Now);

                int id = (int)_MainSubAdapter.InsertCommand.ExecuteScalar();

                lblTransSubId.Text = id.ToString();
                DBClass.connection.Close();

            }
            else
            {
                DBClass.connection.Open();
                _MainSubAdapter.UpdateCommand = new SqlCommand(@"update Trans_SubMaster set Trans_Id=@Trans_Id,Fence_Type=@Fence_Type,Security_Type=@Security_Type,Plot_Height=@Plot_Height,
                                                                Plot_Width=@Plot_Width,Total_Area=@Total_Area,Total_Corner=@Total_Corner,DM_Roll=@DM_Roll,Height_of_Fence=@Height_of_Fence,
                                                                Total_Gate=@Total_Gate,StandardY=@StandardY,Dropper=@Dropper,Gate_Area=@Gate_Area,Entry_UserId=@Entry_UserId,Entry_Date=@Entry_Date 
                                                                where Trans_SubId= @Trans_SubId ", DBClass.connection);

                _MainSubAdapter.UpdateCommand.Parameters.AddWithValue("@Trans_SubId", lblTransSubId.Text);
                _MainSubAdapter.UpdateCommand.Parameters.AddWithValue("@Trans_Id", txtNumber.Text);
                _MainSubAdapter.UpdateCommand.Parameters.AddWithValue("@Fence_Type", lblFenceType.Text);
                _MainSubAdapter.UpdateCommand.Parameters.AddWithValue("@Security_Type", lblSecurity.Text);
                _MainSubAdapter.UpdateCommand.Parameters.AddWithValue("@Plot_Height", Convert.ToInt16((txtPlotHeight.Text == "") ? "0" : txtPlotHeight.Text));
                _MainSubAdapter.UpdateCommand.Parameters.AddWithValue("@Plot_Width", Convert.ToInt16((txtPlotWidth.Text == "") ? "0" : txtPlotWidth.Text));

                string TotalArea = (txtTotalArea.Text == "") ? "0" : txtTotalArea.Text;
                _MainSubAdapter.UpdateCommand.Parameters.AddWithValue("@Total_Area", Convert.ToDecimal(TotalArea));

                string TotalCorner = (txtTotalCorner.Text == "") ? "0" : txtTotalCorner.Text;
                _MainSubAdapter.UpdateCommand.Parameters.AddWithValue("@Total_Corner", Convert.ToInt16(TotalCorner));

                _MainSubAdapter.UpdateCommand.Parameters.AddWithValue("@DM_Roll", txtRollSize.Text);
                _MainSubAdapter.UpdateCommand.Parameters.AddWithValue("@Height_of_Fence", txtHeightOfFence.Text);

                string TotalGate = (txtTotalGate.Text == "") ? "0" : txtTotalGate.Text;
                _MainSubAdapter.UpdateCommand.Parameters.AddWithValue("@Total_Gate", Convert.ToInt16(TotalGate));

                _MainSubAdapter.UpdateCommand.Parameters.AddWithValue("@StandardY", txtStdY.Text);
                _MainSubAdapter.UpdateCommand.Parameters.AddWithValue("@Dropper", txtDropper.Text);

                string TGateArea = (txtTotalGateArea.Text == "") ? "0" : txtTotalGateArea.Text;
                _MainSubAdapter.UpdateCommand.Parameters.AddWithValue("@Gate_Area", decimal.Parse(TGateArea));

                _MainSubAdapter.UpdateCommand.Parameters.AddWithValue("@Entry_UserId", DBClass.UserId);
                _MainSubAdapter.UpdateCommand.Parameters.AddWithValue("@Entry_Date", System.DateTime.Now);

                _MainSubAdapter.UpdateCommand.ExecuteNonQuery();
                DBClass.connection.Close();
            }

            dsMain.Tables["Trans_SubMaster"].Clear();
            _MainSubAdapter.Fill(dsMain.Tables["Trans_SubMaster"]);

            //////////////// Save Trans Detail ///////////

            for (int i = 0; i < dgvEstimateGrid.Rows.Count; i++)
            {
                string DetialId = (dgvEstimateGrid.Rows[i].Cells["Details_Id"].Value.ToString() == "") ? "0" : dgvEstimateGrid.Rows[i].Cells["Details_Id"].Value.ToString();
                DataRow[] r = dsMain.Tables["Trans_Details"].Select("Details_Id=" + Convert.ToInt16(DetialId));
                if (r.Length == 0)
                {
                    _MainDTAdapter.InsertCommand = new SqlCommand(@"insert into Trans_Details(Trans_Id,Item_Code,Item_Qty,VAT_Per,Disc_Per,SellingPrice,Additional_Info,Entry_UserId,Entry_Date) 
                                                              output inserted.Details_Id
                                                              Values(@Trans_Id,@Item_Code,@Item_Qty,@VAT_Per,@Disc_Per,@SellingPrice,@Additional_Info,@Entry_UserId,@Entry_Date)", DBClass.connection);
                    DBClass.connection.Open();

                    _MainDTAdapter.InsertCommand.Parameters.AddWithValue("@Trans_Id", txtNumber.Text);
                    _MainDTAdapter.InsertCommand.Parameters.AddWithValue("@Item_Code", dgvEstimateGrid.Rows[i].Cells["Item_Code"].Value.ToString());

                    string ItemQty = (dgvEstimateGrid.Rows[i].Cells["Item_qty"].Value.ToString() == "") ? "0" : dgvEstimateGrid.Rows[i].Cells["Item_qty"].Value.ToString();
                    _MainDTAdapter.InsertCommand.Parameters.AddWithValue("@Item_Qty", Convert.ToDecimal((ItemQty == "") ? "0" : ItemQty));

                    string VATPer = (dgvEstimateGrid.Rows[i].Cells["VAT_Per"].Value.ToString() == "") ? "0" : dgvEstimateGrid.Rows[i].Cells["VAT_Per"].Value.ToString();
                    _MainDTAdapter.InsertCommand.Parameters.AddWithValue("@VAT_Per", Convert.ToDecimal((VATPer == "") ? "0" : VATPer));

                    string DiscPer = (dgvEstimateGrid.Rows[i].Cells["Disc_Per"].Value.ToString() == "") ? "0" : dgvEstimateGrid.Rows[i].Cells["Disc_Per"].Value.ToString();
                    _MainDTAdapter.InsertCommand.Parameters.AddWithValue("@Disc_Per", Convert.ToDecimal((DiscPer == "") ? "0" : VATPer));

                    string SellingPrice = (dgvEstimateGrid.Rows[i].Cells["SellingPrice"].Value.ToString() == "") ? "0" : dgvEstimateGrid.Rows[i].Cells["SellingPrice"].Value.ToString();
                    _MainDTAdapter.InsertCommand.Parameters.AddWithValue("@SellingPrice", Convert.ToDecimal((DiscPer == "") ? "0" : SellingPrice));

                    _MainDTAdapter.InsertCommand.Parameters.AddWithValue("@Additional_Info", dgvEstimateGrid.Rows[i].Cells["Additional_Info"].Value.ToString());

                    _MainDTAdapter.InsertCommand.Parameters.AddWithValue("@Entry_UserId", DBClass.UserId);
                    _MainDTAdapter.InsertCommand.Parameters.AddWithValue("@Entry_Date", System.DateTime.Now);

                    int id = (int)_MainDTAdapter.InsertCommand.ExecuteScalar();

                    dgvEstimateGrid.Rows[i].Cells["Details_Id"].Value = id.ToString();
                    dgvEstimateGrid.Rows[i].Cells["Trans_Id"].Value = txtNumber.Text;
                    DBClass.connection.Close();
                }
                else
                {
                    DBClass.connection.Open();
                    _MainDTAdapter.UpdateCommand = new SqlCommand(@"update Trans_Details set Trans_Id=@Trans_Id,Item_Code=@Item_Code,Item_Qty=@Item_Qty,VAT_Per=@VAT_Per,
                                                                Disc_Per=@Disc_Per,SellingPrice=@SellingPrice,Additional_Info=@Additional_Info,Entry_UserId=@Entry_UserId,Entry_Date=@Entry_Date 
                                                                where Details_Id= @Details_Id ", DBClass.connection);

                    _MainDTAdapter.InsertCommand.Parameters.AddWithValue("@Details_Id", Convert.ToInt16(dgvEstimateGrid.Rows[i].Cells["Details_Id"].Value.ToString()));
                    _MainDTAdapter.InsertCommand.Parameters.AddWithValue("@Trans_Id",Convert.ToInt16(txtNumber.Text));
                    _MainDTAdapter.InsertCommand.Parameters.AddWithValue("@Item_Code",Convert.ToInt16( dgvEstimateGrid.Rows[i].Cells["Item_Code"].Value.ToString()));

                    string ItemQty = (dgvEstimateGrid.Rows[i].Cells["Item_qty"].Value.ToString() == "") ? "0" : dgvEstimateGrid.Rows[i].Cells["Item_qty"].Value.ToString();
                    _MainDTAdapter.InsertCommand.Parameters.AddWithValue("@Item_Qty", Convert.ToInt16((ItemQty == "") ? "0" : ItemQty));

                    string VATPer = (dgvEstimateGrid.Rows[i].Cells["VAT_Per"].Value.ToString() == "") ? "0" : dgvEstimateGrid.Rows[i].Cells["VAT_Per"].Value.ToString();
                    _MainDTAdapter.InsertCommand.Parameters.AddWithValue("@VAT_Per", Convert.ToDecimal((VATPer == "") ? "0" : VATPer));

                    string DiscPer = (dgvEstimateGrid.Rows[i].Cells["Disc_Per"].Value.ToString() == "") ? "0" : dgvEstimateGrid.Rows[i].Cells["Disc_Per"].Value.ToString();
                    _MainDTAdapter.InsertCommand.Parameters.AddWithValue("@Disc_Per", Convert.ToDecimal((DiscPer == "") ? "0" : VATPer));

                    string SellingPrice = (dgvEstimateGrid.Rows[i].Cells["SellingPrice"].Value.ToString() == "") ? "0" : dgvEstimateGrid.Rows[i].Cells["SellingPrice"].Value.ToString();
                    _MainDTAdapter.InsertCommand.Parameters.AddWithValue("@SellingPrice", Convert.ToDecimal((DiscPer == "") ? "0" : SellingPrice));

                    _MainDTAdapter.InsertCommand.Parameters.AddWithValue("@Additional_Info", dgvEstimateGrid.Rows[i].Cells["Additional_Info"].Value.ToString());

                    _MainDTAdapter.InsertCommand.Parameters.AddWithValue("@Entry_UserId", DBClass.UserId);
                    _MainDTAdapter.InsertCommand.Parameters.AddWithValue("@Entry_Date", System.DateTime.Now);


                    _MainDTAdapter.UpdateCommand.ExecuteNonQuery();
                    DBClass.connection.Close();
                }
            }

            /*SqlCommandBuilder cmdBuilder = new SqlCommandBuilder(_MainDTAdapter);
            _MainDTAdapter.UpdateCommand = cmdBuilder.GetUpdateCommand();
            _MainDTAdapter.InsertCommand = cmdBuilder.GetInsertCommand();
            _MainDTAdapter.Update(dsMain.Tables["Trans_Details"]);
            */

            dsMain.Tables["Trans_Details"].Clear();
            _MainDTAdapter = DBClass.GetAdaptor("Trans_Details");
            _MainDTAdapter.Fill(dsMain.Tables["Trans_Details"]);

            dsForCombo.Tables["Customer_Master"].Clear();
            _CustomerAdapter = DBClass.GetAdapterByQuery("Select * from Customer_Master");
            _CustomerAdapter.Fill(dsForCombo.Tables["Customer_Master"]);

            FillCustomerDetail(txtCustomerCode.Text);
            FillRecord(dsMain.Tables["Trans_Master"].Rows.Count - 1);
            Enable_Disable_Controls(true);
            MessageBox.Show("Estimation Save Successfully!");
        }
        #endregion

        #region Transaction Events
        private void btn_New_Click(object sender, EventArgs e)
        {
            NewRecord();
        }
        private void btn_Edit_Click(object sender, EventArgs e)
        {
            Enable_Disable_Controls(false);
        }
        private void btn_Save_Click(object sender, EventArgs e)
        {
            SaveRecord();
        }
        private void btn_Delete_Click(object sender, EventArgs e)
        {
            DeleteRecord();
        }
        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            Enable_Disable_Controls(true);
        }
        private void btn_Clear_Click(object sender, EventArgs e)
        {
            ClearRecord();
        }
        private void btn_Close_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion

        #region Navigation Events
        private void btn_First_Click(object sender, EventArgs e)
        {
            FillRecord(0);
            IntRowIndex = 0;
        }
        private void btn_previous_Click(object sender, EventArgs e)
        {
            IntRowIndex--;
            if (IntRowIndex >= 0)
            {
                FillRecord(IntRowIndex);
            }
            else
            {
                FillRecord(0);
                IntRowIndex = 0;
            }
        }
        private void btn_next_Click(object sender, EventArgs e)
        {
            IntRowIndex++;
            if (IntRowIndex <= dsMain.Tables["Trans_Master"].Rows.Count - 1)
            {
                FillRecord(IntRowIndex);
            }
            else
            {
                FillRecord(dsMain.Tables["Trans_Master"].Rows.Count - 1);
                IntRowIndex = dsMain.Tables["Trans_Master"].Rows.Count - 1;
            }
        }
        private void btn_last_Click(object sender, EventArgs e)
        {
            FillRecord(dsMain.Tables["Trans_Master"].Rows.Count - 1);
            IntRowIndex = dsMain.Tables["Trans_Master"].Rows.Count - 1;
        }
        #endregion

        #region Grid Events
        private void Set_EstimationGrid()
        {
            dgvEstimateGrid.AutoGenerateColumns = false;

            DataGridViewTextBoxColumn Details_Id = new DataGridViewTextBoxColumn();
            Details_Id.Name = "Details_Id";
            Details_Id.HeaderText = "Details Id";
            Details_Id.DataPropertyName = "Details_Id";
            Details_Id.Width = 35;
            Details_Id.ReadOnly = true;
            Details_Id.Visible = false;
            dgvEstimateGrid.Columns.Add(Details_Id);

            DataGridViewTextBoxColumn Trans_Id = new DataGridViewTextBoxColumn();
            Trans_Id.Name = "Trans_Id";
            Trans_Id.HeaderText = "Trans Id";
            Trans_Id.DataPropertyName = "Trans_Id";
            Trans_Id.ReadOnly = true;
            Trans_Id.Width = 35;
            Trans_Id.Visible = false;
            dgvEstimateGrid.Columns.Add(Trans_Id);

            DataGridViewTextBoxColumn Item_Code = new DataGridViewTextBoxColumn();
            Item_Code.Name = "Item_Code";
            Item_Code.HeaderText = "No.";
            Item_Code.DataPropertyName = "Item_Code";
            Item_Code.Width = 35;
            Item_Code.ReadOnly = true;
            Item_Code.Visible = false;
            dgvEstimateGrid.Columns.Add(Item_Code);

            DataGridViewTextBoxColumn Item_PartNumber = new DataGridViewTextBoxColumn();
            Item_PartNumber.Name = "Item_PartNumber";
            Item_PartNumber.HeaderText = "Part Number";
            Item_PartNumber.DataPropertyName = "Item_PartNumber";
            Item_PartNumber.Width = 120;
            Item_PartNumber.ReadOnly = true;
            dgvEstimateGrid.Columns.Add(Item_PartNumber);

            DataGridViewTextBoxColumn Item_Name = new DataGridViewTextBoxColumn();
            Item_Name.Name = "Item_Name";
            Item_Name.HeaderText = "Item Details";
            Item_Name.DataPropertyName = "Item_Name";
            //Item_Name.Width = 135;
            Item_Name.ReadOnly = true;
            Item_Name.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvEstimateGrid.Columns.Add(Item_Name);

            DataGridViewTextBoxColumn Additional_Info = new DataGridViewTextBoxColumn();
            Additional_Info.Name = "Additional_Info";
            Additional_Info.HeaderText = "Add.Info";
            Additional_Info.DataPropertyName = "Additional_Info";
            Additional_Info.Width = 85;
            Additional_Info.ReadOnly = true;
            Additional_Info.Visible = false;
            dgvEstimateGrid.Columns.Add(Additional_Info);

            DataGridViewTextBoxColumn Item_Qty = new DataGridViewTextBoxColumn();
            Item_Qty.Name = "Item_Qty";
            Item_Qty.HeaderText = "Qty";
            Item_Qty.DataPropertyName = "Item_Qty";
            Item_Qty.Width = 60;
            Item_Qty.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            //Item_Qty.DefaultCellStyle.NullValue = "1";
            dgvEstimateGrid.Columns.Add(Item_Qty);

            DataGridViewTextBoxColumn SellingPrice = new DataGridViewTextBoxColumn();
            SellingPrice.Name = "SellingPrice";
            SellingPrice.HeaderText = "Amount P";
            SellingPrice.DataPropertyName = "SellingPrice";
            SellingPrice.ValueType = typeof(decimal);
            SellingPrice.Width = 100;
            SellingPrice.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            SellingPrice.ReadOnly = true;
            dgvEstimateGrid.Columns.Add(SellingPrice);

            DataGridViewTextBoxColumn ExclusiveAmount = new DataGridViewTextBoxColumn();
            ExclusiveAmount.Name = "ExclusiveAmount";
            ExclusiveAmount.HeaderText = "Exclusive P";
            ExclusiveAmount.DataPropertyName = "ExclusiveAmount";
            ExclusiveAmount.ValueType = typeof(decimal);
            ExclusiveAmount.Width = 100;
            ExclusiveAmount.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            ExclusiveAmount.ReadOnly = true;
            dgvEstimateGrid.Columns.Add(ExclusiveAmount);

            DataGridViewTextBoxColumn VAT_Per = new DataGridViewTextBoxColumn();
            VAT_Per.Name = "VAT_Per";
            VAT_Per.HeaderText = "VAT %";
            VAT_Per.DataPropertyName = "VAT_Per";
            VAT_Per.Width = 80;
            VAT_Per.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            VAT_Per.ReadOnly = true;
            dgvEstimateGrid.Columns.Add(VAT_Per);

            DataGridViewTextBoxColumn VATAmount = new DataGridViewTextBoxColumn();
            VATAmount.Name = "VATAmount";
            VATAmount.HeaderText = "VAT Amt";
            VATAmount.DataPropertyName = "VATAmount";
            VATAmount.ValueType = typeof(decimal);
            VATAmount.Width = 100;
            VATAmount.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            VATAmount.ReadOnly = true;
            dgvEstimateGrid.Columns.Add(VATAmount);

            DataGridViewTextBoxColumn InclusiveAmount = new DataGridViewTextBoxColumn();
            InclusiveAmount.Name = "InclusiveAmount";
            InclusiveAmount.HeaderText = "Inclusive P";
            InclusiveAmount.DataPropertyName = "InclusiveAmount";
            InclusiveAmount.ValueType = typeof(decimal);
            InclusiveAmount.Width = 100;
            InclusiveAmount.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            InclusiveAmount.ReadOnly = true;
            dgvEstimateGrid.Columns.Add(InclusiveAmount);

            DataGridViewTextBoxColumn MaxDisc_Per = new DataGridViewTextBoxColumn();
            MaxDisc_Per.Name = "MaxDisc_Per";
            MaxDisc_Per.HeaderText = "Max Disc %";
            MaxDisc_Per.DataPropertyName = "MaxDisc_Per";
            MaxDisc_Per.Width = 80;
            MaxDisc_Per.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            MaxDisc_Per.Visible = false;
            dgvEstimateGrid.Columns.Add(MaxDisc_Per);

            DataGridViewTextBoxColumn DISC_Per = new DataGridViewTextBoxColumn();
            DISC_Per.Name = "DISC_Per";
            DISC_Per.HeaderText = "DISC %";
            DISC_Per.DataPropertyName = "DISC_Per";
            DISC_Per.Width = 80;
            DISC_Per.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvEstimateGrid.Columns.Add(DISC_Per);

            DataGridViewTextBoxColumn DiscAmount = new DataGridViewTextBoxColumn();
            DiscAmount.Name = "DiscAmount";
            DiscAmount.HeaderText = "Disc Amt";
            DiscAmount.DataPropertyName = "DiscAmount";
            DiscAmount.ValueType = typeof(decimal);
            DiscAmount.Width = 100;
            DiscAmount.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            DiscAmount.ReadOnly = true;
            dgvEstimateGrid.Columns.Add(DiscAmount);

            DataGridViewTextBoxColumn NetAmount = new DataGridViewTextBoxColumn();
            NetAmount.Name = "NetAmount";
            NetAmount.HeaderText = "Net Amount";
            NetAmount.DataPropertyName = "NetAmount";
            NetAmount.ValueType = typeof(decimal);
            NetAmount.Width = 100;
            NetAmount.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            NetAmount.ReadOnly = true;
            dgvEstimateGrid.Columns.Add(NetAmount);

        }
        private void dgvEstimateGrid_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            //if (!dsMain.HasChanges()) return;
            try
            {

                if (dgvEstimateGrid.Columns[e.ColumnIndex].Name.ToUpper() == "ITEM_QTY" 
                    || dgvEstimateGrid.Columns[e.ColumnIndex].Name.ToUpper() == "SELLINGPRICE" 
                    || dgvEstimateGrid.Columns[e.ColumnIndex].Name.ToUpper() == "VAT_PER" 
                    || dgvEstimateGrid.Columns[e.ColumnIndex].Name.ToUpper() == "DISC_PER")
                {
                    decimal SellingPrice = Convert.ToDecimal((dgvEstimateGrid.CurrentRow.Cells["SELLINGPRICE"].Value is DBNull) ? 0 : dgvEstimateGrid.CurrentRow.Cells["SELLINGPRICE"].Value);
                    decimal DiscPer = Convert.ToDecimal((dgvEstimateGrid.CurrentRow.Cells["DISC_PER"].Value is DBNull) ? 0 : dgvEstimateGrid.CurrentRow.Cells["DISC_PER"].Value);
                    decimal MaxDiscPer = Convert.ToDecimal((dgvEstimateGrid.CurrentRow.Cells["MAXDISC_PER"].Value is DBNull) ? 0 : dgvEstimateGrid.CurrentRow.Cells["MAXDISC_PER"].Value);
                    decimal Qty = Convert.ToDecimal((dgvEstimateGrid.CurrentRow.Cells["Item_Qty"].Value is DBNull) ? 0 : dgvEstimateGrid.CurrentRow.Cells["Item_Qty"].Value);
                    decimal VatPer = Convert.ToDecimal((dgvEstimateGrid.CurrentRow.Cells["VAT_PER"].Value is DBNull) ? 0 : dgvEstimateGrid.CurrentRow.Cells["VAT_PER"].Value);
                    decimal ExclusiveAmount = Convert.ToDecimal((dgvEstimateGrid.CurrentRow.Cells["ExclusiveAmount"].Value is DBNull) ? 0 : dgvEstimateGrid.CurrentRow.Cells["ExclusiveAmount"].Value);
                    decimal InclusiveAmount = Convert.ToDecimal((dgvEstimateGrid.CurrentRow.Cells["InclusiveAmount"].Value is DBNull) ? 0 : dgvEstimateGrid.CurrentRow.Cells["InclusiveAmount"].Value);
                    decimal DiscAmt = Convert.ToDecimal((dgvEstimateGrid.CurrentRow.Cells["DiscAmount"].Value is DBNull) ? 0 : dgvEstimateGrid.CurrentRow.Cells["DiscAmount"].Value);

                    decimal VATAmt = 0, NetAmt = 0;


                    if (DiscPer > MaxDiscPer)
                    {
                        MessageBox.Show("Discount Limit is " + MaxDiscPer.ToString() + "%,Please Contact Your Manager.");
                        dgvEstimateGrid.CurrentRow.Cells["DISC_PER"].Value = 0;
                        return;
                    }

                    decimal TAmt = SellingPrice * Qty;
                    dgvEstimateGrid.CurrentRow.Cells["ExclusiveAmount"].Value = TAmt;

                    if (VatPer != 0)
                    {
                        VATAmt = (TAmt * VatPer) / 100;
                        dgvEstimateGrid.CurrentRow.Cells["VATAmount"].Value = VATAmt;
                    }

                    TAmt += VATAmt;
                    dgvEstimateGrid.CurrentRow.Cells["InclusiveAmount"].Value = TAmt;

                    if (dgvEstimateGrid.Columns[e.ColumnIndex].Name.ToUpper() == "DISCAMOUNT")
                    {
                        if (DiscAmt != 0)
                        {
                            //DiscAmt = (TAmt * DiscPer) / 100;
                            dgvEstimateGrid.CurrentRow.Cells["Disc_Per"].Value = Math.Round(((TAmt - DiscAmt) * 100) / DiscAmt, 2);
                            //dgvEstimateGrid.CurrentRow.Cells["DiscAmount"].Value = DiscAmt;
                        }
                    }
                    else
                    {
                        if (DiscPer != 0)
                        {
                            DiscAmt = (TAmt * DiscPer) / 100;
                            dgvEstimateGrid.CurrentRow.Cells["DiscAmount"].Value = DiscAmt;
                        }
                    }

                    NetAmt = TAmt - DiscAmt;
                    dgvEstimateGrid.CurrentRow.Cells["NetAmount"].Value = NetAmt;
                    CalculateTotal();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region txtCustomer event
        private void txtCustomerName_TextChanged(object sender, EventArgs e)
        {
            dsForCombo.Tables["Customer_Master"].DefaultView.RowFilter = "";
            dsForCombo.Tables["Customer_Master"].DefaultView.RowFilter = "Customer_Name like '%" + txtCustomerName.Text + "%'";

            DataTable DT = new DataTable();
            DT = dsForCombo.Tables["Customer_Master"].DefaultView.ToTable();
            if (DT.Rows.Count > 0)
            {
                txtCustomerCode.Text = DT.Rows[0]["Customer_Code"].ToString();
                txtAdders.Text = DT.Rows[0]["Customer_Address"].ToString();
                txtDelivery.Text = DT.Rows[0]["Customer_Delivery"].ToString();
                txtContactNo.Text = DT.Rows[0]["Customer_Phone"].ToString();
                txtWhatsapp.Text = DT.Rows[0]["Customer_Whatsapp"].ToString();
                txtIdentityNo.Text = DT.Rows[0]["Customer_IdentityNo"].ToString();
            }
        }
        private void FillCustomerDetail(string Code)
        {
            if (Code != "" && dsForCombo.Tables["Customer_Master"].Rows.Count > 0)
            {
                DataRow[] row = dsForCombo.Tables["Customer_Master"].Select("Customer_Code=" + Code);
                if (row.Length != 0)
                {
                    if (!row[0].IsNull("Customer_Name"))
                    {
                        txtCustomerName.Text = row[0]["Customer_Name"].ToString();
                        txtAdders.Text = row[0]["Customer_Address"].ToString();
                        txtDelivery.Text = row[0]["Customer_Delivery"].ToString();
                        txtContactNo.Text = row[0]["Customer_Phone"].ToString();
                        txtWhatsapp.Text = row[0]["Customer_Whatsapp"].ToString();
                        txtIdentityNo.Text = row[0]["Customer_IdentityNo"].ToString();
                    }
                }
            }
        }
        private void txtCustomerName_Validated(object sender, EventArgs e)
        {
            if (txtCustomerCode.Text == "" && txtCustomerName.Text != "")
            {
                int maxid = DBClass.GetIdByQuery("SELECT IDENT_CURRENT('Customer_Master')+1");
                txtCustomerCode.Text = maxid.ToString();

            }
            else if (txtCustomerCode.Text != "" && txtCustomerName.Text != "")
            {
                FillCustomerDetail(txtCustomerCode.Text);
            }
        }
        #endregion

        private void dgvEstimateGrid_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

        }

        private void dgvEstimateGrid_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            
        }
    }
}
