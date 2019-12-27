using System;
using System.Text;

namespace WhitespaceAssembler
{
    public static class IntExtensions
    {
        public static string ToWhitespace(this int value)
        {
            StringBuilder builder = new StringBuilder(65);
            builder.Append('\n');
            bool positive = value >= 0;
            while (value > 0)
            {
                if (value % 2 == 0)
                {
                    builder.Append(' ');
                }
                else
                {
                    builder.Append('\t');
                }
                value /= 2;
            }
            if (positive)
            {
                builder.Append(' ');
            }
            else
            {
                builder.Append('\t');
            }
            
            char[] charArray = builder.ToString().ToCharArray();
            Array.Reverse( charArray );
            return new string( charArray );
        }
    }
}