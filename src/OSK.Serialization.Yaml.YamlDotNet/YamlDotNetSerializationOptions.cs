using System.Collections.Generic;
using System.Text;
using YamlDotNet.Serialization;

namespace OSK.Serialization.Yaml.YamlDotNet
{
    public class YamlDotNetSerializationOptions
    {
        public int MaxDepth { get; set; }

        public Encoding TextEncoding { get; set; }

        public INamingConvention SerializationNamingConvention { get; set; }

        public INamingConvention EnumNamingConvention { get; set; }

        public INamingConvention DeserializationNamingConvention { get; set; }

        public DefaultValuesHandling DefualtValueHandlerStrategy { get; set; }

        public IList<IYamlTypeConverter> CustomTypeConverters { get; set; } = new List<IYamlTypeConverter>();
    }
}
