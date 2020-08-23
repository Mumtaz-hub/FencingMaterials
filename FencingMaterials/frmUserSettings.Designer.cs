namespace FencingMaterials
{
    partial class frmUserSettings
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.TabControl1 = new System.Windows.Forms.TabControl();
            this.tabNewUser = new System.Windows.Forms.TabPage();
            this.txtOldPassword = new System.Windows.Forms.TextBox();
            this.txtConfirmPassword = new System.Windows.Forms.TextBox();
            this.txtNewPassword = new System.Windows.Forms.TextBox();
            this.txtUserName = new System.Windows.Forms.TextBox();
            this.txtUserId = new System.Windows.Forms.TextBox();
            this.btn_ChangePasword = new System.Windows.Forms.Button();
            this.lblOldPassword = new System.Windows.Forms.Label();
            this.btn_Close = new System.Windows.Forms.Button();
            this.btn_NewUser = new System.Windows.Forms.Button();
            this.Label3 = new System.Windows.Forms.Label();
            this.cboUserGroup = new System.Windows.Forms.ComboBox();
            this.Label5 = new System.Windows.Forms.Label();
            this.Label4 = new System.Windows.Forms.Label();
            this.Label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tabUserSettings = new System.Windows.Forms.TabPage();
            this.txtUserConfirmPass = new System.Windows.Forms.TextBox();
            this.txtUserNewPass = new System.Windows.Forms.TextBox();
            this.btn_chaneUserPass = new System.Windows.Forms.Button();
            this.Label6 = new System.Windows.Forms.Label();
            this.Label8 = new System.Windows.Forms.Label();
            this.btn_Display = new System.Windows.Forms.Button();
            this.cboAllUser = new System.Windows.Forms.ComboBox();
            this.menu_Grid = new System.Windows.Forms.DataGridView();
            this.Label7 = new System.Windows.Forms.Label();
            this.chkSalesPerson = new System.Windows.Forms.CheckBox();
            this.TabControl1.SuspendLayout();
            this.tabNewUser.SuspendLayout();
            this.tabUserSettings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.menu_Grid)).BeginInit();
            this.SuspendLayout();
            // 
            // TabControl1
            // 
            this.TabControl1.Controls.Add(this.tabNewUser);
            this.TabControl1.Controls.Add(this.tabUserSettings);
            this.TabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TabControl1.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TabControl1.Location = new System.Drawing.Point(0, 0);
            this.TabControl1.Name = "TabControl1";
            this.TabControl1.SelectedIndex = 0;
            this.TabControl1.Size = new System.Drawing.Size(467, 536);
            this.TabControl1.TabIndex = 116;
            this.TabControl1.SelectedIndexChanged += new System.EventHandler(this.TabControl1_SelectedIndexChanged);
            // 
            // tabNewUser
            // 
            this.tabNewUser.BackColor = System.Drawing.SystemColors.Control;
            this.tabNewUser.Controls.Add(this.chkSalesPerson);
            this.tabNewUser.Controls.Add(this.txtOldPassword);
            this.tabNewUser.Controls.Add(this.txtConfirmPassword);
            this.tabNewUser.Controls.Add(this.txtNewPassword);
            this.tabNewUser.Controls.Add(this.txtUserName);
            this.tabNewUser.Controls.Add(this.txtUserId);
            this.tabNewUser.Controls.Add(this.btn_ChangePasword);
            this.tabNewUser.Controls.Add(this.lblOldPassword);
            this.tabNewUser.Controls.Add(this.btn_Close);
            this.tabNewUser.Controls.Add(this.btn_NewUser);
            this.tabNewUser.Controls.Add(this.Label3);
            this.tabNewUser.Controls.Add(this.cboUserGroup);
            this.tabNewUser.Controls.Add(this.Label5);
            this.tabNewUser.Controls.Add(this.Label4);
            this.tabNewUser.Controls.Add(this.Label2);
            this.tabNewUser.Controls.Add(this.label1);
            this.tabNewUser.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabNewUser.Location = new System.Drawing.Point(4, 23);
            this.tabNewUser.Name = "tabNewUser";
            this.tabNewUser.Padding = new System.Windows.Forms.Padding(3);
            this.tabNewUser.Size = new System.Drawing.Size(459, 509);
            this.tabNewUser.TabIndex = 0;
            this.tabNewUser.Text = "New User";
            // 
            // txtOldPassword
            // 
            this.txtOldPassword.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtOldPassword.Font = new System.Drawing.Font("Trebuchet MS", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtOldPassword.Location = new System.Drawing.Point(139, 181);
            this.txtOldPassword.Name = "txtOldPassword";
            this.txtOldPassword.PasswordChar = '*';
            this.txtOldPassword.Size = new System.Drawing.Size(239, 23);
            this.txtOldPassword.TabIndex = 123;
            this.txtOldPassword.Validated += new System.EventHandler(this.txtOldPassword_Validated);
            // 
            // txtConfirmPassword
            // 
            this.txtConfirmPassword.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtConfirmPassword.Font = new System.Drawing.Font("Trebuchet MS", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtConfirmPassword.Location = new System.Drawing.Point(139, 145);
            this.txtConfirmPassword.Name = "txtConfirmPassword";
            this.txtConfirmPassword.PasswordChar = '*';
            this.txtConfirmPassword.Size = new System.Drawing.Size(239, 23);
            this.txtConfirmPassword.TabIndex = 122;
            // 
            // txtNewPassword
            // 
            this.txtNewPassword.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtNewPassword.Font = new System.Drawing.Font("Trebuchet MS", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNewPassword.Location = new System.Drawing.Point(139, 116);
            this.txtNewPassword.Name = "txtNewPassword";
            this.txtNewPassword.PasswordChar = '*';
            this.txtNewPassword.Size = new System.Drawing.Size(239, 23);
            this.txtNewPassword.TabIndex = 121;
            // 
            // txtUserName
            // 
            this.txtUserName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtUserName.Font = new System.Drawing.Font("Trebuchet MS", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtUserName.Location = new System.Drawing.Point(139, 58);
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.Size = new System.Drawing.Size(239, 23);
            this.txtUserName.TabIndex = 120;
            this.txtUserName.TabStop = false;
            this.txtUserName.Validated += new System.EventHandler(this.txtUserName_Validated);
            // 
            // txtUserId
            // 
            this.txtUserId.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtUserId.Font = new System.Drawing.Font("Trebuchet MS", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtUserId.Location = new System.Drawing.Point(139, 24);
            this.txtUserId.Name = "txtUserId";
            this.txtUserId.Size = new System.Drawing.Size(239, 23);
            this.txtUserId.TabIndex = 119;
            this.txtUserId.Validated += new System.EventHandler(this.txtUserId_Validated);
            // 
            // btn_ChangePasword
            // 
            this.btn_ChangePasword.BackColor = System.Drawing.SystemColors.Control;
            this.btn_ChangePasword.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_ChangePasword.Location = new System.Drawing.Point(141, 255);
            this.btn_ChangePasword.Name = "btn_ChangePasword";
            this.btn_ChangePasword.Size = new System.Drawing.Size(146, 40);
            this.btn_ChangePasword.TabIndex = 7;
            this.btn_ChangePasword.Text = "Change Password";
            this.btn_ChangePasword.UseVisualStyleBackColor = false;
            this.btn_ChangePasword.Click += new System.EventHandler(this.btn_ChangePasword_Click);
            // 
            // lblOldPassword
            // 
            this.lblOldPassword.AutoSize = true;
            this.lblOldPassword.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOldPassword.Location = new System.Drawing.Point(28, 188);
            this.lblOldPassword.Name = "lblOldPassword";
            this.lblOldPassword.Size = new System.Drawing.Size(106, 16);
            this.lblOldPassword.TabIndex = 118;
            this.lblOldPassword.Text = "Old Password";
            this.lblOldPassword.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btn_Close
            // 
            this.btn_Close.BackColor = System.Drawing.SystemColors.Control;
            this.btn_Close.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Close.Location = new System.Drawing.Point(293, 255);
            this.btn_Close.Name = "btn_Close";
            this.btn_Close.Size = new System.Drawing.Size(78, 40);
            this.btn_Close.TabIndex = 8;
            this.btn_Close.TabStop = false;
            this.btn_Close.Text = "Close";
            this.btn_Close.UseVisualStyleBackColor = false;
            this.btn_Close.Click += new System.EventHandler(this.btn_Close_Click);
            // 
            // btn_NewUser
            // 
            this.btn_NewUser.BackColor = System.Drawing.SystemColors.Control;
            this.btn_NewUser.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_NewUser.Location = new System.Drawing.Point(26, 255);
            this.btn_NewUser.Name = "btn_NewUser";
            this.btn_NewUser.Size = new System.Drawing.Size(109, 40);
            this.btn_NewUser.TabIndex = 6;
            this.btn_NewUser.Text = "Create User";
            this.btn_NewUser.UseVisualStyleBackColor = false;
            this.btn_NewUser.Click += new System.EventHandler(this.btn_NewUser_Click);
            // 
            // Label3
            // 
            this.Label3.AutoSize = true;
            this.Label3.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label3.Location = new System.Drawing.Point(46, 91);
            this.Label3.Name = "Label3";
            this.Label3.Size = new System.Drawing.Size(88, 16);
            this.Label3.TabIndex = 114;
            this.Label3.Text = "User Group";
            this.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cboUserGroup
            // 
            this.cboUserGroup.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboUserGroup.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboUserGroup.FormattingEnabled = true;
            this.cboUserGroup.Location = new System.Drawing.Point(139, 90);
            this.cboUserGroup.Name = "cboUserGroup";
            this.cboUserGroup.Size = new System.Drawing.Size(239, 22);
            this.cboUserGroup.TabIndex = 2;
            // 
            // Label5
            // 
            this.Label5.AutoSize = true;
            this.Label5.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label5.Location = new System.Drawing.Point(-5, 154);
            this.Label5.Name = "Label5";
            this.Label5.Size = new System.Drawing.Size(139, 16);
            this.Label5.TabIndex = 113;
            this.Label5.Text = "Confirm Password";
            this.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Label4
            // 
            this.Label4.AutoSize = true;
            this.Label4.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label4.Location = new System.Drawing.Point(21, 123);
            this.Label4.Name = "Label4";
            this.Label4.Size = new System.Drawing.Size(113, 16);
            this.Label4.TabIndex = 112;
            this.Label4.Text = "New Password";
            this.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Label2
            // 
            this.Label2.AutoSize = true;
            this.Label2.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label2.Location = new System.Drawing.Point(73, 27);
            this.Label2.Name = "Label2";
            this.Label2.Size = new System.Drawing.Size(61, 16);
            this.Label2.TabIndex = 106;
            this.Label2.Text = "User ID";
            this.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(47, 61);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(87, 16);
            this.label1.TabIndex = 105;
            this.label1.Text = "User Name";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tabUserSettings
            // 
            this.tabUserSettings.BackColor = System.Drawing.SystemColors.Control;
            this.tabUserSettings.Controls.Add(this.txtUserConfirmPass);
            this.tabUserSettings.Controls.Add(this.txtUserNewPass);
            this.tabUserSettings.Controls.Add(this.btn_chaneUserPass);
            this.tabUserSettings.Controls.Add(this.Label6);
            this.tabUserSettings.Controls.Add(this.Label8);
            this.tabUserSettings.Controls.Add(this.btn_Display);
            this.tabUserSettings.Controls.Add(this.cboAllUser);
            this.tabUserSettings.Controls.Add(this.menu_Grid);
            this.tabUserSettings.Controls.Add(this.Label7);
            this.tabUserSettings.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabUserSettings.Location = new System.Drawing.Point(4, 23);
            this.tabUserSettings.Name = "tabUserSettings";
            this.tabUserSettings.Padding = new System.Windows.Forms.Padding(3);
            this.tabUserSettings.Size = new System.Drawing.Size(459, 509);
            this.tabUserSettings.TabIndex = 1;
            this.tabUserSettings.Text = "User Settings";
            // 
            // txtUserConfirmPass
            // 
            this.txtUserConfirmPass.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtUserConfirmPass.Font = new System.Drawing.Font("Trebuchet MS", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtUserConfirmPass.Location = new System.Drawing.Point(151, 73);
            this.txtUserConfirmPass.Name = "txtUserConfirmPass";
            this.txtUserConfirmPass.PasswordChar = '*';
            this.txtUserConfirmPass.Size = new System.Drawing.Size(239, 23);
            this.txtUserConfirmPass.TabIndex = 124;
            // 
            // txtUserNewPass
            // 
            this.txtUserNewPass.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtUserNewPass.Font = new System.Drawing.Font("Trebuchet MS", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtUserNewPass.Location = new System.Drawing.Point(151, 44);
            this.txtUserNewPass.Name = "txtUserNewPass";
            this.txtUserNewPass.PasswordChar = '*';
            this.txtUserNewPass.Size = new System.Drawing.Size(239, 23);
            this.txtUserNewPass.TabIndex = 123;
            // 
            // btn_chaneUserPass
            // 
            this.btn_chaneUserPass.BackColor = System.Drawing.SystemColors.Control;
            this.btn_chaneUserPass.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_chaneUserPass.Location = new System.Drawing.Point(232, 105);
            this.btn_chaneUserPass.Name = "btn_chaneUserPass";
            this.btn_chaneUserPass.Size = new System.Drawing.Size(150, 40);
            this.btn_chaneUserPass.TabIndex = 121;
            this.btn_chaneUserPass.Text = "Change Password";
            this.btn_chaneUserPass.UseVisualStyleBackColor = false;
            this.btn_chaneUserPass.Click += new System.EventHandler(this.btn_chaneUserPass_Click);
            // 
            // Label6
            // 
            this.Label6.AutoSize = true;
            this.Label6.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label6.Location = new System.Drawing.Point(5, 78);
            this.Label6.Name = "Label6";
            this.Label6.Size = new System.Drawing.Size(139, 16);
            this.Label6.TabIndex = 120;
            this.Label6.Text = "Confirm Password";
            this.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Label8
            // 
            this.Label8.AutoSize = true;
            this.Label8.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label8.Location = new System.Drawing.Point(31, 47);
            this.Label8.Name = "Label8";
            this.Label8.Size = new System.Drawing.Size(113, 16);
            this.Label8.TabIndex = 119;
            this.Label8.Text = "New Password";
            this.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btn_Display
            // 
            this.btn_Display.BackColor = System.Drawing.SystemColors.Control;
            this.btn_Display.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Display.Location = new System.Drawing.Point(121, 105);
            this.btn_Display.Name = "btn_Display";
            this.btn_Display.Size = new System.Drawing.Size(83, 40);
            this.btn_Display.TabIndex = 116;
            this.btn_Display.Text = "Display";
            this.btn_Display.UseVisualStyleBackColor = false;
            this.btn_Display.Click += new System.EventHandler(this.btn_Display_Click);
            // 
            // cboAllUser
            // 
            this.cboAllUser.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboAllUser.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboAllUser.FormattingEnabled = true;
            this.cboAllUser.Location = new System.Drawing.Point(150, 14);
            this.cboAllUser.Name = "cboAllUser";
            this.cboAllUser.Size = new System.Drawing.Size(231, 22);
            this.cboAllUser.TabIndex = 9;
            // 
            // menu_Grid
            // 
            this.menu_Grid.AllowUserToAddRows = false;
            this.menu_Grid.AllowUserToDeleteRows = false;
            this.menu_Grid.AllowUserToResizeColumns = false;
            this.menu_Grid.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.NavajoWhite;
            this.menu_Grid.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.menu_Grid.BackgroundColor = System.Drawing.SystemColors.Control;
            this.menu_Grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.menu_Grid.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.menu_Grid.GridColor = System.Drawing.SystemColors.Control;
            this.menu_Grid.Location = new System.Drawing.Point(3, 173);
            this.menu_Grid.Name = "menu_Grid";
            this.menu_Grid.RowHeadersWidth = 30;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.Moccasin;
            this.menu_Grid.RowsDefaultCellStyle = dataGridViewCellStyle2;
            this.menu_Grid.Size = new System.Drawing.Size(453, 333);
            this.menu_Grid.TabIndex = 112;
            this.menu_Grid.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.menu_Grid_CellValueChanged);
            // 
            // Label7
            // 
            this.Label7.AutoSize = true;
            this.Label7.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label7.Location = new System.Drawing.Point(57, 16);
            this.Label7.Name = "Label7";
            this.Label7.Size = new System.Drawing.Size(87, 16);
            this.Label7.TabIndex = 109;
            this.Label7.Text = "User Name";
            this.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // chkSalesPerson
            // 
            this.chkSalesPerson.AutoSize = true;
            this.chkSalesPerson.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkSalesPerson.Location = new System.Drawing.Point(139, 219);
            this.chkSalesPerson.Name = "chkSalesPerson";
            this.chkSalesPerson.Size = new System.Drawing.Size(129, 18);
            this.chkSalesPerson.TabIndex = 124;
            this.chkSalesPerson.Text = "Is Sales Person";
            this.chkSalesPerson.UseVisualStyleBackColor = true;
            // 
            // frmUserSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(467, 536);
            this.ControlBox = false;
            this.Controls.Add(this.TabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.KeyPreview = true;
            this.Name = "frmUserSettings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "User Settings";
            this.Load += new System.EventHandler(this.frmUserSettings_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmUserSettings_KeyDown);
            this.TabControl1.ResumeLayout(false);
            this.tabNewUser.ResumeLayout(false);
            this.tabNewUser.PerformLayout();
            this.tabUserSettings.ResumeLayout(false);
            this.tabUserSettings.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.menu_Grid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        internal System.Windows.Forms.TabControl TabControl1;
        internal System.Windows.Forms.TabPage tabNewUser;
        internal System.Windows.Forms.Button btn_ChangePasword;
        internal System.Windows.Forms.Label lblOldPassword;
        internal System.Windows.Forms.Button btn_Close;
        internal System.Windows.Forms.Button btn_NewUser;
        internal System.Windows.Forms.Label Label3;
        internal System.Windows.Forms.ComboBox cboUserGroup;
        internal System.Windows.Forms.Label Label5;
        internal System.Windows.Forms.Label Label4;
        internal System.Windows.Forms.Label Label2;
        internal System.Windows.Forms.Label label1;
        internal System.Windows.Forms.TabPage tabUserSettings;
        internal System.Windows.Forms.Button btn_chaneUserPass;
        internal System.Windows.Forms.Label Label6;
        internal System.Windows.Forms.Label Label8;
        internal System.Windows.Forms.Button btn_Display;
        internal System.Windows.Forms.ComboBox cboAllUser;
        internal System.Windows.Forms.Label Label7;
        internal System.Windows.Forms.TextBox txtUserId;
        internal System.Windows.Forms.TextBox txtUserName;
        internal System.Windows.Forms.TextBox txtNewPassword;
        internal System.Windows.Forms.TextBox txtOldPassword;
        internal System.Windows.Forms.TextBox txtConfirmPassword;
        internal System.Windows.Forms.TextBox txtUserConfirmPass;
        internal System.Windows.Forms.TextBox txtUserNewPass;
        private System.Windows.Forms.DataGridView menu_Grid;
        private System.Windows.Forms.CheckBox chkSalesPerson;
    }
}