using UnityEngine;
using System.Collections;

public class Level : MonoBehaviour
{
    public string SceneName;

    public void Load()
    {
        Application.LoadLevel(SceneName);
    }
}
