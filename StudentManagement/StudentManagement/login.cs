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
    public partial class login : Form
    {
        public login()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
           
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
          
        }

        private void btnclose_Click_1(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnlogin_Click_1(object sender, EventArgs e)
        {
            string username = txtname.Text;
            string password = txtpass.Text;
            //connection
            string cs = "Data Source=DESKTOP-ADM7KIF;Initial Catalog=Studentdb;Integrated Security=True";
            SqlConnection conn = new SqlConnection(cs);
            conn.Open();

            //comand
            string sql = "SELECT * FROM tbluser WHERE username=@un AND password=@pw";
            SqlCommand com = new SqlCommand(sql, conn);
            com.Parameters.AddWithValue("@un", username);
            com.Parameters.AddWithValue("@pw", password);
            SqlDataReader dr = com.ExecuteReader();
            {
                if (dr.HasRows)
                {
                    MessageBox.Show("Login Successful!");
                    Home home = new Home();
                    home.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Invalid username or password.");

                }
            }
        }
    }
}
