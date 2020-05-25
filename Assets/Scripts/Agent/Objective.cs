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

    public override bool canDivide()
    {
        return requirements.Count > 0;
    }
    public Requirement nextReq()
    {
        foreach (Requirement r in requirements)
        {
            if (!r.sucess()) return r;
        }
        return null;
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


    void objOnionSoup()
    {
        List<Requirement> fr = new List<Requirement>();
        fr.Add(addCut(FoodRequirement.foodType.onion));
        fr.Add(addCut(FoodRequirement.foodType.onion));
        fr.Add(addCut(FoodRequirement.foodType.onion));

        //needs a pan and a list of food
        DishRequirement dr = new DishRequirement(fr, new Requirement(type.pan));
        List<Requirement> panDish = new List<Requirement>();
        panDish.Add(dr);

        //plate the soup
        requirements.Add(new DishRequirement(panDish, new Requirement(type.plate)));

        //boil the soup
        requirements.Add(new Requirement(type.boil));


        //deliver it
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
        List<Requirement> fr = new List<Requirement>();
        fr.Add(addCut(FoodRequirement.foodType.tomato));
        fr.Add(addCut(FoodRequirement.foodType.lettuce));

        //plate the salad
        requirements.Add(new DishRequirement(fr, new Requirement(type.plate)));

        //deliver it
        requirements.Add(new Requirement(type.deliver));
    }

    void objTomatoSoup()
    {
        List<Requirement> fr = new List<Requirement>();
        fr.Add(addCut(FoodRequirement.foodType.tomato));
        fr.Add(addCut(FoodRequirement.foodType.tomato));
        fr.Add(addCut(FoodRequirement.foodType.tomato));

        //needs a pan and a list of food
        DishRequirement dr = new DishRequirement(fr, new Requirement(type.pan));
        List<Requirement> panDish = new List<Requirement>();
        panDish.Add(dr);

        //plate the soup
        requirements.Add(new DishRequirement(panDish, new Requirement(type.plate)));
        //boil the soup
        requirements.Add(new Requirement(type.boil));


        //deliver it
        requirements.Add(new Requirement(type.deliver));
    }

    void objLettuce()
    {
        List<Requirement> fr = new List<Requirement>();
        fr.Add(addCut(FoodRequirement.foodType.lettuce));

        //plate the salad
        requirements.Add(new DishRequirement(fr, new Requirement(type.plate)));

        //deliver it
        requirements.Add(new Requirement(type.deliver));
    }
    void objTomato()
    {
        List<Requirement> fr = new List<Requirement>();
        fr.Add(addCut(FoodRequirement.foodType.tomato));

        //plate the salad
        requirements.Add(new DishRequirement(fr, new Requirement(type.plate)));

        //deliver it
        requirements.Add(new Requirement(type.deliver));
    }

    public override List<Requirement> divide()
    {
        if (!canDivide()) return null;
        List<Requirement> lr = new List<Requirement>();
        foreach (Requirement r in requirements)
        {
            lr.Add(r);
        }
        return lr;
    }
}
