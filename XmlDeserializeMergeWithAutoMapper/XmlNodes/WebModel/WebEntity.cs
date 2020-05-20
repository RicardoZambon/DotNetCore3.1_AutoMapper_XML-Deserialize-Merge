using AutoMapper;
using System.Xml.Serialization;
using XmlDeserializeMergeWithAutoMapper.XmlNodes.Model;

namespace XmlDeserializeMergeWithAutoMapper.XmlNodes.WebModel
{
    [AutoMap(typeof(Entity)), AutoMap(typeof(WebEntity))]
    public class WebEntity : Entity
    {
        [XmlAttribute]
        public string ControllerName { get; set; }
    }
}