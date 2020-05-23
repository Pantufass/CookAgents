using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recipient : Item
{
    public virtual bool addFood(GameObject f)
    {
        return false;
    }
}
