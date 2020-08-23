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
    public partial class BackupRestore : Form
    {
        public BackupRestore()
        {
            InitializeComponent();
        }

        private void BackupRestore_Load(object sender, EventArgs e)
        {

        }

        private void btnBrowseBackup_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                txtBackupPath.Text = fbd.SelectedPath;
                btnBackup.Enabled = true;
            }
        }

        private void btnBackup_Click(object sender, EventArgs e)
        {
            if (txtBackupPath.Text == string.Empty)
            {
                MessageBox.Show("Please Enter a backup File Location ");
            }
            else
            {
                string query = "BACKUP DATABASE [" + DBClass.DBName + "] TO DISK='" + txtBackupPath.Text + "\\"+"Database" + "-"+DateTime.Now.ToString("yyyy-MM-dd--HH-mm-ss")+".bak'" ;
                DBClass.ExecuteNonQuery(query);
                MessageBox.Show("Backup taken Successfully", "Backup Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnBackup.Enabled = false;
            }

        }

        private void btnBrowseRestore_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "SQL SERVER databse backup files|*.bak";
            ofd.Title = "Database Restore";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                txtRestorePath.Text = ofd.FileName;
                btnRestore.Enabled = true;
            }
        }

        private void btnRestore_Click(object sender, EventArgs e)
        {
            string sql1 = string.Format("ALTER DATABASE ["+ DBClass.DBName + "] SET SINGLE_USER WITH ROLLBACK IMMEDIATE");
            DBClass.ExecuteNonQuery(sql1);

            string SQL2 = string.Format("USE MASTER RESTORE DATABASE [" + DBClass.DBName + "] FROM DISK='" + txtRestorePath.Text +"' WITH REPLACE;" );
            DBClass.ExecuteNonQuery(SQL2);

            string sql3 = string.Format("ALTER DATABASE [" + DBClass.DBName + "] SET MULTI_USER");
            DBClass.ExecuteNonQuery(sql3);

            MessageBox.Show("Database Restore Successfully", "Restore Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            btnRestore.Enabled = false;
        }

        private void BackupRestore_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }
    }
}
