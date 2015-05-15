using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace InputHandling
{
    // Specific to the game
    public class MappedInput
    {
        public Dictionary<ActionsConstants.Actions, bool> Actions = new Dictionary<ActionsConstants.Actions, bool>();
        public Dictionary<ActionsConstants.Ranges, float> Ranges = new Dictionary<ActionsConstants.Ranges, float>();

        private int _playerIndex;

        public int PlayerIndex
        {
            get { return _playerIndex; }
        }
        
        public MappedInput(int playerIndex)
        {
            _playerIndex = playerIndex;
        }

        public void Clear()
        {
            Actions.Clear();
            Ranges.Clear();
        }
    }
}