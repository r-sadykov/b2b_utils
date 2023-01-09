using System;
using System.Collections.Generic;
using B2B_Utils.Model.OperationsLog;

namespace B2B_Utils.Model.Common
{
    internal static class WorkingWithText
    {
        public static string ExtractString(ref string text, string startWord, string endWord)
        {
            text.Remove(0, text.IndexOf(startWord));
            text = text.Substring(text.IndexOf(startWord, StringComparison.Ordinal) + startWord.Length);
            string value = text.Substring(0, text.IndexOf(endWord));
            text = text.Substring(text.IndexOf(endWord) + 1);
            return value;
        }

        public static string ExtractString(ref string text, string startWord, char endCharacter)
        {
            text.Remove(0, text.IndexOf(startWord));
            text = text.Substring(text.IndexOf(startWord, StringComparison.Ordinal) + startWord.Length);
            string value = text.Substring(0, text.IndexOf(endCharacter));
            text = text.Substring(text.IndexOf(endCharacter) + 1);
            return value;
        }

        public static string ExtractString(ref string text, char startCharacter, char endCharacter)
        {
            text.Remove(0, text.IndexOf(startCharacter));
            text = text.Substring(text.IndexOf(startCharacter)+1);
            string value = text.Substring(0, text.IndexOf(endCharacter));
            text = text.Substring(text.IndexOf(endCharacter) + 1);
            return value;
        }

        public static string ExtractString(ref string text, char character)
        {
            text.Remove(0, text.IndexOf(character));
            string value = text.Substring(0, text.IndexOf(character));
            text = text.Substring(text.IndexOf(character)+1);
            return value;
        }

        public static bool IsContainText(string content, List<Agencies.IgnoreWord> ignoreList)
        {
            foreach(var item in ignoreList) {
                if (content.Contains(item.Word)) return true;
            }
            return false;
        }

        public static bool IsContainText(string content, List<Agencies.Agent> agencies)
        {
            foreach (var item in agencies) {
                if (content.Contains(item.Code)) return true;
            }
            return false;
        }
    }
}
