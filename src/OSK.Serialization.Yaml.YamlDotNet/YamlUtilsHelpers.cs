using YamlDotNet.Serialization;
using YamlDotNet.Serialization.BufferedDeserialization.TypeDiscriminators;

namespace OSK.Serialization.Yaml.YamlDotNet
{
    public static class YamlUtilsHelpers
    {
        /// <summary>
        /// Creates a YamlDotNet serializer from a <see cref="YamlDotNetSerializationOptions"/> object
        /// </summary>
        /// <param name="options">The options to create the serializer with</param>
        /// <returns>A YamlDotNet Serializer</returns>
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

        /// <summary>
        /// Creates a YamlDotNet deserializer using a <see cref="YamlDotNetSerializationOptions"/> object and a list of potential discriminator objects for polymorphism
        /// </summary>
        /// <param name="options">The options to create the deserialzier with</param>
        /// <param name="typeDiscriminators">The list of polymorphic type discriminator handler</param>
        /// <returns>The YamlDotNet deserializer</returns>
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
