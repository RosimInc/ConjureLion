using UnityEngine;
using System.Collections;

namespace InputHandling
{
    // Specific to the game
    public class MainMenuContext : InputContext
    {
        // TODO: Pull that data from a config or XML file for easy configuration and load the file in InputContext instead of having inheritance

        public MainMenuContext() : base()
        {
            _mappedButtons.Add(InputConstants.Buttons.A, ActionsConstants.Actions.AcceptMenuOption);
            _mappedButtons.Add(InputConstants.Buttons.B, ActionsConstants.Actions.GoToPreviousMenu);
            _mappedButtons.Add(InputConstants.Buttons.DPadUp, ActionsConstants.Actions.SelectPreviousMenuOption);
            _mappedButtons.Add(InputConstants.Buttons.DPadDown, ActionsConstants.Actions.SelectNextMenuOption);

            _mappedAxis.Add(InputConstants.Axis.LeftStickY, ActionsConstants.Ranges.ChangeMenuOption);
        }
    }
}


