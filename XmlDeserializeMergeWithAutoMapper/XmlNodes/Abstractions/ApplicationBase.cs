using System.Collections.Generic;
using System.Xml.Serialization;
using XmlDeserializeMergeWithAutoMapper.Interfaces;
using XmlDeserializeMergeWithAutoMapper.XmlNodes.Model;

namespace XmlDeserializeMergeWithAutoMapper.XmlNodes
{
    public abstract class ApplicationBase<TEntityType, TStaticText> : IApplication
        where TEntityType : Entity
        where TStaticText : StaticText
    {
        [XmlArray, XmlArrayItem(nameof(Entity))]
        public List<TEntityType> EntityTypes { get; set; }

        [XmlArray, XmlArrayItem(nameof(StaticText))]
        public List<TStaticText> StaticTexts { get; set; }
    }
}