using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using XInputDotNetPure;
using System;


namespace InputHandling
{
    public abstract class InputContext
    {
        protected MappedInput _mappedInput;
        private Dictionary<ActionsConstants.Actions, Action<MappedInput>> _mappedActions;

        public Dictionary<ActionsConstants.Actions, Action<MappedInput>> MappedActions
        {
            get { return _mappedActions; }
        }

        public MappedInput MappedInput
        {
            get { return _mappedInput; }
        }


        public InputContext()
        {
            _mappedActions = new Dictionary<ActionsConstants.Actions, Action<InputHandling.MappedInput>>();
            _mappedInput = new MappedInput();
        }

        public abstract void MapButton(InputConstants.Buttons button, bool pressed);
        public abstract void MapAxis(InputConstants.Axis button, float value);
    }
}