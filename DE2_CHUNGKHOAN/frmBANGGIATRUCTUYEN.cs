using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DE2_CHUNGKHOAN
{
    public partial class frmBGTT : DevExpress.XtraEditors.XtraForm
    {
        public string connectString = @"Data Source=MAIANH;Initial Catalog=DE2;User ID=sa;Password=12";

        public SqlConnection connection = null;
        public delegate void NewHome();
        public event NewHome OnNewHome;

        public frmBGTT()
        {
            InitializeComponent();

            try
            {
                SqlClientPermission ss = new SqlClientPermission(System.Security.Permissions.PermissionState.Unrestricted);
                ss.Demand();

                //hủy kết nối 
                SqlDependency.Stop(connectString);


                // Bắt đầu kết nối 
                SqlDependency.Start(connectString);

                //tạo kết nối dependency  
                connection = new SqlConnection(connectString);
            } 
            catch (Exception err)
            {
                MessageBox.Show("Ban chua mo dich vu SQL Broker", err.Message, MessageBoxButtons.OK);
            }

           
        }

        private void bANGGIATRUCTUYENBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.bANGGIATRUCTUYENBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this.dE2DataSet);

        }

        private void frmBGTT_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'dE2DataSet.BANGGIATRUCTUYEN' table. You can move, or remove it, as needed.
            this.bANGGIATRUCTUYENTableAdapter.Fill(this.dE2DataSet.BANGGIATRUCTUYEN);

            try
            {
                OnNewHome += new NewHome(Form1_OnNewHome);

                //load data vào datagrid
                LoadData();
            }
            catch (Exception err)
            {
                MessageBox.Show("Ban chua mo dich vu SQL Broker", err.Message, MessageBoxButtons.OK);
            }

        }

        public void Form1_OnNewHome()
        {
            ISynchronizeInvoke i = (ISynchronizeInvoke)this;
            if (i.InvokeRequired)//tab
            {
                NewHome dd = new NewHome(Form1_OnNewHome);
                i.BeginInvoke(dd, null);
                return;
            }

            LoadData();
        }

        // load dữ liệu
        void LoadData()
        {
            DataTable dt = new DataTable();
            try
            {
                // mở kết nối sqlConnection
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }

                SqlCommand cmd = new SqlCommand("EXEC SelectAllBGTT", connection);
                cmd.Notification = null;


                // Tạo 1 dependency và liên kết nó với đối tượng SqlCommand
                SqlDependency de = new SqlDependency(cmd);

                //nó bắt đầu chạy khi nhận được query của sql
                de.OnChange += new OnChangeEventHandler(dependency_OnChange);

                dt.Load(cmd.ExecuteReader(CommandBehavior.CloseConnection));
                bANGGIATRUCTUYENDataGridView.DataSource = dt;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void dependency_OnChange(object sender, SqlNotificationEventArgs e)
        {
            SqlDependency de = sender as SqlDependency;
            de.OnChange -= dependency_OnChange;
            if (OnNewHome != null)
            {
                OnNewHome();
            }
        }

        private void bANGGIATRUCTUYENDataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
