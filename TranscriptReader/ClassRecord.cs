using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TranscriptReader
{
    class ClassRecord
    {
        public string StateCourseCode { get; set; }
        public string DistrictCourseCode { get; set; }
        public string Description { get; set; }
        public string Grade { get; set; }
        public float CreditEarned { get; set; }
        public float CreditAttempted { get; set; }

        public string[] ArrayOfData()
        {
            string[] vs = {StateCourseCode, DistrictCourseCode, Description, Grade, CreditEarned.ToString(), CreditAttempted.ToString() };
            return vs;
        }
    }
}
