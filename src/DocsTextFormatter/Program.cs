using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace DocsTextFormatter
{
    class Program
    {
        static void Main(string[] args)
        {
            if (Debugger.IsAttached)
            {
                args = new string[] { @"C:\Users\Tony\Desktop\C64CART\combined\docs\docs.txt" };
            }

            if (args.Length == 1)
            {
                string outFile = Path.Combine(Path.GetDirectoryName(args[0]), Path.GetFileNameWithoutExtension(args[0]) + ".out");
                byte[] inData = File.ReadAllBytes(args[0]);
                using (MemoryStream input = new MemoryStream(inData), output = new MemoryStream())
                {
                    BinaryReader reader = new BinaryReader(input);
                    BinaryWriter writer = new BinaryWriter(output);

                    int length = inData.Length;
                    byte outbyte = 0;
                    byte prev = 0;

                    for (int i = 0; i < length; i++)
                    {
                        bool write = true;
                        byte b = reader.ReadByte();

                        if (b >= 0x20 && b < 0x40)
                            outbyte = (byte)(b & 0x3F);
                        else if (b > 0x60 && b <= 0x7A)
                            outbyte = (byte)(b ^ 0x20);
                        else if ((b >= 0x20 && b < 0x7F) || (b == 0x0D))
                            outbyte = b;
                        else
                            write = false;

                        if (write)
                        {
                            if (outbyte == 0x20)
                            {
                                output.Position--;
                                outbyte = (byte)(prev | 0x80);
                            }

                            prev = (byte)(outbyte & 0x7F);
                            writer.Write(outbyte);
                        }
                    }

                    writer.Write((byte)0);
                    writer.Flush();
                    File.WriteAllBytes(outFile, output.ToArray());
                }
            }
        }
    }
}
