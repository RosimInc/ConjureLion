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

        private List<Action<MappedInput>> _callbacks;
        private MappedInput[] _mappedInputs;

        public InputMapper(int maxPlayers)
        {
            _mappedInputs = new MappedInput[maxPlayers];
            _callbacks = new List<Action<MappedInput>>();

            for (int i = 0; i < maxPlayers; i++)
            {
                _mappedInputs[i] = new MappedInput(i);
            }
        }

        public void Dispatch()
        {
            foreach (Action<MappedInput> callback in _callbacks)
            {
                for (int i = 0; i < _mappedInputs.Length; i++)
                {
                    callback(_mappedInputs[i]);
                }
            }
        }

        // TODO: Maybe the player handling shouldn't be handled here, but at a higher level (like the contexts) ?
        public void MapButton(int playerIndex, InputConstants.Buttons button, bool pressed)
        {
            ActionsConstants.Actions action = _currentContext.GetActionForButton(button);
            _mappedInputs[playerIndex].Actions[action] = pressed;
        }

        public void MapAxis(int playerIndex, InputConstants.Axis axis, float value)
        {
            ActionsConstants.Ranges range = _currentContext.GetActionForAxis(axis);

            
            if (_mappedInputs[playerIndex].Ranges.ContainsKey(range))
            {
                if (Mathf.Abs(value) > Mathf.Abs(_mappedInputs[playerIndex].Ranges[range]))
                {
                    _mappedInputs[playerIndex].Ranges[range] = value;
                }
            }
            else
            {
                _mappedInputs[playerIndex].Ranges[range] = value;
            }

            //_mappedInputs[playerIndex].Ranges[range] = value;
        }

        public void AddCallback(Action<MappedInput> callback)
        {
            _callbacks.Add(callback);
        }

        public void SetContext(InputContext context)
        {
            // TODO: Don't clear the callbacks here since we don't know for sure that the context is created before

            _callbacks.Clear();

            _currentContext = context;
        }

        // TODO: TEMPORARY!!! Will remove when we add the "multiple inputs for one action" handling
        public void ResetInputs()
        {
            for (int i = 0; i < _mappedInputs.Length; i++)
            {
                _mappedInputs[i].Clear();
            }
        }
    }
}
