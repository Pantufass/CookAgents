using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pan : Recipient
{
    public Soup soup;

    public List<Sprite> OPan;
    public List<Sprite> TPan;

    private SpriteRenderer sp;

    public Sprite empty;

    private void Start()
    {
        GameObject o = Instantiate(soup.gameObject);
        o.transform.parent = transform;
        soup = o.GetComponent<Soup>();
        sp = GetComponent<SpriteRenderer>();
        empty = sp.sprite;
    }

    public void Empty()
    {
        GameObject o = Instantiate(soup.gameObject);
        soup = o.GetComponent<Soup>();
        sp.sprite = empty;
    }

    public override bool addFood(GameObject o)
    {
        Food f = o.GetComponent<Food>();
        if (f != null)
        {
            if (isFull()) return false;
            if (soup.addFood(f))
            {
                if (soup.type() == type.onion) sp.sprite = OPan[soup.numItems()-1];
                else if (soup.type() == type.tomato) sp.sprite = TPan[soup.numItems()-1];
                return true;
            }
        }
        return false;
    }

    public bool isFull()
    {
        return soup.canBoil();
    }

    public void Boiled()
    {
        if (soup.type() == type.onion) sp.sprite = OPan[soup.numItems()];
        else if (soup.type() == type.tomato) sp.sprite = TPan[soup.numItems()];
        soup.Boiled();
    }

}
