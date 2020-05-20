using AutoMapper;
using System.Xml.Serialization;
using XmlDeserializeMergeWithAutoMapper.XmlNodes.WebModel;

namespace XmlDeserializeMergeWithAutoMapper.XmlNodes.AppModel
{
    [XmlRoot("Application"), AutoMap(typeof(WebApplication)), AutoMap(typeof(AppApplication))]
    public sealed class AppApplication : ApplicationBase<AppEntity>
    {
    }
}