using System.Text.Json.Serialization;

namespace OSK.Serialization.Yaml.YamlDotNet.UnitTests.Helpers
{
    public class TestClass
    {
        [JsonConstructor]
        public TestClass() { }

        public int A { get; set; }

        public string B { get; set; }

        public TestClass C { get; set; }
    }
}
