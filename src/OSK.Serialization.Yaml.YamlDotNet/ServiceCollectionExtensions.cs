using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using OSK.Serialization.Abstractions.Yaml;
using System;
using System.Collections.Generic;
using System.Linq;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.BufferedDeserialization.TypeDiscriminators;

namespace OSK.Serialization.Yaml.YamlDotNet
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddYamlDotNetSerialization(this IServiceCollection services)
            => services.AddYamlSerialization(o =>
            {
                var options = YamlDotNetSerializer.DefaultOptions;
                o.SerializationNamingConvention = options.SerializationNamingConvention;
                o.DeserializationNamingConvention = options.DeserializationNamingConvention;
                o.EnumNamingConvention = options.EnumNamingConvention;
                o.MaxDepth = options.MaxDepth;
                o.DefualtValueHandlerStrategy = options.DefualtValueHandlerStrategy;
                o.TextEncoding = options.TextEncoding;
                o.CustomTypeConverters = new List<IYamlTypeConverter>();
            });

        public static IServiceCollection AddYamlSerialization(this IServiceCollection services,
            Action<YamlDotNetSerializationOptions> options)
        {
            services.Configure(options);
            services.TryAddTransient<IYamlSerializer>(provider =>
            {
                var serializer = provider.GetRequiredService<ISerializer>();
                var deserializer = provider.GetRequiredService<IDeserializer>();
                var options = provider.GetRequiredService<IOptions<YamlDotNetSerializationOptions>>();

                return new YamlDotNetSerializer(serializer, deserializer, options.Value);
            });
            services.TryAddTransient(provider =>
            {
                var options = provider.GetRequiredService<IOptions<YamlDotNetSerializationOptions>>();
                return YamlUtilsHelpers.CreateSerializer(options.Value);
            });
            services.TryAddTransient(provider =>
            {
                var options = provider.GetRequiredService<IOptions<YamlDotNetSerializationOptions>>();
                var typeDiscriminators = provider.GetRequiredService<IEnumerable<ITypeDiscriminator>>();
                return YamlUtilsHelpers.CreateDeserializer(options.Value, typeDiscriminators.ToArray());
            });
            return services;
        }

        public static IServiceCollection AddYamlConverter<TConverter>(this IServiceCollection services)
            where TConverter : class, IYamlTypeConverter
        {
            services.AddTransient<IYamlTypeConverter, TConverter>();

            return services;
        }
    }
}
