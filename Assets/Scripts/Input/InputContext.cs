using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using XInputDotNetPure;
using System;


namespace InputHandling
{
    public abstract class InputContext
    {
        protected Dictionary<InputConstants.Buttons, ActionsConstants.Actions> _mappedButtons;
        protected Dictionary<InputConstants.Axis, ActionsConstants.Ranges> _mappedAxis;

        public InputContext()
        {
            _mappedButtons = new Dictionary<InputConstants.Buttons, ActionsConstants.Actions>();
            _mappedAxis = new Dictionary<InputConstants.Axis, ActionsConstants.Ranges>();
        }

        public ActionsConstants.Actions GetActionForButton(InputConstants.Buttons button)
        {
            return _mappedButtons.ContainsKey(button) ? _mappedButtons[button] : ActionsConstants.Actions.None;
        }
        public ActionsConstants.Ranges GetActionForAxis(InputConstants.Axis axis)
        {
            return _mappedAxis.ContainsKey(axis) ? _mappedAxis[axis] : ActionsConstants.Ranges.None;
        }
    }
}