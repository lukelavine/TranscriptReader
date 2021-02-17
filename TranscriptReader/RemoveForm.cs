using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TranscriptReader
{
    public partial class RemoveForm : Form
    {
        public string DistrictCode { get; set; }

        public RemoveForm()
        {
            InitializeComponent();
            label2.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DistrictCode = textBox1.Text.ToUpper();
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
