using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class addCounter : MonoBehaviour
{
    void Start()
    {
        foreach (Transform t in transform)
        {
            BoxCollider2D b=  t.gameObject.AddComponent<BoxCollider2D>();
            b.size = new Vector2(0.5f, 0.5f);
            //b.offset = new Vector2(0.25f, 0.25f);
            t.gameObject.AddComponent<Counter>();
            t.position = new Vector3(t.position.x,t.position.y,0);
        }
    }
}
