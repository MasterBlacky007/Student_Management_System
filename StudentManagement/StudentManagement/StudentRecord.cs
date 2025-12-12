using CrystalDecisions.Windows.Forms;
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
    public partial class StudentRecord : Form
    {
        public StudentRecord()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string studentId = txtStID.Text;
            string connectionString = "Data Source=DESKTOP-ADM7KIF;Initial Catalog=Studentdb;Integrated Security=True";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    string query = @"
            SELECT 
                r.SubjectID,
                s.Name AS SubjectName,
                s.PassMark,
                r.TotalMark AS MarkObtained,
                r.PassStatus,
                r.Grade,
                r.GPA
            FROM 
                [dbo].[Result] r
            INNER JOIN 
                [dbo].[Subjects] s
            ON 
                r.SubjectID = s.SubjectID
            WHERE 
                r.StudentID = @StudentID";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@StudentID", studentId);

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    if (dataTable.Rows.Count > 0)
                    {
                        // Clear existing rows in the grid
                        dataGridView1.Rows.Clear();

                        // Loop through each row and add it to the DataGridView
                        foreach (DataRow row in dataTable.Rows)
                        {
                            dataGridView1.Rows.Add(
                                row["SubjectID"],
                                row["SubjectName"],
                                row["PassMark"],
                                row["MarkObtained"],
                                row["PassStatus"],
                                row["Grade"],
                                row["GPA"]
                            );
                        }
                    }
                    else
                    {
                        MessageBox.Show("No results found for the specified Student ID.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private DataTable GetStudentResultsData(string studentId)
        {
            string cs = "Data Source=DESKTOP-ADM7KIF;Initial Catalog=Studentdb;Integrated Security=True";
            DataTable dt = new DataTable();

            string query = @"
                    SELECT 
                        r.SubjectID,
                        s.Name AS SubjectName,
                        s.PassMark,
                        r.TotalMark AS MarkObtained,
                        r.PassStatus,
                        r.Grade,
                        r.GPA
                    FROM 
                        [dbo].[Result] r
                    INNER JOIN 
                        [dbo].[Subjects] s
                    ON 
                        r.SubjectID = s.SubjectID
                    WHERE 
                        r.StudentID = @StudentID";

            using (SqlConnection conn = new SqlConnection(cs))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@StudentID", studentId);
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(dt);
                }
            }

            return dt;
        }

        private void GenerateStudentResultsReport(string studentId)
        {
            try
            {
                // Get data for the specified student ID
                DataTable studentResults = GetStudentResultsData(studentId);

                if (studentResults == null || studentResults.Rows.Count == 0)
                {
                    MessageBox.Show("No results found for the specified Student ID.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // Create an instance of the Crystal Report
                StudentReport myReport = new StudentReport(); // Replace with your actual report class name
                myReport.SetDataSource(studentResults);

                ReportViewerForm reportViewer = new ReportViewerForm(myReport);
                reportViewer.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error generating the report: " + ex.Message, "Report Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnPrint_Click(object sender, EventArgs e)
        {
            string studentId = txtStID.Text.Trim();

            if (string.IsNullOrEmpty(studentId))
            {
                MessageBox.Show("Please enter a Student ID.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            GenerateStudentResultsReport(studentId);
        }

        private void StudentRecord_Load(object sender, EventArgs e)
        {

        }
    }
}
