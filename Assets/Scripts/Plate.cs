using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plate : MonoBehaviour
{
    public Soup soup;
    public List<Food> salad;

    public bool full = false;
    public bool nolet = true;
    public bool notom = true;

    public Sprite wTom;
    public Sprite wLet;
    public Sprite Tsoup;
    public Sprite Osoup;
    public Sprite fullSalad;

    public bool addFood(GameObject f)
    {
        if (!full)
        {
            if(f.GetComponent<Soup>() != null && salad.Count == 0)
            {
                soup = f.GetComponent<Soup>();
                full = true;
                if (soup.type() == Item.type.onion) GetComponent<SpriteRenderer>().sprite = Osoup;
                else if (soup.type() == Item.type.tomato) GetComponent<SpriteRenderer>().sprite = Tsoup;
                return true;
            }
            else if(f.GetComponent<Food>() != null)
            {
                if(f.GetComponent<Food>().t == Item.type.lettuce && nolet && f.GetComponent<Food>().cut)
                {
                    salad.Add(f.GetComponent<Food>());
                    if (salad.Count == 2)
                    {
                        GetComponent<SpriteRenderer>().sprite = fullSalad;
                        full = true;
                    }
                    else
                    {
                        GetComponent<SpriteRenderer>().sprite = wLet;
                    }
                    nolet = false;
                    return true;
                }
                else if (f.GetComponent<Food>().t == Item.type.tomato && notom && f.GetComponent<Food>().cut)
                {
                    salad.Add(f.GetComponent<Food>());
                    if (salad.Count == 2)
                    {
                        GetComponent<SpriteRenderer>().sprite = fullSalad;
                        full = true;
                    }
                    else
                    {
                        GetComponent<SpriteRenderer>().sprite = wTom;
                    }
                    notom = false;
                    return true;
                }
            }
        }
        return false;
    }

    
}
