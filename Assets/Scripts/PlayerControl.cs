using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class PlayerControl : MonoBehaviour {
	private List<GameObject> actionListeners;
	public int playerId = 1;
	private PlayerRaw playerRaw;
	float gachette = 0f;

	// Use this for initialization
	void Start () {
		actionListeners = new List<GameObject>();
		playerRaw = new PlayerRaw();
	}
	
	// Update is called once per frame
	void Update () {
		float maxTriggerValue = Mathf.Max( Input.GetAxisRaw("TriggersL_" + playerId), Input.GetAxisRaw("TriggersR_" + playerId));
		gachette = Mathf.Max ( maxTriggerValue, Math.Abs(Input.GetAxisRaw("TriggersLR_" + playerId)));
	}
	
	public float GetGachette () {
		return gachette;
	}
	
	public int addActionListener (GameObject listener)
	{
		if(!actionListeners.Contains(listener) ) {
			actionListeners.Add (listener);
			listener.SendMessage("SetPlayerRaw", playerRaw);
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
	
	public class FlowIntensity : PlayerAction {
		float intensity = 0;
		
		public FlowIntensity (float intensity) {
			this.intensity = intensity;
		}
	}
	
	public class PlayerRaw {
		public float flowIntensity = 0;
		/*float FlowIntensity {
			get { return flowIntensity; }
			set { flowIntensity = value; }
		}*/
		
		public PlayerRaw () {
			this.flowIntensity = 0;
		}
	}
}
