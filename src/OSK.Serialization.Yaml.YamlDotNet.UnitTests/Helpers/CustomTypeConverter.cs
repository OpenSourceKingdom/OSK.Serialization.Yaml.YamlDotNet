using System;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace OSK.Serialization.Yaml.YamlDotNet.UnitTests.Helpers
{
    public class CustomTypeConverter : IYamlTypeConverter
    {
        private readonly INamingConvention _serializationConvention;

        public CustomTypeConverter(INamingConvention serializationConvetion)
        {
            _serializationConvention = serializationConvetion;
        }

        public bool Accepts(Type type)
        {
            return type == typeof(TestData);
        }

        public object ReadYaml(IParser parser, Type type, ObjectDeserializer deserializer)
        {
            var testData = new TestData();

            parser.Consume<MappingStart>();

            parser.Consume<Scalar>(); // Property Name
            testData.Index = int.Parse(parser.Consume<Scalar>().Value); // Property Value

            parser.Consume<Scalar>();
            parser.Consume<SequenceStart>();

            parser.Consume<MappingStart>();

            var testClass = new TestClass();
            parser.Consume<Scalar>();
            testClass.A = int.Parse(parser.Consume<Scalar>().Value);

            parser.Consume<Scalar>();
            testClass.B = parser.Consume<Scalar>().Value;

            parser.Consume<Scalar>();
            testClass.C = parser.Consume<Scalar>().Value == "null" ? null : throw new InvalidCastException();

            testData.Data = testClass;

            parser.Consume<MappingEnd>();

            parser.Consume<SequenceEnd>();
            parser.Consume<MappingEnd>();

            return testData;
        }

        public void WriteYaml(IEmitter emitter, object value, Type type, ObjectSerializer serializer)
        {
            var testData = value as TestData;
            if (testData == null)
            {
                throw new ArgumentNullException(nameof(testData));
            }

            emitter.Emit(new MappingStart());

            emitter.Emit(new Scalar(_serializationConvention.Apply(nameof(TestData.Index))));
            emitter.Emit(new Scalar(testData.Index.ToString()));

            emitter.Emit(new Scalar(_serializationConvention.Apply(nameof(TestData.Data))));
            emitter.Emit(new SequenceStart(AnchorName.Empty, TagName.Empty, false, SequenceStyle.Block));

            var testClass = testData.Data as TestClass;
            if (testClass == null)
            {
                throw new ArgumentNullException(nameof(testClass));
            }
            emitter.Emit(new MappingStart());

            emitter.Emit(new Scalar(_serializationConvention.Apply(nameof(TestClass.A))));
            emitter.Emit(new Scalar(testClass.A.ToString()));

            emitter.Emit(new Scalar(_serializationConvention.Apply(nameof(TestClass.B))));
            emitter.Emit(new Scalar(testClass.B));

            emitter.Emit(new Scalar(_serializationConvention.Apply(nameof(TestClass.C))));
            emitter.Emit(new Scalar("null"));

            emitter.Emit(new MappingEnd());

            emitter.Emit(new SequenceEnd());
            emitter.Emit(new MappingEnd());
        }
    }
}
