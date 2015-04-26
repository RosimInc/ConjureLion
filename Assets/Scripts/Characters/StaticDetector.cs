using UnityEngine;
using System.Collections;

public class StaticDetector : MonoBehaviour {

	public GameObject player;
	private BallEffect ballEffectScr;

	// Use this for initialization
	void Start () {
		ballEffectScr = player.GetComponent<BallEffect>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerStay2D(Collider2D collider)
	{
		ballEffectScr.OnStaticTriggerStay(collider);
	}
}
