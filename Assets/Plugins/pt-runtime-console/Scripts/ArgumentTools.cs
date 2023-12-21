using System.Collections.Generic;

namespace poetools.Console
{
    /// <summary>
    /// Utilities for working with arrays of strings, particularly argument lists.
    /// </summary>
    public static class ArgumentTools
    {
        /// <summary>
        /// Combines a list of arguments into one longer string.
        /// </summary>
        /// <param name="args">The arguments to combine.</param>
        /// <returns>The combined string of arguments.</returns>
        public static string Combine(IEnumerable<string> args)
        {
            string result = "";

            foreach (string arg in args)
                result += arg + ' ';

            result = result.TrimEnd(' ');
            return result;
        }

        /// <summary>
        /// Combines a list of arguments into one longer string.
        /// </summary>
        /// <param name="args">The arguments to combine.</param>
        /// <param name="start">The first argument index to combine from.</param>
        /// <returns>The combined string of arguments.</returns>
        public static string Combine(IList<string> args, int start)
        {
            return Combine(args, start, args.Count);
        }

        /// <summary>
        /// Combines a list of arguments into one longer string.
        /// </summary>
        /// <param name="args">The arguments to combine.</param>
        /// <param name="start">The first argument index to combine from.</param>
        /// <param name="end">The final argument index to combine to.</param>
        /// <returns>The combined string of arguments.</returns>
        public static string Combine(IList<string> args, int start, int end)
        {
            string result = "";

            for (int i = start; i < end; i++)
                result += args[i] + ' ';

            result = result.TrimEnd(' ');
            return result;
        }

        /// <summary>
        /// Extracts all of the arguments from a command, which would normally
        /// start with the command name.
        /// </summary>
        /// <param name="input">The raw user input.</param>
        /// <returns>The arguments the user passed to the command.</returns>
        public static string[] Parse(string[] input)
        {
            string[] args = new string[input.Length - 1];

            for (int i = 0; i < args.Length; i++)
                args[i] = input[i + 1];

            return args;
        }
    }
}
