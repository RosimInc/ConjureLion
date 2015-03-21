using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour
{
    public BallEffect Owner;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log("def");
        if (Owner != null && other.gameObject.layer == 8)
        {
            Debug.Log("ghi");
            Owner.DropBall();
        }
    }
}
