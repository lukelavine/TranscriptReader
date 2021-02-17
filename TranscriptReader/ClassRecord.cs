using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TranscriptReader
{
    class ClassRecord : IComparable<ClassRecord>
    {
        public string StateCourseCode { get; set; }
        public string DistrictCourseCode { get; set; }
        public string Description { get; set; }
        public string Grade { get; set; }
        public float CreditEarned { get; set; }
        public float CreditAttempted { get; set; }
        public bool MultiCredit { get; set; }

        public int CompareTo(ClassRecord other)
        {
            return DistrictCourseCode.Substring(3).CompareTo(other.DistrictCourseCode.Substring(3));

        }

        public List<string> ListOfData()
        {
            List<string> vs = new List<string>();
            //vs.Add(StateCourseCode);
            vs.Add(DistrictCourseCode);
            vs.Add(Description);
            vs.Add(Grade);
            vs.Add(CreditEarned.ToString());
            vs.Add(CreditAttempted.ToString());
            return vs;
        }

        public override string ToString()
        {
            return Description;
        }
    }
}
