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
    public partial class ReportViewerForm : Form
    {
        public ReportViewerForm()
        {
            InitializeComponent();
        }
        public ReportViewerForm(CrystalDecisions.CrystalReports.Engine.ReportDocument report)
        {
            InitializeComponent();
            crystalReportViewer1.ReportSource = report;
            crystalReportViewer1.Refresh();
        }
        private void ReportViewerForm_Load(object sender, EventArgs e)
        {

        }
    }
}
