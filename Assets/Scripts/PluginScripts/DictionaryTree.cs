using System;
using System.Collections.Generic;
using UnityEngine;

namespace poetools.Console
{
    /// <summary>
    /// The DictionaryTree is a method of searching for words using a given prefix. This class is responsible
    /// for inserting words into the tree, finding words in the tree, and removing them from the tree. This class
    /// is primarily used for the Auto-Completer, as a method of finding every word that begins with the prefix
    /// given by the user (For example, if the user types "an", it'll find all words in the tree that begin with "an" -
    /// animal, ant, ancestor, etc) 
    /// </summary>
    public class DictionaryTree
    {
        
        // NOTE: Both Dictionaries are static, which mean they are the same amongst all Dictionary Tree instances
        
        /// <summary> <b>Description</b>: Dictionary that uses a character (char) to find an index (int)
        /// <para> Used to convert an alphanumeric character into its corresponding index </para>
        /// </summary>
        private static Dictionary<char, int> CharToIndex;
        /// <summary> <b>Description</b>: Dictionary that uses an index (int) to find a character (char)
        /// <para> Used to convert an index into its corresponding alphanumeric character </para>
        /// </summary>
        private static Dictionary<int, char> IndexToChar;
        
        // Default WordBuffer, used as a placeholder array to fill with characters when forming words
        private static readonly char[] WordBuffer = new char[100];

        private DictionaryTreeNode _rootNode; // The root node of the DictionaryTree 

        /// <summary>
        ///   <b>Description</b>: This method regenerates the Dictionaries
        /// </summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)] // NOTE: Come back to this
        private static void ResetStatics()
        {
            CharToIndex = new Dictionary<char, int>();
            IndexToChar = new Dictionary<int, char>();
            GenerateConversionTable();
        }

        /// <summary>
        ///   <b>Description</b>: Constructor for the DictionaryTree, creates the initial rootNode
        /// </summary>
        public DictionaryTree()
        {
            _rootNode = DictionaryTreeNode.Create();
        }
        
