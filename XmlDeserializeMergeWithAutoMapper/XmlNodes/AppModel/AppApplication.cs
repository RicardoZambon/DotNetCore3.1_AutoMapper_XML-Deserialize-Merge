using AutoMapper;
using System.Xml.Serialization;
using XmlDeserializeMergeWithAutoMapper.Attributes;
using XmlDeserializeMergeWithAutoMapper.XmlNodes.Model;
using XmlDeserializeMergeWithAutoMapper.XmlNodes.WebModel;

namespace XmlDeserializeMergeWithAutoMapper.XmlNodes.AppModel
{
    [XmlRoot("Application"), ModelMap(typeof(WebApplication))]
    public sealed class AppApplication : ApplicationBase<AppEntity, StaticText>
    {
    }
}