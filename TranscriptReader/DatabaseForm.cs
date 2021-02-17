using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace TranscriptReader
{
    public partial class DatabaseForm : Form
    {
        public string DistrictCode { get; set; }
        public string Description { get; set; }
        public string[] Credits { get; set; }
        public string[] CreditChoices;

        public DatabaseForm()
        {
            InitializeComponent();
            label7.Hide();
            CreditChoices = new string[]{ "English", "Social Studies", "Math", "Math-3rd year", "Lab Science", "Non-Lab Science", "PE", "Health", "Fine Arts", "CTE", "World Language", "Elective"};
            creditbox1.Items.AddRange(CreditChoices);
            creditbox2.Items.AddRange(CreditChoices);
            creditbox3.Items.AddRange(CreditChoices);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Regex regex = new Regex(@"[a-zA-Z]{3}\d{3}");

            if (textBox1.Text == "" || textBox2.Text == "")
            {
                label7.ForeColor = Color.Red;
                label7.Text = "Missing district code or name.";
                label7.Show();
            } else if (!regex.IsMatch(textBox1.Text))
            {
                label7.ForeColor = Color.Red;
                label7.Text = "District code must be 3 letters and 3 numbers i.e. ABC123.";
                label7.Show();

            } else if (creditbox1.Text == "" && creditbox2.Text == "" && creditbox3.Text == "")
            {
                label7.ForeColor = Color.Red;
                label7.Text = "Needs at least one credit type.";
                label7.Show();
            } else
            {
                DistrictCode = textBox1.Text.ToUpper();
                Description = textBox2.Text.ToUpper();
                Credits = new string[3];
                Credits[0] = creditbox1.Text;
                Credits[1] = creditbox2.Text;
                Credits[2] = creditbox3.Text;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }
    }
}
