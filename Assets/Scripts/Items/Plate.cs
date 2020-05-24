using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plate : Recipient
{

    public enum State { empty, salad, tomato, lettuce, tomSoup, onSoup}
    public State s;
    public bool full;

    public Sprite wTom;
    public Sprite wLet;
    public Sprite Tsoup;
    public Sprite Osoup;
    public Sprite fullSalad;


    public Soup soup;
    public Food spot1;
    public Food spot2;

    private void Start()
    {
        s = State.empty;
        full = false;
    }

    public override bool addFood(GameObject f)
    {
        if (!full)
        {
            soup = f.GetComponent<Soup>();
            if (soup != null && s == State.empty)
            {
                full = true;
                if (soup.type() == Item.type.onion){
                    GetComponent<SpriteRenderer>().sprite = Osoup;
                    s = State.onSoup;
                }
                else if (soup.type() == Item.type.tomato) {
                    GetComponent<SpriteRenderer>().sprite = Tsoup;
                    s = State.tomSoup;
                }
                return true;
            }
            else{
                Food food = f.GetComponent<Food>();
                if (food != null)
                {
                    if (food.t == Item.type.lettuce && s != State.lettuce && food.cut)
                    {
                        if (spot1 == null)
                        {
                            GetComponent<SpriteRenderer>().sprite = wLet;
                            spot1 = food;
                        }
                        else
                        {
                            GetComponent<SpriteRenderer>().sprite = fullSalad;
                            spot2 = food;
                            full = true;
                        }
                        return true;
                    }
                    else if (food.t == Item.type.tomato && s != State.tomato && food.cut)
                    {
                        if (spot1 == null)
                        {
                            GetComponent<SpriteRenderer>().sprite = wTom;
                            spot1 = food;
                        }
                        else
                        {
                            GetComponent<SpriteRenderer>().sprite = fullSalad;
                            spot2 = food;
                            full = true;
                        }
                        return true;
                    }
                }
            }
        }
        return false;
    }

    public bool addSoup(Soup soup2)
    {
        if (!full)
        {
            soup = soup2;
            if (soup != null && s == State.empty)
            {
                full = true;
                if (soup.type() == Item.type.onion)
                {
                    GetComponent<SpriteRenderer>().sprite = Osoup;
                    s = State.onSoup;
                    return true;
                }
                else if (soup.type() == Item.type.tomato)
                {
                    GetComponent<SpriteRenderer>().sprite = Tsoup;
                    s = State.tomSoup;
                    return true;
                }
            }
        }
        return false;
    }

    public void Empty()
    {
        spot1 = null;
        spot2 = null;
        s = State.empty;
        full = false;
    }
    
}
