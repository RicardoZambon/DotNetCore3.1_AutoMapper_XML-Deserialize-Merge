using AutoMapper;
using System;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using XmlDeserializeMergeWithAutoMapper.Extensions;
using XmlDeserializeMergeWithAutoMapper.Interfaces;
using XmlDeserializeMergeWithAutoMapper.XmlNodes.AppModel;
using XmlDeserializeMergeWithAutoMapper.XmlNodes.Model;
using XmlDeserializeMergeWithAutoMapper.XmlNodes.WebModel;

namespace XmlDeserializeMergeWithAutoMapper
{
    class Program
    {
        static void Main(string[] args)
        {
            var model = GetModel<Application>("Level0_Model");
            var model_2 = GetModel<Application>("Level0_Model_2");
            var webModel = GetModel<WebApplication>("Level1_WebModel");
            var appModel = GetModel<AppApplication>("Level2_AppModel");

            var mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.AddMaps(typeof(Program).Assembly);

                cfg.AddProfile(new EntityMappingProfile(typeof(AppApplication)));

                cfg.ForAllPropertyMaps(
                    pm => pm.SourceMember.Name == pm.DestinationName,
                    (pm, ce) =>
                    {
                        ce.Condition((source, dest, sMem, destMem) => sMem != null && destMem == null);
                    });
            });
            var mapper = mapperConfiguration.CreateMapper();

            mapper.Map(model, model_2);
            mapper.Map(model_2, webModel);
            mapper.Map(webModel, appModel);

            Console.WriteLine("Hello World!");
        }

        private static TApplication GetModel<TApplication>(string modelFileName)
            where TApplication : class, IApplication
        {
            var xmlSerializer = new XmlSerializer(typeof(TApplication));

            using (var streamReader = new StreamReader(@$"XmlFiles\{modelFileName}.xml"))
            {
                return (TApplication)xmlSerializer.Deserialize(streamReader);
            }
        }

        public class EntityMappingProfile : Profile
        {
            public EntityMappingProfile()
            {

            }

            public EntityMappingProfile(Type parentModelType)
            {
                var listTypes = parentModelType.GetListTypes().ToArray();

                foreach (var listType in listTypes)
                {
                    for (var i = 0; i < listType.Value.Length; i++)
                    {
                        var destinationType = listType.Value[i];
                        this.MapLists(destinationType, destinationType);

                        if (i < listType.Value.Length - 1)
                        {
                            var sourceType = listType.Value[i + 1];
                            this.MapLists(sourceType, destinationType);
                        }
                    }
                }
            }
        }
    }
}