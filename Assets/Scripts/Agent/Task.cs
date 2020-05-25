using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task
{
    private GameObject agent;

    private int taskId;

    private Action action;

    private List<Vector3> path;
    
    public Task(GameObject agent, int taskId, Action action, List<Vector3> path)
    {
        this.agent = agent;
        this.taskId = taskId;
        this.action = action;
        this.path = path;
    }

    public Action GetAction()
    {
        return this.action;
    }

    public List<Vector3> GetPath()
    {
        return this.path;
    }

    public int GetId()
    {
        return this.taskId;
    }


}
