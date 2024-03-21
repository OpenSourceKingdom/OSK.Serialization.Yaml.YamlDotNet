using YamlDotNet.Serialization;
using YamlDotNet.Serialization.BufferedDeserialization.TypeDiscriminators;

namespace OSK.Serialization.Yaml.YamlDotNet
{
    public static class YamlUtilsHelpers
    {
        public static ISerializer CreateSerializer(YamlDotNetSerializationOptions options)
        {
            var serializerBuilder = new SerializerBuilder()
                .WithNamingConvention(options.SerializationNamingConvention)
                .WithEnumNamingConvention(options.EnumNamingConvention)
                .WithMaximumRecursion(options.MaxDepth)
                .ConfigureDefaultValuesHandling(options.DefualtValueHandlerStrategy)
                .EnsureRoundtrip();

            foreach (var typeConverter in options.CustomTypeConverters)
            {
                serializerBuilder.WithTypeConverter(typeConverter);
            }

            return serializerBuilder.Build();
        }

        public static IDeserializer CreateDeserializer(YamlDotNetSerializationOptions options,
            params ITypeDiscriminator[] typeDiscriminators)
        {
            var deserializerBuilder = new DeserializerBuilder()
                .WithNamingConvention(options.DeserializationNamingConvention)
                .WithEnumNamingConvention(options.EnumNamingConvention);

            foreach (var typeConverter in options.CustomTypeConverters)
            {
                deserializerBuilder.WithTypeConverter(typeConverter);
            }

            deserializerBuilder
                .WithTypeDiscriminatingNodeDeserializer(discriminatorOptions =>
                {
                    foreach (var typeDiscriminator in typeDiscriminators)
                    {
                        discriminatorOptions.AddTypeDiscriminator(typeDiscriminator);
                    }
                });

            return deserializerBuilder.Build();
        }
    }
}
