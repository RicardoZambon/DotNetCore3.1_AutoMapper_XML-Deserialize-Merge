using AutoMapper;
using XmlDeserializeMergeWithAutoMapper.XmlNodes.WebModel;

namespace XmlDeserializeMergeWithAutoMapper.XmlNodes.AppModel
{
    [AutoMap(typeof(WebEntity)), AutoMap(typeof(AppEntity))]
    public class AppEntity : WebEntity
    {
    }
}