using UnityEngine;
using System.Collections;

public class Goal : MonoBehaviour
{
    public string NextSceneName;

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.layer == 10)
        {
            Debug.Log("WIN!!!");
            Application.LoadLevel(NextSceneName);
        }
    }
}
