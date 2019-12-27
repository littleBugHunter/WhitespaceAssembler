using System;
using System.Collections.Generic;
using System.Text;

namespace WhitespaceAssembler
{
    public struct AssemblerSettings
    {
        public bool annotateWhitespaces; // put S T L for space tab and lf
        public bool annotateLines;       // add comments containing the asm instructions
    }
    
    public class Assembler
    {
        private readonly StringBuilder m_whitespaceString = new StringBuilder();
        private readonly MemoryManager m_memoryManager = new MemoryManager();
        private readonly Instructions m_instructions;
        
        public Assembler()
        {
            m_instructions = new Instructions(m_memoryManager);
        }

        public string GetString()
        {
            return m_whitespaceString.ToString();
        }

        public void ParseLine(string line)
        {
            var splits = SplitLine(line);
            if (splits.Length == 0) // empty line or comment
                return;
            m_whitespaceString.Append(m_instructions.ConvertInstruction(splits));
        }
        
        private string[] SplitLine(string line)
        {
            line = line.Trim();
            var splits = line.Split(new []{' ', '\t'}, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < splits.Length; i++)
            {
                splits[i] = splits[i].Trim();
                if (splits[i].StartsWith("//"))
                {
                    Array.Resize(ref splits, i);
                    break;
                }
            }
            return splits;
        }
        
    }
}