using System;
using System.Collections.Generic;

namespace poetools.Console
{
    /// <summary>
    /// Interface for the <see cref="InputHistory">Input History</see> Class
    /// </summary>
    public interface IInputHistory
    {
        /// <summary> <b>Description</b>: Adds a new user-input entry into the Input History </summary>
        /// <param name="input"> The user-input to be entered into the input history list </param>
        void AddEntry(string input);
        /// <summary> <b>Description</b>: Clears the user's "traversal" of the command history </summary>
        void Clear();
        /// <summary> <b>Description</b>: Attempts to move backwards in the command history list </summary>
        /// <param name="previous"> The string held in the "current" node that results after this function executes </param>
        bool TryMoveBackwards(out string previous);
        /// <summary> <b>Description</b>: Attempts to move forwards in the command history list </summary>
        /// <param name="next"> The string held in the "current" node that results after this function executes </param>
        bool TryMoveForwards(out string next);
    }
    
    /// <summary>
    /// This class is responsible for holding the history of user-input into the console. 
    /// </summary>
    public class InputHistory : IInputHistory
    {
        /// <summary> <b>Description</b>: Linked LIst that holds the console's history of user-inputted commands </summary>
        private readonly LinkedList<string> _commandHistory;
        /// <summary> <b>Description</b>: The maximum amount of commands that can be saved in the input history </summary>
        private readonly int _maxHistoryLength;
        /// <summary> <b>Description</b>: Whether the command history traversal has been reset / cleared </summary>
        private bool _isCleared = true;
        /// <summary> <b>Description</b>: The currently selected node in the command history linked list </summary>
        private LinkedListNode<string> _currentHistoryNode;

        /// <summary> <b>Description</b>: Constructor for the InputHistory class. </summary>
        /// <param name="maxHistoryLength"> Maximum amount of commands that can be saved to the command history </param>
        public InputHistory(int maxHistoryLength)
        {
            _maxHistoryLength = maxHistoryLength;
            _commandHistory = new LinkedList<string>(); // Sets the command history to a new linked list
            _currentHistoryNode = new LinkedListNode<string>(""); // Sets the current node to a new linked list node
        }

        // === Public API ===
        /// <summary> <b>Description</b>: Adds a new user-input entry into the Input History </summary>
        /// <param name="input"> The user-input to be entered into the input history list </param>
        public void AddEntry(string input)
        {
            // We don't really care about saving duplicates.
            if (_currentHistoryNode != null && input.Equals(_currentHistoryNode.Value, StringComparison.InvariantCulture))
                return;

            // Add the input to the end of the _commandHistory linked list
            _commandHistory.AddLast(input);
            
            // Set the current history node to this new end node of command history
            _currentHistoryNode = _commandHistory.Last;

            // Once the maxHistoryLength has been surpassed, remove the oldest node from the list
            while (_commandHistory.Count > _maxHistoryLength)
                _commandHistory.RemoveFirst();
        }

        /// <summary> <b>Description</b>: Attempts to move backwards in the command history list </summary>
        /// <param name="result"> The string held in the "current" node that results after this function executes </param>
        public bool TryMoveBackwards(out string result)
        {
            if (_isCleared) // If the command history "traversal" has been cleared 
            {
                _isCleared = false; // Set it to not-cleared
                result = _currentHistoryNode.Value; // Set the outputted result to the current / most recent node
                return true; // Return true (successful)
            }

            // Attempts moving to the previous node, and sets "result" to the output of this function
            return TryMoveTo(_currentHistoryNode.Previous, out result); 
        }

        /// <summary> <b>Description</b>: Attempts to move forwards in the command history list </summary>
        /// <param name="result"> The string held in the "current" node that results after this function executes </param>
        public bool TryMoveForwards(out string result)
        {
            /* Attempts to move to the next node, sets "result" to the output of this function, and sets "success"
             to true if the function's execution was successful */
            bool success = TryMoveTo(_currentHistoryNode.Next, out result);

            // If the execution wasn't successful and the list wasn't cleared 
            if (!success && !_isCleared)
            {
                // Set the outputted result to an empty string
                result = string.Empty;
                // Clear the list (traversal)
                Clear();
                // Return true (Successful)
                return true;
            }

            return success;
        }

        /// <summary> <b>Description</b>: Clears the user's "traversal" of the command history </summary>
        public void Clear()
        {
            _isCleared = true; // Command history has been cleared
            _currentHistoryNode = _commandHistory.Last; // Set the current node to the most recent node
        }

        // === Helpers ===
        /// <summary> <b>Description</b>: Attempts moving to a specific node in the command history linked list </summary>
        /// <param name="destination"> The node to move to </param>
        /// <param name="result"> The node to return as the "current" node </param>
        private bool TryMoveTo(LinkedListNode<string> destination, out string result)
        {
            result = _currentHistoryNode.Value; // Sets the result to the current node, as a default output

            if (destination == null) // If the destination is null
                return false; // Return false (unsucessful)

            result = destination.Value; // Set the outputted result to the value of the "destination node"
            _currentHistoryNode = destination; // Set the current node to the destination node
            return true; // Return true (successful)
        }
    }
}
