using UnityEngine;
using System.Collections;

public class HingePlatform : MonoBehaviour {

	public GameObject platform;

	// Use this for initialization
	void Start () {
		//Initialize the main hinge
		HingeJoint2D joint = platform.GetComponent<HingeJoint2D>();
		joint.connectedAnchor = new Vector2(
			transform.position.x, transform.position.y);

		//ResourceManager.Instance.HingePlatforms.Add(this.gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
