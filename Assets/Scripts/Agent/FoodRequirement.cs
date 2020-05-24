using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodRequirement : Requirement
{
    public enum foodType { onion, lettuce, tomato }

    public foodType foodT;
    public bool cut = false;

    public FoodRequirement prev;
    public Requirement cutted;

    public void needCut()
    {
        cut = true;
        FoodRequirement r = new FoodRequirement();
        r.t = type.food;
        r.foodT = foodT;
        prev = r;
        Requirement r1 = new Requirement();
        r.t = type.cut;
        cutted = r1;
        
    }

    public override bool sucess()
    {
        bool b = prev.sucess();
        b = b && cutted.sucess();
        return b;
    }
}
