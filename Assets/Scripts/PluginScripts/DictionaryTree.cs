using System;
using System.Collections.Generic;
using UnityEngine;

namespace poetools.Console
{
    public class DictionaryTree
    {
        private static Dictionary<char, int> CharToIndex;
        private static Dictionary<int, char> IndexToChar;
        private static readonly char[] WordBuffer = new char[100];

        private DictionaryTreeNode _rootNode;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetStatics()
        {
            CharToIndex = new Dictionary<char, int>();
            IndexToChar = new Dictionary<int, char>();
            GenerateConversionTable();
        }

        public DictionaryTree()
        {
            _rootNode = DictionaryTreeNode.Create();
        }

        private static void GenerateConversionTable()
        {
            // Adds all uppercase letters.
            for (int i = 65; i <= 90; i++)
            {
                CharToIndex.Add((char) i, i - 65);
            }

            // Adds all lowercase letters.
            for (int i = 97; i <= 122; i++)
            {
                CharToIndex.Add((char) i, i - 97);
                IndexToChar.Add(i - 97, (char) i);
            }

            CharToIndex.Add(' ', 26);
            CharToIndex.Add('_', 27);
            CharToIndex.Add('-', 28);
            CharToIndex.Add('\0', 29);

            IndexToChar.Add(26, ' ');
            IndexToChar.Add(27, '_');
            IndexToChar.Add(28, '-');
            IndexToChar.Add(29, '\0');

            // Adds all numbers.
            for (int i = 48; i <= 57; i++)
            {
                CharToIndex.Add((char) i, i - 18);
                IndexToChar.Add(i - 18, (char) i);
            }
        }

        public void Insert(string word)
        {
            _rootNode.Insert(word.ToCharArray(), 0);
        }

        public void GetWithPrefix(string prefix, List<string> results)
        {
            var charArray = prefix.ToCharArray();
            var endingNode = _rootNode.GetEndingNodeOfString(charArray, 0);
            endingNode?.GetAllWords(results, WordBuffer, 0);
        }

        public void Remove(string word)
        {
            var endingNode = _rootNode.GetEndingNodeOfString(word.ToCharArray(), 0);

            if (endingNode != null)
                endingNode.IsEnd = false; // todo: this is a bad way to remove words?
        }

        private class DictionaryTreeNode
        {
            private DictionaryTreeNode[] _children;
            public bool IsEnd;

            public static DictionaryTreeNode Create()
            {
                return new DictionaryTreeNode
                {
                    _children = new DictionaryTreeNode[CharToIndex.Count],
                };
            }

            public void Insert(char[] w, int index)
            {
                if (index >= w.Length)
                {
                    IsEnd = true;
                    return;
                }

                char c = w[index];

                if (!CharToIndex.ContainsKey(c))
                    throw new ArgumentException($"Only alphanumeric commands can be added! (\"{c}\" attempted)");

                int convertedIndex = CharToIndex[c];
                DictionaryTreeNode next = _children[convertedIndex];

                if (next == null)
                {
                    next = Create();
                    _children[convertedIndex] = next;
                }

                next.Insert(w, index + 1);
            }

            public void GetAllWords(List<string> output, char[] wordBuffer, int depth)
            {
                if (IsEnd)
                    output.Add(new string(wordBuffer, 0, depth));

                for (int i = 0; i < _children.Length; i++)
                {
                    if (_children[i] != null)
                    {
                        char cur = IndexToChar[i];
                        wordBuffer[depth] = cur;
                        _children[i].GetAllWords(output, wordBuffer, depth + 1);
                    }
                }
            }

            public DictionaryTreeNode GetEndingNodeOfString(char[] value, int index)
            {
                if (index >= value.Length)
                    return this;

                char c = value[index];

                if (!CharToIndex.ContainsKey(c))
                    return null;

                int convertedIndex = CharToIndex[c];

                if (_children[convertedIndex] == null)
                    return default;

                return _children[convertedIndex].GetEndingNodeOfString(value, index + 1);
            }
        }
    }
}
