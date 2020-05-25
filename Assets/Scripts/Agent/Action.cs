using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action
{

    private string actionType;
    private Vector3 goal1;
    private Vector3 goal2;

    public Action(string actionType, Vector3 goal1, Vector3 goal2)
    {
        this.actionType = actionType;
        this.goal1 = goal1;
        this.goal2 = goal2;
    }


    public Vector3 GetGoal1()
    {
        return this.goal1;
    }

    public Vector3 GetGoal2()
    {
        return this.goal2;
    }

    public string GetActionType()
    {
        return this.actionType;
    }
}
