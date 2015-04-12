using UnityEngine;
using System.Collections;

public class Goal : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKey(KeyCode.Return))
        {
            GameManager.Instance.LoadNextLevel();
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.layer == 10)
        {
            Debug.Log("WIN!!!");
            StartCoroutine(LoadNextLevel());
        }
    }

    private IEnumerator LoadNextLevel()
    {
        MusicManager.Instance.PlayGoalLevel();

        yield return new WaitForSeconds(2.5f);

        GameManager.Instance.LoadNextLevel();
    }
}
