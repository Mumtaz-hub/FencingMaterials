using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
using System.IO;
using System.Runtime.InteropServices;
using System.Data.SqlClient;
using System.Globalization;

namespace FencingMaterials
{
    public partial class Report : Form
    {
        public Report()
        {
            InitializeComponent();
        }

        static DataTable dt = new DataTable();
        static DataSet ds = new DataSet();
        ReportClass ReportClassObj = new ReportClass();

        private void Report_Load(object sender, EventArgs e)
        {

        }
        private void txtFromBillDate_TextChanged(object sender, EventArgs e)
        {
            txtToBillDate.Text = txtFromBillDate.Text;
        }
        private void btnOk_Click(object sender, EventArgs e)
        {
            if (rbBillNowiseDetails.Checked)
            {
                if (rbDetails.Checked)
                {
                    //rptBillDetails("Bill_No");
                    ShowBillDetails("Bill_No");
                }
                else
                    ShowBillSummary("No");
            }

            if (rbBillDatewiseDetails.Checked)
            {
                if (rbDetails.Checked)
                    ShowDateWiseDetails("Bill_Date");
                else
                    ShowBillSummary("Date");
            }

            if (rbMonthWiseDetails.Checked)
            {
                if (rbDetails.Checked)
                    ShowMonthWiseDetails();
                else
                    ShowMonthWiseSummary();
                
            }

            if (rbItemDetails.Checked)
            {
                ShowItemsPriceList();
            }
        }

