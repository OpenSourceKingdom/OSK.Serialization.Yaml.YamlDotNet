using OSK.Serialization.Abstractions.Yaml;
using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace OSK.Serialization.Yaml.YamlDotNet
{
    public class YamlDotNetSerializer : IYamlSerializer
    {
        #region Variables

        public static YamlDotNetSerializationOptions DefaultOptions => new YamlDotNetSerializationOptions()
        {
            SerializationNamingConvention = PascalCaseNamingConvention.Instance,
            DeserializationNamingConvention = PascalCaseNamingConvention.Instance,
            EnumNamingConvention = PascalCaseNamingConvention.Instance,
            MaxDepth = 50,
            DefualtValueHandlerStrategy = DefaultValuesHandling.Preserve,
            TextEncoding = Encoding.UTF8
        };

        private readonly ISerializer _serializer;
        private readonly IDeserializer _deserializer;
        private readonly YamlDotNetSerializationOptions _options;

        #endregion

        #region Constructors

        public YamlDotNetSerializer()
            : this(DefaultOptions) { }

        public YamlDotNetSerializer(YamlDotNetSerializationOptions options)
            : this(YamlUtilsHelpers.CreateSerializer(options), YamlUtilsHelpers.CreateDeserializer(options), options)
        {
        }

        public YamlDotNetSerializer(ISerializer serializer, IDeserializer deserializer, YamlDotNetSerializationOptions options)
        {
            _serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
            _deserializer = deserializer ?? throw new ArgumentNullException(nameof(deserializer));
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }

        #endregion

        #region IYamlSerializer

        public ValueTask<object> DeserializeAsync(byte[] data, Type type, CancellationToken cancellationToken = default)
        {
            using var memoryStream = new MemoryStream(data);
            using var streamReader = new StreamReader(memoryStream);

            return new ValueTask<object>(_deserializer.Deserialize(streamReader, type));
        }

        public ValueTask<byte[]> SerializeAsync(object data, CancellationToken cancellationToken = default)
        {
            using var memoryStream = new MemoryStream();
            using var textWriter = new StreamWriter(memoryStream, _options.TextEncoding);
            textWriter.AutoFlush = true;

            _serializer.Serialize(textWriter, data);
            return new ValueTask<byte[]>(memoryStream.ToArray());
        }

        #endregion
    }
}
