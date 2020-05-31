using System.Collections.Generic;
using System.Xml.Serialization;

namespace MonDKP.Entities
{
    [XmlType("loothistory")]
    public class LootHistory
    {
        [XmlElement("lootentry", typeof(LootEntry))]
        public List<LootEntry> LootEntries { get; set; } = new List<LootEntry>();
    }
}