using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentDecisionTask
{
    private Task task;
    private int agentId;

    public AgentDecisionTask(Task task, int agentId)
    {
        this.task = task;
        this.agentId = agentId;
    }

    public Task GetTask()
    {
        return this.task;
    }

    public int GetId()
    {
        return this.agentId;
    }
}
