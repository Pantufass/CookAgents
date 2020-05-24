using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objective : Requirement
{
    public enum objective {salad, onionSoup, tomatoSoup}
    public objective o;

    public List<Requirement> requirements;

    public Objective(objective ob)
    {
        t = type.objective;
        o = ob;
        switch (o)
        {
            case objective.onionSoup:
                objOnionSoup();
                break;
            case objective.salad:
                objSalad();
                break;
            case objective.tomatoSoup:
                objTomatoSoup();
                break;
        }
        requirements = new List<Requirement>();
    }
    public override bool sucess()
    {
        bool b = true;
        foreach(Requirement r in requirements)
        {
            b = b && r.sucess(); 
        }
        return b;
    }

    public void addReq(Requirement r)
    {
        requirements.Add(r);
    }

    void objOnionSoup()
    {
        Requirement r = new Requirement();
        r.t = type.plate;
        requirements.Add(r);

        Requirement r1 = new Requirement();
        r1.t = type.pan;
        requirements.Add(r1);


        requirements.Add(addCut(FoodRequirement.foodType.onion));
        requirements.Add(addCut(FoodRequirement.foodType.onion));
        requirements.Add(addCut(FoodRequirement.foodType.onion));

        Requirement r4 = new Requirement();
        r4.t = type.boil;
        requirements.Add(r4);

    }

    FoodRequirement addCut(FoodRequirement.foodType f)
    {
        FoodRequirement fr = new FoodRequirement();
        fr.t = type.food;
        fr.foodT = f;
        fr.needCut();
        return fr;
    }

    void objSalad()
    {
        Requirement r = new Requirement();
        r.t = type.plate;
        requirements.Add(r);

        requirements.Add(addCut(FoodRequirement.foodType.tomato));
        requirements.Add(addCut(FoodRequirement.foodType.lettuce));

        Requirement r4 = new Requirement();
        r4.t = type.boil;
        requirements.Add(r4);
    }

    void objTomatoSoup()
    {
        Requirement r = new Requirement();
        r.t = type.plate;
        requirements.Add(r);

        Requirement r1 = new Requirement();
        r1.t = type.pan;
        requirements.Add(r1);


        requirements.Add(addCut(FoodRequirement.foodType.tomato));
        requirements.Add(addCut(FoodRequirement.foodType.tomato));
        requirements.Add(addCut(FoodRequirement.foodType.tomato));

        Requirement r4 = new Requirement();
        r4.t = type.boil;
        requirements.Add(r4);
    }
}
