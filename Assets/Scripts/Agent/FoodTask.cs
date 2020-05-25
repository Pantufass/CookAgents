using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodTask : Task
{
    public bool part1;

    public FoodTask(FoodRequirement r, Agent agent) : base(r, agent)
    {

    }

    public void goCut(Agent agent)
    {
        a = action.move;

        Debug.Log("MOOOOOOOOOOOOOOOVE");
        Vector3 position = agent.transform.position;
        float minDist = 10000;
        foreach (Vector3 pos in (original as FoodRequirement).cutted.pos)
        {
            float aux = Vector3.Distance(agent.transform.position, pos);
            if (minDist > aux)
            {
                position = pos;
                minDist = aux;
            }
        }
        targetPos = position;
    }

    public override bool gotThere(Agent agent)
    {
        Vector3 auxPos = agent.transform.position;
        firstThere = Vector3.Distance(auxPos, targetPos) < 1.5f;

        if (firstThere && !part1) this.a = action.pickUp; 

        if (part1)
        {
            this.a = action.cut;
            Debug.Log("LETS CUT");
        }


        return firstThere;
    }

    public bool cutting()
    {
        return a == action.cut;
    }

}
