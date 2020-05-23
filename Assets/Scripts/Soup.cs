using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soup : MonoBehaviour, PlatableFood
{

    private List<Food> cooking = new List<Food>();

    private bool done = false;
    public bool canBoil()
    {
        return cooking.Count == 3;
    }

    public void addFood(Food f)
    {
        if(cooking.Count < 3 && f.cut )
        {
            if(cooking.Count > 0)
            {
                if (f.t == type())
                {
                    cooking.Add(f);
                }
            }
            else
            {
                cooking.Add(f);
            }
      }
    }

    public Item.type type()
    {
        if (cooking.Count > 0)
        {
            return cooking[0].t;
        }
        return Item.type.onion;
    }

    public void Boiled()
    {
        done = true;
    }
}
