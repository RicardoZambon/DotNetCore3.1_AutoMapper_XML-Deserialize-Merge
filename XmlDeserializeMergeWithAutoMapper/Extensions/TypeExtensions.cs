using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using XmlDeserializeMergeWithAutoMapper.Attributes;

namespace XmlDeserializeMergeWithAutoMapper.Extensions
{
    public static class TypeExtensions
    {
        public static Dictionary<string, Type[]> GetListTypes(this Type parentType)
        {
            var mapTypes = new Dictionary<string, Type[]>();

            var listProperties = parentType.GetProperties().Where(x => typeof(IEnumerable).IsAssignableFrom(x.PropertyType) && x.PropertyType.IsGenericType);
            foreach (var listProperty in listProperties)
            {
                var listType = listProperty.PropertyType.GenericTypeArguments[0];

                var propertyTypes = new List<Type>() { listType };

                var baseType = listType.BaseType;
                while (baseType != typeof(object) && !baseType.IsAbstract)
                {
                    propertyTypes.Add(baseType);
                    baseType = baseType.BaseType;
                }

                mapTypes.Add(listProperty.Name, propertyTypes.ToArray());


                foreach (var subProperty in listType.GetListTypes())
                {
                    mapTypes.Add(subProperty.Key, subProperty.Value);
                }
            }

            return mapTypes;
        }

        public static IEnumerable<PropertyInfo> GetMergeProperties(this Type type)
            => type.GetProperties()
                .Where(x => x.CustomAttributes.Any(a => a.AttributeType == typeof(MergeAttribute)));
    }
}