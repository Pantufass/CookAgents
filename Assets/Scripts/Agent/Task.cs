using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task
{
    protected int agentId;

    protected Requirement original;

    protected Requirement targetReq;

    protected Vector3 targetPos;

    protected bool firstThere = false;
    public enum action { cut, pickUp, boil, move, drop}

    public bool holding = false;

    public action a;
    public Task(Requirement r, Agent agent)
    {
        original = r;
        a = action.move;
        agentId = agent.id;
        GameEvents.current.OnPickUp += picked;
    }

    void picked(int id)
    {
        if (id == agentId) holding = true;
        Debug.Log("INEFWMEWFEWFEWF");
    }

    public bool pickedUp()
    {
        Debug.Log("Bruh");
        return holding;
    }

    public bool finish()
    {
        original.finishReq();
        return false;
    }

    public bool[] getDirection(Agent agent)
    {
        bool[] b = new bool[4];
        b[0] = agent.transform.position.x > targetPos.x;
        b[1] = agent.transform.position.x < targetPos.x;
        b[2] = agent.transform.position.y > targetPos.y;
        b[3] = agent.transform.position.y < targetPos.y;

        return b;

    }

    public void findClosestTarget(Agent a)
    {
        Vector3 position = a.transform.position;
        float minDist = 10000;
        foreach(Vector3 pos in original.pos)
        {
            float aux = Vector3.Distance(a.transform.position, pos);
            if(minDist > aux)
            {
                position = pos;
                minDist = aux;
            }
        }
        targetPos = position;
    }

    public virtual bool gotThere(Agent agent)
    {
        Vector3 auxPos = agent.transform.position;
        firstThere = Vector3.Distance(auxPos, targetPos) < 1.5f;
        if (firstThere)
        {
            switch (original.t)
            {
                case Requirement.type.cut:
                    this.a = action.cut;
                    break;
                case Requirement.type.boil:
                    this.a = action.boil;
                    break;
                case Requirement.type.food:
                    this.a = action.pickUp;
                    break;
            }
        }
        

        return firstThere;
    }

    public bool moving()
    {
        return a == action.move;
    }

    public bool picking()
    {
        return a == action.pickUp;
    }


}
