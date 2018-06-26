using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace iBill4mev1
{
    public partial class Main_Menu : Form
    {
        string validstartDate;
        string validendDate;

        public Main_Menu()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var currentstartdate = startDate.Value.ToString("d-MM-yyyy");
            var currentenddate = endDate.Value.ToString("d-MM-yyyy");
            startDateLabel.Text = currentstartdate.ToString();
            endDateLabel.Text = currentenddate.ToString();
            validstartDate = startDateLabel.Text;
            validendDate = endDateLabel.Text;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            Collection_Report cr = new Collection_Report();
            cr.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Stock_Report sr = new Stock_Report();
            this.Hide();
            sr.startDate = validstartDate;
            sr.endDate = validendDate;
            sr.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Hide();
            Dispatch_Report dr = new Dispatch_Report();
            dr.Show();
        }
    }
}
