using System.Collections.Generic;
using System.Xml.Serialization;
using XmlDeserializeMergeWithAutoMapper.Interfaces;
using XmlDeserializeMergeWithAutoMapper.XmlNodes.Model;

namespace XmlDeserializeMergeWithAutoMapper.XmlNodes
{
    public abstract class ApplicationBase<T> : IApplication where T : Entity
    {
        [XmlArray, XmlArrayItem(nameof(Entity))]
        public List<T> EntityTypes { get; set; }
    }
}