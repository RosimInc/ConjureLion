using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour
{
    private int _ballNumber = 0;

    private float _amplitude = 0.25f;

	void Update ()
    {
        if (_ballNumber >= 2)
        {
            StartCoroutine(LoadNextLevel());
            _ballNumber = -1;
        }
	}

    private IEnumerator LoadNextLevel()
    {
        MusicManager.Instance.PlayGoalStart();

        yield return new WaitForSeconds(2.5f);

        GameManager.Instance.LoadNextLevel();
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
