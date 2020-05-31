using System;
using System.Diagnostics;
using System.Xml.Serialization;

namespace MonDKP.Entities
{
    // ReSharper disable once UseNameofExpression
    [DebuggerDisplay("{ToString()}")]
    [XmlType("lootentry")]
    public class LootEntry
    {
        public override String ToString()
        {
            return $"Player={Player}, Item={ItemName}, Zone={Zone}, Boss={Boss}, Cost={Cost}, UtcTime={TimeStampOffset.UtcDateTime}, LocalTime={TimeStampOffset.LocalDateTime}";
        }

        [XmlElement("boss")]
        public String Boss { get; set; }

        [XmlElement("zone")]
        public String Zone { get; set; }

        [XmlElement("itemname")]
        public String ItemName { get; set; }

        [XmlElement("itemnumber")]
        public Int32 ItemNumber{ get; set; }

        [XmlElement("player")]
        public String Player { get; set; }

        [XmlElement("timestamp")]
        public Int64 TimeStamp { get; set; }

        [XmlElement("cost")]
        public Int32 Cost { get; set; }

        [XmlIgnore]
        public DateTimeOffset TimeStampOffset => DateTimeOffset.FromUnixTimeSeconds(TimeStamp);

        [XmlIgnore]
        public String ItemId { get; set; }

        [XmlIgnore]
        public String DeletedBy { get; set; }

        [XmlIgnore]
        public String Deletes { get; set; }
    }
}