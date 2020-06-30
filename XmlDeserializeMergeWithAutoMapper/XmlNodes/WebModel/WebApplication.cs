using System.Xml.Serialization;
using XmlDeserializeMergeWithAutoMapper.Attributes;
using XmlDeserializeMergeWithAutoMapper.XmlNodes.Model;

namespace XmlDeserializeMergeWithAutoMapper.XmlNodes.WebModel
{
    [XmlRoot("Application"), ModelMap(typeof(Application))]
    public sealed class WebApplication : ApplicationBase<WebEntity, StaticText>
    {
    }
}