using System;
using System.Collections.Generic;

namespace poetools.Console
{
    public interface IInputHistory
    {
        void AddEntry(string input);
        void Clear();
        bool TryMoveBackwards(out string previous);
        bool TryMoveForwards(out string next);
    }

    public class InputHistory : IInputHistory
    {
        private readonly LinkedList<string> _commandHistory;
        private readonly int _maxHistoryLength;
        private bool _isCleared = true;
        private LinkedListNode<string> _currentHistoryNode;

        public InputHistory(int maxHistoryLength)
        {
            _maxHistoryLength = maxHistoryLength;
            _commandHistory = new LinkedList<string>();
            _currentHistoryNode = new LinkedListNode<string>("");
        }

        // === Public API ===
        public void AddEntry(string input)
        {
            // We don't really care about saving duplicates.
            if (_currentHistoryNode != null && input.Equals(_currentHistoryNode.Value, StringComparison.InvariantCulture))
                return;

            _commandHistory.AddLast(input);
            _currentHistoryNode = _commandHistory.Last;

            while (_commandHistory.Count > _maxHistoryLength)
                _commandHistory.RemoveFirst();
        }

        public bool TryMoveBackwards(out string result)
        {
            if (_isCleared)
            {
                _isCleared = false;
                result = _currentHistoryNode.Value;
                return true;
            }

            return TryMoveTo(_currentHistoryNode.Previous, out result);
        }

        public bool TryMoveForwards(out string result)
        {
            bool success = TryMoveTo(_currentHistoryNode.Next, out result);

            if (!success && !_isCleared)
            {
                result = string.Empty;
                Clear();
                return true;
            }

            return success;
        }

        public void Clear()
        {
            _isCleared = true;
            _currentHistoryNode = _commandHistory.Last;
        }

        // === Helpers ===
        private bool TryMoveTo(LinkedListNode<string> destination, out string result)
        {
            result = _currentHistoryNode.Value;

            if (destination == null)
                return false;

            result = destination.Value;
            _currentHistoryNode = destination;
            return true;
        }
    }
}
