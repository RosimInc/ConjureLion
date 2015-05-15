using UnityEngine;
using System.Collections;

namespace InputHandling
{
    public class ActionsConstants
    {
        public enum Actions
        {
            None,
            AcceptMenuOption,
            GoToPreviousMenu,
            SelectPreviousMenuOption,
            SelectNextMenuOption,
            OpenPauseMenu,
            ClosePauseMenu,
            ChooseCharacter,
            StartPlaying
        }

        public enum Ranges
        {
            None,
            MoveX,
            MoveY,
            RotateX,
            RotateY,
            Breathe,
            ChangeMenuOption
        }
    }
}