        #region BillNowise/BillDatewise  Details/Summary
        private void ShowBillDetails(string Columnwise)
        {
            DataTable dt = new DataTable();

            #region BindTable
            string SelectColumns = "";
            string TableName = "";
            string JoinTable = "";
            string GroupBy = "";
            string OrderBy = "";
            //string ReportName = "";

            switch (Columnwise.ToUpper())
            {
                case "BILL_NO":
                    SelectColumns = @" M.Bill_No as 'No',M.Bill_Date as 'Date',M.Bill_Type as 'Type',I.Item_Name,
                                      D.Qty,(D.UnitPrice*D.Qty) as 'TotalAmt',
                                      M.Discount as 'Disc',M.Delivery_Charge as 'Delivery',(D.UnitPrice*D.Qty)-M.Discount+M.Delivery_Charge as 'NetAmt'";
                    OrderBy = "Order by M.Bill_No,M.Bill_date,I.Item_Name";
                    //ReportName = "Bill wise Details";
                    Columnwise = "No";
                    break;
            }

            #region ""
            TableName = " Bill_Trans D";
            JoinTable = @"inner join Bill_Master M on M.Bill_No=D.Bill_No
                          inner join Item_Master I on I.Item_Code=D.Item_Code";

            GroupBy = "";

            string strBill_Type = "";
            if (rbDineIn.Checked)
                strBill_Type = "DineIn";
            else if (rbDelivery.Checked)
                strBill_Type = "Delivery";
            else if (rbPickup.Checked)
                strBill_Type = "Pickup";
            else
                strBill_Type = "A";

            string strBill_T = ""; ///for AC
            if (rbAC.Checked)
                strBill_T = "1";


            dt = ReportClassObj.ConstructSqlTable(SelectColumns, TableName, JoinTable,
                                    "M.Bill_No", (txtFromBillNo.Text != "") ? txtFromBillNo.Text.ToString() : "", (txtToBillNo.Text != "") ? txtToBillNo.Text.ToString() : "",
                                    "M.Bill_Date", (txtFromBillDate.Text != "") ? txtFromBillDate.Text : "", (txtToBillDate.Text != "") ? txtToBillDate.Text : "",
                                    "M.Bill_Type", strBill_Type, "M.Bill_T", strBill_T, GroupBy, OrderBy);
            dt.TableName = "Bill";
            #endregion
            #endregion

            #region Set Grid
            dgvGrid.DataSource = null;
            dgvGrid.Columns.Clear();

            dgvGrid.Columns.Add("No","No");
            dgvGrid.Columns.Add("Date", "Date");
            dgvGrid.Columns.Add("Type", "Type");
            dgvGrid.Columns.Add("Item_Name", "Item_Name");
            dgvGrid.Columns.Add("Qty", "Qty");
            dgvGrid.Columns.Add("TotalAmt", "TotalAmt");
            dgvGrid.Columns.Add("Disc", "Disc");
            dgvGrid.Columns.Add("Delivery", "Delivery");
            dgvGrid.Columns.Add("NetAmt", "NetAmt");

            dgvGrid.Columns["No"].Width = 30;
            dgvGrid.Columns["No"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgvGrid.Columns["Date"].Width = 100;
            dgvGrid.Columns["Date"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgvGrid.Columns["Type"].Width = 80;
            dgvGrid.Columns["Type"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            dgvGrid.Columns["Item_Name"].Width = 100;
            dgvGrid.Columns["Item_Name"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgvGrid.Columns["Item_Name"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvGrid.Columns["Item_Name"].HeaderText = "Item Name";

            dgvGrid.Columns["Qty"].Width = 40;
            dgvGrid.Columns["Qty"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            dgvGrid.Columns["TotalAmt"].Width = 80;
            dgvGrid.Columns["TotalAmt"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            dgvGrid.Columns["Disc"].Width = 60;
            dgvGrid.Columns["Disc"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            dgvGrid.Columns["Delivery"].Width = 80;
            dgvGrid.Columns["Delivery"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            
            dgvGrid.Columns["NetAmt"].Width = 80;
            dgvGrid.Columns["NetAmt"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            #endregion

            #region Print Details
            int TotalRows = dt.Rows.Count + 4;
            
            //////Column Value
            string strColumnwiseName = "";
            int IntRow = 0;
            int row = 0;
            double DiscAmt = 0;
            double DeliveryAmt = 0;
            double GDiscAmt = 0;
            double GDeliveryAmt = 0;

            //Print Details
            for (IntRow = 0; IntRow <= dt.Rows.Count - 1; IntRow++)
            {
                DataGridViewRow rw = new DataGridViewRow();
                rw.CreateCells(dgvGrid);

                if (strColumnwiseName != dt.Rows[IntRow][Columnwise].ToString())
                {
                    strColumnwiseName = dt.Rows[IntRow][Columnwise].ToString();
                    
                    rw.Cells[0].Value = strColumnwiseName;
                    
                    DiscAmt += 0;
                    DeliveryAmt += 0;

                    if (dt.Rows[IntRow]["Disc"].ToString() != "")
                        DiscAmt += Convert.ToDouble(dt.Rows[IntRow]["Disc"].ToString());
                    if (dt.Rows[IntRow]["Delivery"].ToString() != "")
                        DeliveryAmt += Convert.ToDouble(dt.Rows[IntRow]["Delivery"].ToString());
                }

                for (int Intcol = 0; Intcol <= dt.Columns.Count - 1; Intcol++)
                {
                    if (dt.Columns[Intcol].ColumnName.ToString().ToUpper() != Columnwise.ToUpper()
                        && dt.Columns[Intcol].ColumnName.ToString().ToUpper() != "DISC"
                        && dt.Columns[Intcol].ColumnName.ToString().ToUpper() != "DELIVERY"
                        && dt.Columns[Intcol].ColumnName.ToString().ToUpper() != "NETAMT")
                    {
                        rw.Cells[Intcol].Value = dt.Rows[IntRow][Intcol].ToString();
                    }
                }

                dgvGrid.Rows.Add(rw);

                #region Print Subtotal
                //Find Subtotal and Print

                bool Print = false;
                if (dt.Rows.Count > IntRow + 1 && strColumnwiseName != dt.Rows[IntRow + 1][Columnwise].ToString())
                {

                    row++;
                    Print = true;

                }
                else if (dt.Rows.Count <= IntRow + 1)
                {
                    row++; //  For Final Row subtotal
                    Print = true;
                }


                if (Print)
                {
                    DataGridViewRow rw1 = new DataGridViewRow();
                    rw1.CreateCells(dgvGrid);

                    rw1.Cells[dt.Columns["Item_Name"].Ordinal].Value = "SubTotal";

                    var intQty = (from t in dt.AsEnumerable()
                                  where t[Columnwise].ToString().Trim().ToUpper() == strColumnwiseName.ToUpper()
                                  select Convert.ToInt32(t["Qty"])).Sum();

                    rw1.Cells[dt.Columns["Qty"].Ordinal].Value = intQty.ToString();
                    
                    var dblTAmt = (from t in dt.AsEnumerable()
                                   where t[Columnwise].ToString().Trim().ToUpper() == strColumnwiseName.ToUpper()
                                   select Convert.ToInt32(t["TotalAmt"])).Sum();

                    rw1.Cells[dt.Columns["TotalAmt"].Ordinal].Value = dblTAmt.ToString();
                    rw1.Cells[dt.Columns["Disc"].Ordinal].Value = DiscAmt.ToString();
                    rw1.Cells[dt.Columns["Delivery"].Ordinal].Value = DeliveryAmt.ToString();

                    var dblNetAmt = (from t in dt.AsEnumerable()
                                     where t[Columnwise].ToString().Trim().ToUpper() == strColumnwiseName.ToUpper()
                                     select Convert.ToInt32(t["NetAmt"])).Sum();

                    rw1.Cells[dt.Columns["NetAmt"].Ordinal].Value = (dblTAmt - DiscAmt + DeliveryAmt).ToString();
                    dgvGrid.Rows.Add(rw1);

                    TotalRows++;

                    GDiscAmt += DiscAmt;
                    GDeliveryAmt += DeliveryAmt;
                    DiscAmt = 0; DeliveryAmt = 0;
                }
                #endregion

                row++;
            }

            #endregion

            #region GrandTotal
            //Find Grand Total and Print

            DataGridViewRow rw3 = new DataGridViewRow();
            rw3.CreateCells(dgvGrid);

            rw3.Cells[dt.Columns["Item_Name"].Ordinal].Value = "GrandTotal";
            
            double GTAmt = 0;
            object sumObject;
            sumObject = dt.Compute("Sum(Qty)", "");

            rw3.Cells[dt.Columns["Qty"].Ordinal].Value = sumObject.ToString();
            
            sumObject = dt.Compute("Sum(TotalAmt)", "");
            GTAmt = Convert.ToDouble(dt.Compute("Sum(TotalAmt)", ""));

            rw3.Cells[dt.Columns["TotalAmt"].Ordinal].Value = sumObject.ToString();
            rw3.Cells[dt.Columns["Disc"].Ordinal].Value = GDiscAmt.ToString();
            rw3.Cells[dt.Columns["Delivery"].Ordinal].Value = GDeliveryAmt.ToString();
            rw3.Cells[dt.Columns["NetAmt"].Ordinal].Value = (GTAmt - GDiscAmt + GDeliveryAmt).ToString();

            dgvGrid.Rows.Add(rw3);
            
            #endregion

            pnlGrid.Dock = DockStyle.Fill;
            pnlGrid.Visible = true;
           
        }
        private void rptBillDetails(string Columnwise)
        {
            
            #region BindTable
            string SelectColumns = "";
            string TableName = "";
            string JoinTable = "";
            string GroupBy = "";
            string OrderBy = "";
            string ReportName = "";

            switch (Columnwise.ToUpper())
            {
                case "BILL_NO":
                    SelectColumns = @" M.Bill_No as 'No',M.Bill_Date as 'Date',M.Bill_Type as 'Type',I.Item_Name,
                                      D.Qty,(D.UnitPrice*D.Qty) as 'TotalAmt',
                                      M.Discount as 'Disc',M.Delivery_Charge as 'Delivery',(D.UnitPrice*D.Qty)-M.Discount+M.Delivery_Charge as 'NetAmt'";
                    OrderBy = "Order by M.Bill_No,M.Bill_date,I.Item_Name";
                    ReportName = "Bill wise Details";
                    Columnwise = "No";
                    break;
            }

            TableName = " Bill_Trans D";
            JoinTable = @"inner join Bill_Master M on M.Bill_No=D.Bill_No
                          inner join Item_Master I on I.Item_Code=D.Item_Code";

            GroupBy = "";

            string strBill_Type = "";
            if (rbDineIn.Checked)
                strBill_Type = "DineIn";
            else if (rbDelivery.Checked)
                strBill_Type = "Delivery";
            else if (rbPickup.Checked)
                strBill_Type = "Pickup";
            else
                strBill_Type = "A";

            string strBill_T = ""; ///for AC
            if (rbAC.Checked)
                strBill_T = "1";


            dt = ReportClassObj.ConstructSqlTable(SelectColumns, TableName, JoinTable,
                                    "M.Bill_No", (txtFromBillNo.Text != "") ? txtFromBillNo.Text.ToString() : "", (txtToBillNo.Text != "") ? txtToBillNo.Text.ToString() : "",
                                    "M.Bill_Date", (txtFromBillDate.Text != "") ? txtFromBillDate.Text : "", (txtToBillDate.Text != "") ? txtToBillDate.Text : "",
                                    "M.Bill_Type", strBill_Type, "M.Bill_T", strBill_T, GroupBy, OrderBy);
            dt.TableName = "Bill";
            #endregion

            ReportClassObj.ExcelFileOpen("Commonreport");

            #region Print

            if (dt.Rows.Count > 0)
            {
                int TotalCols = dt.Columns.Count;
                string ColName = ReportClassObj.ColumnIndexToColumnLetter(TotalCols);

                #region "Print ReportName"
                //Set Report Name

                Excel.Range RngReportTitle;
                RngReportTitle = ReportClassObj.Worksheet.get_Range("A1", ColName + "1");
                ReportClassObj.SetReportName(RngReportTitle, "                 Laziz Pizza                                        " + System.DateTime.Now.ToShortDateString());

                Excel.Range RngReportName;
                RngReportName = ReportClassObj.Worksheet.get_Range("A2", ColName + "2");
                ReportClassObj.SetReportName(RngReportName, ReportName);
                #endregion

                #region "PrintHeader
                //Set Report Header
                Excel.Range RngColumnHeader;
                RngColumnHeader = ReportClassObj.Worksheet.get_Range("A3", ColName + "3");
                ReportClassObj.SetReportHeader(RngColumnHeader, dt);
                #endregion

                #region Print Details
                //Set Report Details
                int TotalRows = dt.Rows.Count + 4;
                Excel.Range RngColumnValue;
                RngColumnValue = ReportClassObj.Worksheet.get_Range("A4", ColName + TotalRows.ToString());

                //////Column Value
                string strColumnwiseName = "";
                int IntRow = 0;
                int row = 0;
                double DiscAmt = 0;
                double DeliveryAmt = 0;
                double GDiscAmt = 0;
                double GDeliveryAmt = 0;

                //Print Details
                for (IntRow = 0; IntRow <= dt.Rows.Count - 1; IntRow++)
                {
                    if (strColumnwiseName != dt.Rows[IntRow][Columnwise].ToString())
                    {
                        strColumnwiseName = dt.Rows[IntRow][Columnwise].ToString();
                        RngColumnValue.set_Item(row + 1, 1, strColumnwiseName);
                        DiscAmt += 0;
                        DeliveryAmt += 0;

                        if (dt.Rows[IntRow]["Disc"].ToString() != "")
                            DiscAmt += Convert.ToDouble(dt.Rows[IntRow]["Disc"].ToString());
                        if (dt.Rows[IntRow]["Delivery"].ToString() != "")
                            DeliveryAmt += Convert.ToDouble(dt.Rows[IntRow]["Delivery"].ToString());
                    }

                    for (int Intcol = 0; Intcol <= dt.Columns.Count - 1; Intcol++)
                    {
                        if (dt.Columns[Intcol].ColumnName.ToString().ToUpper() != Columnwise.ToUpper()
                            && dt.Columns[Intcol].ColumnName.ToString().ToUpper() != "DISC"
                            && dt.Columns[Intcol].ColumnName.ToString().ToUpper() != "DELIVERY"
                            && dt.Columns[Intcol].ColumnName.ToString().ToUpper() != "NETAMT")
                        {
                            RngColumnValue.set_Item(row + 1, Intcol + 1, dt.Rows[IntRow][Intcol].ToString());
                        }
                    }

                    #region Print Subtotal
                    //Find Subtotal and Print

                    bool Print = false;
                    if (dt.Rows.Count > IntRow + 1 && strColumnwiseName != dt.Rows[IntRow + 1][Columnwise].ToString())
                    {

                        row++;
                        Print = true;

                    }
                    else if (dt.Rows.Count <= IntRow + 1)
                    {
                        row++; //  For Final Row subtotal
                        Print = true;
                    }


                    if (Print)
                    {
                        RngColumnValue.set_Item(row + 1, dt.Columns["Item_Name"].Ordinal + 1, "SubTotal");

                        var intQty = (from t in dt.AsEnumerable()
                                      where t[Columnwise].ToString().Trim().ToUpper() == strColumnwiseName.ToUpper()
                                      select Convert.ToInt32(t["Qty"])).Sum();
                        RngColumnValue.set_Item(row + 1, dt.Columns["Qty"].Ordinal + 1, intQty.ToString());

                        var dblTAmt = (from t in dt.AsEnumerable()
                                       where t[Columnwise].ToString().Trim().ToUpper() == strColumnwiseName.ToUpper()
                                       select Convert.ToInt32(t["TotalAmt"])).Sum();
                        RngColumnValue.set_Item(row + 1, dt.Columns["TotalAmt"].Ordinal + 1, dblTAmt.ToString());

                        RngColumnValue.set_Item(row + 1, dt.Columns["Disc"].Ordinal + 1, DiscAmt.ToString());

                        RngColumnValue.set_Item(row + 1, dt.Columns["Delivery"].Ordinal + 1, DeliveryAmt.ToString());

                        var dblNetAmt = (from t in dt.AsEnumerable()
                                         where t[Columnwise].ToString().Trim().ToUpper() == strColumnwiseName.ToUpper()
                                         select Convert.ToInt32(t["NetAmt"])).Sum();

                        RngColumnValue.set_Item(row + 1, dt.Columns["NetAmt"].Ordinal + 1, (dblTAmt - DiscAmt + DeliveryAmt).ToString());

                        TotalRows++;

                        Excel.Range RngColSubTotal;
                        RngColSubTotal = ReportClassObj.Worksheet.get_Range("A" + (row + 4).ToString(), ReportClassObj.ColumnIndexToColumnLetter(TotalCols).ToString() + (row + 4).ToString());
                        RngColSubTotal.EntireRow.Font.Bold = true;

                        GDiscAmt += DiscAmt;
                        GDeliveryAmt += DeliveryAmt;
                        DiscAmt = 0; DeliveryAmt = 0;
                    }
                    #endregion

                    row++;
                }

                #endregion

                #region GrandTotal
                //Find Grand Total and Print

                RngColumnValue.set_Item(row + 1, dt.Columns["Item_Name"].Ordinal + 1, "Grand Total");

                double GTAmt = 0;
                object sumObject;
                sumObject = dt.Compute("Sum(Qty)", "");
                RngColumnValue.set_Item(row + 1, dt.Columns["Qty"].Ordinal + 1, sumObject.ToString());

                sumObject = dt.Compute("Sum(TotalAmt)", "");
                GTAmt = Convert.ToDouble(dt.Compute("Sum(TotalAmt)", ""));
                RngColumnValue.set_Item(row + 1, dt.Columns["TotalAmt"].Ordinal + 1, sumObject.ToString());

                //sumObject = dt.Compute("Sum(Disc)", "");
                RngColumnValue.set_Item(row + 1, dt.Columns["Disc"].Ordinal + 1, GDiscAmt);

                RngColumnValue.set_Item(row + 1, dt.Columns["Delivery"].Ordinal + 1, (GDeliveryAmt).ToString());
                RngColumnValue.set_Item(row + 1, dt.Columns["NetAmt"].Ordinal + 1, (GTAmt - GDiscAmt + GDeliveryAmt).ToString());

                RngColumnValue = ReportClassObj.Worksheet.get_Range("A3", ColName + (TotalRows - 1).ToString());

                ReportClassObj.ApplyStyleRowValue(RngColumnValue);

                Excel.Range RngColGrandTotal;
                RngColGrandTotal = ReportClassObj.Worksheet.get_Range("A" + (TotalRows).ToString(), ReportClassObj.ColumnIndexToColumnLetter(TotalCols).ToString() + (TotalRows).ToString());

                ReportClassObj.ApplyStyleGrandTotalValue(RngColGrandTotal);
                #endregion
            }

            #endregion

            ReportClassObj.SaveAndDisplayExcelFile();

        }
        private void ShowDateWiseDetails(string Columnwise)
        {
            //ReportClassObj.ExcelFileOpen("Commonreport");

            #region BindTable

            string SelectColumns = "";
            string TableName = "";
            string JoinTable = "";
            string GroupBy = "";
            string OrderBy = "";
            //string ReportName = "";

            switch (Columnwise.ToUpper())
            {
                case "BILL_NO":
                    SelectColumns = @" M.Bill_No as 'No',M.Bill_Date as 'Date',M.Bill_Type as 'Type',I.Item_Name,
                                      D.Qty,(D.UnitPrice*D.Qty) as 'TotalAmt',
                                      M.Discount as 'Disc',M.Delivery_Charge as 'Delivery',(D.UnitPrice*D.Qty)-M.Discount+M.Delivery_Charge as 'NetAmt'";
                    OrderBy = "Order by M.Bill_No,M.Bill_date,I.Item_Name";
                    //ReportName = "Bill wise Details";
                    Columnwise = "No";
                    break;
                case "BILL_DATE":
                    SelectColumns = @" M.Bill_Date as 'Date',M.Bill_No as 'No',M.Bill_Type as 'Type',I.Item_Name,
                                        D.Qty,(D.UnitPrice*D.Qty) as 'TotalAmt',
                                        M.Discount as 'Disc',M.Delivery_Charge as 'Delivery',(D.UnitPrice*D.Qty)-M.Discount+M.Delivery_Charge as 'NetAmt'";
                    OrderBy = "Order by M.Bill_date,M.Bill_No,I.Item_Name";
                    //ReportName = "Date wise Bill Details";
                    Columnwise = "Date";
                    break;
            }

            TableName = " Bill_Trans D";
            JoinTable = @"inner join Bill_Master M on M.Bill_No=D.Bill_No
                          inner join Item_Master I on I.Item_Code=D.Item_Code";

            GroupBy = "";

            string strBill_Type = "";
            if (rbDineIn.Checked)
                strBill_Type = "DineIn";
            else if (rbDelivery.Checked)
                strBill_Type = "Delivery";
            else if (rbPickup.Checked)
                strBill_Type = "Pickup";
            else
                strBill_Type = "A";

            string strBill_T = ""; ///for AC
            if (rbAC.Checked)
                strBill_T = "1";


            dt = ReportClassObj.ConstructSqlTable(SelectColumns, TableName, JoinTable,
                                    "M.Bill_No", (txtFromBillNo.Text != "") ? txtFromBillNo.Text.ToString() : "", (txtToBillNo.Text != "") ? txtToBillNo.Text.ToString() : "",
                                    "M.Bill_Date", (txtFromBillDate.Text != "") ? txtFromBillDate.Text : "", (txtToBillDate.Text != "") ? txtToBillDate.Text : "",
                                    "M.Bill_Type", strBill_Type, "M.Bill_T", strBill_T, GroupBy, OrderBy);
            dt.TableName = "Bill";
            #endregion

            #region Set Grid
            dgvGrid.DataSource = null;
            dgvGrid.Columns.Clear();

            dgvGrid.Columns.Add("Date", "Date");
            dgvGrid.Columns.Add("No","No");
            dgvGrid.Columns.Add("Type", "Type");
            dgvGrid.Columns.Add("Item_Name", "Item_Name");
            dgvGrid.Columns.Add("Qty", "Qty");
            dgvGrid.Columns.Add("TotalAmt", "TotalAmt");
            dgvGrid.Columns.Add("Disc", "Disc");
            dgvGrid.Columns.Add("Delivery", "Delivery");
            dgvGrid.Columns.Add("NetAmt", "NetAmt");

            dgvGrid.Columns["No"].Width = 30;
            dgvGrid.Columns["No"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgvGrid.Columns["Date"].Width = 100;
            dgvGrid.Columns["Date"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgvGrid.Columns["Type"].Width = 80;
            dgvGrid.Columns["Type"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            dgvGrid.Columns["Item_Name"].Width = 100;
            dgvGrid.Columns["Item_Name"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgvGrid.Columns["Item_Name"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvGrid.Columns["Item_Name"].HeaderText = "Item Name";

            dgvGrid.Columns["Qty"].Width = 40;
            dgvGrid.Columns["Qty"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            dgvGrid.Columns["TotalAmt"].Width = 80;
            dgvGrid.Columns["TotalAmt"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            dgvGrid.Columns["Disc"].Width = 60;
            dgvGrid.Columns["Disc"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            dgvGrid.Columns["Delivery"].Width = 80;
            dgvGrid.Columns["Delivery"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            
            dgvGrid.Columns["NetAmt"].Width = 80;
            dgvGrid.Columns["NetAmt"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            #endregion

            #region Print


            if (dt.Rows.Count > 0)
            {
                int TotalCols = dt.Columns.Count;
                string ColName = ReportClassObj.ColumnIndexToColumnLetter(TotalCols);

                #region Print Details
                //Set Report Details
                int TotalRows = dt.Rows.Count + 4;

                //Excel.Range //RngColumnValue;
                ////RngColumnValue = ReportClassObj.Worksheet.get_Range("A4", ColName + TotalRows.ToString());

                //////Column Value
                string strColumnwiseName = "";
                int IntRow = 0;
                int row = 0;
                double DiscAmt = 0;
                double DeliveryAmt = 0;
                double SDiscAmt = 0;
                double SDeliveryAmt = 0;
                double GDiscAmt = 0;
                double GDeliveryAmt = 0;
                string strBillNo = "";

                //Print Details
                for (IntRow = 0; IntRow <= dt.Rows.Count - 1; IntRow++)
                {
                    DataGridViewRow rw = new DataGridViewRow();
                    rw.CreateCells(dgvGrid);

                    if (strColumnwiseName != dt.Rows[IntRow][Columnwise].ToString())
                    {

                        strColumnwiseName = dt.Rows[IntRow][Columnwise].ToString();
                        rw.Cells[0].Value = strColumnwiseName;
                   //     //RngColumnValue.set_Item(row + 1, 1, strColumnwiseName);
                        SDiscAmt = 0;
                        SDeliveryAmt = 0;
                        DiscAmt = 0;
                        DeliveryAmt = 0;
                    }

                    for (int Intcol = 0; Intcol <= dt.Columns.Count - 1; Intcol++)
                    {
                        if (dt.Columns[Intcol].ColumnName.ToUpper() == "NO")
                        {
                            if (strBillNo != dt.Rows[IntRow]["No"].ToString())
                            {
                                strBillNo = dt.Rows[IntRow]["No"].ToString();

                                if (dt.Rows[IntRow]["Disc"].ToString() != "")
                                    DiscAmt = Convert.ToDouble(dt.Rows[IntRow]["Disc"].ToString());
                                if (dt.Rows[IntRow]["Delivery"].ToString() != "")
                                    DeliveryAmt = Convert.ToDouble(dt.Rows[IntRow]["Delivery"].ToString());

                                rw.Cells[dt.Columns["No"].Ordinal].Value = strBillNo;
                                rw.Cells[dt.Columns["Disc"].Ordinal].Value = DiscAmt.ToString();
                                rw.Cells[dt.Columns["Delivery"].Ordinal].Value = DeliveryAmt.ToString();

                                ////RngColumnValue.set_Item(row + 1, dt.Columns["No"].Ordinal + 1, strBillNo);
                                //RngColumnValue.set_Item(row + 1, dt.Columns["Disc"].Ordinal + 1, DiscAmt);
                                //RngColumnValue.set_Item(row + 1, dt.Columns["Delivery"].Ordinal + 1, DeliveryAmt);

                                SDiscAmt += DiscAmt;
                                SDeliveryAmt += DeliveryAmt;
                            }
                            else
                            {
                                //decimal val = 0;

                                rw.Cells[dt.Columns["Disc"].Ordinal].Value = DiscAmt.ToString();
                                rw.Cells[dt.Columns["Delivery"].Ordinal].Value = DeliveryAmt.ToString();

                                //RngColumnValue.set_Item(row + 1, dt.Columns["Disc"].Ordinal + 1, val);
                                //RngColumnValue.set_Item(row + 1, dt.Columns["Delivery"].Ordinal + 1, val);
                            }
                        }

                        if (dt.Columns[Intcol].ColumnName.ToUpper() == "DISC" || dt.Columns[Intcol].ColumnName.ToUpper() == "DELIVERY")
                        {
                            //decimal val = 0;
                            //if (strBillNo == dt.Rows[IntRow]["No"].ToString())
                            //{
                            //    //RngColumnValue.set_Item(row + 1, dt.Columns["Disc"].Ordinal + 1, val);
                            //    //RngColumnValue.set_Item(row + 1, dt.Columns["Delivery"].Ordinal + 1, val);
                            //}
                        }
                        if (dt.Columns[Intcol].ColumnName.ToString().ToUpper() != Columnwise.ToUpper()
                            && dt.Columns[Intcol].ColumnName.ToString().ToUpper() != "DISC"
                            && dt.Columns[Intcol].ColumnName.ToString().ToUpper() != "DELIVERY"
                            && dt.Columns[Intcol].ColumnName.ToString().ToUpper() != "NETAMT")
                        {
                            rw.Cells[Intcol].Value = dt.Rows[IntRow][Intcol].ToString();
                            //RngColumnValue.set_Item(row + 1, Intcol + 1, dt.Rows[IntRow][Intcol].ToString());

                        }
                    }

                    dgvGrid.Rows.Add(rw);

                    #region Print Subtotal
                    //Find Subtotal and Print

                    bool Print = false;
                    if (dt.Rows.Count > IntRow + 1 && strColumnwiseName != dt.Rows[IntRow + 1][Columnwise].ToString())
                    {

                        row++;
                        Print = true;

                    }
                    else if (dt.Rows.Count <= IntRow + 1)
                    {
                        row++; //  For Final Row subtotal
                        Print = true;
                    }


                    if (Print)
                    {
                        DataGridViewRow rw1 = new DataGridViewRow();
                        rw1.CreateCells(dgvGrid);

                        //rw1.Cells[dt.Columns["No"].Ordinal].Value = strBillNo;
                        rw1.Cells[dt.Columns["Item_Name"].Ordinal].Value = "SubTotal";

                        ////RngColumnValue.set_Item(row + 1, dt.Columns["No"].Ordinal + 1, strBillNo);
                        //RngColumnValue.set_Item(row + 1, dt.Columns["Item_Name"].Ordinal + 1, "SubTotal");

                        var intQty = (from t in dt.AsEnumerable()
                                      where t[Columnwise].ToString().Trim().ToUpper() == strColumnwiseName.ToUpper()
                                      select Convert.ToInt32(t["Qty"])).Sum();

                        rw1.Cells[dt.Columns["Qty"].Ordinal].Value = intQty.ToString();
                        //RngColumnValue.set_Item(row + 1, dt.Columns["Qty"].Ordinal + 1, intQty.ToString());

                        var dblTAmt = (from t in dt.AsEnumerable()
                                       where t[Columnwise].ToString().Trim().ToUpper() == strColumnwiseName.ToUpper()
                                       select Convert.ToInt32(t["TotalAmt"])).Sum();

                        rw1.Cells[dt.Columns["TotalAmt"].Ordinal].Value = dblTAmt.ToString();
                        rw1.Cells[dt.Columns["Disc"].Ordinal].Value = SDiscAmt.ToString();
                        rw1.Cells[dt.Columns["Delivery"].Ordinal].Value = SDeliveryAmt.ToString();

                        //RngColumnValue.set_Item(row + 1, dt.Columns["TotalAmt"].Ordinal + 1, dblTAmt.ToString());

                        //RngColumnValue.set_Item(row + 1, dt.Columns["Disc"].Ordinal + 1, SDiscAmt.ToString());

                        //RngColumnValue.set_Item(row + 1, dt.Columns["Delivery"].Ordinal + 1, SDeliveryAmt.ToString());

                        var dblNetAmt = (from t in dt.AsEnumerable()
                                         where t[Columnwise].ToString().Trim().ToUpper() == strColumnwiseName.ToUpper()
                                         select Convert.ToInt32(t["NetAmt"])).Sum();

                        rw1.Cells[dt.Columns["NetAmt"].Ordinal].Value = (dblTAmt - SDiscAmt + SDeliveryAmt).ToString();
                        //RngColumnValue.set_Item(row + 1, dt.Columns["NetAmt"].Ordinal + 1, (dblTAmt - SDiscAmt + SDeliveryAmt).ToString());
                        dgvGrid.Rows.Add(rw1);

                        TotalRows++;

                        /*Excel.Range RngColSubTotal;
                        RngColSubTotal = ReportClassObj.Worksheet.get_Range("A" + (row + 4).ToString(), ReportClassObj.ColumnIndexToColumnLetter(TotalCols).ToString() + (row + 4).ToString());
                        RngColSubTotal.EntireRow.Font.Bold = true;
                        */
                        
                        GDiscAmt += SDiscAmt;
                        GDeliveryAmt += SDeliveryAmt;
                        SDiscAmt = 0; SDeliveryAmt = 0;
                    }
                    #endregion
                    row++;
                }

                #endregion

                #region GrandTotal
                //Find Grand Total and Print

                DataGridViewRow rw2 = new DataGridViewRow();
                rw2.CreateCells(dgvGrid);

                rw2.Cells[dt.Columns["Item_Name"].Ordinal].Value = "Grand Total";

                //RngColumnValue.set_Item(row + 1, dt.Columns["Item_Name"].Ordinal + 1, "Grand Total");

                double GTAmt = 0;
                object sumObject;
                sumObject = dt.Compute("Sum(Qty)", "");
                rw2.Cells[dt.Columns["Qty"].Ordinal].Value = sumObject.ToString();
                //RngColumnValue.set_Item(row + 1, dt.Columns["Qty"].Ordinal + 1, sumObject.ToString());

                sumObject = dt.Compute("Sum(TotalAmt)", "");
                GTAmt = Convert.ToDouble(dt.Compute("Sum(TotalAmt)", ""));

                rw2.Cells[dt.Columns["TotalAmt"].Ordinal].Value = sumObject.ToString();
                //RngColumnValue.set_Item(row + 1, dt.Columns["TotalAmt"].Ordinal + 1, sumObject.ToString());

                //sumObject = dt.Compute("Sum(Disc)", "");
                rw2.Cells[dt.Columns["Disc"].Ordinal].Value = GDiscAmt.ToString();
                rw2.Cells[dt.Columns["Delivery"].Ordinal].Value = GDeliveryAmt.ToString();
                rw2.Cells[dt.Columns["NetAmt"].Ordinal].Value = (GTAmt - GDiscAmt + GDeliveryAmt).ToString();
                dgvGrid.Rows.Add(rw2);

                //RngColumnValue.set_Item(row + 1, dt.Columns["Disc"].Ordinal + 1, GDiscAmt);
                //RngColumnValue.set_Item(row + 1, dt.Columns["Delivery"].Ordinal + 1, (GDeliveryAmt).ToString());
                //RngColumnValue.set_Item(row + 1, dt.Columns["NetAmt"].Ordinal + 1, (GTAmt - GDiscAmt + GDeliveryAmt).ToString());

                //RngColumnValue = ReportClassObj.Worksheet.get_Range("A3", ColName + (TotalRows - 1).ToString());

                // ReportClassObj.ApplyStyleRowValue(//RngColumnValue);

                /*Excel.Range RngColGrandTotal;
                RngColGrandTotal = ReportClassObj.Worksheet.get_Range("A" + (TotalRows).ToString(), ReportClassObj.ColumnIndexToColumnLetter(TotalCols).ToString() + (TotalRows).ToString());

                ReportClassObj.ApplyStyleGrandTotalValue(RngColGrandTotal);*/

                pnlGrid.Dock = DockStyle.Fill;
                pnlGrid.Visible = true;
                #endregion
            }

            #endregion

            //ReportClassObj.SaveAndDisplayExcelFile();

        }
        private void rptDateWiseDetails(string Columnwise)
        {
            ReportClassObj.ExcelFileOpen("Commonreport");

            #region BindTable

            string SelectColumns = "";
            string TableName = "";
            string JoinTable = "";
            string GroupBy = "";
            string OrderBy = "";
            string ReportName = "";

            switch (Columnwise.ToUpper())
            {
                case "BILL_NO":
                    SelectColumns = @" M.Bill_No as 'No',M.Bill_Date as 'Date',M.Bill_Type as 'Type',I.Item_Name,
                                      D.Qty,(D.UnitPrice*D.Qty) as 'TotalAmt',
                                      M.Discount as 'Disc',M.Delivery_Charge as 'Delivery',(D.UnitPrice*D.Qty)-M.Discount+M.Delivery_Charge as 'NetAmt'";
                    OrderBy = "Order by M.Bill_No,M.Bill_date,I.Item_Name";
                    ReportName = "Bill wise Details";
                    Columnwise = "No";
                    break;
                case "BILL_DATE":
                    SelectColumns = @" M.Bill_Date as 'Date',M.Bill_No as 'No',M.Bill_Type as 'Type',I.Item_Name,
                                        D.Qty,(D.UnitPrice*D.Qty) as 'TotalAmt',
                                        M.Discount as 'Disc',M.Delivery_Charge as 'Delivery',(D.UnitPrice*D.Qty)-M.Discount+M.Delivery_Charge as 'NetAmt'";
                    OrderBy = "Order by M.Bill_date,M.Bill_No,I.Item_Name";
                    ReportName = "Date wise Bill Details";
                    Columnwise = "Date";
                    break;
            }

            TableName = " Bill_Trans D";
            JoinTable = @"inner join Bill_Master M on M.Bill_No=D.Bill_No
                          inner join Item_Master I on I.Item_Code=D.Item_Code";

            GroupBy = "";

            string strBill_Type = "";
            if (rbDineIn.Checked)
                strBill_Type = "DineIn";
            else if (rbDelivery.Checked)
                strBill_Type = "Delivery";
            else if (rbPickup.Checked)
                strBill_Type = "Pickup";
            else
                strBill_Type = "A";

            string strBill_T = ""; ///for AC
            if (rbAC.Checked)
                strBill_T = "1";


            dt = ReportClassObj.ConstructSqlTable(SelectColumns, TableName, JoinTable,
                                    "M.Bill_No", (txtFromBillNo.Text != "") ? txtFromBillNo.Text.ToString() : "", (txtToBillNo.Text != "") ? txtToBillNo.Text.ToString() : "",
                                    "M.Bill_Date", (txtFromBillDate.Text != "") ? txtFromBillDate.Text : "", (txtToBillDate.Text != "") ? txtToBillDate.Text : "",
                                    "M.Bill_Type", strBill_Type, "M.Bill_T", strBill_T, GroupBy, OrderBy);
            dt.TableName = "Bill";
            #endregion

            #region Print


            if (dt.Rows.Count > 0)
            {
                int TotalCols = dt.Columns.Count;
                string ColName = ReportClassObj.ColumnIndexToColumnLetter(TotalCols);

                #region "Print ReportName"

                Excel.Range RngReportTitle;
                RngReportTitle = ReportClassObj.Worksheet.get_Range("A1", ColName + "1");
                ReportClassObj.SetReportName(RngReportTitle, "                 Laziz Pizza                                        " + System.DateTime.Now.ToShortDateString());

                //Set Report Name
                Excel.Range RngReportName;
                RngReportName = ReportClassObj.Worksheet.get_Range("A2", ColName + "2");
                ReportClassObj.SetReportName(RngReportName, ReportName);
                #endregion

                #region "PrintHeader
                //Set Report Header
                Excel.Range RngColumnHeader;
                RngColumnHeader = ReportClassObj.Worksheet.get_Range("A3", ColName + "3");
                ReportClassObj.SetReportHeader(RngColumnHeader, dt);
                #endregion

                #region Print Details
                //Set Report Details
                int TotalRows = dt.Rows.Count + 4;
                Excel.Range RngColumnValue;
                RngColumnValue = ReportClassObj.Worksheet.get_Range("A4", ColName + TotalRows.ToString());

                //////Column Value
                string strColumnwiseName = "";
                int IntRow = 0;
                int row = 0;
                double DiscAmt = 0;
                double DeliveryAmt = 0;
                double SDiscAmt = 0;
                double SDeliveryAmt = 0;
                double GDiscAmt = 0;
                double GDeliveryAmt = 0;
                string strBillNo = "";

                //Print Details
                for (IntRow = 0; IntRow <= dt.Rows.Count - 1; IntRow++)
                {
                    if (strColumnwiseName != dt.Rows[IntRow][Columnwise].ToString())
                    {
                        strColumnwiseName = dt.Rows[IntRow][Columnwise].ToString();
                        RngColumnValue.set_Item(row + 1, 1, strColumnwiseName);
                        SDiscAmt = 0;
                        SDeliveryAmt = 0;
                        DiscAmt = 0;
                        DeliveryAmt = 0;
                    }

                    for (int Intcol = 0; Intcol <= dt.Columns.Count - 1; Intcol++)
                    {
                        if (dt.Columns[Intcol].ColumnName.ToUpper() == "NO")
                        {
                            if (strBillNo != dt.Rows[IntRow]["No"].ToString())
                            {
                                strBillNo = dt.Rows[IntRow]["No"].ToString();

                                if (dt.Rows[IntRow]["Disc"].ToString() != "")
                                    DiscAmt = Convert.ToDouble(dt.Rows[IntRow]["Disc"].ToString());
                                if (dt.Rows[IntRow]["Delivery"].ToString() != "")
                                    DeliveryAmt = Convert.ToDouble(dt.Rows[IntRow]["Delivery"].ToString());

                                RngColumnValue.set_Item(row + 1, dt.Columns["No"].Ordinal + 1, strBillNo);
                                RngColumnValue.set_Item(row + 1, dt.Columns["Disc"].Ordinal + 1, DiscAmt);
                                RngColumnValue.set_Item(row + 1, dt.Columns["Delivery"].Ordinal + 1, DeliveryAmt);

                                SDiscAmt += DiscAmt;
                                SDeliveryAmt += DeliveryAmt;
                            }
                            else
                            {
                                decimal val = 0;
                                RngColumnValue.set_Item(row + 1, dt.Columns["Disc"].Ordinal + 1, val);
                                RngColumnValue.set_Item(row + 1, dt.Columns["Delivery"].Ordinal + 1, val);
                            }
                        }

                        if (dt.Columns[Intcol].ColumnName.ToUpper() == "DISC" || dt.Columns[Intcol].ColumnName.ToUpper() == "DELIVERY")
                        {
                            //decimal val = 0;
                            //if (strBillNo == dt.Rows[IntRow]["No"].ToString())
                            //{
                            //    RngColumnValue.set_Item(row + 1, dt.Columns["Disc"].Ordinal + 1, val);
                            //    RngColumnValue.set_Item(row + 1, dt.Columns["Delivery"].Ordinal + 1, val);
                            //}
                        }
                        if (dt.Columns[Intcol].ColumnName.ToString().ToUpper() != Columnwise.ToUpper()
                            && dt.Columns[Intcol].ColumnName.ToString().ToUpper() != "DISC"
                            && dt.Columns[Intcol].ColumnName.ToString().ToUpper() != "DELIVERY"
                            && dt.Columns[Intcol].ColumnName.ToString().ToUpper() != "NETAMT")
                        {
                            RngColumnValue.set_Item(row + 1, Intcol + 1, dt.Rows[IntRow][Intcol].ToString());

                        }
                    }

                    #region Print Subtotal
                    //Find Subtotal and Print

                    bool Print = false;
                    if (dt.Rows.Count > IntRow + 1 && strColumnwiseName != dt.Rows[IntRow + 1][Columnwise].ToString())
                    {

                        row++;
                        Print = true;

                    }
                    else if (dt.Rows.Count <= IntRow + 1)
                    {
                        row++; //  For Final Row subtotal
                        Print = true;
                    }


                    if (Print)
                    {
                        //RngColumnValue.set_Item(row + 1, dt.Columns["No"].Ordinal + 1, strBillNo);
                        RngColumnValue.set_Item(row + 1, dt.Columns["Item_Name"].Ordinal + 1, "SubTotal");

                        var intQty = (from t in dt.AsEnumerable()
                                      where t[Columnwise].ToString().Trim().ToUpper() == strColumnwiseName.ToUpper()
                                      select Convert.ToInt32(t["Qty"])).Sum();
                        RngColumnValue.set_Item(row + 1, dt.Columns["Qty"].Ordinal + 1, intQty.ToString());

                        var dblTAmt = (from t in dt.AsEnumerable()
                                       where t[Columnwise].ToString().Trim().ToUpper() == strColumnwiseName.ToUpper()
                                       select Convert.ToInt32(t["TotalAmt"])).Sum();
                        RngColumnValue.set_Item(row + 1, dt.Columns["TotalAmt"].Ordinal + 1, dblTAmt.ToString());

                        RngColumnValue.set_Item(row + 1, dt.Columns["Disc"].Ordinal + 1, SDiscAmt.ToString());

                        RngColumnValue.set_Item(row + 1, dt.Columns["Delivery"].Ordinal + 1, SDeliveryAmt.ToString());

                        var dblNetAmt = (from t in dt.AsEnumerable()
                                         where t[Columnwise].ToString().Trim().ToUpper() == strColumnwiseName.ToUpper()
                                         select Convert.ToInt32(t["NetAmt"])).Sum();

                        RngColumnValue.set_Item(row + 1, dt.Columns["NetAmt"].Ordinal + 1, (dblTAmt - SDiscAmt + SDeliveryAmt).ToString());

                        TotalRows++;

                        Excel.Range RngColSubTotal;
                        RngColSubTotal = ReportClassObj.Worksheet.get_Range("A" + (row + 4).ToString(), ReportClassObj.ColumnIndexToColumnLetter(TotalCols).ToString() + (row + 4).ToString());
                        RngColSubTotal.EntireRow.Font.Bold = true;

                        GDiscAmt += SDiscAmt;
                        GDeliveryAmt += SDeliveryAmt;
                        SDiscAmt = 0; SDeliveryAmt = 0;
                    }
                    #endregion
                    row++;
                }

                #endregion

                #region GrandTotal
                //Find Grand Total and Print

                RngColumnValue.set_Item(row + 1, dt.Columns["Item_Name"].Ordinal + 1, "Grand Total");

                double GTAmt = 0;
                object sumObject;
                sumObject = dt.Compute("Sum(Qty)", "");
                RngColumnValue.set_Item(row + 1, dt.Columns["Qty"].Ordinal + 1, sumObject.ToString());

                sumObject = dt.Compute("Sum(TotalAmt)", "");
                GTAmt = Convert.ToDouble(dt.Compute("Sum(TotalAmt)", ""));
                RngColumnValue.set_Item(row + 1, dt.Columns["TotalAmt"].Ordinal + 1, sumObject.ToString());

                //sumObject = dt.Compute("Sum(Disc)", "");
                RngColumnValue.set_Item(row + 1, dt.Columns["Disc"].Ordinal + 1, GDiscAmt);

                RngColumnValue.set_Item(row + 1, dt.Columns["Delivery"].Ordinal + 1, (GDeliveryAmt).ToString());
                RngColumnValue.set_Item(row + 1, dt.Columns["NetAmt"].Ordinal + 1, (GTAmt - GDiscAmt + GDeliveryAmt).ToString());

                RngColumnValue = ReportClassObj.Worksheet.get_Range("A3", ColName + (TotalRows - 1).ToString());

                ReportClassObj.ApplyStyleRowValue(RngColumnValue);

                Excel.Range RngColGrandTotal;
                RngColGrandTotal = ReportClassObj.Worksheet.get_Range("A" + (TotalRows).ToString(), ReportClassObj.ColumnIndexToColumnLetter(TotalCols).ToString() + (TotalRows).ToString());

                ReportClassObj.ApplyStyleGrandTotalValue(RngColGrandTotal);
                #endregion
            }

            #endregion

            ReportClassObj.SaveAndDisplayExcelFile();

        }
        private void ShowBillSummary(string Columnwise)
        {
            //ReportClassObj.ExcelFileOpen("Commonreport");
            string SelectColumns = "";
            string TableName = "";
            string JoinTable = "";
            string GroupBy = "";
            string OrderBy = "";
            //string ReportName = "";

            #region BindTable
            switch (Columnwise.ToUpper())
            {
                case "NO":
                    SelectColumns = @"M.Bill_No as 'No',M.Bill_Date as 'Date',count(M.Bill_No) as 'Items',Sum(D.Qty) as 'Qty',sum(D.UnitPrice*D.Qty) as 'TotalAmt',
                            (select Discount from Bill_Master  where Bill_No=M.Bill_No) as 'Disc',
                            (select Delivery_Charge from Bill_Master  where Bill_No=M.Bill_No) as 'Delivery',
                            (sum(D.UnitPrice*D.Qty)-(select Discount from Bill_Master  where Bill_No=M.Bill_No)+
                            (select Delivery_Charge from Bill_Master  where Bill_No=M.Bill_No)) as 'NetAmt'";
                    GroupBy = "group by M.Bill_No,M.Bill_Date";
                    OrderBy = "Order by M.Bill_No,M.Bill_Date";
                    //ReportName = "Bill wise Summary Details";
                    break;

                case "DATE":
                    SelectColumns = @"M.Bill_Date as 'Date',M.Bill_No as 'No',count(M.Bill_No) as 'Items',Sum(D.Qty) as 'Qty',sum(D.UnitPrice*D.Qty) as 'TotalAmt',
                            (select Discount from Bill_Master  where Bill_No=M.Bill_No) as 'Disc',
                            (select Delivery_Charge from Bill_Master  where Bill_No=M.Bill_No) as 'Delivery',
                            (sum(D.UnitPrice*D.Qty)-(select Discount from Bill_Master  where Bill_No=M.Bill_No)+
                            (select Delivery_Charge from Bill_Master  where Bill_No=M.Bill_No)) as 'NetAmt'";
                    GroupBy = "group by M.Bill_No,M.Bill_Date";
                    OrderBy = "Order by M.Bill_No,M.Bill_Date";
                    //ReportName = "Date wise Summary Details";
                    break;
            }

            TableName = "Bill_Trans D";
            JoinTable = @"inner join Bill_Master M on M.Bill_No=D.Bill_No";

            string strBill_Type = "";
            if (rbDineIn.Checked)
                strBill_Type = "DineIn";
            else if (rbDelivery.Checked)
                strBill_Type = "Delivery";
            else if (rbPickup.Checked)
                strBill_Type = "Pickup";
            else
                strBill_Type = "A";

            string strBill_T = ""; ///for AC
            if (rbAC.Checked)
                strBill_T = "1";


            dt = ReportClassObj.ConstructSqlTable(SelectColumns, TableName, JoinTable,
                                    "M.Bill_No", (txtFromBillNo.Text != "") ? txtFromBillNo.Text.ToString() : "", (txtToBillNo.Text != "") ? txtToBillNo.Text.ToString() : "",
                                    "M.Bill_Date", (txtFromBillDate.Text != "") ? txtFromBillDate.Text : "", (txtToBillDate.Text != "") ? txtToBillDate.Text : "",
                                    "M.Bill_Type", strBill_Type, "M.Bill_T", strBill_T, GroupBy, OrderBy);
            dt.TableName = "Bill";
            #endregion

            #region Set Grid
            dgvGrid.DataSource = null;
            dgvGrid.Columns.Clear();

            if (Columnwise.ToUpper() == "NO")
            {
                dgvGrid.Columns.Add("No", "No");
                dgvGrid.Columns.Add("Date", "Date");
            }
            else if (Columnwise.ToUpper() == "DATE")
            {
                dgvGrid.Columns.Add("Date", "Date");
                dgvGrid.Columns.Add("No", "No");
            }

            dgvGrid.Columns.Add("Items", "Items");
            dgvGrid.Columns.Add("Qty", "Qty");
            dgvGrid.Columns.Add("TotalAmt", "TotalAmt");
            dgvGrid.Columns.Add("Disc", "Disc");
            dgvGrid.Columns.Add("Delivery", "Delivery");
            dgvGrid.Columns.Add("NetAmt", "NetAmt");

            dgvGrid.Columns["No"].Width = 30;
            dgvGrid.Columns["No"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgvGrid.Columns["Date"].Width = 100;
            dgvGrid.Columns["Date"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgvGrid.Columns["Items"].Width = 80;
            dgvGrid.Columns["Items"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            //dgvGrid.Columns["Items"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvGrid.Columns["Items"].HeaderText = "Items";

            dgvGrid.Columns["Qty"].Width = 40;
            dgvGrid.Columns["Qty"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            dgvGrid.Columns["TotalAmt"].Width = 80;
            dgvGrid.Columns["TotalAmt"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            dgvGrid.Columns["Disc"].Width = 60;
            dgvGrid.Columns["Disc"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            dgvGrid.Columns["Delivery"].Width = 80;
            dgvGrid.Columns["Delivery"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            dgvGrid.Columns["NetAmt"].Width = 80;
            dgvGrid.Columns["NetAmt"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            #endregion

            #region Print
            if (dt.Rows.Count > 0)
            {
                int TotalCols = dt.Columns.Count;
                
                #region Print Details
                //Set Column Value
                int TotalRows = dt.Rows.Count + 4;
                //Excel.Range //RngColumnValue;
                //RngColumnValue = ReportClassObj.Worksheet.get_Range("A4", LastColName + TotalRows.ToString());

                string strItem = "";
                int IntRow = 0;
                for (IntRow = 0; IntRow <= dt.Rows.Count - 1; IntRow++)
                {
                    DataGridViewRow rw = new DataGridViewRow();
                    rw.CreateCells(dgvGrid);

                    if (strItem != dt.Rows[IntRow][Columnwise].ToString())
                    {
                        strItem = dt.Rows[IntRow][Columnwise].ToString();

                        rw.Cells[0].Value = strItem;
                        //RngColumnValue.set_Item(IntRow + 1, 1, strItem);
                    }

                    for (int Intcol = 0; Intcol <= dt.Columns.Count - 1; Intcol++)
                    {
                        if (dt.Columns[Intcol].ColumnName.ToString().ToUpper() != Columnwise.ToUpper())
                        {
                            rw.Cells[Intcol].Value = dt.Rows[IntRow][Intcol].ToString();
                            //RngColumnValue.set_Item(IntRow + 1, Intcol + 1, dt.Rows[IntRow][Intcol].ToString());
                        }
                    }

                    dgvGrid.Rows.Add(rw);
                }
                //ReportClassObj.ApplyStyleRowValue(RngColumnValue);
                #endregion

                #region Print GrandTotal
                //Set GrandTotal

                DataGridViewRow rw2 = new DataGridViewRow();
                rw2.CreateCells(dgvGrid);

                object sumObject;
                sumObject = dt.Compute("Sum(Qty)", "");
                rw2.Cells[dt.Columns["Qty"].Ordinal].Value = sumObject.ToString();
                //RngColumnValue.set_Item(IntRow + 1, dt.Columns["Qty"].Ordinal + 1, sumObject.ToString());

                sumObject = dt.Compute("Sum(TotalAmt)", "");
                rw2.Cells[dt.Columns["TotalAmt"].Ordinal].Value = sumObject.ToString();
                //RngColumnValue.set_Item(IntRow + 1, dt.Columns["TotalAmt"].Ordinal + 1, sumObject.ToString());

                sumObject = dt.Compute("Sum(Disc)", "");
                rw2.Cells[dt.Columns["Disc"].Ordinal].Value = sumObject.ToString();
                //RngColumnValue.set_Item(IntRow + 1, dt.Columns["Disc"].Ordinal + 1, sumObject.ToString());

                sumObject = dt.Compute("Sum(Delivery)", "");
                rw2.Cells[dt.Columns["Delivery"].Ordinal].Value = sumObject.ToString();
                //RngColumnValue.set_Item(IntRow + 1, dt.Columns["Delivery"].Ordinal + 1, sumObject.ToString());

                sumObject = dt.Compute("Sum(NetAmt)", "");
                rw2.Cells[dt.Columns["NetAmt"].Ordinal].Value = sumObject.ToString();

                dgvGrid.Rows.Add(rw2);
                //RngColumnValue.set_Item(IntRow + 1, dt.Columns["NetAmt"].Ordinal + 1, sumObject.ToString());

                /*Excel.Range RngColGrandTotal;
                RngColGrandTotal = ReportClassObj.Worksheet.get_Range("A" + TotalRows, ReportClassObj.ColumnIndexToColumnLetter(TotalCols).ToString() + TotalRows);

                ReportClassObj.ApplyStyleGrandTotalValue(RngColGrandTotal);*/

                pnlGrid.Dock = DockStyle.Fill;
                pnlGrid.Visible = true;

                #endregion
            }
            
            //ReportClassObj.SaveAndDisplayExcelFile();
        #endregion
        }
        private void rptBillSummary(string Columnwise)
        {

            ReportClassObj.ExcelFileOpen("Commonreport");

            string SelectColumns = "";
            string TableName = "";
            string JoinTable = "";
            string GroupBy = "";
            string OrderBy = "";
            string ReportName = "";

            #region BindTable
            switch (Columnwise.ToUpper())
            {
                case "NO":
                    SelectColumns = @"M.Bill_No as 'No',M.Bill_Date as 'Date',count(M.Bill_No) as 'Items',Sum(D.Qty) as 'Qty',sum(D.UnitPrice*D.Qty) as 'TotalAmt',
                            (select Discount from Bill_Master  where Bill_No=M.Bill_No) as 'Disc',
                            (select Delivery_Charge from Bill_Master  where Bill_No=M.Bill_No) as 'Delivery',
                            (sum(D.UnitPrice*D.Qty)-(select Discount from Bill_Master  where Bill_No=M.Bill_No)+
                            (select Delivery_Charge from Bill_Master  where Bill_No=M.Bill_No)) as 'NetAmt'";
                    GroupBy = "group by M.Bill_No,M.Bill_Date";
                    OrderBy = "Order by M.Bill_No,M.Bill_Date";
                    ReportName = "Bill wise Summary Details";
                    break;

                case "DATE":
                    SelectColumns = @"M.Bill_Date as 'Date',M.Bill_No as 'No',count(M.Bill_No) as 'Items',Sum(D.Qty) as 'Qty',sum(D.UnitPrice*D.Qty) as 'TotalAmt',
                            (select Discount from Bill_Master  where Bill_No=M.Bill_No) as 'Disc',
                            (select Delivery_Charge from Bill_Master  where Bill_No=M.Bill_No) as 'Delivery',
                            (sum(D.UnitPrice*D.Qty)-(select Discount from Bill_Master  where Bill_No=M.Bill_No)+
                            (select Delivery_Charge from Bill_Master  where Bill_No=M.Bill_No)) as 'NetAmt'";
                    GroupBy = "group by M.Bill_No,M.Bill_Date";
                    OrderBy = "Order by M.Bill_No,M.Bill_Date";
                    ReportName = "Date wise Summary Details";
                    break;
            }

            TableName = "Bill_Trans D";
            JoinTable = @"inner join Bill_Master M on M.Bill_No=D.Bill_No";

            string strBill_Type = "";
            if (rbDineIn.Checked)
                strBill_Type = "DineIn";
            else if (rbDelivery.Checked)
                strBill_Type = "Delivery";
            else if (rbPickup.Checked)
                strBill_Type = "Pickup";
            else
                strBill_Type = "A";

            string strBill_T = ""; ///for AC
            if (rbAC.Checked)
                strBill_T = "1";


            dt = ReportClassObj.ConstructSqlTable(SelectColumns, TableName, JoinTable,
                                    "M.Bill_No", (txtFromBillNo.Text != "") ? txtFromBillNo.Text.ToString() : "", (txtToBillNo.Text != "") ? txtToBillNo.Text.ToString() : "",
                                    "M.Bill_Date", (txtFromBillDate.Text != "") ? txtFromBillDate.Text : "", (txtToBillDate.Text != "") ? txtToBillDate.Text : "",
                                    "M.Bill_Type", strBill_Type, "M.Bill_T", strBill_T, GroupBy, OrderBy);
            dt.TableName = "Bill";
            #endregion

            #region Print
            if (dt.Rows.Count > 0)
            {
                int TotalCols = dt.Columns.Count;

                #region Print ReportName

                //Set Report Name
                string LastColName = ReportClassObj.ColumnIndexToColumnLetter(TotalCols);

                Excel.Range RngReportTitle;
                RngReportTitle = ReportClassObj.Worksheet.get_Range("A1", LastColName + "1");
                ReportClassObj.SetReportName(RngReportTitle, "                 Laziz Pizza                                        " + System.DateTime.Now.ToShortDateString());

                Excel.Range RngReportName;
                RngReportName = ReportClassObj.Worksheet.get_Range("A2", LastColName + "2");
                ReportClassObj.SetReportName(RngReportName, ReportName);
                #endregion

                #region Print ReportHeader
                //Set Header
                Excel.Range RngColumnHeader;
                RngColumnHeader = ReportClassObj.Worksheet.get_Range("A3", LastColName + "3");
                ReportClassObj.SetReportHeader(RngColumnHeader, dt);
                #endregion

                #region Print Details
                //Set Column Value
                int TotalRows = dt.Rows.Count + 4;
                Excel.Range RngColumnValue;
                RngColumnValue = ReportClassObj.Worksheet.get_Range("A4", LastColName + TotalRows.ToString());

                string strItem = "";
                int IntRow = 0;
                for (IntRow = 0; IntRow <= dt.Rows.Count - 1; IntRow++)
                {
                    if (strItem != dt.Rows[IntRow][Columnwise].ToString())
                    {
                        strItem = dt.Rows[IntRow][Columnwise].ToString();
                        RngColumnValue.set_Item(IntRow + 1, 1, strItem);
                    }

                    for (int Intcol = 0; Intcol <= dt.Columns.Count - 1; Intcol++)
                    {
                        if (dt.Columns[Intcol].ColumnName.ToString().ToUpper() != Columnwise.ToUpper())
                        {
                            RngColumnValue.set_Item(IntRow + 1, Intcol + 1, dt.Rows[IntRow][Intcol].ToString());
                        }
                    }
                }

                ReportClassObj.ApplyStyleRowValue(RngColumnValue);
                #endregion

                #region Print GrandTotal
                //Set GrandTotal

                object sumObject;
                sumObject = dt.Compute("Sum(Qty)", "");
                RngColumnValue.set_Item(IntRow + 1, dt.Columns["Qty"].Ordinal + 1, sumObject.ToString());

                sumObject = dt.Compute("Sum(TotalAmt)", "");
                RngColumnValue.set_Item(IntRow + 1, dt.Columns["TotalAmt"].Ordinal + 1, sumObject.ToString());

                sumObject = dt.Compute("Sum(Disc)", "");
                RngColumnValue.set_Item(IntRow + 1, dt.Columns["Disc"].Ordinal + 1, sumObject.ToString());

                sumObject = dt.Compute("Sum(Delivery)", "");
                RngColumnValue.set_Item(IntRow + 1, dt.Columns["Delivery"].Ordinal + 1, sumObject.ToString());

                sumObject = dt.Compute("Sum(NetAmt)", "");
                RngColumnValue.set_Item(IntRow + 1, dt.Columns["NetAmt"].Ordinal + 1, sumObject.ToString());

                Excel.Range RngColGrandTotal;
                RngColGrandTotal = ReportClassObj.Worksheet.get_Range("A" + TotalRows, ReportClassObj.ColumnIndexToColumnLetter(TotalCols).ToString() + TotalRows);

                ReportClassObj.ApplyStyleGrandTotalValue(RngColGrandTotal);
                #endregion
            }
            #endregion

            ReportClassObj.SaveAndDisplayExcelFile();

        }
        #endregion

        #region Monthly Summary /Details
        private void ShowMonthWiseDetails()
        {
            //ReportClassObj.ExcelFileOpen("Commonreport");

            #region BindTable

            string SelectColumns = "";
            string TableName = "";
            string JoinTable = "";
            string GroupBy = "";
            string OrderBy = "";
            string ReportName = "";


            SelectColumns = @" MONTH(M.Bill_Date) as 'Month', day(M.Bill_Date) as 'Day',Sum(M.Amount) as 'TotalAmt',
                                Sum(M.Discount) as 'Disc',Sum(M.Delivery_Charge) as 'Delivery',
                                Sum(M.Amount-M.Discount+M.Delivery_Charge) as 'NetAmt'";
            OrderBy = " Order by month(M.Bill_Date), day(M.Bill_Date)";
            ReportName = "Monthly Day wise Total Counter Details";

            TableName = " Bill_Master M";
            JoinTable = "";

            GroupBy = " Group by month(M.Bill_Date), day(M.Bill_Date)";

            #region
            string strBill_Type = "";
            if (rbDineIn.Checked)
                strBill_Type = "DineIn";
            else if (rbDelivery.Checked)
                strBill_Type = "Delivery";
            else if (rbPickup.Checked)
                strBill_Type = "Pickup";
            else
                strBill_Type = "A";

            string strBill_T = ""; ///for AC
            if (rbAC.Checked)
                strBill_T = "1";


            dt = ReportClassObj.ConstructSqlTable(SelectColumns, TableName, JoinTable,
                                    "M.Bill_No", (txtFromBillNo.Text != "") ? txtFromBillNo.Text.ToString() : "", (txtToBillNo.Text != "") ? txtToBillNo.Text.ToString() : "",
                                    "M.Bill_Date", (txtFromBillDate.Text != "") ? txtFromBillDate.Text : "", (txtToBillDate.Text != "") ? txtToBillDate.Text : "",
                                    "M.Bill_Type", strBill_Type, "M.Bill_T", strBill_T, GroupBy, OrderBy);
            dt.TableName = "Bill";
            #endregion
            #endregion

            #region Set Grid
            dgvGrid.DataSource = null;
            dgvGrid.Columns.Clear();

            dgvGrid.Columns.Add("Month", "Month");
            dgvGrid.Columns.Add("Day", "Day");
            dgvGrid.Columns.Add("TotalAmt", "TotalAmt");
            dgvGrid.Columns.Add("Disc", "Disc");
            dgvGrid.Columns.Add("Delivery", "Delivery");
            dgvGrid.Columns.Add("NetAmt", "NetAmt");

            dgvGrid.Columns["Month"].Width = 120;
            dgvGrid.Columns["Month"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            dgvGrid.Columns["Day"].Width = 80;
            dgvGrid.Columns["Day"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgvGrid.Columns["TotalAmt"].Width = 120;
            dgvGrid.Columns["TotalAmt"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            dgvGrid.Columns["Disc"].Width = 80;
            dgvGrid.Columns["Disc"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            dgvGrid.Columns["Delivery"].Width = 80;
            dgvGrid.Columns["Delivery"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            dgvGrid.Columns["NetAmt"].Width = 120;
            dgvGrid.Columns["NetAmt"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            #endregion

            #region Print

            if (dt.Rows.Count > 0)
            {
                int TotalCols = dt.Columns.Count;
                string ColName = ReportClassObj.ColumnIndexToColumnLetter(TotalCols);

                #region Print Details
                //Set Column Value
                int TotalRows = dt.Rows.Count + 4;
                string LastColName = ReportClassObj.ColumnIndexToColumnLetter(TotalCols);
                //Excel.Range //RngColumnValue;
                //RngColumnValue = ReportClassObj.Worksheet.get_Range("A4", LastColName + (TotalRows + 1).ToString());

                string strItem = "";
                int IntRow = 0;
                int Row = 0;

                for (IntRow = 0; IntRow <= dt.Rows.Count - 1; IntRow++)
                {
                    DataGridViewRow rw = new DataGridViewRow();
                    rw.CreateCells(dgvGrid);
                    if (strItem != dt.Rows[IntRow]["Month"].ToString())
                    {
                        strItem = dt.Rows[IntRow]["Month"].ToString();
                        //RngColumnValue.set_Item(Row + 1, 1, CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(int.Parse(strItem)));
                        rw.Cells[0].Value = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(int.Parse(strItem));

                    }

                    for (int Intcol = 0; Intcol <= dt.Columns.Count - 1; Intcol++)
                    {
                        if (dt.Columns[Intcol].ColumnName.ToString().ToUpper() != "MONTH")
                        {
                            //RngColumnValue.set_Item(Row + 1, Intcol + 1, dt.Rows[IntRow][Intcol].ToString());
                            rw.Cells[Intcol].Value = dt.Rows[IntRow][Intcol].ToString();
                        }
                    }

                    dgvGrid.Rows.Add(rw);

                    #region Print Subtotal
                    //Find Subtotal and Print

                    bool Print = false;
                    if (dt.Rows.Count > IntRow + 1 && strItem != dt.Rows[IntRow + 1]["MONTH"].ToString())
                    {

                        Row++;
                        Print = true;

                    }
                    else if (dt.Rows.Count <= IntRow + 1)
                    {
                        Row++; //  For Final Row subtotal
                        Print = true;
                    }


                    if (Print)
                    {
                        DataGridViewRow rw1 = new DataGridViewRow();
                        rw1.CreateCells(dgvGrid);

                        rw1.Cells[dt.Columns["Day"].Ordinal].Value = "SubTotal";
                        ////RngColumnValue.set_Item(row + 1, dt.Columns["No"].Ordinal + 1, strBillNo);
                        //RngColumnValue.set_Item(Row + 1, dt.Columns["Month"].Ordinal + 1, "SubTotal");


                        var dblTAmt = (from t in dt.AsEnumerable()
                                       where t["Month"].ToString().Trim().ToUpper() == strItem.ToUpper()
                                       select Convert.ToInt32(t["TotalAmt"])).Sum();
                        rw1.Cells[dt.Columns["TotalAmt"].Ordinal].Value = dblTAmt.ToString();
                        //RngColumnValue.set_Item(Row + 1, dt.Columns["TotalAmt"].Ordinal + 1, dblTAmt.ToString());

                        var SDiscAmt = (from t in dt.AsEnumerable()
                                        where t["Month"].ToString().Trim().ToUpper() == strItem.ToUpper()
                                        select Convert.ToInt32(t["Disc"])).Sum();
                        rw1.Cells[dt.Columns["Disc"].Ordinal].Value = SDiscAmt.ToString();
                        //RngColumnValue.set_Item(Row + 1, dt.Columns["Disc"].Ordinal + 1, SDiscAmt.ToString());

                        var SDeliveryAmt = (from t in dt.AsEnumerable()
                                            where t["Month"].ToString().Trim().ToUpper() == strItem.ToUpper()
                                            select Convert.ToInt32(t["Delivery"])).Sum();
                        //RngColumnValue.set_Item(Row + 1, dt.Columns["Delivery"].Ordinal + 1, SDeliveryAmt.ToString());

                        var dblNetAmt = (from t in dt.AsEnumerable()
                                         where t["Month"].ToString().Trim().ToUpper() == strItem.ToUpper()
                                         select Convert.ToInt32(t["NetAmt"])).Sum();

                        rw1.Cells[dt.Columns["NetAmt"].Ordinal].Value = dblNetAmt.ToString();
                        //RngColumnValue.set_Item(Row + 1, dt.Columns["NetAmt"].Ordinal + 1, (dblNetAmt).ToString());

                        TotalRows++;

                        dgvGrid.Rows.Add(rw1);

                        /*Excel.Range RngColSubTotal;
                        RngColSubTotal = ReportClassObj.Worksheet.get_Range("A" + (Row + 4).ToString(), ReportClassObj.ColumnIndexToColumnLetter(TotalCols).ToString() + (Row + 4).ToString());
                        RngColSubTotal.EntireRow.Font.Bold = true;
                        */
                    }
                    #endregion
                    Row++;
                }
                //ReportClassObj.ApplyStyleRowValue(RngColumnValue);
                #endregion

                #region Print GrandTotal
                //Set GrandTotal

                object sumObject;

                DataGridViewRow rw2 = new DataGridViewRow();
                rw2.CreateCells(dgvGrid);

                rw2.Cells[dt.Columns["Day"].Ordinal].Value = "GrandTotal";

                sumObject = dt.Compute("Sum(TotalAmt)", "");
                rw2.Cells[dt.Columns["TotalAmt"].Ordinal].Value = sumObject.ToString();

                //RngColumnValue.set_Item(Row + 1, dt.Columns["TotalAmt"].Ordinal + 1, sumObject.ToString());

                sumObject = dt.Compute("Sum(Disc)", "");
                rw2.Cells[dt.Columns["Disc"].Ordinal].Value = sumObject.ToString();
                //RngColumnValue.set_Item(Row + 1, dt.Columns["Disc"].Ordinal + 1, sumObject.ToString());

                sumObject = dt.Compute("Sum(Delivery)", "");
                rw2.Cells[dt.Columns["Delivery"].Ordinal].Value = sumObject.ToString();
                //RngColumnValue.set_Item(Row + 1, dt.Columns["Delivery"].Ordinal + 1, sumObject.ToString());

                sumObject = dt.Compute("Sum(NetAmt)", "");
                rw2.Cells[dt.Columns["NetAmt"].Ordinal].Value = sumObject.ToString();
                //RngColumnValue.set_Item(Row + 1, dt.Columns["NetAmt"].Ordinal + 1, sumObject.ToString());

                dgvGrid.Rows.Add(rw2);
                /*Excel.Range RngColGrandTotal;
                RngColGrandTotal = ReportClassObj.Worksheet.get_Range("A" + TotalRows, ReportClassObj.ColumnIndexToColumnLetter(TotalCols).ToString() + TotalRows);

                ReportClassObj.ApplyStyleGrandTotalValue(RngColGrandTotal);*/
                #endregion
            }
            #endregion

            pnlGrid.Dock = DockStyle.Fill;
            pnlGrid.Visible = true;

            //ReportClassObj.SaveAndDisplayExcelFile();

        }
        private void rptMonthWiseDetails()
        {
            ReportClassObj.ExcelFileOpen("Commonreport");

            #region BindTable

            string SelectColumns = "";
            string TableName = "";
            string JoinTable = "";
            string GroupBy = "";
            string OrderBy = "";
            string ReportName = "";


            SelectColumns = @" MONTH(M.Bill_Date) as 'Month', day(M.Bill_Date) as 'Day',Sum(M.Amount) as 'TotalAmt',
                                Sum(M.Discount) as 'Disc',Sum(M.Delivery_Charge) as 'Delivery',
                                Sum(M.Amount-M.Discount+M.Delivery_Charge) as 'NetAmt'";
            OrderBy = " Order by month(M.Bill_Date), day(M.Bill_Date)";
            ReportName = "Monthly Day wise Total Counter Details";

            TableName = " Bill_Master M";
            JoinTable = "";

            GroupBy = " Group by month(M.Bill_Date), day(M.Bill_Date)";

            string strBill_Type = "";
            if (rbDineIn.Checked)
                strBill_Type = "DineIn";
            else if (rbDelivery.Checked)
                strBill_Type = "Delivery";
            else if (rbPickup.Checked)
                strBill_Type = "Pickup";
            else
                strBill_Type = "A";

            string strBill_T = ""; ///for AC
            if (rbAC.Checked)
                strBill_T = "1";


            dt = ReportClassObj.ConstructSqlTable(SelectColumns, TableName, JoinTable,
                                    "M.Bill_No", (txtFromBillNo.Text != "") ? txtFromBillNo.Text.ToString() : "", (txtToBillNo.Text != "") ? txtToBillNo.Text.ToString() : "",
                                    "M.Bill_Date", (txtFromBillDate.Text != "") ? txtFromBillDate.Text : "", (txtToBillDate.Text != "") ? txtToBillDate.Text : "",
                                    "M.Bill_Type", strBill_Type, "M.Bill_T", strBill_T, GroupBy, OrderBy);
            dt.TableName = "Bill";
            #endregion

            #region Print


            if (dt.Rows.Count > 0)
            {
                int TotalCols = dt.Columns.Count;
                string ColName = ReportClassObj.ColumnIndexToColumnLetter(TotalCols);

                #region "Print ReportName"

                Excel.Range RngReportTitle;
                RngReportTitle = ReportClassObj.Worksheet.get_Range("A1", ColName + "1");
                ReportClassObj.SetReportName(RngReportTitle, "                 Laziz Pizza                                " + System.DateTime.Now.ToShortDateString());

                //Set Report Name
                Excel.Range RngReportName;
                RngReportName = ReportClassObj.Worksheet.get_Range("A2", ColName + "2");
                ReportClassObj.SetReportName(RngReportName, ReportName);
                #endregion

                #region "PrintHeader
                //Set Report Header
                Excel.Range RngColumnHeader;
                RngColumnHeader = ReportClassObj.Worksheet.get_Range("A3", ColName + "3");
                ReportClassObj.SetReportHeader(RngColumnHeader, dt);
                #endregion

                #region Print Details
                //Set Column Value
                int TotalRows = dt.Rows.Count + 4;
                string LastColName = ReportClassObj.ColumnIndexToColumnLetter(TotalCols);
                Excel.Range RngColumnValue;
                RngColumnValue = ReportClassObj.Worksheet.get_Range("A4", LastColName + (TotalRows+1).ToString());

                string strItem = "";
                int IntRow = 0;
                int Row = 0;
                
                for (IntRow = 0; IntRow <= dt.Rows.Count - 1; IntRow++)
                {
                    if (strItem != dt.Rows[IntRow]["Month"].ToString())
                    {
                        strItem = dt.Rows[IntRow]["Month"].ToString();
                        RngColumnValue.set_Item(Row + 1, 1, CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(int.Parse(strItem)));
                        
                    }

                    for (int Intcol = 0; Intcol <= dt.Columns.Count - 1; Intcol++)
                    {
                        if (dt.Columns[Intcol].ColumnName.ToString().ToUpper() != "MONTH")
                        {
                            RngColumnValue.set_Item(Row + 1, Intcol + 1, dt.Rows[IntRow][Intcol].ToString());
                        }
                    }


                    #region Print Subtotal
                    //Find Subtotal and Print

                    bool Print = false;
                    if (dt.Rows.Count > IntRow + 1 && strItem != dt.Rows[IntRow + 1]["MONTH"].ToString())
                    {

                        Row++;
                        Print = true;

                    }
                    else if (dt.Rows.Count <= IntRow + 1)
                    {
                        Row++; //  For Final Row subtotal
                        Print = true;
                    }


                    if (Print)
                    {
                        //RngColumnValue.set_Item(row + 1, dt.Columns["No"].Ordinal + 1, strBillNo);
                        RngColumnValue.set_Item(Row + 1, dt.Columns["Month"].Ordinal + 1, "SubTotal");

                        
                        var dblTAmt = (from t in dt.AsEnumerable()
                                       where t["Month"].ToString().Trim().ToUpper() == strItem.ToUpper()
                                       select Convert.ToInt32(t["TotalAmt"])).Sum();
                        RngColumnValue.set_Item(Row + 1, dt.Columns["TotalAmt"].Ordinal + 1, dblTAmt.ToString());

                        var SDiscAmt = (from t in dt.AsEnumerable()
                                        where t["Month"].ToString().Trim().ToUpper() == strItem.ToUpper()
                                        select Convert.ToInt32(t["Disc"])).Sum();
                        RngColumnValue.set_Item(Row + 1, dt.Columns["Disc"].Ordinal + 1, SDiscAmt.ToString());

                        var SDeliveryAmt = (from t in dt.AsEnumerable()
                                            where t["Month"].ToString().Trim().ToUpper() == strItem.ToUpper()
                                            select Convert.ToInt32(t["Delivery"])).Sum();
                        RngColumnValue.set_Item(Row + 1, dt.Columns["Delivery"].Ordinal + 1, SDeliveryAmt.ToString());

                        var dblNetAmt = (from t in dt.AsEnumerable()
                                         where t["Month"].ToString().Trim().ToUpper() == strItem.ToUpper()
                                         select Convert.ToInt32(t["NetAmt"])).Sum();

                        RngColumnValue.set_Item(Row + 1, dt.Columns["NetAmt"].Ordinal + 1, (dblNetAmt).ToString());

                        TotalRows++;

                        Excel.Range RngColSubTotal;
                        RngColSubTotal = ReportClassObj.Worksheet.get_Range("A" + (Row + 4).ToString(), ReportClassObj.ColumnIndexToColumnLetter(TotalCols).ToString() + (Row + 4).ToString());
                        RngColSubTotal.EntireRow.Font.Bold = true;

                    }
                    #endregion
                    Row++;
                }
                ReportClassObj.ApplyStyleRowValue(RngColumnValue);
                #endregion

                #region Print GrandTotal
                //Set GrandTotal

                object sumObject;
                
                sumObject = dt.Compute("Sum(TotalAmt)", "");
                RngColumnValue.set_Item(Row + 1, dt.Columns["TotalAmt"].Ordinal + 1, sumObject.ToString());

                sumObject = dt.Compute("Sum(Disc)", "");
                RngColumnValue.set_Item(Row + 1, dt.Columns["Disc"].Ordinal + 1, sumObject.ToString());

                sumObject = dt.Compute("Sum(Delivery)", "");
                RngColumnValue.set_Item(Row + 1, dt.Columns["Delivery"].Ordinal + 1, sumObject.ToString());

                sumObject = dt.Compute("Sum(NetAmt)", "");
                RngColumnValue.set_Item(Row + 1, dt.Columns["NetAmt"].Ordinal + 1, sumObject.ToString());

                Excel.Range RngColGrandTotal;
                RngColGrandTotal = ReportClassObj.Worksheet.get_Range("A" + TotalRows, ReportClassObj.ColumnIndexToColumnLetter(TotalCols).ToString() + TotalRows);

                ReportClassObj.ApplyStyleGrandTotalValue(RngColGrandTotal);
                #endregion
            }
            

            #endregion

            ReportClassObj.SaveAndDisplayExcelFile();

        }

        private void ShowMonthWiseSummary()
        {
            //ReportClassObj.ExcelFileOpen("Commonreport");

            #region BindTable

            string SelectColumns = "";
            string TableName = "";
            string JoinTable = "";
            string GroupBy = "";
            string OrderBy = "";
            string ReportName = "";


            SelectColumns = @" MONTH(M.Bill_Date) as 'Month', Sum(M.Amount) as 'TotalAmt',
                                Sum(M.Discount) as 'Disc',Sum(M.Delivery_Charge) as 'Delivery',
                                Sum(M.Amount-M.Discount+M.Delivery_Charge) as 'NetAmt'";
            OrderBy = " Order by month(M.Bill_Date)";
            ReportName = "Monthly Day wise Total Counter Details";

            TableName = " Bill_Master M";
            JoinTable = "";

            GroupBy = " Group by month(M.Bill_Date)";

            string strBill_Type = "";
            if (rbDineIn.Checked)
                strBill_Type = "DineIn";
            else if (rbDelivery.Checked)
                strBill_Type = "Delivery";
            else if (rbPickup.Checked)
                strBill_Type = "Pickup";
            else
                strBill_Type = "A";

            string strBill_T = ""; ///for AC
            if (rbAC.Checked)
                strBill_T = "1";


            dt = ReportClassObj.ConstructSqlTable(SelectColumns, TableName, JoinTable,
                                    "M.Bill_No", (txtFromBillNo.Text != "") ? txtFromBillNo.Text.ToString() : "", (txtToBillNo.Text != "") ? txtToBillNo.Text.ToString() : "",
                                    "M.Bill_Date", (txtFromBillDate.Text != "") ? txtFromBillDate.Text : "", (txtToBillDate.Text != "") ? txtToBillDate.Text : "",
                                    "M.Bill_Type", strBill_Type, "M.Bill_T", strBill_T, GroupBy, OrderBy);
            dt.TableName = "Bill";
            #endregion

            #region Set Grid
            dgvGrid.DataSource = null;
            dgvGrid.Columns.Clear();

            dgvGrid.Columns.Add("Month", "Month");
            dgvGrid.Columns.Add("TotalAmt", "TotalAmt");
            dgvGrid.Columns.Add("Disc", "Disc");
            dgvGrid.Columns.Add("Delivery", "Delivery");
            dgvGrid.Columns.Add("NetAmt", "NetAmt");

            dgvGrid.Columns["Month"].Width = 120;
            dgvGrid.Columns["Month"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            dgvGrid.Columns["TotalAmt"].Width = 120;
            dgvGrid.Columns["TotalAmt"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            dgvGrid.Columns["Disc"].Width = 80;
            dgvGrid.Columns["Disc"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            dgvGrid.Columns["Delivery"].Width = 80;
            dgvGrid.Columns["Delivery"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            dgvGrid.Columns["NetAmt"].Width = 120;
            dgvGrid.Columns["NetAmt"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            #endregion

            #region Print


            if (dt.Rows.Count > 0)
            {
                int TotalCols = dt.Columns.Count;
                string ColName = ReportClassObj.ColumnIndexToColumnLetter(TotalCols);

                #region Print Details
                //Set Column Value
                int TotalRows = dt.Rows.Count + 4;
                string LastColName = ReportClassObj.ColumnIndexToColumnLetter(TotalCols);
                //Excel.Range //RngColumnValue;
                //RngColumnValue = ReportClassObj.Worksheet.get_Range("A4", LastColName + (TotalRows).ToString());

                string strItem = "";
                int IntRow = 0;
                int Row = 0;

                for (IntRow = 0; IntRow <= dt.Rows.Count - 1; IntRow++)
                {
                    DataGridViewRow rw = new DataGridViewRow();
                    rw.CreateCells(dgvGrid);

                    if (strItem != dt.Rows[IntRow]["Month"].ToString())
                    {
                        strItem = dt.Rows[IntRow]["Month"].ToString();
                        //RngColumnValue.set_Item(Row + 1, 1, CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(int.Parse(strItem)));
                        rw.Cells[0].Value = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(int.Parse(strItem));
                    }

                    for (int Intcol = 0; Intcol <= dt.Columns.Count - 1; Intcol++)
                    {
                        if (dt.Columns[Intcol].ColumnName.ToString().ToUpper() != "MONTH")
                        {
                            //RngColumnValue.set_Item(Row + 1, Intcol + 1, dt.Rows[IntRow][Intcol].ToString());
                            rw.Cells[Intcol].Value = dt.Rows[IntRow][Intcol].ToString();
                        }
                    }

                    dgvGrid.Rows.Add(rw);
                #endregion

                }
                //ReportClassObj.ApplyStyleRowValue(RngColumnValue);
            #endregion

                #region Print GrandTotal
                //Set GrandTotal

                object sumObject;

                DataGridViewRow rw1 = new DataGridViewRow();
                rw1.CreateCells(dgvGrid);


                sumObject = dt.Compute("Sum(TotalAmt)", "");
                rw1.Cells[dt.Columns["TotalAmt"].Ordinal].Value = sumObject.ToString();
                //RngColumnValue.set_Item(Row + 1, dt.Columns["TotalAmt"].Ordinal + 1, sumObject.ToString());

                sumObject = dt.Compute("Sum(Disc)", "");
                rw1.Cells[dt.Columns["Disc"].Ordinal].Value = sumObject.ToString();
                //RngColumnValue.set_Item(Row + 1, dt.Columns["Disc"].Ordinal + 1, sumObject.ToString());

                sumObject = dt.Compute("Sum(Delivery)", "");
                rw1.Cells[dt.Columns["Delivery"].Ordinal].Value = sumObject.ToString();
                //RngColumnValue.set_Item(Row + 1, dt.Columns["Delivery"].Ordinal + 1, sumObject.ToString());

                sumObject = dt.Compute("Sum(NetAmt)", "");
                rw1.Cells[dt.Columns["NetAmt"].Ordinal].Value = sumObject.ToString();
                //RngColumnValue.set_Item(Row + 1, dt.Columns["NetAmt"].Ordinal + 1, sumObject.ToString());

                dgvGrid.Rows.Add(rw1);
                /*Excel.Range RngColGrandTotal;
                RngColGrandTotal = ReportClassObj.Worksheet.get_Range("A" + TotalRows, ReportClassObj.ColumnIndexToColumnLetter(TotalCols).ToString() + TotalRows);

                ReportClassObj.ApplyStyleGrandTotalValue(RngColGrandTotal);
                 */
                #endregion
            }

            pnlGrid.Dock = DockStyle.Fill;
            pnlGrid.Visible = true;

            //ReportClassObj.SaveAndDisplayExcelFile();

        }
        private void rptMonthWiseSummary()
        {
            ReportClassObj.ExcelFileOpen("Commonreport");

            #region BindTable

            string SelectColumns = "";
            string TableName = "";
            string JoinTable = "";
            string GroupBy = "";
            string OrderBy = "";
            string ReportName = "";


            SelectColumns = @" MONTH(M.Bill_Date) as 'Month', Sum(M.Amount) as 'TotalAmt',
                                Sum(M.Discount) as 'Disc',Sum(M.Delivery_Charge) as 'Delivery',
                                Sum(M.Amount-M.Discount+M.Delivery_Charge) as 'NetAmt'";
            OrderBy = " Order by month(M.Bill_Date)";
            ReportName = "Monthly Day wise Total Counter Details";

            TableName = " Bill_Master M";
            JoinTable = "";

            GroupBy = " Group by month(M.Bill_Date)";

            string strBill_Type = "";
            if (rbDineIn.Checked)
                strBill_Type = "DineIn";
            else if (rbDelivery.Checked)
                strBill_Type = "Delivery";
            else if (rbPickup.Checked)
                strBill_Type = "Pickup";
            else
                strBill_Type = "A";

            string strBill_T = ""; ///for AC
            if (rbAC.Checked)
                strBill_T = "1";


            dt = ReportClassObj.ConstructSqlTable(SelectColumns, TableName, JoinTable,
                                    "M.Bill_No", (txtFromBillNo.Text != "") ? txtFromBillNo.Text.ToString() : "", (txtToBillNo.Text != "") ? txtToBillNo.Text.ToString() : "",
                                    "M.Bill_Date", (txtFromBillDate.Text != "") ? txtFromBillDate.Text : "", (txtToBillDate.Text != "") ? txtToBillDate.Text : "",
                                    "M.Bill_Type", strBill_Type, "M.Bill_T", strBill_T, GroupBy, OrderBy);
            dt.TableName = "Bill";
            #endregion

            #region Print


            if (dt.Rows.Count > 0)
            {
                int TotalCols = dt.Columns.Count;
                string ColName = ReportClassObj.ColumnIndexToColumnLetter(TotalCols);

                #region "Print ReportName"

                Excel.Range RngReportTitle;
                RngReportTitle = ReportClassObj.Worksheet.get_Range("A1", ColName + "1");
                ReportClassObj.SetReportName(RngReportTitle, "                 Laziz Pizza                                " + System.DateTime.Now.ToShortDateString());

                //Set Report Name
                Excel.Range RngReportName;
                RngReportName = ReportClassObj.Worksheet.get_Range("A2", ColName + "2");
                ReportClassObj.SetReportName(RngReportName, ReportName);
                #endregion

                #region "PrintHeader
                //Set Report Header
                Excel.Range RngColumnHeader;
                RngColumnHeader = ReportClassObj.Worksheet.get_Range("A3", ColName + "3");
                ReportClassObj.SetReportHeader(RngColumnHeader, dt);
                #endregion

                #region Print Details
                //Set Column Value
                int TotalRows = dt.Rows.Count + 4;
                string LastColName = ReportClassObj.ColumnIndexToColumnLetter(TotalCols);
                Excel.Range RngColumnValue;
                RngColumnValue = ReportClassObj.Worksheet.get_Range("A4", LastColName + (TotalRows).ToString());

                string strItem = "";
                int IntRow = 0;
                int Row = 0;

                for (IntRow = 0; IntRow <= dt.Rows.Count - 1; IntRow++)
                {
                    if (strItem != dt.Rows[IntRow]["Month"].ToString())
                    {
                        strItem = dt.Rows[IntRow]["Month"].ToString();
                        RngColumnValue.set_Item(IntRow + 1, 1, CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(int.Parse(strItem)));

                    }

                    for (int Intcol = 0; Intcol <= dt.Columns.Count - 1; Intcol++)
                    {
                        if (dt.Columns[Intcol].ColumnName.ToString().ToUpper() != "MONTH")
                        {
                            RngColumnValue.set_Item(IntRow + 1, Intcol + 1, dt.Rows[IntRow][Intcol].ToString());
                        }
                    }

                    #endregion
                    
                }
                ReportClassObj.ApplyStyleRowValue(RngColumnValue);
                #endregion

            #region Print GrandTotal
                //Set GrandTotal

                object sumObject;

                sumObject = dt.Compute("Sum(TotalAmt)", "");
                RngColumnValue.set_Item(IntRow + 1, dt.Columns["TotalAmt"].Ordinal + 1, sumObject.ToString());

                sumObject = dt.Compute("Sum(Disc)", "");
                RngColumnValue.set_Item(IntRow + 1, dt.Columns["Disc"].Ordinal + 1, sumObject.ToString());

                sumObject = dt.Compute("Sum(Delivery)", "");
                RngColumnValue.set_Item(IntRow + 1, dt.Columns["Delivery"].Ordinal + 1, sumObject.ToString());

                sumObject = dt.Compute("Sum(NetAmt)", "");
                RngColumnValue.set_Item(IntRow + 1, dt.Columns["NetAmt"].Ordinal + 1, sumObject.ToString());

                Excel.Range RngColGrandTotal;
                RngColGrandTotal = ReportClassObj.Worksheet.get_Range("A" + TotalRows, ReportClassObj.ColumnIndexToColumnLetter(TotalCols).ToString() + TotalRows);

                ReportClassObj.ApplyStyleGrandTotalValue(RngColGrandTotal);
                #endregion
            }

            ReportClassObj.SaveAndDisplayExcelFile();

        }
        #endregion
        
        #region ItemSettings
        private void ShowItemsPriceList()
        {

            string SelectColumns = "";
            string TableName = "";
            string JoinTable = "";
            string GroupBy = "";
            string OrderBy = "";
            string ReportName = "";

            #region BindTable

            SelectColumns = " C.Category_Name,I.Item_Code,I.Item_Number as 'Menu',I.Short_Code,I.Item_Name,I.UnitPrice";
            GroupBy = "";
            OrderBy = "Order by C.Category_Name,I.Item_Code,I.Item_Number,I.Item_Name";
            ReportName = "Items Price List";
            TableName = "Item_Master I";
            JoinTable = " inner join Category_Master C on C.Category_Id=I.Category_Id";

            dt = ReportClassObj.ConstructSqlTable(SelectColumns, TableName, JoinTable, "",
                                                    "", "", "", "", "", "", "", "", "", GroupBy, OrderBy);
            dt.TableName = "Item_Master";
            #endregion

            #region Set Grid
            dgvGrid.DataSource = null;
            dgvGrid.Columns.Clear();

            dgvGrid.Columns.Add("Category_Name", "Category_Name");
            dgvGrid.Columns.Add("Item_Code", "Item_Code");
            dgvGrid.Columns.Add("Menu", "Menu");
            dgvGrid.Columns.Add("Short_Code", "Short_Code");
            dgvGrid.Columns.Add("Item_Name", "Item_Name");
            dgvGrid.Columns.Add("UnitPrice", "UnitPrice");

            dgvGrid.Columns["Category_Name"].Width = 120;
            dgvGrid.Columns["Category_Name"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgvGrid.Columns["Category_Name"].HeaderText = "Category";

            dgvGrid.Columns["Item_Code"].Width = 120;
            dgvGrid.Columns["Item_Code"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgvGrid.Columns["Item_Code"].HeaderText = "Item Code";

            dgvGrid.Columns["Menu"].Width = 80;
            dgvGrid.Columns["Menu"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            dgvGrid.Columns["Short_Code"].Width = 80;
            dgvGrid.Columns["Short_Code"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgvGrid.Columns["Short_Code"].HeaderText = "Short Code";

            dgvGrid.Columns["Item_Name"].Width = 120;
            dgvGrid.Columns["Item_Name"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgvGrid.Columns["Item_Name"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvGrid.Columns["Item_Name"].HeaderText = "Item Name";
            
            dgvGrid.Columns["UnitPrice"].Width = 120;
            dgvGrid.Columns["UnitPrice"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            #endregion

            #region Print
            if (dt.Rows.Count > 0)
            {
                int TotalCols = dt.Columns.Count;

                #region Print Details
                //Set Column Value
                int TotalRows = dt.Rows.Count + 3;
                //Excel.Range //RngColumnValue;
                //RngColumnValue = ReportClassObj.Worksheet.get_Range("A4", LastColName + TotalRows.ToString());


                int IntRow = 0;
                for (IntRow = 0; IntRow <= dt.Rows.Count - 1; IntRow++)
                {
                    DataGridViewRow rw = new DataGridViewRow();
                    rw.CreateCells(dgvGrid);

                    for (int Intcol = 0; Intcol <= dt.Columns.Count - 1; Intcol++)
                    {
                        rw.Cells[Intcol].Value = dt.Rows[IntRow][Intcol].ToString();
                        //RngColumnValue.set_Item(IntRow + 1, Intcol + 1, dt.Rows[IntRow][Intcol].ToString());
                    }

                    dgvGrid.Rows.Add(rw);
                }
                //ReportClassObj.ApplyStyleRowValue(RngColumnValue);
                #endregion

                #region Print GrandTotal
                //Set GrandTotal
                DataGridViewRow rw1 = new DataGridViewRow();
                rw1.CreateCells(dgvGrid);

                object sumObject;
                sumObject = dt.Compute("Sum(UnitPrice)", "");
                rw1.Cells[dt.Columns["UnitPrice"].Ordinal].Value = sumObject.ToString();
                //RngColumnValue.set_Item(IntRow + 1, dt.Columns["UnitPrice"].Ordinal + 1, sumObject.ToString());

                dgvGrid.Rows.Add(rw1);
                /*Excel.Range RngColGrandTotal;
                RngColGrandTotal = ReportClassObj.Worksheet.get_Range("A" + TotalRows, ReportClassObj.ColumnIndexToColumnLetter(TotalCols).ToString() + TotalRows);

                ReportClassObj.ApplyStyleGrandTotalValue(RngColGrandTotal);
                 * */
                #endregion
            }
            #endregion

            pnlGrid.Dock = DockStyle.Fill;
            pnlGrid.Visible = true;
            //ReportClassObj.SaveAndDisplayExcelFile();

        }
        private void rptItemsPriceList()
        {

            ReportClassObj.ExcelFileOpen("Commonreport");
            string SelectColumns = "";
            string TableName = "";
            string JoinTable = "";
            string GroupBy = "";
            string OrderBy = "";
            string ReportName = "";

            #region BindTable

            SelectColumns = " C.Category_Name,I.Item_Code,I.Item_Number as 'Menu',I.Short_Code,I.Item_Name,I.UnitPrice";
            GroupBy = "";
            OrderBy = "Order by C.Category_Name,I.Item_Code,I.Item_Number,I.Item_Name";
            ReportName = "Items Price List";
            TableName = "Item_Master I";
            JoinTable = " inner join Category_Master C on C.Category_Id=I.Category_Id";
            
            dt = ReportClassObj.ConstructSqlTable(SelectColumns, TableName, JoinTable, "",
                                                    "", "", "", "", "", "", "", "","", GroupBy, OrderBy);
            dt.TableName = "Item_Master";
            #endregion

            #region Print
            if (dt.Rows.Count > 0)
            {
                int TotalCols = dt.Columns.Count;

                #region Print ReportName
                //Set Report Name
                string LastColName = ReportClassObj.ColumnIndexToColumnLetter(TotalCols);

                Excel.Range RngReportTitle;
                RngReportTitle = ReportClassObj.Worksheet.get_Range("A1", LastColName + "1");
                ReportClassObj.SetReportName(RngReportTitle, "                 Laziz Pizza                                        " + System.DateTime.Now.ToShortDateString());

                Excel.Range RngReportName;
                RngReportName = ReportClassObj.Worksheet.get_Range("A2", LastColName + "2");
                ReportClassObj.SetReportName(RngReportName, ReportName);
                #endregion

                #region Print ReportHeader
                //Set Header
                Excel.Range RngColumnHeader;
                RngColumnHeader = ReportClassObj.Worksheet.get_Range("A3", LastColName + "3");
                ReportClassObj.SetReportHeader(RngColumnHeader, dt);
                #endregion

                #region Print Details
                //Set Column Value
                int TotalRows = dt.Rows.Count + 3;
                Excel.Range RngColumnValue;
                RngColumnValue = ReportClassObj.Worksheet.get_Range("A4", LastColName + TotalRows.ToString());

                
                int IntRow = 0;
                for (IntRow = 0; IntRow <= dt.Rows.Count - 1; IntRow++)
                {
                    for (int Intcol = 0; Intcol <= dt.Columns.Count - 1; Intcol++)
                    {
                        RngColumnValue.set_Item(IntRow + 1, Intcol + 1, dt.Rows[IntRow][Intcol].ToString());
                    }
                }
                ReportClassObj.ApplyStyleRowValue(RngColumnValue);
                #endregion

                #region Print GrandTotal
                //Set GrandTotal

                object sumObject;
                sumObject = dt.Compute("Sum(UnitPrice)", "");
                RngColumnValue.set_Item(IntRow + 1, dt.Columns["UnitPrice"].Ordinal + 1, sumObject.ToString());

                Excel.Range RngColGrandTotal;
                RngColGrandTotal = ReportClassObj.Worksheet.get_Range("A" + TotalRows, ReportClassObj.ColumnIndexToColumnLetter(TotalCols).ToString() + TotalRows);

                ReportClassObj.ApplyStyleGrandTotalValue(RngColGrandTotal);
                #endregion
            }
            #endregion

            ReportClassObj.SaveAndDisplayExcelFile();

        }
        #endregion

        private void btnExportToExcel_Click(object sender, EventArgs e)
        {
            if (rbBillNowiseDetails.Checked)
            {
                if (rbDetails.Checked)
                {
                    rptBillDetails("Bill_No");
                }
                else
                    rptBillSummary("No");
            }

            if (rbBillDatewiseDetails.Checked)
            {
                if (rbDetails.Checked)
                    rptDateWiseDetails("Bill_Date");
                else
                    rptBillSummary("Date");
            }

            if (rbMonthWiseDetails.Checked)
            {
                if (rbDetails.Checked)
                    rptMonthWiseDetails();
                else
                    rptMonthWiseSummary();

            }

            if (rbItemDetails.Checked)
            {
                rptItemsPriceList();
            }
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            pnlGrid.Visible = false;
            dgvGrid.DataSource = null;
            dgvGrid.Rows.Clear();
            dgvGrid.Columns.Clear();
        }

        private void dgvGrid_KeyDown(object sender, KeyEventArgs e)
        {
            
        }

        private void Report_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }
        
    }
}