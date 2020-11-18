using System;
using System.IO;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace DocsTextFormatter.Test
{
    [TestFixture]
    public class DocFormatterTests
    {
        private MemoryStream GetStreamFromString(string text)
        {
            var result = new MemoryStream();
            var writer = new BinaryWriter(result);
            writer.Write(text.ToCharArray());
            result.Position = 0;
            return result;
        }

        private void Assert(string input, byte[] output)
        {
            var inStream = GetStreamFromString(input);
            var outStream = new MemoryStream();
            DocFormatter.Format(inStream, (int) inStream.Length, outStream);
            outStream.ToArray().Should().Equal(output);
        }

        [Test]
        public void MixedCaseBecomesUpper()
        {
            Assert("TeSt", new byte[] {0x54, 0x45, 0x53, 0x54, 0x00});
        }

        [Test]
        public void MultipleLineEncoding()
        {
            Assert("multi\x0D\x0Aline", new byte[] {0x4D, 0x55, 0x4C, 0x54, 0x49, 0x0D, 0x4C, 0x49, 0x4E, 0x45, 0x00});
        }

        [Test]
        public void SpaceEncoding()
        {
            Assert("space text", new byte[] {0x53, 0x50, 0x41, 0x43, 0xC5, 0x54, 0x45, 0x58, 0x54, 0x00});
        }

        [Test]
        public void SymbolEncoding()
        {
            Assert(" !\"#$%&'", new byte[] {0x20, 0x21, 0x22, 0x23, 0x24, 0x25, 0x26, 0x27, 0x00});
            Assert("()*+,-./", new byte[] {0x28, 0x29, 0x2A, 0x2B, 0x2C, 0x2D, 0x2E, 0x2F, 0x00});
        }

        [Test]
        public void MultiSpaceEncoding()
        {
            Assert("     ", new byte[] {0xA0, 0xA0, 0x20, 0x00});
        }

        [Test]
        public void SkipInvalidCharacters()
        {
            Assert("\x01\x02\x03" + "a", new byte[] {0x41, 0x00});
        }
    }
}