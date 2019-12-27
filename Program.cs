using System;
using System.Diagnostics;
using System.IO;

namespace WhitespaceAssembler
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.Out.WriteLine("Usage: WhitespaceAssembler [InputFile] [OutputFile]");
                return;
            }
            var reader = File.OpenText(args[0]);
            Assembler asm = new Assembler();
            int line = 0;
            while (!reader.EndOfStream)
            {
                ++line;
                try
                {
                    asm.ParseLine(reader.ReadLine());
                }
                catch (Exception e)
                {
                    Console.Write("Error in Line " + line + ": ");
                    Console.WriteLine(e);
                    return;
                }
            }
            File.WriteAllText(args[1], asm.GetString());

        }
    }
}