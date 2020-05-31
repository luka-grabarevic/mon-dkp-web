using System;
using System.Diagnostics;
using System.Xml.Serialization;

namespace MonDKP.Entities
{
    // ReSharper disable once UseNameofExpression
    [DebuggerDisplay("{ToString()}")]
    [XmlType("dkpentry")]
    public class DkpEntry
    {
        public override String ToString()
        {
            return $"{Player}, {Class}, DKP={Dkp}, Gained={LifetimeGained}, Spent={LifetimeSpent}";
        }

        [XmlElement("player")]
        public String Player { get; set; }

        [XmlElement("class")]
        public String Class { get; set; }

        [XmlElement("lifetimegained")]
        public Int32 LifetimeGained { get; set; }

        [XmlElement("dkp")]
        public Int32 Dkp { get; set; }

        [XmlElement("lifetimespent")]
        public Int32 LifetimeSpent { get; set; }
    }
}