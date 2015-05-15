﻿using UnityEngine;
using System.Collections;

namespace InputHandling
{
    // Specific to the game
    public class GameplayContext : InputContext
    {
        // TODO: Pull that data from a config or XML file for easy configuration and load the file in InputContext instead of having inheritance
        // TODO: Maybe divide ranges between Triggers (1 float value) and sticks (2 float values) ?

        public GameplayContext() : base()
        {
            _mappedButtons.Add(InputConstants.Buttons.Start, ActionsConstants.Actions.OpenPauseMenu);

            _mappedAxis.Add(InputConstants.Axis.LeftStickX, ActionsConstants.Ranges.MoveX);
            _mappedAxis.Add(InputConstants.Axis.LeftStickY, ActionsConstants.Ranges.MoveY);
            _mappedAxis.Add(InputConstants.Axis.RightStickX, ActionsConstants.Ranges.RotateX);
            _mappedAxis.Add(InputConstants.Axis.RightStickY, ActionsConstants.Ranges.RotateY);
            _mappedAxis.Add(InputConstants.Axis.TriggerLeft, ActionsConstants.Ranges.Breathe);
            _mappedAxis.Add(InputConstants.Axis.TriggerRight, ActionsConstants.Ranges.Breathe);
        }
    }
}
