using AutoMapper;
using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using XmlDeserializeMergeWithAutoMapper.Attributes;
using XmlDeserializeMergeWithAutoMapper.Extensions;
using XmlDeserializeMergeWithAutoMapper.Interfaces;

namespace XmlDeserializeMergeWithAutoMapper.AutoMapper
{
    public class ModelMappingProfile<TApplication> : Profile where TApplication : class, IApplication
    {
        public ModelMappingProfile()
        {

            MapModels(typeof(TApplication));

        }

        private void MapModels(Type modelNodeType)
        {
            CreateMap(modelNodeType, modelNodeType);

            //Map the current type with his parents
            MapType(modelNodeType);

            //Search for all properties, if any of them is a node of the model
            var properties = modelNodeType.GetProperties().Where(p => typeof(IXmlNode).IsAssignableFrom(p.PropertyType) || (typeof(IEnumerable).IsAssignableFrom(p.PropertyType) && p.PropertyType.GenericTypeArguments.Any()));
            foreach (var property in properties)
            {
                MapModels(property.PropertyType);
            }
        }

        private void MapType(Type type)
        {
            bool isList = typeof(IEnumerable).IsAssignableFrom(type) && type.IsGenericType;
            if (isList)
            {
                type = type.GenericTypeArguments[0];
            }

            while (!type.IsAbstract && type != typeof(object))
            {
                CreateModelMap(type, type, isList);

                if (type.GetCustomAttribute<ModelMapAttribute>() is ModelMapAttribute modelMap)
                {
                    CreateModelMap(modelMap.SourceType, type, false);

                    MapType(modelMap.SourceType);
                }

                if (!type.BaseType.IsAbstract && type.BaseType != typeof(object))
                {
                    CreateModelMap(type.BaseType, type, isList);
                }
                type = type.BaseType;
            }
        }

        private void CreateModelMap(Type sourceType, Type destinationType, bool isList)
        {
            //GetType()
            //    .GetMethod(nameof(CreateModelMapDynamic))
            //    .MakeGenericMethod(new Type[] { sourceType, destinationType })
            //    .Invoke(this, new object[] { isList });

            CreateMap(sourceType, destinationType, MemberList.Source)
                .ForAllMembers(o =>
                    o.Condition((source, destination, sourceMember, destinationMember) => sourceMember != null && destinationMember == null)
                );

            if (isList)
            {
                this.MapLists(sourceType, destinationType);
            }
        }

        //public void CreateModelMapDynamic<TSource, TDestination>(bool isList)
        //    where TSource : class
        //    where TDestination : class
        //{
        //    CreateMap<TSource, TDestination>()
        //        .ForAllMembers(o =>
        //         {
        //             o.Condition((source, destination, sourceMember, destMember)
        //                 => { Console.WriteLine($"source: {source}, destination: {destination}, sourceMember: {sourceMember}, destMember: {destMember}.");
        //                     return sourceMember != null && destMember == null; });
        //         });

        //    if (isList)
        //    {
        //        this.MapGenericLists<TSource, TDestination>();
        //    }
        //}
    }
}