using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

/*
    Strongly inspired from Mike Lewis' excellent post about input handling
    http://www.gamedev.net/blog/355/entry-2250186-designing-a-robust-input-handling-system-for-games/
*/

namespace InputHandling
{
    public class InputMapper
    {
        // Right now, the only active context is the peek of the stack, but when we will need multiple contexts at once, this is going to be useful
        private Stack<InputContext> _activeContexts;

        private List<Action<MappedInput>> _callbacks;

        private MappedInput _currentFrameMappedInput;

        public InputMapper()
        {
            _callbacks = new List<Action<MappedInput>>();
            _activeContexts = new Stack<InputContext>();
            _currentFrameMappedInput = new MappedInput();
        }

        public void Dispatch()
        {
            foreach (Action<MappedInput> callback in _callbacks)
            {
                callback(_currentFrameMappedInput);
            }
        }

        public void PushContext(InputContext context)
        {
            if (_activeContexts.Count == 0 || _activeContexts.Peek().GetType() != context.GetType())
            {
                _activeContexts.Push(context);
            }
        }

        public void PopContext()
        {
            if (_activeContexts.Count != 0)
            {
                _activeContexts.Pop();
            }
        }

        public void ClearContexts()
        {
            _activeContexts.Clear();
        }

        public void AddCallback(Action<MappedInput> callback)
        {
            _callbacks.Add(callback);
        }

        public void SetRawButtonState(RawInputConstants.Buttons button, bool pressed, bool previouslyPressed)
        {
            string action = GetActionForButton(button);
            string state = GetStateForButton(button);

            if (pressed)
            {
                if (!previouslyPressed && action != null)
                {
                    _currentFrameMappedInput.Actions.Add(action);
                    return;
                }

                if (state != null)
                {
                    _currentFrameMappedInput.States.Add(state);
                    return;
                }
            }

            // Uncomment if we start to have problems
            //RemoveButtonFromLists(button);
        }

        public void SetRawAxisValue(RawInputConstants.Axis axis, float value)
        {
            // TODO: Have contexts for every single player?

            // TODO: Use the commented code below instead when we will want multiple contexts to be available at the same time (maybe for when the player holds a weapon?). We'll keep it simple for now.

            /*
            foreach (InputContext activeContext in _activeContexts)
            {
                InputConstants.Ranges range = activeContext.GetRangeForAxis(axis);

                if (range != InputConstants.Ranges.None)
                {
                    // We only want the first active "range behaviour" of the player to handle the ranges values, since we don't want multiple actions to react to it
                    _mappedInputs[playerIndex].Ranges[range] = value;
                    break;
                }
            }*/

            string range = null;

            if (_activeContexts.Count != 0)
            {
                range = _activeContexts.Peek().GetRangeForAxis(axis);
            }

            if (range != null)
            {
                _currentFrameMappedInput.Ranges[range] = value;
            }
        }

        public void ResetInputs()
        {
            _currentFrameMappedInput.Clear();
        }

        #region Helper methods

        private string GetActionForButton(RawInputConstants.Buttons button)
        {
            // TODO: Have contexts for every single player?

            // TODO: Use the commented code below instead when we will want multiple contexts to be available at the same time (maybe for when the player holds a weapon?). We'll keep it simple for now.

            /*
            foreach (InputContext activeContext in _activeContexts)
            {
                InputConstants.Actions action = activeContext.GetActionForButton(button);

                if (action != InputConstants.Actions.None)
                {
                    return action;
                }
            }*/

            string action = null;

            if (_activeContexts.Count != 0)
            {
                action = _activeContexts.Peek().GetActionForButton(button);
            }

            return action;
        }

        private string GetStateForButton(RawInputConstants.Buttons button)
        {
            // TODO: Have contexts for every single player?

            // TODO: Use the commented code below instead when we will want multiple contexts to be available at the same time (maybe for when the player holds a weapon?). We'll keep it simple for now.

            /*
            foreach (InputContext activeContext in _activeContexts)
            {
                InputConstants.States state = activeContext.GetStateForButton(button);

                if (state != InputConstants.States.None)
                {
                    return state;
                }
            }*/

            string state = null;

            if (_activeContexts.Count != 0)
            {
                state = _activeContexts.Peek().GetStateForButton(button);
            }

            return state;
        }

        private void RemoveButtonFromLists(RawInputConstants.Buttons button)
        {
            string action = GetActionForButton(button);
            string state = GetStateForButton(button);

            if (action != null)
            {
                _currentFrameMappedInput.Actions.Remove(action);
            }

            if (state != null)
            {
                _currentFrameMappedInput.States.Remove(state);
            }
        }

        #endregion
    }
}
