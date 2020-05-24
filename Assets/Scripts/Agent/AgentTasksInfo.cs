using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentTasksInfo
{

    private Agent originAgent;
    private int id;
    private List<Task> iterationTasks;
    private Task currentTask;

    public AgentTasksInfo(Agent originAgent, List<Task> iterationTasks, Task currentTask)
    {
        this.originAgent = originAgent;
        this.id = this.originAgent.GetId();
        this.iterationTasks = iterationTasks;
        this.currentTask = currentTask;
    }

    public List<Task> GetIterationTasks()
    {
        return this.iterationTasks;
    }
    public Task GetCurrentTask()
    {
        return currentTask;
    }
}
