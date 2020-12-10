using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;

namespace TranscriptReader
{
    public partial class Form1 : Form
    {
        SQLiteConnection conn;

        public Form1()
        {
            InitializeComponent();
            //AddColumns();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            conn = Database.CreateConnection();
        }

        public void AddTranscriptColumns()
        {
            dataGridView1.Columns.Clear();
            dataGridView1.ColumnCount = 6;
            dataGridView1.Columns[0].Name = "State ID";
            dataGridView1.Columns[1].Name = "District ID";
            dataGridView1.Columns[2].Name = "Description";
            dataGridView1.Columns[3].Name = "Grade";
            dataGridView1.Columns[4].Name = "Credits Earned";
            dataGridView1.Columns[5].Name = "Credits Attempted";
        }

        public void AddDatabaseColumns()
        {
            dataGridView1.Columns.Clear();
            dataGridView1.ColumnCount = 5;
            dataGridView1.Columns[0].Name = "District ID";
            dataGridView1.Columns[1].Name = "Descritpion";
            dataGridView1.Columns[2].Name = "Credit";
            dataGridView1.Columns[3].Name = "Credit";
            dataGridView1.Columns[4].Name = "Credit";
        }

        private void addEditClassToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DatabaseForm popup = new DatabaseForm();
            popup.Show(this);
        }

        private void viewDatabaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            AddDatabaseColumns();
            dataGridView1.Refresh();


            List<string[]> data = Database.ReadClass(Database.CreateConnection());
            foreach (string[] record in data)
            {
                dataGridView1.Rows.Add(record);
            }
        }

        private void loadTranscriptToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            AddTranscriptColumns();
            dataGridView1.Refresh();

            Reader r = new Reader();
            List<ClassRecord> rs = r.Parse(@"D:\Projects\Transcript to Box Sheet\Transcript LuLa.pdf");
            foreach (ClassRecord record in rs)
            {
                dataGridView1.Rows.Add(record.ArrayOfData());
                if (record.Grade == "P")
                {
                    dataGridView1.Rows[dataGridView1.Rows.Count - 2].DefaultCellStyle.BackColor = Color.Red;
                }
            }
        }
    }
}
