using AutoMapper;
using System.Xml.Serialization;
using XmlDeserializeMergeWithAutoMapper.Attributes;

namespace XmlDeserializeMergeWithAutoMapper.XmlNodes.Model
{
    [AutoMap(typeof(Entity))]
    public class Entity
    {
        [XmlAttribute, MergeKey]
        public string Id { get; set; }

        [XmlAttribute]
        public string DisplayName { get; set; }
    }
}