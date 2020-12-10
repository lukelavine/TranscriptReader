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

namespace TranscriptReader
{
    class Writer
    {
        public string PdfFields(string path)
        {
            PdfReader r = new PdfReader(path);
            string text = string.Empty;
            foreach (var de in r.AcroFields.Fields)
            {
                text += de.Key.ToString() + " " + de.Value.ToString() + Environment.NewLine;
            }
            //r.AcroFields.SetField(de.Key, "test");
            foreach (var de in r.AcroForm.Fields)
            {
                text += de.Name + Environment.NewLine;
               
            }
            return text;
        }

        //Do this after I have the info
        /*public void Fill(string path, string ogfile, string newfile)
        {
            //File.Copy(Path.Combine(path, name1), Path.Combine(path, name2));
            FileStream fs = new FileStream(path, FileMode.Create);
            Document document = new Document(PageSize.LETTER, 40, 40, 40, 40);

            PdfWriter writer = PdfWriter.GetInstance(document, fs);

            document.Open();

        }*/
    }
}
