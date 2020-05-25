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

    private Sprite empty;

    public Soup soup;
    public bool spot1;

    private void Start()
    {
        spot1 = false;
        s = State.empty;
        full = false;
        empty = GetComponent<SpriteRenderer>().sprite;
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
                        if (!spot1)
                        {
                            GetComponent<SpriteRenderer>().sprite = wLet;
                            s = State.lettuce;
                            spot1 = true;
                        }
                        else
                        {
                            GetComponent<SpriteRenderer>().sprite = fullSalad;
                            s = State.salad;
                            full = true;
                        }
                        return true;
                    }
                    else if (food.t == Item.type.tomato && s != State.tomato && food.cut)
                    {
                        if (!spot1)
                        {
                            GetComponent<SpriteRenderer>().sprite = wTom;
                            s = State.tomato;
                            spot1 = true;
                        }
                        else
                        {
                            GetComponent<SpriteRenderer>().sprite = fullSalad;
                            s = State.salad;
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
        spot1 = false;
        s = State.empty;
        full = false;
        GetComponent<SpriteRenderer>().sprite = empty;
    }

    public State GetState()
    {
        return s;
    }
    
}
