using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace InputHandling
{
    // Specific to the game
    public class MappedInput : MonoBehaviour
    {
        // TODO: Instead of callback maps, do maps of valid actions

        private Dictionary<InputConstants.Buttons, Action> _actionsMap;
        private Dictionary<InputConstants.Axis, Action> _rangesMap;

        public MappedInput()
        {
            _actionsMap = new Dictionary<InputConstants.Buttons, Action>();
            _rangesMap = new Dictionary<InputConstants.Axis, Action>();
        }

        public void SetButtonForAction(InputConstants.Buttons button, Action action)
        {
            _actionsMap[button] = action;
        }

        public void SetAxisForAction(InputConstants.Axis axis, Action action)
        {
            _rangesMap[axis] = action;
        }
    }
}