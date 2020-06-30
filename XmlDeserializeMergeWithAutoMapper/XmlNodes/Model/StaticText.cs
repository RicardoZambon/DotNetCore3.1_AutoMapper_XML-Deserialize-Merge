using System.Xml.Serialization;
using XmlDeserializeMergeWithAutoMapper.Attributes;
using XmlDeserializeMergeWithAutoMapper.Interfaces;

namespace XmlDeserializeMergeWithAutoMapper.XmlNodes.Model
{
    public class StaticText : IXmlNode
    {
        [XmlAttribute, MergeKey]
        public string Key { get; set; }

        [XmlAttribute]
        public string Value { get; set; }
    }
}