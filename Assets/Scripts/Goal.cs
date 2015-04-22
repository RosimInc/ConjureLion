using UnityEngine;
using System.Collections;

public class Goal : MonoBehaviour
{
    private bool _goalHasBeenTriggered = false;

    void Update()
    {
        if (Input.GetKey(KeyCode.Return))
        {
            GameManager.Instance.LoadNextLevel();
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.layer == 10 && !_goalHasBeenTriggered)
        {
            _goalHasBeenTriggered = true;

            MusicManager.Instance.PlayGoalLevel();
            GameManager.Instance.LoadNextLevel();
        }
    }
}
