using System;
using System.Collections.Generic;
using System.Text;

namespace LazyConsole.Utilities
{
    public static class OptionExtensions
    {
        public static int CompareOptions(KeyValuePair<char, Tuple<string, Action>> optionA, KeyValuePair<char, Tuple<string, Action>> optionB)
        {
            string optionAValue = new string(new char[] { optionA.Key });
            string optionBValue = new string(new char[] { optionB.Key });

            return optionAValue.CompareTo(optionBValue);
        }
    }
}
