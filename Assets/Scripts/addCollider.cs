using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class addCollider : MonoBehaviour
{ 

    void Start()
    {
        foreach(Transform t in transform)
        {
            t.gameObject.AddComponent<BoxCollider2D>();
        }
    }
}
