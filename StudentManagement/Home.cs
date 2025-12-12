using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StudentManagement
{
    public partial class Home : Form
    {
        public Home()
        {
            InitializeComponent();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void studentRegisterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Register st = new Register();
            st.MdiParent = this;
            st.Show();
        }

        private void addSubjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Subject st = new Subject();
            st.MdiParent = this;
            st.Show();
        }

        private void addMarksToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Result st = new Result();
            st.MdiParent = this;
            st.Show();
        }

        private void studentReportToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            using (studentreport reportViewer = new studentreport())
            {
                reportViewer.WindowState = FormWindowState.Maximized;
                reportViewer.ShowDialog();
            }
        }

        private void subjectReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (SubjectViewerForm reportViewer = new SubjectViewerForm())
            {
                reportViewer.WindowState = FormWindowState.Maximized;
                reportViewer.ShowDialog();
            }
        }

        private void studentReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (ResultReport reportViewer = new ResultReport())
            {
                reportViewer.WindowState = FormWindowState.Maximized;
                reportViewer.ShowDialog();
            }
        }

        private void studentDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StudentRecord st = new StudentRecord();
            st.MdiParent = this;
            st.Show();
        }
    }
}
