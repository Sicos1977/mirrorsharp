using System.Buffers;
using System.Text;
using Xunit;
using MirrorSharp.Internal;
using MirrorSharp.Advanced;

namespace MirrorSharp.Tests {
    public class FastUtf8JsonWriterTests {
        [Fact]
        public void WriteValue_WritesNull() {
            var writer = CreateWriter();
            writer.WriteValue((string?)null);

            var result = GetWrittenAsString(writer);
            Assert.Equal("null", result);
        }

        [Theory]
        [InlineData("a\nb", "a\\nb")]
        [InlineData("a\"b", @"a\""b")]
        [InlineData("a\\b", "a\\\\b")]
        [InlineData("a\0b", "a\\u0000b")]
        [InlineData("aÀb", "aÀb")]
        [InlineData("a❀b", "a❀b")]
        [InlineData("a🌄b", "a🌄b")]
        public void WriteValue_WritesString(string input, string expected) {
            var writer = CreateWriter();
            writer.WriteValue(input);

            var result = GetWrittenAsString(writer);
            Assert.Equal('"' + expected + '"', result);
        }

        [Fact]
        public void WriteValue_WritesVeryLongString() {
            var input = new string('x', 10000);
            var writer = CreateWriter();
            writer.WriteValue(input);

            var result = GetWrittenAsString(writer);
            Assert.Equal("\"" + input + "\"", result);
        }

        [Theory]
        [InlineData('a',  "a")]
        [InlineData('\n', "\\n")]
        [InlineData('\\', "\\\\")]
        [InlineData('❀', "❀")]
        [InlineData('\u0080', "\u0080")]
        [InlineData('\u00a0', "\u00a0")]
        [InlineData('À', "À")]
        public void WriteValue_WritesChar(char input, string expected) {
            var writer = CreateWriter();
            writer.WriteValue(input);

            var result = GetWrittenAsString(writer);
            Assert.Equal('"' + expected + '"', result);
        }
        
        [Theory]
        [InlineData(-10)]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(13)]
        [InlineData(100)]
        [InlineData(113)]
        [InlineData(1113)]
        [InlineData(11113)]
        [InlineData(111113)]
        public void WriteValue_WritesInt32(int input) {
            var writer = CreateWriter();
            writer.WriteValue(input);

            var result = GetWrittenAsString(writer);
            Assert.Equal(input.ToString("D", null), result);
        }

        [Theory]
        [InlineData(true, "true")]
        [InlineData(false, "false")]
        public void WriteValue_WritesBoolean(bool input, string expected) {
            var writer = CreateWriter();
            writer.WriteValue(input);

            var result = GetWrittenAsString(writer);
            Assert.Equal(expected, result);
        }

        [Fact]
        public void WriteValue_WritesCommaBeforeSecondArrayValue_WhenValueIsString() {
            var writer = CreateWriter();
            writer.WriteStartArray();
            writer.WriteValue("e1");
            writer.WriteValue("e2");
            writer.WriteEndArray();

            var result = GetWrittenAsString(writer);
            Assert.Equal("[\"e1\",\"e2\"]", result);
        }

        [Fact]
        public void WriteValue_WritesCommaBeforeSecondArrayValue_WhenValueIsInt32() {
            var writer = CreateWriter();
            writer.WriteStartArray();
            writer.WriteValue(1);
            writer.WriteValue(2);
            writer.WriteEndArray();

            var result = GetWrittenAsString(writer);
            Assert.Equal("[1,2]", result);
        }

        [Fact]
        public void WriteValue_WritesCommaBeforeSecondArrayValue_WhenValueIsBoolean() {
            var writer = CreateWriter();
            writer.WriteStartArray();
            writer.WriteValue(false);
            writer.WriteValue(true);
            writer.WriteEndArray();

            var result = GetWrittenAsString(writer);
            Assert.Equal("[false,true]", result);
        }

        [Fact]
        public void WriteProperty_WritesCommaBeforeProperty_AfterNestedObject() {
            var writer = CreateWriter();
            writer.WriteStartObject();
            writer.WritePropertyStartObject("p1");
            writer.WriteEndObject();
            writer.WriteProperty("p2", 0);
            writer.WriteEndObject();

            var result = GetWrittenAsString(writer);
            Assert.Equal("{\"p1\":{},\"p2\":0}", result);
        }

        [Fact]
        public void OpenString_ReturnsWriterThatCorrectlyWritesValueIntoString_WhenValueIsInt32() {
            var writer = CreateWriter();
            using (var stringWriter = writer.OpenString()) {
                stringWriter.Write(42);
            }

            var result = GetWrittenAsString(writer);
            Assert.Equal("\"42\"", result);
        }

        [Fact]
        public void OpenString_ReturnsWriterThatCorrectlyWritesValueIntoString_WhenValueIsChar() {
            var writer = CreateWriter();
            using (var stringWriter = writer.OpenString()) {
                stringWriter.Write('"');
            }

            var result = GetWrittenAsString(writer);
            Assert.Equal("\"\\\"\"", result);
        }

        [Fact]
        public void OpenString_ReturnsWriterThatCorrectlyWritesValueIntoString_WhenValueIsString() {
            var writer = CreateWriter();
            using (var stringWriter = writer.OpenString()) {
                stringWriter.Write("a\"b");
            }

            var result = GetWrittenAsString(writer);
            Assert.Equal("\"a\\\"b\"", result);
        }

        [Fact]
        public void OpenString_CanBeUsedMultipleTimes() {
            var writer = CreateWriter();
            writer.WriteStartArray();
            using (var stringWriter = writer.OpenString()) stringWriter.Write("a");
            using (var stringWriter = writer.OpenString()) stringWriter.Write("b");
            writer.WriteEndArray();

            var result = GetWrittenAsString(writer);
            Assert.Equal("[\"a\",\"b\"]", result);
        }

        private static string GetWrittenAsString(FastUtf8JsonWriter writer) {
            return Encoding.UTF8.GetString(writer.WrittenSegment);
        }

        private static FastUtf8JsonWriter CreateWriter() {
            return new FastUtf8JsonWriter(ArrayPool<byte>.Shared);
        }
    }
}
