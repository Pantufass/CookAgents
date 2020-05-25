using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action
{

    private string actionType;
    private Vector3 goal1;
    private Transform goal2;
    private int priority;

    public Action(string actionType, Vector3 goal1, Transform goal2, int priority)
    {
        this.actionType = actionType;
        this.goal1 = goal1;
        this.goal2 = goal2;
        this.priority = priority;
    }


    public Vector3 GetGoal1()
    {
        return this.goal1;
    }

    public Transform GetGoal2()
    {
        return this.goal2;
    }

    public string GetActionType()
    {
        return this.actionType;
    }

    public int GetPriority()
    {
        return this.priority;
    }
}
