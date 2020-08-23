using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Excel = Microsoft.Office.Interop.Excel;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using DBLibrary;

namespace FencingMaterials
{
    public class ReportClass
    {
        public static Excel.Application ExcelApp;
        public Excel.Workbook Workbook;
        public Excel.Worksheet Worksheet;
        DataTable dt = new DataTable();

        public void ExcelFileOpen(string FileName)
        {
            ExcelApp = new Excel.Application();

            if (File.Exists(Application.StartupPath + @"\" + FileName + ".xls"))
            {
                File.Delete(Application.StartupPath + @"\" + FileName + ".xls");
            }

            Workbook = ExcelApp.Workbooks.Open(Application.StartupPath + @"\" + FileName + ".xlt", 0, true, 5, "", "", false, Excel.XlPlatform.xlWindows, "\t", true, false, 0, true, Type.Missing, Type.Missing);

            Worksheet = (Excel.Worksheet)Workbook.Sheets[1];
        }

        public void SetReportName(Excel.Range RngReportName, string Header = "")
        {
            RngReportName.Merge(false);
            RngReportName.Value2 = Header;
            RngReportName.EntireRow.Font.Size = 12;
            RngReportName.EntireRow.Font.Bold = true;
            //chartRange.FormulaR1C1 = "Your Heading Here";
            RngReportName.HorizontalAlignment = 3;
            RngReportName.VerticalAlignment = 3;
            RngReportName.BorderAround(Excel.XlLineStyle.xlContinuous, Excel.XlBorderWeight.xlThin, Excel.XlColorIndex.xlColorIndexAutomatic, Excel.XlColorIndex.xlColorIndexAutomatic);
        }
        public void SetReportHeader(Excel.Range RngColumnHeader, DataTable dtTemp)
        {
            /////Column Header
            for (int Intcol = 0; Intcol <= dtTemp.Columns.Count - 1; Intcol++)
            {
                RngColumnHeader.set_Item(1, Intcol + 1, dtTemp.Columns[Intcol].ColumnName);

                string DataType = dtTemp.Columns[Intcol].DataType.Name.ToString();

                string Formatedcolname = ColumnIndexToColumnLetter(Intcol + 1);
                Excel.Range RngColFormat;
                RngColFormat = Worksheet.get_Range(Formatedcolname + "3", Formatedcolname + dtTemp.Rows.Count + 3);

                switch (DataType.ToUpper())
                {
                    case "DECIMAL":
                    case "Double":
                        RngColFormat.NumberFormat = "#,###,###.00";
                        break;

                    case "DATETIME":
                        RngColFormat.NumberFormat = "dd/mm/yyyy";
                        break;

                    case "INT16":
                    case "INT32":
                    case "INT64":
                        RngColFormat.NumberFormat = "###";
                        break;
                }

            }

            RngColumnHeader.EntireRow.Font.Size = 10;
            RngColumnHeader.EntireRow.Font.Bold = true;
            RngColumnHeader.HorizontalAlignment = 3;
            RngColumnHeader.VerticalAlignment = 3;
            RngColumnHeader.BorderAround(Excel.XlLineStyle.xlContinuous, Excel.XlBorderWeight.xlThin, Excel.XlColorIndex.xlColorIndexAutomatic, Excel.XlColorIndex.xlColorIndexAutomatic);
            RngColumnHeader.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
            RngColumnHeader.Borders.Weight = 2d;
            RngColumnHeader.EntireColumn.AutoFit();

        }
        public void ApplyStyleRowValue(Excel.Range RngColumnValue)
        {
            //RngColumnValue.EntireRow.Font.Size = 10;
            //RngColumnValue.EntireRow.Font.Bold = false;
            RngColumnValue.HorizontalAlignment = 3;
            RngColumnValue.VerticalAlignment = 3;
            RngColumnValue.BorderAround(Excel.XlLineStyle.xlContinuous, Excel.XlBorderWeight.xlThin, Excel.XlColorIndex.xlColorIndexAutomatic, Excel.XlColorIndex.xlColorIndexAutomatic);
            RngColumnValue.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
            RngColumnValue.Borders.Weight = 2d;
            RngColumnValue.EntireColumn.AutoFit();
        }
        public void ApplyStyleGrandTotalValue(Excel.Range RngColGrandTotal)
        {
            RngColGrandTotal.EntireRow.Font.Size = 12;
            RngColGrandTotal.EntireRow.Font.Bold = true;
            RngColGrandTotal.HorizontalAlignment = 3;
            RngColGrandTotal.VerticalAlignment = 3;
            RngColGrandTotal.BorderAround(Excel.XlLineStyle.xlContinuous, Excel.XlBorderWeight.xlThin, Excel.XlColorIndex.xlColorIndexAutomatic, Excel.XlColorIndex.xlColorIndexAutomatic);
            RngColGrandTotal.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
            RngColGrandTotal.Borders.Weight = 2d;
            RngColGrandTotal.EntireColumn.AutoFit();
        }
        public void SaveAndDisplayExcelFile()
        {
            ExcelApp.ActiveWorkbook.SaveAs(Application.StartupPath + @"\Commonreport", Excel.XlFileFormat.xlWorkbookNormal, Type.Missing, Type.Missing,
                                      Type.Missing, Type.Missing, Excel.XlSaveAsAccessMode.xlNoChange, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);

            GC.Collect();
            GC.WaitForPendingFinalizers();

            Marshal.FinalReleaseComObject(Worksheet);
            ExcelApp.Workbooks.Close();
            Marshal.FinalReleaseComObject(Workbook);
            ExcelApp.Application.Quit();
            Marshal.FinalReleaseComObject(ExcelApp);
            ExcelApp = null;

            Excel.Application xlApp = new Excel.Application();  // create new Excel application
            xlApp.Visible = true;                               // application becomes visible
            xlApp.Workbooks.Open(Application.StartupPath + @"\Commonreport.xls", 0, true, 5, "", "", true, Excel.XlPlatform.xlWindows, "\t", true, false, 0, true, Type.Missing, Type.Missing);

            MessageBox.Show("Press Enter To Continue.......", "", MessageBoxButtons.OK);

            GC.Collect();
            GC.WaitForPendingFinalizers();

            xlApp.Workbooks.Close();
            xlApp.Application.Quit();
            Marshal.FinalReleaseComObject(xlApp);
            xlApp = null;
        }

        public DataTable ConstructSqlTable(string SelectColumns, string TableName, string JoinTables = "",
                                            string BillNoFieldName = "", string BillNoFromFieldValue = "", string BillNoToFieldValue = "",
                                            string BillDateFieldName = "", string BillFromDateFieldValue = "", string BillToDateFieldValue = "",
                                            string BillTypeFieldName = "", string BillTypeFieldValue = "", string Bill_TFieldName = "", string Bill_TFieldValue = "",
                                            string GroupBy = "", string OrderBy = "")
        {
            string strSql;
            strSql = @"select " + SelectColumns + " From " + TableName;
            if (JoinTables != "")
            {
                strSql = strSql + " " + JoinTables;
            }

            strSql = strSql + " Where 1=1";

            string strCriteria = "";
            
            if (BillNoFieldName != "" && BillNoFromFieldValue != "")
            {
                strCriteria = strCriteria + " And " + BillNoFieldName + ">='" + BillNoFromFieldValue + "'";
            }
            if (BillNoFieldName != "" && BillNoToFieldValue != "")
            {
                strCriteria = strCriteria + " And " + BillNoFieldName + "<='" + BillNoToFieldValue + "'";
            }
            if (BillDateFieldName != "" && BillFromDateFieldValue != "")
            {
                strCriteria = strCriteria + " And " + BillDateFieldName + ">='" + BillFromDateFieldValue + "'";
            }
            if (BillDateFieldName != "" && BillToDateFieldValue != "")
            {
                strCriteria = strCriteria + " And " + BillDateFieldName + "<='" + BillToDateFieldValue + "'";
            }
            if (BillTypeFieldName != "" && BillTypeFieldValue != "" && BillTypeFieldValue != "A")
            {
                strCriteria = strCriteria + " And " + BillTypeFieldName + "='" + BillTypeFieldValue + "'";
            }

            if (Bill_TFieldName != "" && Bill_TFieldValue != "" && Bill_TFieldValue != "A")
            {
                strCriteria = strCriteria + " And " + Bill_TFieldName + "=1";
            }
            
            strSql = strSql + strCriteria;

            if (GroupBy != "")
            {
                strSql = strSql + " " + GroupBy;
            }
            if (OrderBy != "")
            {
                strSql = strSql + " " + OrderBy;
            }


            dt = DBClass.GetTableByQuery(strSql);
            return dt;
        }

        #region "ColumnNoToLetter/LetterToColumnNo"
        public string ColumnIndexToColumnLetter(int colIndex)
        {
            int div = colIndex;
            string colLetter = String.Empty;
            int mod = 0;

            while (div > 0)
            {
                mod = (div - 1) % 26;
                colLetter = (char)(65 + mod) + colLetter;
                div = (int)((div - mod) / 26);
            }
            return colLetter;
        }
        public int ColumnLetterToColumnIndex(string columnLetter)
        {
            columnLetter = columnLetter.ToUpper();
            int sum = 0;

            for (int i = 0; i < columnLetter.Length; i++)
            {
                sum *= 26;
                sum += (columnLetter[i] - 'A' + 1);
            }
            return sum;
        }
        #endregion
    }
}