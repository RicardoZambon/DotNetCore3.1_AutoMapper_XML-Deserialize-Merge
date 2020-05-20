using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using XmlDeserializeMergeWithAutoMapper.MapperTypeConverters;

namespace XmlDeserializeMergeWithAutoMapper.Extensions
{
    public static class MapperExtensions
    {
        public static void MapLists(this Profile mapper, Type sourceType, Type destinationType)
        {
            typeof(MapperExtensions)
                .GetMethod(nameof(MapGenericLists))
                .MakeGenericMethod(new Type[] { sourceType, destinationType })
                .Invoke(null, new object[] { mapper });
        }

        public static void MapGenericLists<TSource, TDestination>(this Profile mapper)
        {
            mapper.CreateMap<List<TSource>, List<TDestination>>().ConvertUsing(
                new ListTypeConverter<List<TSource>, List<TDestination>, TSource, TDestination>(
                    (src, dest)
                        => src.GetType().GetMergeProperties().All(s => s.GetValue(src).Equals(dest.GetType().GetProperty(s.Name).GetValue(dest)))
                )
            );
        }

    }
}