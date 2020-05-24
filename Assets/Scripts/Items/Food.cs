using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : Item, PlatableFood
{
    public bool cut = false;
    public Sprite cutSprite;

    public void Cut()
    {
        cut = true;
        this.GetComponent<SpriteRenderer>().sprite = cutSprite;
    }
}
