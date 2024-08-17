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
            string result = ""; // Empty placeholder for the result

            foreach (string arg in args) // For every argument string
                result += arg + ' '; // Add it to result, w/ a space between each one

            result = result.TrimEnd(' '); // Removes the last unnecessary space from the result
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
            // Calls Combine, w/ the "end" index automatically being set to the end of the list
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
            string result = ""; // Empty placeholder for the result

            for (int i = start; i < end; i++) // For each index from "start" to "end"
                result += args[i] + ' '; // Add the corresponding argument to the result, w/ a space in between each

            result = result.TrimEnd(' '); // Removes the last unnecessary space from the result
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
            // Creates an string array, 1 slot smaller than the inputted array
            string[] args = new string[input.Length - 1]; 

            for (int i = 0; i < args.Length; i++) // For every index in args
                args[i] = input[i + 1]; // Set its entry to the corresponding "one-up" entry in input

            return args;
        }
    }
}
