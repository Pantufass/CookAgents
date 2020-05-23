using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Counter : MonoBehaviour
{
    public Item onTop;

    public void addOnTop(GameObject o)
    {
        o.transform.position = this.transform.position;
        onTop = o.GetComponent<Item>();
    }

    public void addOnTop(Item o)
    {
        o.gameObject.transform.position = this.transform.position;
        onTop = o;
    }
}
