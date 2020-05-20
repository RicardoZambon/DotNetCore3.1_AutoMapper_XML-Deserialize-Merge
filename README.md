# XML deserialization with merge using AutoMapper
Example of how deserialize a XML file and merge its values with other XML deserialized objects using AutoMapper.


Created all classes for XML deserialization and added the ```[AutoMap]``` attribute, with this, the AutoMapper can map each class with its own and the base class.

```c#
[AutoMap(typeof(Entity))]
public class Entity
{
}

[AutoMap(typeof(WebEntity)), AutoMap(typeof(Entity))]
public class WebEntity : Entity
{
}

[AutoMap(typeof(AppEntity)), AutoMap(typeof(WebEntity))]
public class AppEntity : WebEntity
{
}
```

When mapping lists, I want to have some conditions when merging the items and not, for that, I created the attribute ```[MergeKey]```. This indicates the merge should only happen when both instances have the same key values.

```c#
public class MergeKeyAttribute : Attribute
{
}

public class Entity
{
  [XmlAttribute, MergeKey]
  public string Id { get; set; }

  [XmlAttribute]
  public string DisplayName { get; set; }
}
```

The issue was how the AutoMapper should understand when Map both item instances and I resolved this with a custom TypeConverter from AutoMapper.

```c#
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
```

The only issue now was how read the list properties and Map each of them and their parent classes dynamically in AutoMapper, well, for that, I used Reflection.

Created a method that reads all properties from the main class, search for any property implementing ```IEnumerable``` and read all base classes.

```c#
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

```

And finally, created a AutoMapper profile and the magic happens!

```c#
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
```

```c#
static void Main(string[] args)
{
    var model = GetModel<Application>("Level0_Model");
    var webModel = GetModel<WebApplication>("Level1_WebModel");
    var appModel = GetModel<AppApplication>("Level2_AppModel");

    var mapperConfiguration = new MapperConfiguration(cfg =>
    {
        cfg.AddMaps(typeof(Program).Assembly);
        cfg.AddProfile(new EntityMappingProfile(typeof(AppApplication)));
    });
    var mapper = mapperConfiguration.CreateMapper();

    mapper.Map(model, model_2);
    mapper.Map(webModel, appModel);
}
```
