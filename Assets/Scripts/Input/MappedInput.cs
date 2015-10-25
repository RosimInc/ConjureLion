using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace InputHandling
{
    // Specific to the game
    public class MappedInput
    {
        // We use hashets for the actions and the states because we just need to check if they are in the collection, and not retrieve them
        public HashSet<string> Actions = new HashSet<string>();
        public HashSet<string> States = new HashSet<string>();
        public Dictionary<string, float> Ranges = new Dictionary<string, float>();

        public void Clear()
        {
            Actions.Clear();
            States.Clear();
            Ranges.Clear();
        }
    }
}