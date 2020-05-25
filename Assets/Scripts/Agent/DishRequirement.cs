using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DishRequirement : Requirement
{

    public List<Requirement> dish;
    public Requirement recipient;

    public DishRequirement(List<Requirement> foods, Requirement r)
    {
        recipient = r;
        dish = foods;
        pos = r.pos;
        t = type.dish;
        
    }

    public override bool sucess()
    {
        bool b = recipient.sucess();
        foreach(Requirement r in dish)
        {
            b = b && r.sucess();
        }
        return b;
    }

    public override bool canDivide()
    {
        bool b = true;
        foreach (Requirement r in dish)
        {
            b = b && r.sucess();
        }
        return !b;
    }

    public override List<Requirement> divide()
    {
        if (!canDivide()) return null;
        List<Requirement> lr = new List<Requirement>();
        foreach(Requirement r in dish)
        {
            lr.Add(r);
        }
        return lr;
    }
}
