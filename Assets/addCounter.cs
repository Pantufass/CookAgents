using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class addCounter : MonoBehaviour
{
    void Start()
    {
        foreach (Transform t in transform)
        {
            t.gameObject.AddComponent<BoxCollider2D>();
            t.gameObject.AddComponent<Counter>();
        }
    }
}
