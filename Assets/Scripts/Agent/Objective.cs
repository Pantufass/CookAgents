using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objective : Requirement
{
    public enum objective {salad, onionSoup, tomatoSoup,tomato, lettuce}
    public objective o;

    public List<Requirement> requirements;

    public Objective(objective ob)
    {
        pos = new List<Vector3>();
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
            case objective.tomato:
                objTomato();
                break;
            case objective.lettuce:
                objLettuce();
                break;
        }
        requirements = new List<Requirement>();
    }

    public Objective(Plate.State r)
    {
        requirements = new List<Requirement>();
        pos = new List<Vector3>();
        t = type.objective;
        switch (r)
        {
            case Plate.State.onSoup:
                objOnionSoup();
                o = objective.onionSoup;
                break;
            case Plate.State.salad:
                objSalad();
                o = objective.salad;
                break;
            case Plate.State.tomSoup:
                objTomatoSoup();
                o = objective.tomatoSoup;
                break;
            case Plate.State.lettuce:
                objLettuce();
                o = objective.lettuce;
                break;
            case Plate.State.tomato:
                objTomato();
                o = objective.tomato;
                break;
        }
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
        requirements.Add(new Requirement(type.plate));

        requirements.Add(new Requirement(type.pan));


        requirements.Add(addCut(FoodRequirement.foodType.onion));
        requirements.Add(addCut(FoodRequirement.foodType.onion));
        requirements.Add(addCut(FoodRequirement.foodType.onion));

        requirements.Add(new Requirement(type.boil));

        requirements.Add(new Requirement(type.deliver));

    }

    FoodRequirement addCut(FoodRequirement.foodType f)
    {
        FoodRequirement fr = new FoodRequirement(f);
        fr.needCut();
        return fr;
    }

    void objSalad()
    {
        requirements.Add(new Requirement(type.plate));

        requirements.Add(addCut(FoodRequirement.foodType.tomato));
        requirements.Add(addCut(FoodRequirement.foodType.lettuce));

        requirements.Add(new Requirement(type.deliver));
    }

    void objTomatoSoup()
    {
        requirements.Add(new Requirement(type.plate));

        requirements.Add(new Requirement(type.pan));


        requirements.Add(addCut(FoodRequirement.foodType.tomato));
        requirements.Add(addCut(FoodRequirement.foodType.tomato));
        requirements.Add(addCut(FoodRequirement.foodType.tomato));

        requirements.Add(new Requirement(type.boil));

        requirements.Add(new Requirement(type.deliver));
    }

    void objLettuce()
    {
        requirements.Add(new Requirement(type.plate));
        requirements.Add(addCut(FoodRequirement.foodType.lettuce));
        requirements.Add(new Requirement(type.deliver));
    }
    void objTomato()
    {
        requirements.Add(new Requirement(type.plate));
        requirements.Add(addCut(FoodRequirement.foodType.tomato));
        requirements.Add(new Requirement(type.deliver));
    }

}
