using System;

namespace XmlDeserializeMergeWithAutoMapper.Attributes
{
    public class ModelMapAttribute : Attribute
    {
        public Type SourceType { get; }

        public ModelMapAttribute(Type sourceType)
        {
            SourceType = sourceType;
        }
    }
}