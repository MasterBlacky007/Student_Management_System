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
    public partial class Result : Form
    {
        public Result()
        {
            InitializeComponent();
        }

        private void LoadResultData()
        {
            string cs = "Data Source=DESKTOP-ADM7KIF;Initial Catalog=Studentdb;Integrated Security=True";
            SqlConnection conn = new SqlConnection(cs);
            conn.Open();

            // Query to fetch all records from the result table
            string sql = "SELECT * FROM Result"; // Replace "ResultTable" with your actual result table name
            SqlDataAdapter da = new SqlDataAdapter(sql, conn);
            DataTable dt = new DataTable();
            da.Fill(dt);

            dataGridViewResult.DataSource = dt; // Bind the DataTable to the Result DataGridView
            conn.Close();
        }

        private void LoadDataToGrid()
        {
            string connectionString = "Data Source=DESKTOP-ADM7KIF;Initial Catalog=Studentdb;Integrated Security=True";
            SqlConnection conn = new SqlConnection(connectionString);

            try
            {
                conn.Open();
                string query = "SELECT * FROM Students";
                SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                dataGridView1.DataSource = dataTable; // Ensure 'dataGridView1' matches the name of your DataGridView control
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        private void LoadSubjectsToGrid()
        {
            string connectionString = "Data Source=DESKTOP-ADM7KIF;Initial Catalog=Studentdb;Integrated Security=True";
            SqlConnection conn = new SqlConnection(connectionString);

            try
            {
                conn.Open();
                string query = "SELECT * FROM Subjects";
                SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                dataGridView2.DataSource = dataTable;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            // Get input values and trim spaces
            string studentID = txtStudentID.Text.Trim();
            string subjectID = txtSubjectID.Text.Trim();
            string paperMarkInput = txtPaperMark.Text.Trim();
            string cwMarkInput = txtCWMark.Text.Trim();

            // Validate all inputs
            if (string.IsNullOrEmpty(studentID) ||
                string.IsNullOrEmpty(subjectID) ||
                string.IsNullOrEmpty(paperMarkInput) ||
                string.IsNullOrEmpty(cwMarkInput))
            {
                MessageBox.Show("All fields must be filled. Please complete the form.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Validate paperMark and cwMark as integers between 0 and 100
            int paperMark, cwMark;
            if (!int.TryParse(paperMarkInput, out paperMark) || paperMark < 0 || paperMark > 100)
            {
                MessageBox.Show("Invalid Paper Mark. Please enter a value between 0 and 100.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!int.TryParse(cwMarkInput, out cwMark) || cwMark < 0 || cwMark > 100)
            {
                MessageBox.Show("Invalid CW Mark. Please enter a value between 0 and 100.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Connection string
            string cs = "Data Source=DESKTOP-ADM7KIF;Initial Catalog=Studentdb;Integrated Security=True";

            using (SqlConnection conn = new SqlConnection(cs))
            {
                conn.Open();

                // Check if StudentID exists
                string checkStudentQuery = "SELECT COUNT(*) FROM Students WHERE StudentID = @sid";
                SqlCommand checkStudentCmd = new SqlCommand(checkStudentQuery, conn);
                checkStudentCmd.Parameters.AddWithValue("@sid", studentID);
                int studentExists = (int)checkStudentCmd.ExecuteScalar();

                // Check if SubjectID exists
                string checkSubjectQuery = "SELECT COUNT(*) FROM Subjects WHERE SubjectID = @subid";
                SqlCommand checkSubjectCmd = new SqlCommand(checkSubjectQuery, conn);
                checkSubjectCmd.Parameters.AddWithValue("@subid", subjectID);
                int subjectExists = (int)checkSubjectCmd.ExecuteScalar();

                // If both exist, proceed with grade and GPA calculation
                if (studentExists > 0 && subjectExists > 0)
                {
                    // Calculate Total Marks
                    int totalMark = (paperMark + cwMark) / 2;

                    // Calculate Grade and GPA
                    string grade;
                    double gpa;

                    if (totalMark >= 75) { grade = "A"; gpa = 4.0; }
                    else if (totalMark >= 65) { grade = "B"; gpa = 3.0; }
                    else if (totalMark >= 50) { grade = "C"; gpa = 2.0; }
                    else if (totalMark == 0) { grade = "X"; gpa = 0.0; }
                    else { grade = "F"; gpa = 0.0; }

                    // Get Pass Mark and determine Pass/Repeat status
                    string getPassMarkQuery = "SELECT PassMark FROM Subjects WHERE SubjectID = @subid";
                    SqlCommand getPassMarkCmd = new SqlCommand(getPassMarkQuery, conn);
                    getPassMarkCmd.Parameters.AddWithValue("@subid", subjectID);
                    int passMark = (int)getPassMarkCmd.ExecuteScalar();

                    string passStatus = totalMark >= passMark ? "Pass" : "Repeat";

                    // Display results in text boxes and labels
                    txtGrade.Text = grade;
                    txtGPA.Text = gpa.ToString("0.00");
                    textResult.Text = passStatus;
                }
                else
                {
                    // Display error message if either StudentID or SubjectID does not exist
                    if (studentExists == 0)
                    {
                        MessageBox.Show($"StudentID '{studentID}' does not exist in the Students table.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    if (subjectExists == 0)
                    {
                        MessageBox.Show($"SubjectID '{subjectID}' does not exist in the Subjects table.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtStudentID.Clear();
            txtSubjectID.Clear();
            txtPaperMark.Clear();
            txtCWMark.Clear();
            txtGrade.Clear();
            txtGPA.Clear();
            textResult.Clear();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            // Get input values and trim spaces
            string studentID = txtStudentID.Text.Trim();
            string subjectID = txtSubjectID.Text.Trim();
            string paperMarkInput = txtPaperMark.Text.Trim();
            string cwMarkInput = txtCWMark.Text.Trim();
            string grade = txtGrade.Text.Trim();
            string gpaInput = txtGPA.Text.Trim();

            // Validate all fields are filled
            if (string.IsNullOrEmpty(studentID) ||
                string.IsNullOrEmpty(subjectID) ||
                string.IsNullOrEmpty(paperMarkInput) ||
                string.IsNullOrEmpty(cwMarkInput) ||
                string.IsNullOrEmpty(grade) ||
                string.IsNullOrEmpty(gpaInput))
            {
                MessageBox.Show("All fields must be filled. Please complete the form.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Validate paperMark and cwMark as integers between 0 and 100
            int paperMark, cwMark;
            double gpa;

            // Attempt to parse paperMark and cwMark. If they are not valid integers, set them to 0.
            paperMark = int.TryParse(paperMarkInput, out paperMark) && paperMark >= 0 && paperMark <= 100 ? paperMark : 0;
            cwMark = int.TryParse(cwMarkInput, out cwMark) && cwMark >= 0 && cwMark <= 100 ? cwMark : 0;

            // Validate GPA as a valid double between 0.0 and 4.0
            if (!double.TryParse(gpaInput, out gpa) || gpa < 0.0 || gpa > 4.0)
            {
                MessageBox.Show("Invalid GPA. Please enter a value between 0.0 and 4.0.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Calculate total marks (average of PaperMark and CWMark)
            int totalMark = (paperMark + cwMark) / 2;

            // Check for uniqueness and insert data into the database
            string cs = "Data Source=DESKTOP-ADM7KIF;Initial Catalog=Studentdb;Integrated Security=True";
            using (SqlConnection conn = new SqlConnection(cs))
            {
                conn.Open();

                // Check if the combination of StudentID and SubjectID already exists
                string checkQuery = "SELECT COUNT(*) FROM Result WHERE StudentID = @sid AND SubjectID = @subid";
                SqlCommand checkCmd = new SqlCommand(checkQuery, conn);
                checkCmd.Parameters.AddWithValue("@sid", studentID);
                checkCmd.Parameters.AddWithValue("@subid", subjectID);

                int recordExists = (int)checkCmd.ExecuteScalar();
                if (recordExists > 0)
                {
                    MessageBox.Show($"Result for StudentID '{studentID}' and SubjectID '{subjectID}' already exists.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Insert data if unique
                string insertQuery = "INSERT INTO Result (StudentID, SubjectID, PaperMark, CWMark, TotalMark, Grade, GPA, PassStatus) VALUES (@sid, @subid, @paper, @cw, @total, @grade, @gpa, @pass)";
                SqlCommand insertCmd = new SqlCommand(insertQuery, conn);
                insertCmd.Parameters.AddWithValue("@sid", studentID);
                insertCmd.Parameters.AddWithValue("@subid", subjectID);
                insertCmd.Parameters.AddWithValue("@paper", paperMark);
                insertCmd.Parameters.AddWithValue("@cw", cwMark);
                insertCmd.Parameters.AddWithValue("@total", totalMark);
                insertCmd.Parameters.AddWithValue("@grade", grade);
                insertCmd.Parameters.AddWithValue("@gpa", gpa);
                insertCmd.Parameters.AddWithValue("@pass", totalMark >= 50 ? "Pass" : "Repeat"); // Adjust as per your pass mark logic

                insertCmd.ExecuteNonQuery();
                MessageBox.Show("Result Added to Database!");
            }

            // Refresh the result grid
            LoadResultData();

        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string studentID = txtStID.Text;
            string subjectID = txtSuID.Text;

            string cs = "Data Source=DESKTOP-ADM7KIF;Initial Catalog=Studentdb;Integrated Security=True";
            SqlConnection conn = new SqlConnection(cs);
            conn.Open();

            string sql = "SELECT * FROM Result WHERE StudentID=@sid AND SubjectID=@subid";
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@sid", studentID);
            cmd.Parameters.AddWithValue("@subid", subjectID);

            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                txtStudentID.Text = dr["StudentID"].ToString();
                txtSubjectID.Text = dr["SubjectID"].ToString();
                txtPaperMark.Text = dr["PaperMark"].ToString();
                txtCWMark.Text = dr["CWMark"].ToString();
                txtGrade.Text = dr["Grade"].ToString();
                txtGPA.Text = dr["GPA"].ToString();
                
            }
            else
            {
                MessageBox.Show("Record Not Found.");
            }
            conn.Close();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            string studentID = txtStID.Text;
            string subjectID = txtSuID.Text;

            string cs = "Data Source=DESKTOP-ADM7KIF;Initial Catalog=Studentdb;Integrated Security=True";
            SqlConnection conn = new SqlConnection(cs);
            conn.Open();

            string sql = "DELETE FROM Result WHERE StudentID=@sid AND SubjectID=@subid";
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@sid", studentID);
            cmd.Parameters.AddWithValue("@subid", subjectID);

            cmd.ExecuteNonQuery();
            conn.Close();

            MessageBox.Show("Record Deleted!");
            LoadResultData();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            // Get input values and trim spaces
            string studentID = txtStudentID.Text.Trim();
            string subjectID = txtSubjectID.Text.Trim();
            string paperMarkInput = txtPaperMark.Text.Trim();
            string cwMarkInput = txtCWMark.Text.Trim();
            string grade = txtGrade.Text.Trim();
            string gpaInput = txtGPA.Text.Trim();

            // Validate all fields are filled
            if (string.IsNullOrEmpty(studentID) ||
                string.IsNullOrEmpty(subjectID) ||
                string.IsNullOrEmpty(paperMarkInput) ||
                string.IsNullOrEmpty(cwMarkInput) ||
                string.IsNullOrEmpty(grade) ||
                string.IsNullOrEmpty(gpaInput))
            {
                MessageBox.Show("All fields must be filled. Please complete the form.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Validate paperMark and cwMark as integers between 0 and 100
            int paperMark, cwMark;
            double gpa;

            // Attempt to parse paperMark and cwMark. If they are not valid integers, set them to 0.
            paperMark = int.TryParse(paperMarkInput, out paperMark) && paperMark >= 0 && paperMark <= 100 ? paperMark : 0;
            cwMark = int.TryParse(cwMarkInput, out cwMark) && cwMark >= 0 && cwMark <= 100 ? cwMark : 0;

            // Validate GPA as a valid double between 0.0 and 4.0
            if (!double.TryParse(gpaInput, out gpa) || gpa < 0.0 || gpa > 4.0)
            {
                MessageBox.Show("Invalid GPA. Please enter a value between 0.0 and 4.0.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Calculate total marks (average of PaperMark and CWMark)
            int totalMark = (paperMark + cwMark) / 2;

            // Database connection string
            string cs = "Data Source=DESKTOP-ADM7KIF;Initial Catalog=Studentdb;Integrated Security=True";
            using (SqlConnection conn = new SqlConnection(cs))
            {
                conn.Open();

                // SQL Update query to update the result record (this will update the record if it exists)
                string sql = "UPDATE Result SET PaperMark=@pmark, CWMark=@cmark, TotalMark=@tmark, Grade=@grade, GPA=@gpa, PassStatus=@pass WHERE StudentID=@sid AND SubjectID=@subid";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@pmark", paperMark);
                cmd.Parameters.AddWithValue("@cmark", cwMark);
                cmd.Parameters.AddWithValue("@tmark", totalMark);
                cmd.Parameters.AddWithValue("@grade", grade);
                cmd.Parameters.AddWithValue("@gpa", gpa);
                cmd.Parameters.AddWithValue("@pass", totalMark >= 50 ? "Pass" : "Repeat"); // Adjust as per your pass mark logic
                cmd.Parameters.AddWithValue("@sid", studentID);
                cmd.Parameters.AddWithValue("@subid", subjectID);

                // Execute the update query
                cmd.ExecuteNonQuery();
                conn.Close();

                // Notify the user and refresh the grid
                MessageBox.Show("Record Updated!");
                LoadResultData();
            }

        }

        private void Result_Load(object sender, EventArgs e)
        {
            LoadResultData();
            LoadDataToGrid();
            LoadSubjectsToGrid();
        }
    }
}
