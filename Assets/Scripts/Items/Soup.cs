using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soup : MonoBehaviour, PlatableFood
{

    private List<Food> cooking;

    private bool done;
    public bool canBoil()
    {
        return cooking.Count == 3;
    }

    public Soup()
    {
        cooking = new List<Food>();
        done = false;
    }

    public int numItems()
    {
        return cooking.Count;
    }

    public bool addFood(Food f)
    {
        if(cooking.Count < 3 && f.cut )
        {
            if(cooking.Count > 0)
            {
                if (f.t == type())
                {
                    cooking.Add(f);
                    return true;
                }
            }
            else
            {
                cooking.Add(f);
                return true;
            }
      }
        return false;
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

    public bool isDone()
    {
        return done;
    }
}
