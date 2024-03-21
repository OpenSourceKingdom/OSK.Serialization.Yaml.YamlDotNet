using System.Collections.Generic;

namespace OSK.Serialization.Yaml.YamlDotNet.UnitTests.Helpers
{
    public class TestMessage
    {
        public string Name { get; set; }

        public IEnumerable<TestData> Data { get; set; }
    }
}
