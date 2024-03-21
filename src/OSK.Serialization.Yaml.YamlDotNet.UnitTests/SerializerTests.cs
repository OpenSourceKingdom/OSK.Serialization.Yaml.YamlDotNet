using OSK.Serialization.Abstractions.Yaml;
using OSK.Serialization.Yaml.YamlDotNet.UnitTests.Helpers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace OSK.Serialization.Yaml.YamlDotNet.UnitTests
{
    public abstract class SerializerTests
    {
        #region Variables

        protected IYamlSerializer Serializer { get; }

        #endregion

        #region Constructors

        protected SerializerTests(IYamlSerializer serializer)
        {
            Serializer = serializer;
        }

        #endregion

        #region SerializeAsync

        [Fact]
        public async Task SerializeAsync_ValidObject_ReturnsByteArray()
        {
            // Arrange
            var testParameters = new TestMessage()
            {
                Name = "HelloWorld",
                Data = new List<TestData>() {
                        new TestData() {
                            Index = 117,
                            Data = new TestClass()
                            {
                                A = 42,
                                B = "ABC",
                                C = null
                            }
                        }
                    }
            };

            // Act
            var result = await Serializer.SerializeAsync(testParameters);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Length > 0);
        }

        #endregion

        #region DeserializeAsync

        [Fact]
        public async Task DeserializeAsync_ValidBytes_ReturnsObjectOfType()
        {
            // Arrange
            var testParameters = await GetDeserializationTestParametersAsync();

            // Act
            var result = await Serializer.DeserializeAsync(testParameters.Data, testParameters.ExpectedResult.GetType());

            // Assert
            AssertEqual(testParameters.ExpectedResult, result);
        }

        #endregion

        #region End To End 

        [Fact]
        public async Task Serializer_EndToEnd_ValidateObjectToBytes_And_BytesToObject()
        {
            // Arrange
            var testParameters = new TestMessage()
            {
                Name = "HelloWorld",
                Data = new List<TestData>() {
                        new TestData() {
                            Index = 117,
                            Data = new TestClass()
                            {
                                A = 42,
                                B = "ABC",
                                C = null
                            }
                        }
                    }
            };

            // Act
            var serializedData = await Serializer.SerializeAsync(testParameters);
            var deserializedData = await Serializer.DeserializeAsync(serializedData, typeof(TestMessage));

            // Assert
            AssertEqual(testParameters, deserializedData);
        }

        #endregion

        #region Helpers

        protected abstract Task<DeserializationTestParameters> GetDeserializationTestParametersAsync();

        private void AssertEqual(TestMessage expected, object actual)
        {
            Assert.NotNull(actual);
            Assert.IsType<TestMessage>(actual);

            var message = (TestMessage)actual;
            Assert.Equal(expected.Name, message.Name);
            Assert.Equal(expected.Data.Count(), message.Data.Count());

            for (var i = 0; i < expected.Data.Count(); i++)
            {
                var expectedData = expected.Data.ElementAt(i);
                var actualData = message.Data.ElementAt(i);

                Assert.Equal(expectedData.Index, actualData.Index);

                Assert.IsType<TestClass>(expectedData.Data);
                Assert.IsType<TestClass>(actualData.Data);

                var expectedTestClass = (TestClass)expectedData.Data;
                var actualTestClass = (TestClass)actualData.Data;

                AssertEqual(expectedTestClass, actualTestClass);
            }
        }

        private void AssertEqual(TestClass expected, TestClass actual)
        {
            Assert.Equal(expected.A, actual.A);
            Assert.Equal(expected.B, actual.B);

            if (expected.C == null && actual.C == null)
            {
                return;
            }

            Assert.NotNull(expected.C);
            Assert.NotNull(actual.C);

            AssertEqual(expected.C, actual.C);
        }

        #endregion
    }
}
