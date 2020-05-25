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
}
