using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StudentManagement
{
    public partial class studentreport : Form
    {
        public studentreport()
        {
            InitializeComponent();
        }

        private DataTable GetStudentsData()
        {
            string cs = "Data Source=DESKTOP-ADM7KIF;Initial Catalog=Studentdb;Integrated Security=True";
            DataTable dt = new DataTable();

            string query = @"
             SELECT 
                 StudentID,
                 BatchID,
                 Name,
                 Age,
                 EnrollmentDate
             FROM 
                 [dbo].[Students]";

            using (SqlConnection conn = new SqlConnection(cs))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(dt);
                }
            }

            return dt;
        }
        private void studentreport_Load(object sender, EventArgs e)
        {
            try
            {
                DataTable salesData = GetStudentsData();

                if (salesData == null || salesData.Rows.Count == 0)
                {
                    MessageBox.Show("No sales data found.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                StudentCrystalReport myReport = new StudentCrystalReport();
                myReport.SetDataSource(salesData);

                crystalReportViewer1.ReportSource = myReport;
                crystalReportViewer1.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Report Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
