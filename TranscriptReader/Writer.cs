using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.xml;
using System.Windows.Forms;

namespace TranscriptReader
{
    class Writer
    {
        private Dictionary<string, int[,]> box_locs;
        
        public Writer()
        {
            //Ideally you could just offset each box instead of typing the coordinates for each one but
            //I couldn't find a number that perfectly offset them
            box_locs = new Dictionary<string, int[,]>
            {
                { "English", new int[,] { { 140, 692, 185, 740}, { 195, 692, 245, 740 }, { 250, 692, 300, 740}, { 308, 692, 355, 740 }, { 365, 692, 410, 740 }, { 420, 692, 470, 740 }, { 480, 692, 525, 740 }, { 535, 692, 580, 740 } } },
                { "Math", new int[,] { { 140, 649, 185, 690}, { 195, 649, 245, 690 }, { 250, 649, 300, 690}, { 308, 649, 355, 690 }, { 365, 649, 410, 683 }, { 420, 649, 470, 683 }, { 480, 649, 525, 672 }, { 535, 649, 580, 672 } } },
                { "Science", new int[,] { { 140, 599, 185, 632}, { 195, 599, 245, 632 }, { 250, 599, 300, 632}, { 308, 599, 355, 632 }, { 365, 599, 410, 622 }, { 420, 599, 470, 622 }, { 480, 599, 525, 622 }, { 535, 599, 580, 622 } } },
                { "Social Studies", new int[,] { { 140, 549, 185, 582}, { 195, 549, 245, 582 }, { 250, 549, 300, 582}, { 308, 549, 355, 582 }, { 365, 549, 410, 582 }, { 420, 549, 470, 582 } } },
                { "Health", new int[,] { { 140, 449, 185, 482} } },
                { "PE", new int[,] { { 195, 449, 245, 482 }, { 250, 449, 300, 482}, { 308, 449, 355, 482 } } },
                { "Art", new int[,] { { 140, 399, 185, 432}, { 195, 399, 245, 432 }, { 250, 399, 300, 432}, { 308, 399, 355, 432 } } },
                { "World Language", new int[,] { { 140, 349, 185, 372}, { 195, 349, 245, 372 }, { 250, 349, 300, 372}, { 308, 349, 355, 372 } } },
                { "Elective", new int[,] { { 140, 299, 185, 340}, { 195, 299, 245, 340 }, { 250, 299, 300, 340}, { 308, 299, 355, 340 }, { 365, 299, 410, 340 }, { 140, 249, 185, 290 }, { 195, 249, 245, 290 }, { 250, 249, 300, 290 }, { 308, 249, 355, 290 }, { 365, 249, 410, 290 } } },
                { "PP", new int[,] { { 140, 210, 300, 220}, { 140, 195, 300, 205 }, { 140, 180, 300, 190}, { 140, 165, 300, 175 }, { 140, 150, 300, 160 }, { 140, 135, 300, 145 }, { 140, 120, 300, 130 }, { 140, 105, 300, 115 } } }
            };
        }

        

        public void ExportToBoxSheet(string path, Dictionary<string, List<ComboBox>> cb_dic, List<CheckBox> cb_list, CheckBox WASH_cb, List<CheckBox> gradreq_cbs)
        {
            PdfReader reader = new PdfReader("boxsheet_flat.pdf");
            PdfStamper stamper = new PdfStamper(reader, new FileStream(path, FileMode.Create));

            BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.EMBEDDED);
            PdfContentByte cb = stamper.GetOverContent(1);

            cb.SetColorFill(BaseColor.DARK_GRAY);

            Font font = new Font(bf, 7);

            //All of the text boxes
            ColumnText ct;
            foreach(string key in cb_dic.Keys)
            {
                if (box_locs.Keys.Contains(key))
                {
                    for (int i = 0; i < cb_dic[key].Count; i++)
                    {
                        ct = new ColumnText(cb);
                        ct.SetSimpleColumn(
                            new Phrase(cb_dic[key][i].Text, font),
                            box_locs[key][i, 0], box_locs[key][i, 1], box_locs[key][i, 2], box_locs[key][i, 3], 8, key.Equals("PP") ? Element.ALIGN_LEFT : Element.ALIGN_CENTER);
                        ct.Go();
                    }
                }
            }

            font = new Font(bf, 26);

            //The "check" boxes
            //Had to use x instead of ✓ because it's a unicode character and this is easier

            //Washington State History checkbox
            if (WASH_cb.Checked)
            {
                ct = new ColumnText(cb);
                ct.SetSimpleColumn(
                    new Phrase("x", font),
                    535, 549, 580, 568, 8, Element.ALIGN_CENTER);
                ct.Go();
            }

            //CTE checkboxes
            for (int i =0; i<2; i++)
            {
                if (cb_list[i].Checked)
                {
                    ct = new ColumnText(cb);
                    ct.SetSimpleColumn(
                        new Phrase("x", font),
                        140+(i*55), 409, 185+(i*55), 522, 8, Element.ALIGN_CENTER);
                    ct.Go();
                }
            }

            //Additionaly high school graduation requirements checkboxes
            for (int i = 0; i < 3; i++)
            {
                if (gradreq_cbs[i].Checked)
                {
                    ct = new ColumnText(cb);
                    ct.SetSimpleColumn(
                        new Phrase("x", font),
                        205 + (i * 114), 25, 230 + (i * 114), 42, 8, Element.ALIGN_CENTER);
                    ct.Go();
                }
            }

            stamper.Close();
            reader.Close();
        }
    }
}
