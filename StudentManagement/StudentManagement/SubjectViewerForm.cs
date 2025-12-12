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
    public partial class SubjectViewerForm : Form
    {
        public SubjectViewerForm()
        {
            InitializeComponent();
        }

        private DataTable GetSubjectsData()
        {
            string cs = "Data Source=DESKTOP-ADM7KIF;Initial Catalog=Studentdb;Integrated Security=True";
            DataTable dt = new DataTable();

            string query = @"
             SELECT 
                 SubjectID,
                 Name,
                 Lecturer,
                 PassMark,
                 Module
             FROM 
                 [dbo].[Subjects]";

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

        private void SubjectViewerForm_Load(object sender, EventArgs e)
        {
            try
            {
                DataTable salesData = GetSubjectsData();

                if (salesData == null || salesData.Rows.Count == 0)
                {
                    MessageBox.Show("No sales data found.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                SubjectReport myReport = new SubjectReport();
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
