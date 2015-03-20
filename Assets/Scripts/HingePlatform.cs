using UnityEngine;
using System.Collections;

public class HingePlatform : MonoBehaviour {

	public GameObject platform;

	// Use this for initialization
	void Start () {
		//Initialize the main hinge
		HingeJoint2D joint = GetComponent<HingeJoint2D>();
		joint.connectedAnchor = new Vector2(
			transform.position.x, transform.position.y);

		//Populate the reactive hinges

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
