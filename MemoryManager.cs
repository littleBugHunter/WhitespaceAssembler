using System.Collections.Generic;

namespace WhitespaceAssembler
{
    public class MemoryManager
    {
        private Dictionary<string, int> m_labels    = new Dictionary<string, int>();
        private Dictionary<string, int> m_variables = new Dictionary<string, int>();

        public string GetVariable(string name)
        {
            if (!m_variables.ContainsKey(name))
                m_variables[name] = m_variables.Count;
            return m_variables[name].ToWhitespace();
        }
        
        public string GetLabel(string name)
        {
            if (!m_labels.ContainsKey(name))
                m_labels[name] = m_labels.Count;
            return m_labels[name].ToWhitespace();
        }
    }
}