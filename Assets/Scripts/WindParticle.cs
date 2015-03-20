using UnityEngine;
using System.Collections;

public class WindParticle : MonoBehaviour {

    void OnParticleCollision(GameObject Other)
    {
        Debug.Log("COLLISION");
    }
}
