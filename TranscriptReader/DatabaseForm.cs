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
    public partial class DatabaseForm : Form
    {
        public string DistrictCode { get; set; }
        public string Name { get; set; }
        public string Credit1 { get; set; }
        public string Credit2 { get; set; }
        public string Credit3 { get; set; }

        public DatabaseForm()
        {
            InitializeComponent();
        }
    }
}
