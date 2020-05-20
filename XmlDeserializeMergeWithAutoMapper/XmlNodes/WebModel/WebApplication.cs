using AutoMapper;
using System.Xml.Serialization;
using XmlDeserializeMergeWithAutoMapper.XmlNodes.Model;

namespace XmlDeserializeMergeWithAutoMapper.XmlNodes.WebModel
{
    [XmlRoot("Application"), AutoMap(typeof(Application)), AutoMap(typeof(WebApplication))]
    public sealed class WebApplication : ApplicationBase<WebEntity>
    {
    }
}