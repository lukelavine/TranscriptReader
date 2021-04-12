using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using System.Text.RegularExpressions;

namespace TranscriptReader
{
    class Reader
    {
        //Original regex that grabbed Course Designation, was a little greedy and would sometimes grab a whole word:
        //\d{5} \w{6} .*? \d\.\d{3} \d\.\d{3}\s[A-Z]*

        //Regex pattern to match a class on a transcript
        private readonly string class_pattern = @"\d{5} \w{6} .*? \d\.\d{3} \d\.\d{3}";

        private readonly string wasth_pattern = @"WASHINGTON STATE HISTORY\w+";

        public string PdfText(PdfReader r)
        {
            string text = String.Empty;
            for(int page = 1; page <= r.NumberOfPages; page++)
            {
                text += PdfTextExtractor.GetTextFromPage(r, page);
            }
            return text;
        }

        public List<string> Records(string path)
        { 
            using (PdfReader r = new PdfReader(path))
            {
                string text = PdfText(r);
                List<string> results = new List<string>();

                Regex rgx = new Regex(class_pattern);

                foreach (Match match in rgx.Matches(text))
                {
                    results.Add(match.Value);
                }

                return results;
            }
        }

        public List<ClassRecord> Parse(string path)
        {
            List<ClassRecord> classes = new List<ClassRecord>();
            List<string> records = Records(path);
            foreach (string record in records)
            {
                string[] word = record.Split(' ');
                ClassRecord c = new ClassRecord();
                c.StateCourseCode = word[0];
                c.DistrictCourseCode = word[1];
                c.CreditAttempted = float.Parse(word[word.Length - 1]);
                c.CreditEarned = float.Parse(word[word.Length - 2]);
                c.Grade = word[word.Length - 3];
                for(int i = 2; i < word.Length - 3; i++)
                {
                    c.Description += word[i] + ' ';
                }
                classes.Add(c);
            }
            return classes;
        }

        public bool WASTH(string path)
        {
            using (PdfReader r = new PdfReader(path))
            {
                string text = PdfText(r);

                Regex rgx = new Regex(wasth_pattern);

                return (rgx.Match(text) != null);
            }
        }
    }
}
