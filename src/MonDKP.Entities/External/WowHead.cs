using System;
using System.Xml.Serialization;

namespace MonDKP.Entities.External
{
    [XmlRoot("wowhead")]
    public class WowHead
    {
        [XmlElement("item")] public Item Item { get; set; }
    }

    [XmlRoot("item")]
    public class Item
    {
        [XmlAttribute("id")]
        public Int32 Id { get; set; }

        [XmlElement("quality")]
        public Quality Quality { get; set; }
    }

    [XmlRoot("quality")]
    public class Quality
    {
        [XmlAttribute("id")]
        public Int32 Id { get; set; }

        [XmlText]
        public string Name { get; set; }
    }
}