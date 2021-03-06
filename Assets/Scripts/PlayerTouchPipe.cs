﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class PlayerTouchPipe : MonoBehaviour {

	private List<GameObject> touchedList;
	float gachette = 0f;
	public int playerNumber = 1;
	public bool attract = false;
	
	private void Start () {
		touchedList = new List<GameObject>();
	}
	
	// Update is called once per frame
	void Update () {
		float maxTriggerValue = Mathf.Max( Input.GetAxisRaw("TriggersL_" + playerNumber), Input.GetAxisRaw("TriggersR_" + playerNumber));
		gachette = Mathf.Max ( maxTriggerValue, Math.Abs(Input.GetAxisRaw("TriggersLR_" + playerNumber)));
		
		
		if(attract)
			gachette = - gachette;
			
		for(int i = 0; i< touchedList.Count; i++) {
			//Vérifier la direction avec RayCast
			
			//Envoyer 0 si pas dans le bon axe
			
			//Envoi de la valeur gachette
			touchedList[i].SendMessage("FlowIntensity", gachette);
			Debug.Log ("Flow "+gachette);
		}
	}
	
	private void OnTriggerEnter2D (Collider2D coll)
	{
		Debug.Log("Embout is in "+coll.gameObject.tag);
		
		if(coll.tag != "Toe")
			return;
			
		touchedList.Add(coll.gameObject);
		
		
	}
	
	private void OnTriggerStay2D (Collider2D coll)
	{
		Debug.Log("Embout is in "+coll.gameObject.tag);
		if(coll.gameObject.tag != "Ball")
			return;
	}
	
	private void OnTriggerExit2D (Collider2D coll)
	{
		if(coll.tag != "Toe")
			return;
		coll.gameObject.SendMessage("FlowIntensity", 0f);
		touchedList.Remove(coll.gameObject);
	}
}
