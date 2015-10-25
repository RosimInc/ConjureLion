using UnityEngine;
using System.Collections;

namespace InputHandling
{
    // Specific to the game
    public class PauseMenuContext : InputContext
    {
        // TODO: Pull that data from a config or XML file for easy configuration and load the file in InputContext instead of having inheritance

        public PauseMenuContext() : base()
        {
            _mappedButtons.Add(RawInputConstants.Buttons.Button1, InputConstants.PAUSE);
            _mappedButtons.Add(RawInputConstants.Buttons.Button2, InputConstants.ACCEPT_MENU_OPTION);
            _mappedButtons.Add(RawInputConstants.Buttons.Button3, InputConstants.BACK_MENU_OPTION);
            

            _mappedAxis.Add(RawInputConstants.Axis.Axis2, InputConstants.CHANGE_MENU_OPTION_VERTICAL);
        }
    }
}
