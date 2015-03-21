using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerControl : MonoBehaviour {
	private List<GameObject> actionListeners;
	public int playerId = 1;

	// Use this for initialization
	void Start () {
		actionListeners = new List<GameObject>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	public int addActionListener (GameObject listener)
	{
		if(!actionListeners.Contains(listener) ) {
			actionListeners.Add (listener);
		}
		return playerId;
	}
	
	public int removeActionListener (GameObject listener)
	{
		actionListeners.Remove (listener);
		return playerId;
	}
	
	void throwAction (PlayerAction action)
	{		
		//action.throwAction();
		foreach (GameObject listener in actionListeners)
		{
			listener.SendMessage("receiveAction", action);
		}
		
		Debug.Log ("Action A of " + playerId);
	}
	
	public abstract class PlayerAction {
		//public abstract void throwAction ();
	}
	
	public class ActionGachette : PlayerAction {
		float intensity = 0;
	}
}
