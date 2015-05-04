using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace InputHandling
{
    public class InputMapper
    {
        // TODO: This class should be abstract and the inheriting children should handle which type of input is mapped (keyboard + mouse, controller, etc.)
        // TODO: Add a list of active contexts instead of only one (but one should work now for our needs)
        private InputContext _currentContext;

        public InputContext CurrentContext
        {
            get { return _currentContext; }
            set { _currentContext = value; }
        }

        // For other types of games, we might want to add a States enum

        private Dictionary<InputConstants.Buttons, bool>[] _buttonInputs;
        private Dictionary<InputConstants.Axis, float>[] _rangeInputs;

        public InputMapper(int maxPlayers)
        {
            _buttonInputs = new Dictionary<InputConstants.Buttons, bool>[maxPlayers];
            _rangeInputs = new Dictionary<InputConstants.Axis, float>[maxPlayers];

            for (int i = 0; i < maxPlayers; i++)
            {
                _buttonInputs[i] = new Dictionary<InputConstants.Buttons, bool>();
                _rangeInputs[i] = new Dictionary<InputConstants.Axis, float>();
            }
        }

        public void Dispatch()
        {
            // TODO: Verify which contexts are active and map the input accordingly

            for (int i = 0; i < _buttonInputs.Length; i++)
			{
                foreach (KeyValuePair<InputConstants.Buttons, bool> kvp in _buttonInputs[i])
                {
                    _currentContext.MapButton(kvp.Key, kvp.Value);
                }

                foreach (KeyValuePair<InputConstants.Axis, float> kvp in _rangeInputs[i])
                {
                    _currentContext.MapAxis(kvp.Key, kvp.Value);
                }
			}

            foreach (Action<MappedInput> action in _currentContext.Actions)
            {
                action(_currentContext.MappedInput);
            }
        }

        // TODO: Maybe the player handling shouldn't be handled here, but at a higher level (like the contexts) ?

        public void MapButton(int playerIndex, InputConstants.Buttons button, bool pressed)
        {
            _buttonInputs[playerIndex][button] = pressed;
        }

        public void MapAxis(int playerIndex, InputConstants.Axis axis, float value)
        {
            _rangeInputs[playerIndex][axis] = value;
        }
    }

}
