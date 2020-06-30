using System.Xml.Serialization;
using XmlDeserializeMergeWithAutoMapper.XmlNodes.Model;

namespace XmlDeserializeMergeWithAutoMapper.XmlNodes.WebModel
{
    public class WebEntity : Entity
    {
        [XmlAttribute]
        public string ControllerName { get; set; }
    }
}