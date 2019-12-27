using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace WhitespaceAssembler
{
    public class InstructionAttribute : Attribute
    {
        public string Name => m_name;
        public string ParamLayout => m_paramLayout;
        private string m_name;
        private string m_paramLayout;

        public InstructionAttribute(string name, string paramLayout = "")
        {
            m_name = name;
            m_paramLayout = paramLayout;
        }

    }
    
    public class Instructions
    {
        delegate string InstructionFunctionDelegate(string[] parameters, ref MemoryManager memoryManager);

        private static Dictionary<string, InstructionFunctionDelegate> instructions;

        private MemoryManager m_memoryManager;
        
        public Instructions(MemoryManager memoryManager)
        {
            m_memoryManager = memoryManager;
            if (instructions == null)
            {
                instructions = new Dictionary<string, InstructionFunctionDelegate>();
                var functions = typeof(Instructions).GetMethods(BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public).Where(info => info.GetCustomAttributes(typeof(Attribute), false).Length > 0).ToArray();
                foreach (var function in functions)
                {
                    InstructionAttribute attrib = function.GetCustomAttributes(typeof(Attribute), false)[0] as InstructionAttribute;
                    instructions[attrib.Name + "_" + attrib.ParamLayout] = (string[] parameters, ref MemoryManager manager) => (string) function.Invoke(null, new object[] {parameters, manager});
                }
            }
        }

        public string ConvertInstruction(string[] splits)
        {
            string name = splits[0].ToLower() + "_";
            for(int i = 1; i < splits.Length; ++i)
            {
                var parameter = splits[i];
                if(char.IsNumber(parameter[0]) || parameter[0] == '&' || parameter[0] == '#' || parameter[0] == '\'')
                    name += '#';
                else if (parameter[0] == '.')
                    name += '.';
                else if (parameter[0] == '*')
                    name += '*';
                else
                    throw new Exception("Unable to parse parameter: " + parameter);
            }

            if (instructions.ContainsKey(name))
            {
                string[] parameters = new string[splits.Length-1];
                Array.Copy(splits, 1, parameters, 0, splits.Length-1);
                return instructions[name](parameters, ref m_memoryManager);
            }
            else
            {
                throw new Exception("Unknown Command: " + name);
            }
        }

        #region Stack Maniupulation
        [Instruction("push", "#")]
        private static string PushNumber(string[] parameters, ref MemoryManager memoryManager)
        {
            return "  " + GetNumber(parameters[0], ref memoryManager);
        }

        [Instruction("push", "*")]
        private static string PushVariable(string[] parameters, ref MemoryManager memoryManager)
        {
            return GetVariable(parameters[0], ref memoryManager); //Retrieve the variable value
        }

        [Instruction("dup")]
        private static string Duplicate(string[] parameters, ref MemoryManager memoryManager)
        {
            return " \n ";
        }
        
        [Instruction("swap")]
        private static string Swap(string[] parameters, ref MemoryManager memoryManager)
        {
            return " \n\t";
        }
        
        [Instruction("pop")]
        private static string Pop(string[] parameters, ref MemoryManager memoryManager)
        {
            return " \n\n";
        }
        #endregion
        
        #region Arithmetic
        #region Add
        [Instruction("add")]
        private static string AddStackStack(string[] parameters, ref MemoryManager memoryManager)
        {
            return "\t   ";
        }
        
        [Instruction("add", "#")]
        private static string AddStackNumber(string[] parameters, ref MemoryManager memoryManager)
        {
            return "  " + GetNumber(parameters[0], ref memoryManager) + 
                   "\t   ";
        }
        
        [Instruction("add", "*")]
        private static string AddStackVariable(string[] parameters, ref MemoryManager memoryManager)
        {
            return GetVariable(parameters[0], ref memoryManager) + 
                   "\t   ";
        }
        
        [Instruction("add", "*#")]
        private static string AddVariableNumber(string[] parameters, ref MemoryManager memoryManager)
        {
            return GetVariable(parameters[0], ref memoryManager) + 
                   "  " + GetNumber(parameters[1], ref memoryManager) + 
                   "\t   ";
        }
        
        [Instruction("add", "#*")]
        private static string AddNumberVariable(string[] parameters, ref MemoryManager memoryManager)
        {
            return "  " + GetNumber(parameters[0], ref memoryManager) + 
                   GetVariable(parameters[1], ref memoryManager) + 
                   "\t   ";
        }
        
        [Instruction("add", "**")]
        private static string AddVariableVariable(string[] parameters, ref MemoryManager memoryManager)
        {
            return GetVariable(parameters[0], ref memoryManager) + 
                   GetVariable(parameters[1], ref memoryManager) + 
                   "\t   ";
        }
        #endregion
        #region Sub
        [Instruction("sub")]
        private static string SubStackStack(string[] parameters, ref MemoryManager memoryManager)
        {
            return "\t  \t";
        }
        
        [Instruction("sub", "#")]
        private static string SubStackNumber(string[] parameters, ref MemoryManager memoryManager)
        {
            return "  " + GetNumber(parameters[0], ref memoryManager) + 
                   "\t  \t";
        }
        
        [Instruction("sub", "*")]
        private static string SubStackVariable(string[] parameters, ref MemoryManager memoryManager)
        {
            return GetVariable(parameters[0], ref memoryManager) + 
                   "\t  \t";
        }
        
        [Instruction("sub", "*#")]
        private static string SubVariableNumber(string[] parameters, ref MemoryManager memoryManager)
        {
            return GetVariable(parameters[0], ref memoryManager) + 
                   "  "                                      + GetNumber(parameters[1], ref memoryManager) + 
                   "\t  \t";
        }
        
        [Instruction("sub", "#*")]
        private static string SubNumberVariable(string[] parameters, ref MemoryManager memoryManager)
        {
            return "  "                                      + GetNumber(parameters[0], ref memoryManager) + 
                   GetVariable(parameters[1], ref memoryManager) + 
                   "\t  \t";
        }
        
        [Instruction("sub", "**")]
        private static string SubVariableVariable(string[] parameters, ref MemoryManager memoryManager)
        {
            return GetVariable(parameters[0], ref memoryManager) + 
                   GetVariable(parameters[1], ref memoryManager) + 
                   "\t  \t";
        }
        #endregion
        #region Mul
        [Instruction("mul")]
        private static string MulStackStack(string[] parameters, ref MemoryManager memoryManager)
        {
            return "\t  \n";
        }
        
        [Instruction("mul", "#")]
        private static string MulStackNumber(string[] parameters, ref MemoryManager memoryManager)
        {
            return "  " + GetNumber(parameters[0], ref memoryManager) + 
                   "\t  \n";
        }
        
        [Instruction("mul", "*")]
        private static string MulStackVariable(string[] parameters, ref MemoryManager memoryManager)
        {
            return GetVariable(parameters[0], ref memoryManager) + 
                   "\t  \n";
        }
        
        [Instruction("mul", "*#")]
        private static string MulVariableNumber(string[] parameters, ref MemoryManager memoryManager)
        {
            return GetVariable(parameters[0], ref memoryManager) + 
                   "  "                                      + GetNumber(parameters[1], ref memoryManager) + 
                   "\t  \n";
        }
        
        [Instruction("mul", "#*")]
        private static string MulNumberVariable(string[] parameters, ref MemoryManager memoryManager)
        {
            return "  "                                      + GetNumber(parameters[0], ref memoryManager) + 
                   GetVariable(parameters[1], ref memoryManager) + 
                   "\t  \n";
        }
        
        [Instruction("mul", "**")]
        private static string MulVariableVariable(string[] parameters, ref MemoryManager memoryManager)
        {
            return GetVariable(parameters[0], ref memoryManager) + 
                   GetVariable(parameters[1], ref memoryManager) + 
                   "\t  \n";
        }
        #endregion
        #region Div
        [Instruction("div")]
        private static string DivStackStack(string[] parameters, ref MemoryManager memoryManager)
        {
            return "\t \t ";
        }
        
        [Instruction("div", "#")]
        private static string DivStackNumber(string[] parameters, ref MemoryManager memoryManager)
        {
            return "  " + GetNumber(parameters[0], ref memoryManager) + 
                   "\t \t ";
        }
        
        [Instruction("div", "*")]
        private static string DivStackVariable(string[] parameters, ref MemoryManager memoryManager)
        {
            return GetVariable(parameters[0], ref memoryManager) + 
                   "\t \t ";
        }
        
        [Instruction("div", "*#")]
        private static string DivVariableNumber(string[] parameters, ref MemoryManager memoryManager)
        {
            return GetVariable(parameters[0], ref memoryManager) + 
                   "  "                                      + GetNumber(parameters[1], ref memoryManager) + 
                   "\t \t ";
        }
        
        [Instruction("div", "#*")]
        private static string DivNumberVariable(string[] parameters, ref MemoryManager memoryManager)
        {
            return "  "                                      + GetNumber(parameters[0], ref memoryManager) + 
                   GetVariable(parameters[1], ref memoryManager) + 
                   "\t \t ";
        }
        
        [Instruction("div", "**")]
        private static string DivVariableVariable(string[] parameters, ref MemoryManager memoryManager)
        {
            return GetVariable(parameters[0], ref memoryManager) + 
                   GetVariable(parameters[1], ref memoryManager) + 
                   "\t \t ";
        }
        #endregion
        #region Mod
        [Instruction("mod")]
        private static string ModStackStack(string[] parameters, ref MemoryManager memoryManager)
        {
            return "\t \t\t";
        }
        
        [Instruction("mod", "#")]
        private static string ModStackNumber(string[] parameters, ref MemoryManager memoryManager)
        {
            return "  " + GetNumber(parameters[0], ref memoryManager) + 
                   "\t \t\t";
        }
        
        [Instruction("mod", "*")]
        private static string ModStackVariable(string[] parameters, ref MemoryManager memoryManager)
        {
            return GetVariable(parameters[0], ref memoryManager) + 
                   "\t \t\t";
        }
        
        [Instruction("mod", "*#")]
        private static string ModVariableNumber(string[] parameters, ref MemoryManager memoryManager)
        {
            return GetVariable(parameters[0], ref memoryManager) + 
                   "  "                                      + GetNumber(parameters[1], ref memoryManager) + 
                   "\t \t\t";
        }
        
        [Instruction("mod", "#*")]
        private static string ModNumberVariable(string[] parameters, ref MemoryManager memoryManager)
        {
            return "  "                                      + GetNumber(parameters[0], ref memoryManager) + 
                   GetVariable(parameters[1], ref memoryManager) + 
                   "\t \t\t";
        }
        
        [Instruction("mod", "**")]
        private static string ModVariableVariable(string[] parameters, ref MemoryManager memoryManager)
        {
            return GetVariable(parameters[0], ref memoryManager) + 
                   GetVariable(parameters[1], ref memoryManager) + 
                   "\t \t\t";
        }
        #endregion
        #endregion
        
        #region Heap Access
        [Instruction("store")]
        private static string Store(string[] parameters, ref MemoryManager memoryManager)
        {
            return "\t\t ";
        }
        
        [Instruction("store", "#")]
        private static string StoreAtAddress(string[] parameters, ref MemoryManager memoryManager)
        {
            return "  " + GetNumber(parameters[0], ref memoryManager) + 
                   " \n\t" +  //Swap
                   "\t\t ";
        }
        
        [Instruction("store", "##")]
        private static string StoreNumberAtAddress(string[] parameters, ref MemoryManager memoryManager)
        {
            return "  " + GetNumber(parameters[1], ref memoryManager) + 
                   "  " + GetNumber(parameters[0], ref memoryManager) + 
                   "\t\t ";
        }

        [Instruction("retrieve")]
        private static string Retrieve(string[] parameters, ref MemoryManager memoryManager)
        {
            return "\t\t\t";
        }
        [Instruction("retrieve", "#")]
        private static string RetrieveFromAddress(string[] parameters, ref MemoryManager memoryManager)
        {
            return "  " + GetNumber(parameters[0], ref memoryManager) + 
                   "\t\t\t";
        }
        #endregion

        #region Flow Control
        
        [Instruction("lbl", ".")]
        private static string Label(string[] parameters, ref MemoryManager memoryManager)
        {
            return "\n  " + memoryManager.GetLabel(parameters[0].Substring(1));
        }
        
        [Instruction("call", ".")]
        private static string Call(string[] parameters, ref MemoryManager memoryManager)
        {
            return "\n \t" + memoryManager.GetLabel(parameters[0].Substring(1));
        }
        
        [Instruction("jmp", ".")]
        private static string Jump(string[] parameters, ref MemoryManager memoryManager)
        {
            return "\n \n" + memoryManager.GetLabel(parameters[0].Substring(1));
        }
        
        [Instruction("jpz", ".")]
        private static string JumpZero(string[] parameters, ref MemoryManager memoryManager)
        {
            return "\n\t " + memoryManager.GetLabel(parameters[0].Substring(1));
        }
        
        [Instruction("jpn", ".")]
        private static string JumpNegative(string[] parameters, ref MemoryManager memoryManager)
        {
            return "\n\t\t" + memoryManager.GetLabel(parameters[0].Substring(1));
        }

        [Instruction("ret")]
        private static string Ret(string[] parameters, ref MemoryManager memoryManager)
        {
            return "\n\t\n";
        }
        
        [Instruction("exit")]
        private static string Exit(string[] parameters, ref MemoryManager memoryManager)
        {
            return "\n\n\n";
        }
        #endregion

        #region IO
        [Instruction("print_char")]
        private static string PrintChar(string[] parameters, ref MemoryManager memoryManager)
        {
            return "\t\n  ";
        }
        
        [Instruction("print_number")]
        private static string PrintNumber(string[] parameters, ref MemoryManager memoryManager)
        {
            return "\t\n \t";
        }
        
        [Instruction("read_char")]
        private static string ReadChar(string[] parameters, ref MemoryManager memoryManager)
        {
            return "\t\n\t ";
        }
        
        [Instruction("read_number")]
        private static string ReadNumber(string[] parameters, ref MemoryManager memoryManager)
        {
            return "\t\n\t\t";
        }
        
        #endregion
        

        

        private static string GetVariable(string var, ref MemoryManager memoryManager)
        {
            return "  " + memoryManager.GetVariable(var.Substring(1)) + // Push the address to the Stack
                   "\t\t\t";                                            //Retrieve the variable value
        }

        private static string GetNumber(string text, ref MemoryManager memoryManager)
        {
            if(char.IsNumber(text[0]))
            {
                return int.Parse(text, NumberStyles.Integer).ToWhitespace();
            }
            else if (text[0] == '#')
            {
                return int.Parse(text.Substring(1), NumberStyles.HexNumber).ToWhitespace();
            }
            else if (text[0] == '\'' && text.Length == 3)
            {
                return ((int) text[1]).ToWhitespace();
            }
            else if (text[0] == '&')
            {
                return memoryManager.GetVariable(text.Substring(1));
            }
            throw new Exception("Unable to parse Number " + text);
        }
        
        
    }
}