using System;
using System.Text.RegularExpressions;

namespace MongoCrud.Test
{
    public class TestType 
    {
        [DataProperty(true)]
        public string Id { get; set; }

        [DataProperty]
        public string String { get; set; }

        [DataProperty]
        public DateTime DateTime { get; set; }

        [DataProperty]
        public DateTime? DateTimeNull { get; set; }

        [DataProperty]
        public bool Bool{ get; set; }

        [DataProperty]
        public bool? BoolNull { get; set; }

        [DataProperty]
        public Int32 Int32 { get; set; }

        [DataProperty]
        public Int32? Int32Null { get; set; }

        [DataProperty]
        public Int64 Int64 { get; set; }

        [DataProperty]
        public Int64? Int64Null { get; set; }

        [DataProperty]
        public byte[] ByteArray { get; set; }

        [DataProperty]
        public Regex Regex { get; set; }

        [DataProperty]
        public Double Double { get; set; }

        [DataProperty]
        public Double? DoubleNull { get; set; }
    }
}
