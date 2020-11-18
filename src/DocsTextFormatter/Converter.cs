using System.IO;

namespace DocsTextFormatter
{
    public static class Converter
    {
        public static void Convert(Stream input, int inputLength, Stream output)
        {
            var reader = new BinaryReader(input);
            var writer = new BinaryWriter(output);

            var length = inputLength;
            byte outbyte = 0;
            byte prev = 0;

            for (var i = 0; i < length; i++)
            {
                var write = true;
                var b = reader.ReadByte();

                if (b >= 0x20 && b < 0x40)
                    outbyte = (byte) (b & 0x3F);
                else if (b > 0x60 && b <= 0x7A)
                    outbyte = (byte) (b ^ 0x20);
                else if ((b >= 0x20 && b < 0x7F) || (b == 0x0D))
                    outbyte = b;
                else
                    write = false;

                if (!write)
                    continue;

                if (outbyte == 0x20)
                {
                    output.Position--;
                    outbyte = (byte) (prev | 0x80);
                }

                prev = (byte) (outbyte & 0x7F);
                writer.Write(outbyte);
            }

            writer.Write((byte) 0);
        }
    }
}