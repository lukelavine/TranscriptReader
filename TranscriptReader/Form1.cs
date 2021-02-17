﻿using System;
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

        List<string> fields = new List<string>() { "English", "Math", "Science", "Social Studies", "CTE", "PE", "Health", "Art", "World Language", "Elective", "PP" };

        Dictionary<string, List<ClassRecord>> base_reqdic;
        Dictionary<string, int> credit_reqdic;
        Dictionary<string, List<ComboBox>> cb_dic;

        List<ClassRecord> unselected;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            conn = Database.CreateConnection();
            InitializeReqDic();
            InitializeCreditReqDic();
            InitializeCBs();
        }

        private void InitializeReqDic()
        {
            base_reqdic = new Dictionary<string, List<ClassRecord>>();
            foreach (string field in fields)
            {
                base_reqdic.Add(field, new List<ClassRecord>());
            }

        }

        private void InitializeCreditReqDic()
        {
            credit_reqdic = new Dictionary<string, int>
            {
                { "English", 8 },
                { "Math", 8 },
                { "Science", 8 },
                { "Social Studies", 7 },
                { "CTE", 2 },
                { "Health", 1 },
                { "PE", 3 },
                { "Art", 4 },
                { "World Language", 4 },
                { "Elective", 10 },
                { "PP", 8 }
            };
        }

        private void InitializeCBs()
        {
            cb_dic = new Dictionary<string, List<ComboBox>>
            {
                { "English", new List<ComboBox>() { englishCB1, englishCB2, englishCB3, englishCB4, englishCB5, englishCB6, englishCB7, englishCB8 } },
                { "Math", new List<ComboBox>() { mathCB1, mathCB2, mathCB3, mathCB4, mathCB5, mathCB6, mathCB7, mathCB8 } },
                { "Science", new List<ComboBox>() { scienceCB1, scienceCB2, scienceCB3, scienceCB4, scienceCB5, scienceCB6, scienceCB7, scienceCB8 } },
                { "Social Studies", new List<ComboBox>() { socialstudiesCB1, socialstudiesCB2, socialstudiesCB3, socialstudiesCB4, socialstudiesCB5, socialstudiesCB6 } },
                { "Health", new List<ComboBox>() { healthpeCB1 } },
                { "PE", new List<ComboBox>() { healthpeCB2, healthpeCB3, healthpeCB4 }},
                { "Art", new List<ComboBox>() { artCB1, artCB2, artCB3, artCB4 } },
                { "World Language", new List<ComboBox>() { worldlanguageCB1, worldlanguageCB2, worldlanguageCB3, worldlanguageCB4 } },
                { "Elective", new List<ComboBox>() { electiveCB1, electiveCB2, electiveCB3, electiveCB4, electiveCB5, electiveCB6, electiveCB7, electiveCB8, electiveCB9, electiveCB10 } },
                { "PP", new List<ComboBox>() { ppCB1, ppCB2, ppCB3, ppCB4, ppCB5, ppCB6, ppCB7, ppCB8 } }
            };
        }

        public void AddTranscriptColumns()
        {
            dataGridView1.Columns.Clear();
            dataGridView1.ColumnCount = 5;
            dataGridView1.Columns[0].Name = "District ID";
            dataGridView1.Columns[1].Name = "Description";
            dataGridView1.Columns[2].Name = "Grade";
            dataGridView1.Columns[3].Name = "Credits Earned";
            dataGridView1.Columns[4].Name = "Credits Attempted";
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

            if (popup.ShowDialog(this) == DialogResult.OK)
            {
                Database.AddClass(conn, popup.DistrictCode, popup.Description, popup.Credits);
            }

            popup.Dispose();

        }

        private void viewDatabaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            AddDatabaseColumns();
            dataGridView1.Refresh();


            List<string[]> data = Database.ReadClass(conn);
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
            unselected = new List<ClassRecord>();

            Dictionary<string, List<ClassRecord>> req_dic = new Dictionary<string, List<ClassRecord>>(base_reqdic);
            foreach (ClassRecord record in rs)
            {
                List<string> row = record.ListOfData();
                dataGridView1.Rows.Add(row.ToArray());

                string credits = Database.CheckClass(conn, record.DistrictCourseCode);

                if (credits != "")
                {

                    string[] c = credits.Split(',');

                    if (c.Length > 1)
                    {
                        record.MultiCredit = true;
                    }
                    foreach (string cred in c)
                    {
                        foreach(string field in fields)
                        {
                            if (cred.Contains(field))
                            {
                                req_dic[field].Add(record);
                            }
                        }
                    }
                    req_dic["Elective"].Add(record);
                    req_dic["PP"].Add(record);
                }
                else
                {
                    dataGridView1.Rows[dataGridView1.Rows.Count - 1].DefaultCellStyle.BackColor = Color.Yellow;
                }
            }

            Dictionary<string, int> selected_count = new Dictionary<string, int>();

            foreach (string field in fields)
            {
                if (!field.Equals("CTE"))
                {
                    req_dic[field].Sort();
                    addCBClasses(field, req_dic);
                    selected_count.Add(field, selectCBClasses(field, req_dic));
                }
            }

            /*foreach (ClassRecord rec in unselected)
            {
                foreach (string field in fields)
                {
                    if (req_dic[field].Contains(rec))
                    {
                        if (selected_count[field] == credit_reqdic[field])
                        {
                            
                        }
                    }
                } 
            }*/

        }

        private void addCBClasses(string field, Dictionary<string, List<ClassRecord>> req_dic)
        {
            foreach (ComboBox cb in cb_dic[field])
            {
                cb.Items.AddRange(req_dic[field].ToArray());
            }
        }

        private int selectCBClasses(string field, Dictionary<string, List<ClassRecord>> req_dic)
        {
            int count1 = 0, count2 = 0;

            foreach (ClassRecord record in req_dic[field])
            {
                if (!record.MultiCredit && count2 < credit_reqdic[field] && !field.Equals("Elective") && !field.Equals("PP"))
                {
                    cb_dic[field][count1++].SelectedIndex = count2;
                }
                else
                {
                    unselected.Add(record);
                }
                count2++;
            }
            return count1;
        }

        private void removeClassToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RemoveForm popup = new RemoveForm();

            if (popup.ShowDialog(this) == DialogResult.OK)
            {
                Database.RemoveClass(conn, popup.DistrictCode);
            }

            popup.Dispose();
        }

    }
}
