using AutoMapper;
using System;
using System.Collections.Generic;

namespace XmlDeserializeMergeWithAutoMapper.MapperTypeConverters
{
    public class ListTypeConverter<TSourceArray, TDestArray, TSource, TDest> : ITypeConverter<TSourceArray, TDestArray>
        where TSourceArray : IList<TSource>
        where TDestArray : IList<TDest>
    {
        private Func<TSource, TDest, bool> filter;

        public ListTypeConverter(Func<TSource, TDest, bool> filter)
        {
            this.filter = filter;
        }

        public TDestArray Convert(TSourceArray source, TDestArray destination, ResolutionContext context)
        {
            var typeMap = context.ConfigurationProvider.ResolveTypeMap(typeof(TSource), typeof(TDest));

            foreach (var src in source)
            {
                var exits = false;
                foreach (var dest in destination)
                {
                    if (filter(src, dest))
                    {
                        context.Mapper.Map(src, dest);
                        exits = true;
                        break;
                    }
                }

                if (!exits)
                {
                    destination.Add(context.Mapper.Map<TSource, TDest>(src));
                }
            }

            return destination;
        }
    }
}