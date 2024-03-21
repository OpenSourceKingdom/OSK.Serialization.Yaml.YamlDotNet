using OSK.Serialization.Yaml.YamlDotNet.UnitTests.Helpers;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.Serialization.NamingConventions;

namespace OSK.Serialization.Yaml.YamlDotNet.UnitTests
{
    public class YamlDotNetSerializerTests : SerializerTests
    {
        #region Variables

        private static YamlDotNetSerializationOptions Options = new YamlDotNetSerializationOptions()
        {
            MaxDepth = 50,
            SerializationNamingConvention = UnderscoredNamingConvention.Instance,
            DeserializationNamingConvention = CamelCaseNamingConvention.Instance,
            EnumNamingConvention = PascalCaseNamingConvention.Instance,
            TextEncoding = Encoding.UTF8,
            CustomTypeConverters = new[]
            {
                new CustomTypeConverter(UnderscoredNamingConvention.Instance)
            }
        };

        #endregion

        #region Constructors

        public YamlDotNetSerializerTests()
            : base(new YamlDotNetSerializer(Options))
        {
        }

        #endregion

        #region SerializerTests Overrides

        protected override Task<DeserializationTestParameters> GetDeserializationTestParametersAsync()
        {
            var ymlData = @"
name: HelloWorld
data:
  - index: 117
    data:
      - a: 42
        b: ABC
        c: null
";

            return Task.FromResult(new DeserializationTestParameters()
            {
                Data = Encoding.UTF8.GetBytes(ymlData),
                ExpectedResult = new TestMessage()
                {
                    Name = "HelloWorld",
                    Data = new List<TestData>()
                    {
                        new TestData()
                        {
                            Index = 117,
                            Data = new TestClass()
                            {
                                A = 42,
                                B = "ABC",
                                C = null
                            }
                        }
                    }
                }
            });
        }

        #endregion
    }
}
