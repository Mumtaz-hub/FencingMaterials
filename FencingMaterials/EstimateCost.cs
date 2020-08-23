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
using System.Text.RegularExpressions;

namespace FencingMaterials
{
    public partial class EstimateCost : Form
    {
        public EstimateCost()
        {
            InitializeComponent();
        }

        DataSet dsMain = new DataSet();
        DataSet ds = new DataSet();
        SqlDataAdapter _MainAdapter;
        SqlDataAdapter adpItems;
        DataTable dt;
        public Base mainForm;

        private void EstimateCost_Load(object sender, EventArgs e)
        {
            FillDaimondMeshItems();
            FillGate();
            FillPostCorner();
            FillStaySupported();
            FillStandardY();
            FillDropper();
            Set_Grid();
            Set_EstimationGrid();
            FillWire();
            FillBoltAndNut();

            cmbDaimondMeshItemSize.Text = "";
            cmbstandardy.Text = "";
            cmbStaySupported.Text = "";
            CmbPostCorner.Text = "";
            cmbBoltAndNut.Text="";

            SetDefaultValueForSecurity();

            txtPlotHeight.Text = "";
            txtPlotWidth.Text = "";
            txtTotalArea.Text = "";
            txtQtyDaimondMesh.Text = "0";
            txtTotalCorner.Text = "";
            txtQtyGate.Text = "0";
            txtQtyPostCorner.Text = "0";
            txtQtyStaySupporter.Text = "0";
            txtQtyStandardy.Text = "0";
            txtQtyNutBolt.Text = "0";

            this.ActiveControl = txtPlotHeight;
        }
        private void FillDaimondMeshItems(string DMesh="",string HFence="")
        {
            
            dt = new DataTable();
            dt = DBClass.GetTableByQuery(@"Select * from Item_Master I 
                                            inner join Category_Master C on I.Category_Id=C.Category_Id 
                                            Where Category_Name like '%FENCE MESH DAIMOND%'");

            dt.TableName = "FENCEMESHDAIMOND";

            cmbDaimondMeshItemSize.Text = "";
            DataTable FilterTable = new DataTable();
            FilterTable.Columns.Add("Item_Code");
            FilterTable.Columns.Add("Item_Name");

            if (DMesh != "" && HFence != "")
            {

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        string Item = null;
                        string[] strArr = null;
                        int count = 0;
                        Item = dt.Rows[i]["Item_Name"].ToString();
                        char[] splitchar = { 'x' };
                        strArr = Item.Split(splitchar);

                        if (HFence == strArr[0] && DMesh == strArr[strArr.Length - 1])
                        {
                            FilterTable.Rows.Add(dt.Rows[i]["Item_Code"].ToString(), dt.Rows[i]["Item_Name"].ToString());
                        }
                
                    }
            }
            else
            {
                FilterTable = dt;    
            }

            cmbDaimondMeshItemSize.DisplayMember = "Item_Name";
            cmbDaimondMeshItemSize.ValueMember = "Item_Code";
            cmbDaimondMeshItemSize.DataSource = FilterTable;
        }
        private void FillGate(string HFence = "")
        {
            dt = new DataTable();
            dt = DBClass.GetTableByQuery("Select * from Item_Master I inner join Category_Master C on I.Category_Id=C.Category_Id Where Category_Name like '%FENCE GATE%'");

            dt.TableName = "FENCEGATE";
            DataTable FilterTable = new DataTable();
            FilterTable.Columns.Add("Item_Code");
            FilterTable.Columns.Add("Item_Name");

            ChkGate.Items.Clear();
            if (HFence != "")
            {

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string Item = null;
                    string[] strArr = null;
                    Item = dt.Rows[i]["Item_Name"].ToString();
                    char[] splitchar = { 'x' };
                    strArr = Item.Split(splitchar);
                    int count = strArr[0].Split('M').Length - 1;

                    string StrHght = "";
                    decimal Hght = 0;

                    if (count == 1)
                    {
                        Hght = decimal.Parse(strArr[0].Substring(0, strArr[0].Length - 1));
                        Hght *= 1000;
                        StrHght = Hght.ToString().Substring(0, Hght.ToString().Length - 2) + "MM";

                    }
                    else if (count == 2)
                    {
                        StrHght = strArr[0].Trim();
                    }

                    if (HFence.Trim() == StrHght.Trim())
                    {
                        FilterTable.Rows.Add(dt.Rows[i]["Item_Code"].ToString(), dt.Rows[i]["Item_Name"].ToString());
                    }
                }
            }
            else
            {
                FilterTable = dt;
            }
            for (int i = 0; i < FilterTable.Rows.Count; i++)
            {
                ChkGate.Items.Add(FilterTable.Rows[i]["Item_Name"].ToString(), CheckState.Unchecked);   
            }
            
        }
        private void FillPostCorner(string HFence = "")
        {
            if (HFence == "1800MM")
            {
                HFence = "2400MM";
            }

            if (HFence == "1200MM")
            {
                HFence = "1800MM";
            }
            
            dt = new DataTable();
            dt = DBClass.GetTableByQuery("Select * from Item_Master I inner join Category_Master C on I.Category_Id=C.Category_Id Where Category_Name like '%FENCE POST CORNER%'");
            dt.TableName = "FENCEPOSTCORNER";
            CmbPostCorner.Text = "";

            DataTable FilterTable = new DataTable();
            FilterTable.Columns.Add("Item_Code");
            FilterTable.Columns.Add("Item_Name");

            if (HFence != "")
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string Item = null;
                    string[] strArr = null;
                    Item = dt.Rows[i]["Item_Name"].ToString();
                    char[] splitchar = { 'x' };
                    strArr = Item.Split(splitchar);
                    int count = strArr[0].Split('M').Length - 1;

                    string StrHght = "";
                    decimal Hght = 0;

                    if (count == 1)
                    {
                        Hght = decimal.Parse(strArr[0].Substring(0, strArr[0].Length - 1));
                        Hght *= 1000;
                        StrHght = Hght.ToString().Substring(0, Hght.ToString().Length - 2) + "MM";
                    }
                    else if (count == 2)
                    {
                        StrHght = strArr[0].Trim();
                    }

                    if (HFence.Trim() == StrHght.Trim())
                    {
                        FilterTable.Rows.Add(dt.Rows[i]["Item_Code"].ToString(), dt.Rows[i]["Item_Name"].ToString());
                    }
                }
            }
            else
            {
                FilterTable = dt;
            }

            CmbPostCorner.DisplayMember = "Item_Name";
            CmbPostCorner.ValueMember = "Item_Code";
            CmbPostCorner.DataSource = FilterTable;

        }
        private void FillStaySupported(string HFence = "")
        {
            if (HFence == "1800MM")
            {
                HFence = "2400MM";
            }

            if (HFence == "1200MM")
            {
                HFence = "1800MM";
            }
            
            //FENCE STAY SUPPORTED 
            dt = new DataTable();
            dt = DBClass.GetTableByQuery("Select * from Item_Master I inner join Category_Master C on I.Category_Id=C.Category_Id Where Category_Name like '%FENCE STAY SUPPORTED%'");
            dt.TableName = "FENCESTAYSUPPORTED";

            cmbStaySupported.Text = "";

            DataTable FilterTable = new DataTable();
            FilterTable.Columns.Add("Item_Code");
            FilterTable.Columns.Add("Item_Name");

            if (HFence != "")
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string Item = null;
                    string[] strArr = null;
                    Item = dt.Rows[i]["Item_Name"].ToString();
                    char[] splitchar = { 'x' };
                    strArr = Item.Split(splitchar);
                    int count = strArr[0].Split('M').Length - 1;

                    string StrHght = "";
                    decimal Hght = 0;

                    if (count == 1)
                    {
                        Hght = decimal.Parse(strArr[0].Substring(0, strArr[0].Length - 1));
                        Hght *= 1000;
                        StrHght = Hght.ToString().Substring(0, Hght.ToString().Length - 2) + "MM";
                    }
                    else if (count == 2)
                    {
                        StrHght = strArr[0].Trim();
                    }

                    if (HFence.Trim() == StrHght.Trim())
                    {
                        FilterTable.Rows.Add(dt.Rows[i]["Item_Code"].ToString(), dt.Rows[i]["Item_Name"].ToString());
                    }
                }
            }
            else
            {
                FilterTable = dt;
            }

            cmbStaySupported.DisplayMember = "Item_Name";
            cmbStaySupported.ValueMember = "Item_Code";
            cmbStaySupported.DataSource = FilterTable;
        }
        private void FillStandardY(string HFence = "")
        {
            if (HFence == "1800MM")
            {
                HFence = "2400MM";
            }

            if (HFence == "1200MM")
            {
                HFence = "1800MM";
            }

            //FENCE standard 
            dt = new DataTable();
            dt = DBClass.GetTableByQuery("Select * from Item_Master I inner join Category_Master C on I.Category_Id=C.Category_Id Where Category_Name like '%FENCE STANDARD%'");
            dt.TableName = "FENCESTANDARDY";

            cmbstandardy.Text = "";

            DataTable FilterTable = new DataTable();
            FilterTable.Columns.Add("Item_Code");
            FilterTable.Columns.Add("Item_Name");

            if (HFence != "")
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string Item = null;
                    string[] strArr = null;
                    Item = dt.Rows[i]["Item_Name"].ToString();
                    char[] splitchar = { 'x' };
                    strArr = Item.Split(splitchar);
                    int count = strArr[0].Split('M').Length - 1;

                    string StrHght = "";
                    decimal Hght = 0;

                    if (count == 1)
                    {
                        Hght = decimal.Parse(strArr[0].Substring(0, strArr[0].Length - 1));
                        Hght *= 1000;
                        StrHght = Hght.ToString().Substring(0, Hght.ToString().Length - 2) + "MM";
                    }
                    else if (count == 2)
                    {
                        StrHght = strArr[0].Trim();
                    }

                    if (HFence.Trim() == StrHght.Trim())
                    {
                        FilterTable.Rows.Add(dt.Rows[i]["Item_Code"].ToString(), dt.Rows[i]["Item_Name"].ToString());
                    }
                }
            }
            else
            {
                FilterTable = dt;
            }

            cmbstandardy.DisplayMember = "Item_Name";
            cmbstandardy.ValueMember = "Item_Code";
            cmbstandardy.DataSource = FilterTable;
        }
        private void FillDropper(string HFence = "")
        {
            
            //FENCE standard 
            dt = new DataTable();
            dt = DBClass.GetTableByQuery("Select I.*,C.Category_Name from Item_Master I inner join Category_Master C on I.Category_Id=C.Category_Id Where Category_Name like '%FENCE DROPPER%'");
            dt.TableName = "FENCEDROPPER";

            cmbdropper.Text = "";

            DataTable FilterTable = new DataTable();
            FilterTable.Columns.Add("Item_Code");
            FilterTable.Columns.Add("Item_Name");

            if (HFence != "")
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string Item = null;
                    string[] strArr = null;
                    Item = dt.Rows[i]["Item_Name"].ToString();
                    char[] splitchar = { 'x' };
                    strArr = Item.Split(splitchar);
                    int count = strArr[0].Split('M').Length - 1;

                    string StrHght = "";
                    decimal Hght = 0;

                    if (count == 1)
                    {
                        Hght = decimal.Parse(strArr[0].Substring(0, strArr[0].Length - 1));
                        Hght *= 1000;
                        StrHght = Hght.ToString().Substring(0, Hght.ToString().Length - 2) + "MM";
                    }
                    else if (count == 2)
                    {
                        StrHght = strArr[0].Trim();
                    }

                    if (HFence.Trim() == StrHght.Trim())
                    {
                        FilterTable.Rows.Add(dt.Rows[i]["Item_Code"].ToString(), dt.Rows[i]["Item_Name"].ToString());
                    }
                }
            }
            else
            {
                FilterTable = dt;
            }

            cmbdropper.DisplayMember = "Item_Name";
            cmbdropper.ValueMember = "Item_Code";
            cmbdropper.DataSource = FilterTable;
        }
        private void FillWire(string HFence = "")
        {
            dt = new DataTable();
            dt = DBClass.GetTableByQuery(@"Select * from Item_Master I inner join Category_Master C on I.Category_Id=C.Category_Id 
                                        Where Category_Name like '%FENCE WIRE%'");

            dt.TableName = "FENCEWIRE";
            dgvWireItem.DataSource = null;
            dgvWireItem.DataSource = dt;

            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dgvWireItem.Rows.Count; i++)
                {
                    dgvWireItem.Rows[i].Cells["Select"].Value = true;
                }
            }
        }
        private void FillBoltAndNut()
        {

            dt = new DataTable();
            dt = DBClass.GetTableByQuery(@"Select * from Item_Master I inner join Category_Master C on I.Category_Id=C.Category_Id 
                                        Where Category_Name like '%FENCE BOLT & NUT%'");

            dt.TableName = "FENCEBOLTNUT";
            
            cmbBoltAndNut.DisplayMember = "Item_Name";
            cmbBoltAndNut.ValueMember = "Item_Code";
            cmbBoltAndNut.DataSource = dt;

            if (dt.Rows.Count > 0)
            {
                cmbBoltAndNut.SelectedIndex = 0;
            }

        }
        private void btnSeletedDaimondMesh_Click(object sender, EventArgs e)
        {
            string DMeshSize = "",HFence="";
            
            if (cmbDaimondMesh.Text != "")
            {
                DMeshSize = cmbDaimondMesh.Text;
            }
            if (cmbHeightOfFence.Text != "")
            {
                HFence = cmbHeightOfFence.Text;
            }

            chkSingleGate.Checked = false;
            ChkDoubleGate.Checked = false;
            txtTotalGateArea.Text = "0";
            txtQtyGate.Text = "0";
            txtQtyPostCorner.Text = "0";
            txtQtyStaySupporter.Text = "0";
            txtQtyStandardy.Text = "0";
            txtQtyNutBolt.Text = "0";
            txtQtyDropper.Text = "0";

            FillDaimondMeshItems(DMeshSize, HFence);
            FillGate(HFence);
            FillPostCorner(HFence);
            FillStaySupported(HFence);
            FillStandardY(HFence);
            FillDropper(HFence);
            FillWire(HFence);
            FillBoltAndNut();
            CalculateQtyPostCorner_StaySupporter();
            CalculateQtyStandardY();
            CalculateQtyDropper();
            CalulateBoltAndNut();

        }
        private void btnRefreshDaimondMesh_Click(object sender, EventArgs e)
        {
            chkSingleGate.Checked = false;
            ChkDoubleGate.Checked = false;
            txtTotalGateArea.Text = "0";
            txtQtyGate.Text = "0";
            txtQtyPostCorner.Text = "0";
            txtQtyStaySupporter.Text = "0";
            txtQtyStandardy.Text = "0";
            txtQtyNutBolt.Text = "0";
            txtQtyDropper.Text = "0";

            FillDaimondMeshItems();
            FillGate();
            FillPostCorner();
            FillStaySupported();
            FillStandardY();
            FillDropper();
            FillBoltAndNut();
            CalculateQtyPostCorner_StaySupporter();
            CalculateQtyStandardY();
            CalculateQtyDropper();
            CalulateBoltAndNut();
        }
        private void cmbDaimondMesh_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbDaimondMesh.Text != "" && txtTotalArea.Text != "")
            {
                Decimal DaimondMeshRoll = Decimal.Parse(cmbDaimondMesh.Text.Substring(0, cmbDaimondMesh.Text.Length - 1));
                Decimal TotalArea = Decimal.Parse(txtTotalArea.Text);
                decimal RollQty = TotalArea / DaimondMeshRoll;
                txtQtyDaimondMesh.Text = Math.Round(RollQty, 2).ToString();
            }
        }
        private void txtPlotHeight_TextChanged(object sender, EventArgs e)
        {
            txtPlotWidth.Text = txtPlotHeight.Text;
            CalculateAreaAndCorner();
        }
        private void CalculateAreaAndCorner()
        {
            if (txtPlotWidth.Text != "" && txtPlotHeight.Text != "")
            {
                int Height = int.Parse(txtPlotHeight.Text);
                int Width = int.Parse(txtPlotWidth.Text);
                int Area = Height + Height + Width + Width;
                txtTotalArea.Text = Area.ToString();
                txtTotalCorner.Text = "4";
            }
        }
        private void txtPlotWidth_TextChanged(object sender, EventArgs e)
        {
            CalculateAreaAndCorner();
        }
        private void ChkGate_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ChkGate.SelectedIndex == -1)
                MessageBox.Show("Please select a Gate first!");

            chkSingleGate.Checked = false;
            ChkDoubleGate.Checked = false;
            txtTotalGateArea.Text = "0";

            for (int i = 0; i < ChkGate.CheckedItems.Count; i++)
            {
                if (ChkGate.CheckedItems[i].ToString().Contains("3600") || ChkGate.CheckedItems[i].ToString().Contains("3.6"))
                {
                    ChkDoubleGate.Checked = true;
                }
                if (ChkGate.CheckedItems[i].ToString().Contains("900") || ChkGate.CheckedItems[i].ToString().Contains("0.9"))
                {
                    chkSingleGate.Checked = true;
                }
            }
        }
        private void CalculateGateTotalArea_Qty()
        {
            decimal TGateArea = 0;
            int qtygate = 0;
            if (chkSingleGate.Checked)
            {
                TGateArea += Convert.ToDecimal(0.9);
                qtygate += 1;
            }
            if (ChkDoubleGate.Checked)
            {
                TGateArea += Convert.ToDecimal(3.6);
                qtygate += 1;
            }

            txtTotalGateArea.Text = Math.Round(TGateArea,2).ToString();
            txtQtyGate.Text = qtygate.ToString();
            CalculateQtyPostCorner_StaySupporter();
            CalculateQtyStandardY();
            CalculateQtyDropper();
            CalulateBoltAndNut();
        }
        private void CalculateQtyPostCorner_StaySupporter()
        {
            int TCornerQty = int.Parse((txtTotalCorner.Text=="") ? "0" : txtTotalCorner.Text);
            int TGateQty = int.Parse((txtQtyGate.Text == "") ? "0" : txtQtyGate.Text);
            txtQtyPostCorner.Text = (TCornerQty + ((TGateQty>0)?2:0)).ToString();
            txtQtyStaySupporter.Text = ((TCornerQty + ((TGateQty > 0) ? 2 : 0)) * 2).ToString();
        }
        private void CalulateBoltAndNut()
        {
            int qty = 0;
            if (txtQtyPostCorner.Text != "")
            {
                if (cmbHeightOfFence.Text == "1200MM")
                {
                    qty = int.Parse(txtQtyPostCorner.Text) * 2;
                }
                else if (cmbHeightOfFence.Text == "1800MM")
                {
                    qty = int.Parse(txtQtyPostCorner.Text) * 3;
                }

                txtQtyNutBolt.Text = qty.ToString();
            }
            else
            {
                txtQtyNutBolt.Text = "0";
            }
        }
        private void CalculateQtyStandardY()
        {
            decimal TArea = decimal.Parse((txtTotalArea.Text == "") ? "0" : txtTotalArea.Text);
            decimal TGateArea = decimal.Parse((txtTotalGateArea.Text == "") ? "0" : txtTotalGateArea.Text);
            decimal NetArea = TArea - TGateArea;

            decimal StandardY = 0;
            if (rb2MStandardY.Checked)
            {
                StandardY = Convert.ToDecimal(NetArea / 2);
            }
            else if (rb4MStandardY.Checked)
            {
                StandardY = Convert.ToDecimal(NetArea / 4);
            }
            else if (rb10MStandardY.Checked)
            {
                StandardY = Convert.ToDecimal(NetArea / 10);
            }

            StandardY = Math.Round(StandardY, 0);
            StandardY -= decimal.Parse(txtQtyPostCorner.Text);
            txtQtyStandardy.Text = StandardY.ToString();
        }
        private void CalculateQtyDropper()
        {
            decimal TArea = decimal.Parse((txtTotalArea.Text == "") ? "0" : txtTotalArea.Text);
            decimal TGateArea = decimal.Parse((txtTotalGateArea.Text == "") ? "0" : txtTotalGateArea.Text);
            decimal NetArea = TArea - TGateArea;

            decimal Standardy = 0;
            decimal dropper = 0;
            if (rb2MStandardY.Checked && rb1Mdropper.Checked)
            {
                Standardy = Convert.ToDecimal(NetArea / 2);
                Standardy = Math.Round(Standardy, 0);
                dropper = Standardy - 1;
            }
            else if (rb4MStandardY.Checked && rb1Mdropper.Checked)
            {
                Standardy = Convert.ToDecimal(NetArea / 4);
                Standardy = Math.Round(Standardy, 0);
                dropper = (Standardy - 1)*3;
            }
            else if (rb4MStandardY.Checked && rb2Mdropper.Checked)
            {
                Standardy = Convert.ToDecimal(NetArea / 4);
                Standardy = Math.Round(Standardy, 0);
                dropper = Standardy - 1;
            }
            else if (rb10MStandardY.Checked && rb1Mdropper.Checked)
            {
                Standardy = Convert.ToDecimal(NetArea / 10);
                Standardy = Math.Round(Standardy, 0);
                dropper = (Standardy - 1) * 9;
            }
            else if (rb10MStandardY.Checked && rb2Mdropper.Checked)
            {
                Standardy = Convert.ToDecimal(NetArea / 10);
                Standardy = Math.Round(Standardy, 0);
                dropper = (Standardy - 1) * 4;
            }
            else if (rb10MStandardY.Checked && rb4Mdropper.Checked)
            {
                Standardy = Convert.ToDecimal(NetArea / 10);
                Standardy = Math.Round(Standardy, 0);
                dropper = (Standardy - 1) * 2;
            }
            
            txtQtyDropper.Text = Math.Round(dropper, 2).ToString();

        }
        private void chkSingleGate_CheckStateChanged(object sender, EventArgs e)
        {
            CalculateGateTotalArea_Qty();
        }
        private void ChkDoubleGate_CheckedChanged(object sender, EventArgs e)
        {
            CalculateGateTotalArea_Qty();
        }
        private void SetDefaultValueForSecurity()
        {
            if (rbWithSecurity.Checked)
            {
                chkwithsecurityGate460OH.Checked = true;
                chkwithoutsecurityGateHD.Checked = false;

                chkWithSecurityPostCorner460OH.Checked = true;
                chkWithoutSecurityPostCornerECONO.Checked = false;
            }

            if (rbWithoutSecurity.Checked)
            {
                chkwithsecurityGate460OH.Checked = false;
                chkwithoutsecurityGateHD.Checked = true;

                chkWithSecurityPostCorner460OH.Checked = false;
                chkWithoutSecurityPostCornerECONO.Checked = true;
            }
        }
        private void rbWithSecurity_CheckedChanged(object sender, EventArgs e)
        {
            SetDefaultValueForSecurity();
        }
        private void rbWithoutSecurity_CheckedChanged(object sender, EventArgs e)
        {
            SetDefaultValueForSecurity();
        }
        private void rb2MStandardY_CheckedChanged(object sender, EventArgs e)
        {
            CalculateQtyStandardY();
            CalculateQtyDropper();
        }
        private void rb4MStandardY_CheckedChanged(object sender, EventArgs e)
        {
            CalculateQtyStandardY();
            CalculateQtyDropper();
        }
        private void rb10MStandardY_CheckedChanged(object sender, EventArgs e)
        {
            CalculateQtyStandardY();
            CalculateQtyDropper();
        }
        private void rb2Mdropper_CheckedChanged(object sender, EventArgs e)
        {
            CalculateQtyDropper();
        }
        private void rb4Mdropper_CheckedChanged(object sender, EventArgs e)
        {
            CalculateQtyDropper();
        }
        private void Set_Grid()
        {
            dgvWireItem.AutoGenerateColumns = false;

            DataGridViewCheckBoxColumn Select = new DataGridViewCheckBoxColumn();
            Select.Name = "Select";
            Select.HeaderText = "";
            Select.Width = 20;
            Select.FalseValue = 0;
            Select.TrueValue = 1;
            dgvWireItem.Columns.Add(Select);

            DataGridViewTextBoxColumn Item_Code = new DataGridViewTextBoxColumn();
            Item_Code.Name = "Item_Code";
            Item_Code.HeaderText = "No.";
            Item_Code.DataPropertyName = "Item_Code";
            Item_Code.ReadOnly = true;
            Item_Code.Visible = false;
            dgvWireItem.Columns.Add(Item_Code);

            DataGridViewTextBoxColumn Item_Name = new DataGridViewTextBoxColumn();
            Item_Name.Name = "Item_Name";
            Item_Name.HeaderText = "Description";
            Item_Name.DataPropertyName = "Item_Name";
            Item_Name.ReadOnly = true;
            Item_Name.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvWireItem.Columns.Add(Item_Name);

            DataGridViewComboBoxColumn AdditionalInfo = new DataGridViewComboBoxColumn();
            AdditionalInfo.Name = "AdditionalInfo";
            AdditionalInfo.HeaderText = "Additional Info.";
            AdditionalInfo.Items.Add("");
            AdditionalInfo.Items.Add("Binding");
            AdditionalInfo.Items.Add("Tying Purpose");
            AdditionalInfo.Items.Add("Lines of Top of Fence Security");
            
            AdditionalInfo.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvWireItem.Columns.Add(AdditionalInfo);

            DataGridViewTextBoxColumn Qty = new DataGridViewTextBoxColumn();
            Qty.Name = "Qty";
            Qty.HeaderText = "Qty";
            Qty.Width = 35;
            Qty.DefaultCellStyle.NullValue = "1";
            dgvWireItem.Columns.Add(Qty);

        }
        private void btnGenerate_Click(object sender, EventArgs e)
        {
            pnlResize.Visible = true;
            pnlResize.Dock = DockStyle.Fill;
            btnGenerate.Visible = false;
            FillGridWithEstimateValue();

        }
        private void btnBack_Click(object sender, EventArgs e)
        {
            pnlResize.Dock = DockStyle.None;
            pnlResize.Visible = false;
            btnGenerate.Visible = true;
        }
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
            Additional_Info.Visible=false;
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
            SellingPrice.Width = 100;
            SellingPrice.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            SellingPrice.ReadOnly = true;
            dgvEstimateGrid.Columns.Add(SellingPrice);

            DataGridViewTextBoxColumn ExclusiveAmount = new DataGridViewTextBoxColumn();
            ExclusiveAmount.Name = "ExclusiveAmount";
            ExclusiveAmount.HeaderText = "Exclusive P";
            ExclusiveAmount.DataPropertyName = "ExclusiveAmount";
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
            VATAmount.Width = 100;
            VATAmount.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            VATAmount.ReadOnly = true;
            dgvEstimateGrid.Columns.Add(VATAmount);

            DataGridViewTextBoxColumn InclusiveAmount = new DataGridViewTextBoxColumn();
            InclusiveAmount.Name = "InclusiveAmount";
            InclusiveAmount.HeaderText = "Inclusive P";
            InclusiveAmount.DataPropertyName = "InclusiveAmount";
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
            DiscAmount.Width = 100;
            DiscAmount.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            DiscAmount.ReadOnly = true;
            dgvEstimateGrid.Columns.Add(DiscAmount);

            DataGridViewTextBoxColumn NetAmount = new DataGridViewTextBoxColumn();
            NetAmount.Name = "NetAmount";
            NetAmount.HeaderText = "Net Amount";
            NetAmount.DataPropertyName = "NetAmount";
            NetAmount.Width = 100;
            NetAmount.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            NetAmount.ReadOnly = true;
            dgvEstimateGrid.Columns.Add(NetAmount);

        }
        DataTable dummy = new DataTable();
        private void FillGridWithEstimateValue()
        {
            dummy = new DataTable();
            dummy.Columns.Add("Details_Id");
            dummy.Columns.Add("Trans_Id");
            dummy.Columns.Add("Item_Code");
            dummy.Columns.Add("Item_PartNumber");
            dummy.Columns.Add("Item_Name");
            dummy.Columns.Add("Additional_Info");
            dummy.Columns.Add("Item_Qty");
            dummy.Columns.Add("SellingPrice");
            dummy.Columns.Add("ExclusiveAmount");
            dummy.Columns.Add("VAT_Per");
            dummy.Columns.Add("VATAmount");
            dummy.Columns.Add("InclusiveAmount");
            dummy.Columns.Add("MaxDisc_Per");
            dummy.Columns.Add("Disc_Per");
            dummy.Columns.Add("DiscAmount");
            dummy.Columns.Add("NetAmount");

            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            
            string Sql = @"Select I.*,C.Category_Name  
                            from Item_Master I inner Join Category_Master C on I.Category_Id=C.Category_Id Where 1=1";
            dt = DBClass.GetTableByQuery(Sql);
            ds.Tables.Add(dt);
            ds.Tables[0].TableName = "Item_Master";

            /////////////////For Daimond Mesh/////////////////////////////////////////////////////////
            string AdditionalInfo = "";
            if (cmbDaimondMeshItemSize.SelectedValue != null && cmbDaimondMeshItemSize.Text != "")
            {
                ds.Tables["Item_Master"].DefaultView.RowFilter = "";
                ds.Tables["Item_Master"].DefaultView.RowFilter = " Item_Code=" + int.Parse(cmbDaimondMeshItemSize.SelectedValue.ToString()) + "";

                dt = ds.Tables["Item_Master"].DefaultView.ToTable();
                if (dt.Rows.Count > 0)
                {
                    decimal TPrice = decimal.Parse(dt.Rows[0]["SellingPrice"].ToString()) * decimal.Parse(txtQtyDaimondMesh.Text);
                    decimal VATAmt = TPrice * Convert.ToDecimal(dt.Rows[0]["VAT_Per"].ToString()) / 100;
                    decimal NetAmt = TPrice + VATAmt;

                    string ItemDetails = dt.Rows[0]["Category_Name"].ToString() + " " + cmbDaimondMeshItemSize.Text;
                    dummy.Rows.Add("", "",  cmbDaimondMeshItemSize.SelectedValue.ToString(), dt.Rows[0]["Item_PartNumber"].ToString(), ItemDetails, AdditionalInfo,
                                    txtQtyDaimondMesh.Text, dt.Rows[0]["SellingPrice"].ToString(), TPrice, dt.Rows[0]["VAT_Per"].ToString(), VATAmt,NetAmt,dt.Rows[0]["MaxDisc_Per"].ToString(),
                                    0,0, NetAmt);
                }
            }
            //////////////POST Corner///////////////////////////////////////////////////////////

            AdditionalInfo = "";
            if (CmbPostCorner.SelectedValue != null && CmbPostCorner.Text != "")
            {
                ds.Tables["Item_Master"].DefaultView.RowFilter = "";
                ds.Tables["Item_Master"].DefaultView.RowFilter = " Item_Code=" + int.Parse(CmbPostCorner.SelectedValue.ToString()) + "";

                dt = ds.Tables["Item_Master"].DefaultView.ToTable();
                if (dt.Rows.Count > 0)
                {
                    string ItemDetails = dt.Rows[0]["Category_Name"].ToString() + " " + CmbPostCorner.Text;
                    if (chkWithSecurityPostCorner460OH.Checked)
                    {
                        AdditionalInfo = "+460 OH";
                        ItemDetails += AdditionalInfo;
                    }
                    else if (chkWithoutSecurityPostCornerECONO.Checked)
                    {
                        AdditionalInfo = "ECONO";
                        ItemDetails += AdditionalInfo;
                    }

                    decimal TPrice = decimal.Parse(dt.Rows[0]["SellingPrice"].ToString()) * decimal.Parse(txtQtyPostCorner.Text);
                    decimal VATAmt = TPrice * Convert.ToDecimal(dt.Rows[0]["VAT_Per"].ToString()) / 100;
                    decimal NetAmt = TPrice + VATAmt;

                    dummy.Rows.Add("", "",  CmbPostCorner.SelectedValue.ToString(), dt.Rows[0]["Item_PartNumber"].ToString(), ItemDetails, AdditionalInfo,
                                    txtQtyPostCorner.Text, dt.Rows[0]["SellingPrice"].ToString(), TPrice, dt.Rows[0]["VAT_Per"].ToString(), VATAmt,NetAmt, dt.Rows[0]["MaxDisc_Per"].ToString(),
                                    0,0, NetAmt);
                }
            }

            //////////////////// STAY SUPPORTED /////////////////////////////////////////////////////////

            AdditionalInfo = "";
            if (cmbStaySupported.SelectedValue != null && cmbStaySupported.Text != "")
            {
                ds.Tables["Item_Master"].DefaultView.RowFilter = "";
                ds.Tables["Item_Master"].DefaultView.RowFilter = "  Item_Code=" + int.Parse(cmbStaySupported.SelectedValue.ToString()) + "";

                dt = ds.Tables["Item_Master"].DefaultView.ToTable();
                if (dt.Rows.Count > 0)
                {

                    string ItemDetails = dt.Rows[0]["Category_Name"].ToString() + " " + cmbStaySupported.Text;
                    decimal TPrice = decimal.Parse(dt.Rows[0]["SellingPrice"].ToString()) * decimal.Parse(txtQtyStaySupporter.Text);
                    decimal VATAmt = TPrice * Convert.ToDecimal(dt.Rows[0]["VAT_Per"].ToString()) / 100;
                    decimal NetAmt = TPrice + VATAmt;
                    dummy.Rows.Add("", "",  cmbStaySupported.SelectedValue.ToString(), dt.Rows[0]["Item_PartNumber"].ToString(), ItemDetails, AdditionalInfo,
                                        txtQtyStaySupporter.Text, dt.Rows[0]["SellingPrice"].ToString(), TPrice, dt.Rows[0]["VAT_Per"].ToString(), VATAmt,NetAmt, dt.Rows[0]["MaxDisc_Per"].ToString(),
                                        0,0, NetAmt);
                }
            }
            //////////////GATE///////////////////////////////////////////////////////////

            for (int x = 0; x <= ChkGate.CheckedItems.Count - 1; x++)
            {
                AdditionalInfo = "";
                ds.Tables["Item_Master"].DefaultView.RowFilter = "";
                ds.Tables["Item_Master"].DefaultView.RowFilter = "  Item_Name like '%" + ChkGate.CheckedItems[x].ToString() + "%'";
                dt = ds.Tables["Item_Master"].DefaultView.ToTable();
                if (dt.Rows.Count > 0)
                {
                    string ItemDetails = dt.Rows[0]["Category_Name"].ToString() + " " + dt.Rows[0]["Item_Name"].ToString();
                    if (chkWithSecurityPostCorner460OH.Checked)
                    {
                        AdditionalInfo = "+460 OH";
                        ItemDetails += AdditionalInfo;
                    }
                    else if (chkWithoutSecurityPostCornerECONO.Checked)
                    {
                        AdditionalInfo = "ECONO";
                        ItemDetails += AdditionalInfo;
                    }

                    decimal TPrice = decimal.Parse(dt.Rows[0]["SellingPrice"].ToString()) * 1;
                    decimal VATAmt = TPrice * Convert.ToDecimal(dt.Rows[0]["VAT_Per"].ToString()) / 100;
                    decimal NetAmt = TPrice + VATAmt;

                    dummy.Rows.Add("", "",  dt.Rows[0]["Item_Code"].ToString(), dt.Rows[0]["Item_PartNumber"].ToString(), ItemDetails, AdditionalInfo,
                                        1, dt.Rows[0]["SellingPrice"].ToString(), TPrice, dt.Rows[0]["VAT_Per"].ToString(), VATAmt,NetAmt, dt.Rows[0]["MaxDisc_Per"].ToString(),
                                        0,0,  NetAmt);
                }
            }

            //////////////STANDARD [Y] ///////////////////////////////////////////////////////////

            AdditionalInfo = "";
            if (cmbstandardy.SelectedValue != null && cmbstandardy.Text != "")
            {
                ds.Tables["Item_Master"].DefaultView.RowFilter = "";
                ds.Tables["Item_Master"].DefaultView.RowFilter = " Item_Code=" + int.Parse(cmbstandardy.SelectedValue.ToString()) + "";

                dt = ds.Tables["Item_Master"].DefaultView.ToTable();
                if (dt.Rows.Count > 0)
                {
                    string ItemDetails = dt.Rows[0]["Category_Name"].ToString() + " " + cmbstandardy.Text;
                    if (chkWithSecuritystandardy460OH.Checked)
                    {
                        AdditionalInfo = "+460 OH";
                        ItemDetails += AdditionalInfo;
                    }
                    decimal TPrice = decimal.Parse(dt.Rows[0]["SellingPrice"].ToString()) * decimal.Parse(txtQtyStandardy.Text);
                    decimal VATAmt = TPrice * Convert.ToDecimal(dt.Rows[0]["VAT_Per"].ToString()) / 100;
                    decimal NetAmt = TPrice + VATAmt;

                    dummy.Rows.Add("", "",  cmbstandardy.SelectedValue.ToString(), dt.Rows[0]["Item_PartNumber"].ToString(), ItemDetails, AdditionalInfo,
                                    txtQtyStandardy.Text, dt.Rows[0]["SellingPrice"].ToString(), TPrice, dt.Rows[0]["VAT_Per"].ToString(), VATAmt,NetAmt,dt.Rows[0]["MaxDisc_Per"].ToString(),
                                    0,0,NetAmt);
                }
            }

            ////////////// DROPPERS ///////////////////////////////////////////////////////////

            AdditionalInfo = "";
            if (cmbdropper.SelectedValue != null && cmbdropper.Text != "")
            {
                ds.Tables["Item_Master"].DefaultView.RowFilter = "";
                ds.Tables["Item_Master"].DefaultView.RowFilter = " Item_Code=" + int.Parse(cmbdropper.SelectedValue.ToString()) + "";

                dt = ds.Tables["Item_Master"].DefaultView.ToTable();
                if (dt.Rows.Count > 0)
                {
                    string ItemDetails = dt.Rows[0]["Category_Name"].ToString() + " " + cmbdropper.Text;
                    decimal TPrice = decimal.Parse(dt.Rows[0]["SellingPrice"].ToString()) * decimal.Parse(txtQtyDropper.Text);
                    decimal VATAmt = TPrice * Convert.ToDecimal(dt.Rows[0]["VAT_Per"].ToString()) / 100;
                    decimal NetAmt = TPrice + VATAmt;

                    dummy.Rows.Add("", "",  cmbdropper.SelectedValue.ToString(), dt.Rows[0]["Item_PartNumber"].ToString(), ItemDetails, AdditionalInfo,
                                    txtQtyDropper.Text, dt.Rows[0]["SellingPrice"].ToString(), TPrice, dt.Rows[0]["VAT_Per"].ToString(), VATAmt,NetAmt, dt.Rows[0]["MaxDisc_Per"].ToString(),
                                    0,0,  NetAmt);
                }
            }

            //////////////WIRE ///////////////////////////////////////////////////////////

            for (int x = 0; x < dgvWireItem.Rows.Count ; x++)
            {
                AdditionalInfo = "";
                ds.Tables["Item_Master"].DefaultView.RowFilter = "";
                ds.Tables["Item_Master"].DefaultView.RowFilter = "  Item_Code=" + int.Parse(dgvWireItem.Rows[x].Cells["Item_Code"].Value.ToString()) + "";
                dt = ds.Tables["Item_Master"].DefaultView.ToTable();
                if (dt.Rows.Count > 0)
                {
                    string ItemDetails = dt.Rows[0]["Category_Name"].ToString() + " " + dt.Rows[0]["Item_Name"].ToString();
                    AdditionalInfo = (dgvWireItem.Rows[x].Cells["AdditionalInfo"].Value==null)? "": dgvWireItem.Rows[x].Cells["AdditionalInfo"].Value.ToString();
                    ItemDetails += AdditionalInfo;
                    int Qty = (dgvWireItem.Rows[x].Cells["Qty"].Value == null) ? 1 : int.Parse(dgvWireItem.Rows[x].Cells["Qty"].Value.ToString()); //int.Parse(dgvWireItem.Rows[x].Cells["Qty"].Value.ToString());

                    decimal TPrice = decimal.Parse(dt.Rows[0]["SellingPrice"].ToString()) * (Qty);
                    decimal VATAmt = TPrice * Convert.ToDecimal(dt.Rows[0]["VAT_Per"].ToString()) / 100;
                    decimal NetAmt = TPrice + VATAmt;

                    dummy.Rows.Add("", "", dt.Rows[0]["Item_Code"].ToString(), dt.Rows[0]["Item_PartNumber"].ToString(), ItemDetails, AdditionalInfo,
                                        Qty, dt.Rows[0]["SellingPrice"].ToString(), TPrice, dt.Rows[0]["VAT_Per"].ToString(), VATAmt,NetAmt, dt.Rows[0]["MaxDisc_Per"].ToString(),
                                        0,0, NetAmt);
                }
            }

            //////////////////// BOLT AND NUT /////////////////////////////////////////////////////////

            AdditionalInfo = "";
            if (cmbBoltAndNut.SelectedValue != null && cmbBoltAndNut.Text != "")
            {
                ds.Tables["Item_Master"].DefaultView.RowFilter = "";
                ds.Tables["Item_Master"].DefaultView.RowFilter = "  Item_Code=" + int.Parse(cmbBoltAndNut.SelectedValue.ToString()) + "";

                dt = ds.Tables["Item_Master"].DefaultView.ToTable();
                if (dt.Rows.Count > 0)
                {

                    string ItemDetails = dt.Rows[0]["Category_Name"].ToString() + " " + cmbBoltAndNut.Text;
                    decimal TPrice = decimal.Parse(dt.Rows[0]["SellingPrice"].ToString()) * decimal.Parse(txtQtyNutBolt.Text);
                    decimal VATAmt = TPrice * Convert.ToDecimal(dt.Rows[0]["VAT_Per"].ToString()) / 100;
                    decimal NetAmt = TPrice + VATAmt;

                    dummy.Rows.Add("", "", cmbBoltAndNut.SelectedValue.ToString(), dt.Rows[0]["Item_PartNumber"].ToString(), ItemDetails, AdditionalInfo,
                                        txtQtyNutBolt.Text, dt.Rows[0]["SellingPrice"].ToString(), TPrice, dt.Rows[0]["VAT_Per"].ToString(), VATAmt,NetAmt, dt.Rows[0]["MaxDisc_Per"].ToString(),
                                        0,0,NetAmt);
                }
            }
            dgvEstimateGrid.DataSource = dummy;
        }
        private void dgvEstimateGrid_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            //if (!dsMain.HasChanges()) return;
            try
            {

                if (dgvEstimateGrid.Columns[e.ColumnIndex].Name.ToUpper() == "ITEM_QTY" || dgvEstimateGrid.Columns[e.ColumnIndex].Name.ToUpper() == "SELLINGPRICE" || dgvEstimateGrid.Columns[e.ColumnIndex].Name.ToUpper() == "VAT_PER" || dgvEstimateGrid.Columns[e.ColumnIndex].Name.ToUpper() == "DISC_PER")
                {
                    decimal SellingPrice = Convert.ToDecimal((dgvEstimateGrid.CurrentRow.Cells["SELLINGPRICE"].Value is DBNull) ? 0 : dgvEstimateGrid.CurrentRow.Cells["SELLINGPRICE"].Value);
                    decimal DiscPer = Convert.ToDecimal((dgvEstimateGrid.CurrentRow.Cells["DISC_PER"].Value is DBNull) ? 0 : dgvEstimateGrid.CurrentRow.Cells["DISC_PER"].Value);
                    decimal MaxDiscPer = Convert.ToDecimal((dgvEstimateGrid.CurrentRow.Cells["MAXDISC_PER"].Value is DBNull) ? 0 : dgvEstimateGrid.CurrentRow.Cells["MAXDISC_PER"].Value);
                    decimal Qty = Convert.ToDecimal((dgvEstimateGrid.CurrentRow.Cells["Item_Qty"].Value is DBNull) ? 0 : dgvEstimateGrid.CurrentRow.Cells["Item_Qty"].Value);
                    decimal VatPer = Convert.ToDecimal((dgvEstimateGrid.CurrentRow.Cells["VAT_PER"].Value is DBNull) ? 0 : dgvEstimateGrid.CurrentRow.Cells["VAT_PER"].Value);
                    decimal DiscAmt=0, VATAmt=0, NetAmt=0;

                    if (DiscPer > MaxDiscPer)
                    {
                        MessageBox.Show("Discount Limit is "+ MaxDiscPer.ToString() + "%,Please Contact Your Manager.");
                        dgvEstimateGrid.CurrentRow.Cells["DISC_PER"].Value = 0;
                        return;
                    }

                    decimal TAmt= SellingPrice * Qty;
                    dgvEstimateGrid.CurrentRow.Cells["ExclusiveAmount"].Value = TAmt;

                    if (VatPer != 0)
                    {
                        VATAmt = (TAmt * VatPer) / 100;
                        dgvEstimateGrid.CurrentRow.Cells["VATAmount"].Value = VATAmt;
                    }

                    TAmt += VATAmt;
                    dgvEstimateGrid.CurrentRow.Cells["InclusiveAmount"].Value = TAmt;

                    if (DiscPer != 0)
                    {
                        DiscAmt=(TAmt * DiscPer) / 100;
                        dgvEstimateGrid.CurrentRow.Cells["DiscAmount"].Value = DiscAmt;
                    }

                    NetAmt = TAmt - DiscAmt;
                    dgvEstimateGrid.CurrentRow.Cells["NetAmount"].Value = NetAmt;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void EstimateCost_KeyDown(object sender, KeyEventArgs e)
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

        private void btnSaveEstimate_Click(object sender, EventArgs e)
        {
            FinalEstimationEntry obj = new FinalEstimationEntry("NewRecord");
            
            obj.txtPlotHeight.Text = txtPlotHeight.Text;
            obj.txtPlotWidth.Text = txtPlotWidth.Text;
            obj.txtTotalArea.Text = txtTotalArea.Text;
            obj.txtTotalCorner.Text = txtTotalCorner.Text;
            obj.txtTotalGateArea.Text = txtTotalGateArea.Text;
            obj.txtRollSize.Text = cmbDaimondMesh.Text;
            obj.txtHeightOfFence.Text = cmbHeightOfFence.Text;

            if(rb2MStandardY.Checked)
                obj.txtStdY.Text = "2M";
            else if (rb4MStandardY.Checked)
                obj.txtStdY.Text = "4M";
            else if (rb10MStandardY.Checked)
                obj.txtStdY.Text = "10M";

            if(rb1Mdropper.Checked)
                obj.txtDropper.Text = "1M";
            else if (rb2Mdropper.Checked)
                    obj.txtDropper.Text = "2M";
            else if (rb4Mdropper.Checked)
                obj.txtDropper.Text = "4M";

            obj.dgvEstimateGrid.AutoGenerateColumns = false;
            obj.dgvEstimateGrid.DataSource = dummy;
            mainForm.pnlMiddle.Controls.Clear();

            obj.TopLevel = false;
            obj.AutoScroll = true;
            mainForm.pnlMiddle.Controls.Add(obj);
            obj.Show();

            //mainForm.pnlMiddle.
            //obj.ShowDialog();
            //this.Close();

        }

        private void dgvEstimateGrid_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvEstimateGrid.Columns[e.ColumnIndex].Name.ToUpper() == "ITEM_QTY" || 
                dgvEstimateGrid.Columns[e.ColumnIndex].Name.ToUpper() == "SELLINGPRICE" || 
                dgvEstimateGrid.Columns[e.ColumnIndex].Name.ToUpper() == "EXCLUSIVEAMOUNT" || 
                dgvEstimateGrid.Columns[e.ColumnIndex].Name.ToUpper() == "VAT_PER" ||
                dgvEstimateGrid.Columns[e.ColumnIndex].Name.ToUpper() == "VATAMOUNT" || 
                dgvEstimateGrid.Columns[e.ColumnIndex].Name.ToUpper() == "INCLUSIVEAMOUNT" || 
                dgvEstimateGrid.Columns[e.ColumnIndex].Name.ToUpper() == "MAXDISC_PER"||
                dgvEstimateGrid.Columns[e.ColumnIndex].Name.ToUpper() == "DISC_PER"||
                dgvEstimateGrid.Columns[e.ColumnIndex].Name.ToUpper() == "DISCAMOUNT"||
                dgvEstimateGrid.Columns[e.ColumnIndex].Name.ToUpper() == "NETAMOUNT")
            {
                //e.CellStyle.Format = "N2";
                dgvEstimateGrid.Columns[e.ColumnIndex].DefaultCellStyle.Format = "N0";
            }
        }


    }
}