        /// <summary>
        ///   <b>Description</b>:  Creates the CharToIndex and IndexToChar Conversion Tables
        /// (Fills the entries of both)
        /// </summary>
        private static void GenerateConversionTable()
        {
            // Adds all uppercase letters.
            for (int i = 65; i <= 90; i++)
            {
                // Converts "i" into its "char" alphanumeric counterpart, and then adds it to CharToIndex
                CharToIndex.Add((char) i, i - 65);
                // Converse of the CharToIndex list
                IndexToChar.Add(i - 65, (char) i);
            }

            // Adds all lowercase letters.
            for (int i = 97; i <= 122; i++)
            {
                // Converts "i" into its "char" alphanumeric counterpart, and then adds it to CharToIndex
                CharToIndex.Add((char) i, i - 57);
                // Converse of the CharToIndex list
                IndexToChar.Add(i - 57, (char) i);
            }
            
            // Adds non-number & non-letter characters

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

        /// <summary>
        ///   <b>Description</b>: Inserts the word "path" into the Dictionary Tree, starting at the root node
        /// </summary>
        /// <param name="word"> The string to create a DictionaryTree "path" or "branch" for </param>
        public void Insert(string word)
        {
            // Begins the recursive Insert() function, beginning at the root node & going until the word path is complete
            _rootNode.Insert(word.ToCharArray(), 0); 
        }

        /// <summary>
        ///   <b>Description</b>: Finds all words that have a given prefix 
        /// </summary>
        /// <param name="prefix"> The prefix used to search for words </param>
        /// <param name="results"> Where the found words are outputted to </param>
        public void GetWithPrefix(string prefix, List<string> results)
        {
            var charArray = prefix.ToCharArray(); // Turns the given prefix into a char Array
            // Finds the node for the last character of the prefix (Ex. for prefix "ban-", it would find "n")
            var endingNode = _rootNode.GetEndingNodeOfString(charArray, 0); 
            /* If the ending node is not null, find all words that start from this position
             (Basically, find any words with this given prefix) */
            endingNode?.GetAllWords(results, WordBuffer, 0);
        }

        /// <summary>
        ///   <b>Description</b>: Remove a word from the Dictionary Tree
        /// </summary>
        /// <param name="word"> The word you want removed from the tree </param>
        public void Remove(string word)
        {
            // Find the ending node of the word
            var endingNode = _rootNode.GetEndingNodeOfString(word.ToCharArray(), 0);

            // If this endingNode is not null
            if (endingNode != null)
                // Make it so this ending node is no longer the last node
                endingNode.IsEnd = false; // todo: this is a bad way to remove words? - Daniel
                                          // todo: I mean, probably kinda yeah - Justin
            
        }

        /// <summary>
        ///   <b>Description</b>: A single node of a Dictionary Tree
        /// <list type="table">
        ///       <item>
        ///           <description><b>children[]</b> - An array of the node's children </description>
        ///       </item>
        ///       <item>
        ///           <description><b>IsEnd</b> - ___ </description>
        ///       </item>
        ///   </list>
        /// </summary>
        private class DictionaryTreeNode
        {
            private DictionaryTreeNode[] _children; // Children nodes of this node 
            public bool IsEnd; // Whether this node is an end node

            /// <summary> <b>Description</b>: Creates a new DictionaryTreeNode, along with its "children" DictionaryTreeNode </summary>
            public static DictionaryTreeNode Create()
            {
                return new DictionaryTreeNode // Return a new DictionaryTreeNode object
                {
                    /* Set the array of children to a new array the size of the CharToIndex Dictionary 
                    (Enough array spaces to hold every single alphanumeric character) */
                    _children = new DictionaryTreeNode[CharToIndex.Count], 
                };
            }

            /// <summary>
            ///   <b>Description</b>: This is a recursive function that, starting at the rootnode, creates a "word path" by
            /// converting each character of the word to its corresponding index, and then by either selecting or creating a
            /// new DictionaryTreeNode at that index. This node and its index represent an alphanumeric value. The function then
            /// recurses through this node.
            /// </summary>
            /// <param name="w"> A character array that represents the word inserted into the DictionaryTree </param>
            /// <param name="index"> Represents which character of the word the recursive function is working with </param>
            public void Insert(char[] w, int index)
            {
                if (index >= w.Length) // If the index is greater than / equal to the word
                {
                    IsEnd = true; // Indicate that this node is an end node
                    return;
                }

                char c = w[index]; // "c" is set to the character at the index

                if (!CharToIndex.ContainsKey(c)) // If the CharToIndex Dictionary doesn't contain the [char] key "c"
                    // Throws an exception 
                    throw new ArgumentException($"Only alphanumeric commands can be added! (\"{c}\" attempted)");

                int convertedIndex = CharToIndex[c]; // Finds the index based on the given [char] key
                DictionaryTreeNode next = _children[convertedIndex]; // Uses the index to find the corresponding child node

                if (next == null) // If no corresponding child node was found
                {
                    next = Create(); // Create the node
                    _children[convertedIndex] = next; // Set the entry at the convertedIndex to this new node
                }

                next.Insert(w, index + 1); // Recall the function at the next index with the same "w" array
            }
            
            /// <summary>
            ///   <b>Description</b>: This function finds all the words from a starting node by recursing through the
            /// Dictionary Tree. Once the end of a "word path" is found, the word is put into an "output" list. The starting
            /// node dictates the beginning of the word (For example, starting at node path "a" -> "n" will return all words
            /// starting with "an")
            /// </summary>
            /// <param name="output"> List where found words are outputted to </param>
            /// <param name="wordBuffer"> Char array that holds every character of the found word </param>
            /// <param name="depth"> Length of the found word </param>
            public void GetAllWords(List<string> output, char[] wordBuffer, int depth)
            {
                if (IsEnd) // If this node is an End node
                    output.Add(new string(wordBuffer, 0, depth)); // Add the word into output

                for (int i = 0; i < _children.Length; i++) // For the amount of children in the node
                {
                    if (_children[i] != null) // If the child at the index is not null
                    {
                        char cur = IndexToChar[i]; // Get the character corresponding to the index
                        wordBuffer[depth] = cur; // Set the character at index "depth" to cur
                        _children[i].GetAllWords(output, wordBuffer, depth + 1); // Recall the function w/ the new node
                    }
                }
            }

            /// <summary>
            ///   <b>Description</b>: This function finds and returns the ending node of a given word by recursing through
            /// the word's character nodes for the amount of times as the word's length, and then returning the last found node.
            /// </summary>
            /// <param name="value"> Character array holding the word's characters </param>
            /// <param name="index"> Index to keep track of the recursive function's position in the word </param>
            public DictionaryTreeNode GetEndingNodeOfString(char[] value, int index)
            {
                if (index >= value.Length) // If the index is greater than / equal to the word length, return this node
                    return this;

                char c = value[index]; // Set "c" to the character at the index

                if (!CharToIndex.ContainsKey(c)) // If the CharToIndex array does not contain "c", return
                    return null;

                int convertedIndex = CharToIndex[c]; // Find "c"s corresponding index

                if (_children[convertedIndex] == null) // If the child at this index is null, 
                    return default; // Return the default instance of a DictionaryTreeNode
                // Recurse the function through this next node
                return _children[convertedIndex].GetEndingNodeOfString(value, index + 1); 
            }
        }
    }
}
