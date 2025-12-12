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
    public partial class Register : Form
    {
        public Register()
        {
            InitializeComponent();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtStudentID.Clear();
            cmbBatchID.SelectedIndex = -1;
            txtName.Clear();
            txtAge.Clear();
            dtpEnrollmentDate.Value = DateTime.Now;
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

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string studentId = txtStudentID.Text.Trim();
            string batchId = cmbBatchID.Text.Trim();
            string name = txtName.Text.Trim();
            int age;
            DateTime enrollmentDate = dtpEnrollmentDate.Value;

            // Validate that no fields are empty
            if (string.IsNullOrEmpty(studentId) ||
                string.IsNullOrEmpty(batchId) ||
                string.IsNullOrEmpty(name) ||
                string.IsNullOrEmpty(txtAge.Text))
            {
                MessageBox.Show("All fields must be filled. Please complete the form.");
                return;
            }

            // Validate Batch ID
            if (batchId != "DSE" && batchId != "DNE" && batchId != "DCSD")
            {
                MessageBox.Show("Invalid Batch ID. Please select from DSE, DNE, or DCSD.");
                return;
            }

            // Validate Age
            if (!int.TryParse(txtAge.Text, out age) || age < 1 || age > 100)
            {
                MessageBox.Show("Invalid age. Please enter a value between 1 and 100.");
                return;
            }

            // Validate Name (only letters and spaces are allowed)
            if (!System.Text.RegularExpressions.Regex.IsMatch(name, @"^[a-zA-Z\s]+$"))
            {
                MessageBox.Show("Invalid name. The name should only contain letters and spaces.");
                return;
            }

            // Validate Enrollment Date
            if (enrollmentDate.Date < DateTime.Now.Date)
            {
                MessageBox.Show("Invalid enrollment date. The date cannot be before today.");
                return;
            }

            string connectionString = "Data Source=DESKTOP-ADM7KIF;Initial Catalog=Studentdb;Integrated Security=True";
            SqlConnection conn = new SqlConnection(connectionString);

            try
            {
                conn.Open();
                string query = "INSERT INTO Students (StudentID, BatchID, Name, Age, EnrollmentDate) VALUES (@StudentID, @BatchID, @Name, @Age, @EnrollmentDate)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@StudentID", studentId);
                cmd.Parameters.AddWithValue("@BatchID", batchId);
                cmd.Parameters.AddWithValue("@Name", name);
                cmd.Parameters.AddWithValue("@Age", age);
                cmd.Parameters.AddWithValue("@EnrollmentDate", enrollmentDate);

                cmd.ExecuteNonQuery();
                MessageBox.Show("Student registered successfully!");

                LoadDataToGrid(); // Refresh the data in the grid
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

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string studentId = txtStID.Text;
            string connectionString = "Data Source=DESKTOP-ADM7KIF;Initial Catalog=Studentdb;Integrated Security=True";
            SqlConnection conn = new SqlConnection(connectionString);

            try
            {
                conn.Open();
                string query = "SELECT * FROM Students WHERE StudentID = @StudentID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@StudentID", studentId);

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    cmbBatchID.Text = reader["BatchID"].ToString();
                    txtName.Text = reader["Name"].ToString();
                    txtAge.Text = reader["Age"].ToString();
                    dtpEnrollmentDate.Value = Convert.ToDateTime(reader["EnrollmentDate"]);
                }
                else
                {
                    MessageBox.Show("Student not found.");
                }
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

        private void btnDelete_Click(object sender, EventArgs e)
        {
            string studentId = txtStID.Text;
            string connectionString = "Data Source=DESKTOP-ADM7KIF;Initial Catalog=Studentdb;Integrated Security=True";
            SqlConnection conn = new SqlConnection(connectionString);

            try
            {
                conn.Open();
                string query = "DELETE FROM Students WHERE StudentID = @StudentID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@StudentID", studentId);

                cmd.ExecuteNonQuery();
                MessageBox.Show("Student record deleted successfully!");

                LoadDataToGrid(); // Refresh the data in the grid
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

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            string studentId = txtStID.Text.Trim();
            string batchId = cmbBatchID.Text.Trim();
            string name = txtName.Text.Trim();
            int age;
            DateTime enrollmentDate = dtpEnrollmentDate.Value;

            // Validate that no fields are empty
            if (string.IsNullOrEmpty(studentId) ||
                string.IsNullOrEmpty(batchId) ||
                string.IsNullOrEmpty(name) ||
                string.IsNullOrEmpty(txtAge.Text))
            {
                MessageBox.Show("All fields must be filled. Please complete the form.");
                return;
            }

            // Validate Batch ID
            if (batchId != "DSE" && batchId != "DNE" && batchId != "DCSD")
            {
                MessageBox.Show("Invalid Batch ID. Please select from DSE, DNE, or DCSD.");
                return;
            }

            // Validate Age
            if (!int.TryParse(txtAge.Text, out age) || age < 1 || age > 100)
            {
                MessageBox.Show("Invalid age. Please enter a value between 1 and 100.");
                return;
            }

            // Validate Name (only letters and spaces are allowed)
            if (!System.Text.RegularExpressions.Regex.IsMatch(name, @"^[a-zA-Z\s]+$"))
            {
                MessageBox.Show("Invalid name. The name should only contain letters and spaces.");
                return;
            }

            // Validate Enrollment Date
            if (enrollmentDate.Date < DateTime.Now.Date)
            {
                MessageBox.Show("Invalid enrollment date. The date cannot be before today.");
                return;
            }

            // Database connection and update logic
            string connectionString = "Data Source=DESKTOP-ADM7KIF;Initial Catalog=Studentdb;Integrated Security=True";
            SqlConnection conn = new SqlConnection(connectionString);

            try
            {
                conn.Open();
                string query = "UPDATE Students SET BatchID = @BatchID, Name = @Name, Age = @Age, EnrollmentDate = @EnrollmentDate WHERE StudentID = @StudentID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@StudentID", studentId);
                cmd.Parameters.AddWithValue("@BatchID", batchId);
                cmd.Parameters.AddWithValue("@Name", name);
                cmd.Parameters.AddWithValue("@Age", age);
                cmd.Parameters.AddWithValue("@EnrollmentDate", enrollmentDate);

                int rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    MessageBox.Show("Student details updated successfully!");
                    LoadDataToGrid(); // Refresh the data in the grid
                }
                else
                {
                    MessageBox.Show("No student record found with the provided Student ID.");
                }
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

        private void Register_Load(object sender, EventArgs e)
        {
            // Populate the BatchID combo box with predefined values
            cmbBatchID.Items.Add("DSE");
            cmbBatchID.Items.Add("DCSD");
            cmbBatchID.Items.Add("DNE");

            // Optionally set a default value
            cmbBatchID.SelectedIndex = 0;

            // Load existing data into the grid   
            LoadDataToGrid();
        }
    }
}
