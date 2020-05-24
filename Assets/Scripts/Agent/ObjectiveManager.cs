using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveManager
{
    public static List<Objective> objectives;

    public ObjectiveManager()
    {
        Objective o = new Objective(Objective.objective.onionSoup);
        Objective o1 = new Objective(Objective.objective.tomatoSoup);
        Objective o2 = new Objective(Objective.objective.salad);
        objectives.Add(o);
        objectives.Add(o1);
        objectives.Add(o2);
    }
}
