using UnityEngine;
using System.Collections;
using System;

namespace InputHandling
{
    // Specific to the game
    public class MainMenuContext : InputContext
    {
        // TODO: Pull that data from a config or XML file for easy configuration

        public override void MapButton(InputConstants.Buttons button, bool pressed)
        {
            // TODO: Verify if the callbacks are null or not before calling them

            /*
            switch (button)
            {
                case InputMapper.Buttons.A:
                    _mappedInput.SetButtonForAction(button, OnAcceptChoice);
                    break;
                case InputMapper.Buttons.B:
                    _mappedInput.SetButtonForAction(button, OnGoToPreviousMenu);
                    break;
                case InputMapper.Buttons.DPadUp:
                    _mappedInput.SetButtonForAction(button, OnSelectPreviousChoice);
                    break;
                case InputMapper.Buttons.DPadDown:
                    _mappedInput.SetButtonForAction(button, OnSelectNextChoice);
                    break;
            }*/

            // TODO: Pass the MappedInput to all registered callbacks
        }

        public override void MapAxis(InputConstants.Axis axis, float value)
        {/*
            switch (axis)
            {
                case InputMapper.Axis.LeftStickY:
                    if (value > 0)
                    {
                        _mappedInput.SetAxisForAction(axis, OnSelectPreviousChoice);
                    }
                    else if (value < 0)
                    {
                        _mappedInput.SetAxisForAction(axis, OnSelectNextChoice);
                    }
                    break;
            }*/

            // TODO: Pass the MappedInput to all registered callbacks
        }
    }
}


