using AutoMapper;
using System.Xml.Serialization;

namespace XmlDeserializeMergeWithAutoMapper.XmlNodes.Model
{
    [XmlRoot("Application"), AutoMap(typeof(Application))]
    public sealed class Application : ApplicationBase<Entity, StaticText>
    {
    }
}