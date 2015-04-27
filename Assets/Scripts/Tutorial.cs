using UnityEngine;
using System.Collections;

public class Tutorial : MonoBehaviour
{
    private int _ballNumber = 0;

    private float _amplitude = 0.25f;

	void Update ()
    {
        if (_ballNumber >= 2)
        {
            MusicManager.Instance.PlayGoalStart();

            GameManager.Instance.LoadNextLevel();

            _ballNumber = -1;
        }
	}

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (_ballNumber == 0)
        {
            MusicManager.Instance.PlayGoalLevel();
        }

        coll.collider.enabled = false;
        coll.rigidbody.isKinematic = true;
        _ballNumber++;
    }
}
