using UnityEngine;
using System.Collections;

namespace InputHandling
{
    // Specific to the game
    public class GameplayContext : InputContext
    {
        // TODO: Pull that data from a config or XML file for easy configuration

        public override void MapButton(InputConstants.Buttons button, bool pressed)
        {
            // TODO: Pass the MappedInput to all registered callbacks
        }

        public override void MapAxis(InputConstants.Axis axis, float value)
        {
            // TODO: Pass the MappedInput to all registered callbacks
        }
    }
}
