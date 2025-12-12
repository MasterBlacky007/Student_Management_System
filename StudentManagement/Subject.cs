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
    public partial class Subject : Form
    {
        public Subject()
        {
            InitializeComponent();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtSubjectID.Clear();
            txtName.Clear();
            txtLecturer.Clear();
            txtPassMark.Clear();
            txtModule.Clear();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string subjectId = txtSubjectID.Text.Trim();
            string name = txtName.Text.Trim();
            string lecturer = txtLecturer.Text.Trim();
            int passMark;
            string module = txtModule.Text.Trim();

            // Validate that no fields are empty
            if (string.IsNullOrEmpty(subjectId) ||
                string.IsNullOrEmpty(name) ||
                string.IsNullOrEmpty(lecturer) ||
                string.IsNullOrEmpty(txtPassMark.Text) ||
                string.IsNullOrEmpty(module))
            {
                MessageBox.Show("All fields must be filled. Please complete the form.");
                return;
            }

            // Validate Name (only letters and spaces are allowed)
            if (!System.Text.RegularExpressions.Regex.IsMatch(name, @"^[a-zA-Z\s]+$"))
            {
                MessageBox.Show("Invalid name. The name should only contain letters and spaces.");
                return;
            }

            // Validate Lecturer (only letters and spaces are allowed)
            if (!System.Text.RegularExpressions.Regex.IsMatch(lecturer, @"^[a-zA-Z\s]+$"))
            {
                MessageBox.Show("Invalid lecturer name. The lecturer's name should only contain letters and spaces.");
                return;
            }

            // Validate Pass Mark
            if (!int.TryParse(txtPassMark.Text, out passMark) || passMark < 0)
            {
                MessageBox.Show("Invalid pass mark. Please enter a positive integer value.");
                return;
            }

            string connectionString = "Data Source=DESKTOP-ADM7KIF;Initial Catalog=Studentdb;Integrated Security=True";
            SqlConnection conn = new SqlConnection(connectionString);

            try
            {
                conn.Open();
                string query = "INSERT INTO Subjects (SubjectID, Name, Lecturer, PassMark, Module) VALUES (@SubjectID, @Name, @Lecturer, @PassMark, @Module)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@SubjectID", subjectId);
                cmd.Parameters.AddWithValue("@Name", name);
                cmd.Parameters.AddWithValue("@Lecturer", lecturer);
                cmd.Parameters.AddWithValue("@PassMark", passMark);
                cmd.Parameters.AddWithValue("@Module", module);

                cmd.ExecuteNonQuery();
                MessageBox.Show("Subject added successfully!");
                LoadSubjectsToGrid(); // Refresh the grid to display the new subject
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
            string subjectId = txtSuID.Text;

            string connectionString = "Data Source=DESKTOP-ADM7KIF;Initial Catalog=Studentdb;Integrated Security=True";
            SqlConnection conn = new SqlConnection(connectionString);

            try
            {
                conn.Open();
                string query = "SELECT * FROM Subjects WHERE SubjectID = @SubjectID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@SubjectID", subjectId);

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    txtName.Text = reader["Name"].ToString();
                    txtLecturer.Text = reader["Lecturer"].ToString();
                    txtPassMark.Text = reader["PassMark"].ToString();
                    txtModule.Text = reader["Module"].ToString();
                }
                else
                {
                    MessageBox.Show("Subject not found.");
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
            string subjectId = txtSuID.Text;

            string connectionString = "Data Source=DESKTOP-ADM7KIF;Initial Catalog=Studentdb;Integrated Security=True";
            SqlConnection conn = new SqlConnection(connectionString);

            try
            {
                conn.Open();
                string query = "DELETE FROM Subjects WHERE SubjectID = @SubjectID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@SubjectID", subjectId);

                cmd.ExecuteNonQuery();
                MessageBox.Show("Subject deleted successfully!");
                LoadSubjectsToGrid();
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
            string subjectId = txtSuID.Text.Trim();
            string name = txtName.Text.Trim();
            string lecturer = txtLecturer.Text.Trim();
            int passMark;
            string module = txtModule.Text.Trim();

            // Validate that no fields are empty
            if (string.IsNullOrEmpty(subjectId) ||
                string.IsNullOrEmpty(name) ||
                string.IsNullOrEmpty(lecturer) ||
                string.IsNullOrEmpty(txtPassMark.Text) ||
                string.IsNullOrEmpty(module))
            {
                MessageBox.Show("All fields must be filled. Please complete the form.");
                return;
            }

            // Validate Name (only letters and spaces are allowed)
            if (!System.Text.RegularExpressions.Regex.IsMatch(name, @"^[a-zA-Z\s]+$"))
            {
                MessageBox.Show("Invalid name. The name should only contain letters and spaces.");
                return;
            }

            // Validate Lecturer (only letters and spaces are allowed)
            if (!System.Text.RegularExpressions.Regex.IsMatch(lecturer, @"^[a-zA-Z\s]+$"))
            {
                MessageBox.Show("Invalid lecturer name. The lecturer's name should only contain letters and spaces.");
                return;
            }

            // Validate Pass Mark
            if (!int.TryParse(txtPassMark.Text, out passMark) || passMark < 0)
            {
                MessageBox.Show("Invalid pass mark. Please enter a positive integer value.");
                return;
            }

            string connectionString = "Data Source=DESKTOP-ADM7KIF;Initial Catalog=Studentdb;Integrated Security=True";
            SqlConnection conn = new SqlConnection(connectionString);

            try
            {
                conn.Open();
                string query = "UPDATE Subjects SET Name = @Name, Lecturer = @Lecturer, PassMark = @PassMark, Module = @Module WHERE SubjectID = @SubjectID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@SubjectID", subjectId);
                cmd.Parameters.AddWithValue("@Name", name);
                cmd.Parameters.AddWithValue("@Lecturer", lecturer);
                cmd.Parameters.AddWithValue("@PassMark", passMark);
                cmd.Parameters.AddWithValue("@Module", module);

                cmd.ExecuteNonQuery();
                MessageBox.Show("Subject updated successfully!");
                LoadSubjectsToGrid(); // Refresh the data grid
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

                dataGridView1.DataSource = dataTable;
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
        private void Subject_Load(object sender, EventArgs e)
        {
            LoadSubjectsToGrid();
        }
    }
}
