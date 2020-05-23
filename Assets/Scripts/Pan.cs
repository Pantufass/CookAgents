using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pan : Item
{
    public Soup soup;

    private void Start()
    {
        soup = new Soup();
    }

    public void Empty()
    {
        soup = new Soup();
    }


}
