using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerTouchPipe : MonoBehaviour {

	private List<GameObject> touchedList;

	
	
	private void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	private void OnTriggerEnter2D (Collider2D coll)
	{
		touchedList.Add(coll.gameObject);
		
		Debug.Log("Embout is in "+coll.gameObject.tag);
	}
	
	private void OnTriggerStay2D (Collider2D coll)
	{
		Debug.Log("Embout is in "+coll.gameObject.tag);
		if(coll.gameObject.tag != "Ball")
			return;
	}
	
	private void OnTriggerExit2D (Collider2D coll)
	{
		if(coll.gameObject.tag != "Ball")
			return;
		touchedList.Remove(coll.gameObject);
	}
}
