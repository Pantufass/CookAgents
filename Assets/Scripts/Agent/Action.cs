using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action
{

    private string actionType;
    private List<Vector3> goal;

    //private List<Vector3> secondGoal;

    public Action(string action)
    {
        this.actionType = action;
    }

    public List<Vector3> GetGoal()
    {
        return this.goal;
    }
    public string GetActionType()
    {
        return this.actionType;
    }
}
