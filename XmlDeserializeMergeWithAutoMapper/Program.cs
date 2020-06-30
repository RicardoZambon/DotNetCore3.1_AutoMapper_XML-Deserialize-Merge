using AutoMapper;
using System;
using System.IO;
using System.Xml.Serialization;
using XmlDeserializeMergeWithAutoMapper.AutoMapper;
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
                cfg.AddProfile(new ModelMappingProfile<AppApplication>());
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
    }
}