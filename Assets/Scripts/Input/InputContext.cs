using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace InputHandling
{
    public abstract class InputContext
    {
        protected Dictionary<RawInputConstants.Buttons, string> _mappedButtons;
        protected Dictionary<RawInputConstants.Buttons, string> _mappedStates;
        protected Dictionary<RawInputConstants.Axis, string> _mappedAxis;

        public InputContext()
        {
            _mappedButtons = new Dictionary<RawInputConstants.Buttons, string>();
            _mappedStates = new Dictionary<RawInputConstants.Buttons, string>();
            _mappedAxis = new Dictionary<RawInputConstants.Axis, string>();
        }

        public string GetActionForButton(RawInputConstants.Buttons button)
        {
            return _mappedButtons.ContainsKey(button) ? _mappedButtons[button] : null;
        }

        public string GetStateForButton(RawInputConstants.Buttons button)
        {
            return _mappedStates.ContainsKey(button) ? _mappedStates[button] : null;
        }

        public string GetRangeForAxis(RawInputConstants.Axis axis)
        {
            return _mappedAxis.ContainsKey(axis) ? _mappedAxis[axis] : null;
        }
    }
}