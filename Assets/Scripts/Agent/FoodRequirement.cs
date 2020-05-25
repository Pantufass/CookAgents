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

    public FoodRequirement(foodType f)
    {
        pos = new List<Vector3>();
        t = type.food;
        foodT = f;
        switch (f)
        {
            case foodType.lettuce:
                getPos("Lettuce");
                break;
            case foodType.onion:
                getPos("Onion");
                break;
            case foodType.tomato:
                getPos("Tomato");
                break;
        }
    }


    public void needCut()
    {
        cut = true;
        FoodRequirement r = new FoodRequirement(foodT);
        prev = r;
        Requirement r1 = new Requirement(type.cut);
        cutted = r1;
        
    }

    public override bool sucess()
    {
        bool b = prev.sucess();
        b = b && cutted.sucess();
        return b;
    }
}
