using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Counter : MonoBehaviour
{
    public Item onTop;
    public bool hasItem = false;

    public bool addOnTop(GameObject o)
    {
        Recipient r = onTop as Recipient;
        if (r != null)
        {
            bool b = r.addFood(o);
            Destroy(o);
            return b;
        }
        if (hasItem) return false;
        o.transform.position = this.transform.position + new Vector3(0, 0, -1);
        onTop = o.GetComponent<Item>();
        hasItem = true;
        return true;
    }

    public bool addOnTop(Item o)
    {
        Recipient r = onTop as Recipient;
        if (r != null)
        {
            if (r.addFood(o.gameObject))
            {
                Destroy(o.gameObject);
                return true;
            }
            return false;
        }
        if (hasItem) return false;
        o.gameObject.transform.position = this.transform.position + new Vector3(0,0,-1);
        o.transform.parent = null;
        onTop = o;
        hasItem = true;
        return true;
    }

    public virtual bool use(GameObject player) { return false; }

    public bool pickUp(GameObject player)
    {
        if (!hasItem) return false;
        player.GetComponent<PlayerController>().holdItem(onTop);
        onTop.transform.parent = player.transform;
        onTop.transform.position = player.transform.position;
        onTop = null;
        hasItem = false;
        return true;

    }

    public bool addPan(Pan pan)
    {
        Plate plate = onTop as Plate;
        if (plate != null && pan != null)
        {
            if (plate.addSoup(pan.soup)) pan.Empty();
            return false;
        }
        if (hasItem) return false;
        pan.gameObject.transform.position = this.transform.position + new Vector3(0, 0, -1);
        pan.transform.parent = null;
        onTop = pan;
        hasItem = true;
        return true;
    }

}
