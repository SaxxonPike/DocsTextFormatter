using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace DocsTextFormatter
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            if (args.Length < 1 || args.Length > 2)
            {
                Console.WriteLine("Usage: DocsTextFormatter <infile.txt> [<outfile.bin>]");
                return;
            }

            var inFile = args[0];
            if (!File.Exists(inFile))
            {
                Console.WriteLine($"File not found: {inFile}");
                return;
            }

            var outFile = args.Length > 1
                ? args[1]
                : Path.Combine(
                    Path.GetDirectoryName(inFile),
                    $"{Path.GetFileNameWithoutExtension(inFile)}.out");

            var inData = File.ReadAllBytes(inFile);

            Console.WriteLine($"Input size: {inData.Length} bytes");

            using (var input = new MemoryStream(inData))
            using (var output = new MemoryStream())
            {
                DocFormatter.Format(input, (int) input.Length, output);
                output.Flush();
                File.WriteAllBytes(outFile, output.ToArray());
                Console.WriteLine($"Output size: {output.Length}");
                Console.WriteLine($"Output file: {outFile}");
            }
        }
    }
}