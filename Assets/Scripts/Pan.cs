using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pan : Recipient
{
    public Soup soup = new Soup();

    private void Start()
    {
        soup = new Soup();
    }

    public void Empty()
    {
        soup = new Soup();
    }

    public override bool addFood(GameObject o)
    {
        Food f = o.GetComponent<Food>();
        if (f != null)
        {
            return soup.addFood(f);
        }
        return false;
    }

}
