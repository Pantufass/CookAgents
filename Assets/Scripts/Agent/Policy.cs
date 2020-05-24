using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Policy
{

    private List<TaskSet> iterationSets;
    public List<Task> RationaleDecision(List<AgentTasksInfo> agentTasks)
    {
        iterationSets = new List<TaskSet>();

        List<List<Task>> allTasks = new List<List<Task>>();

        //Handle currenTask, only task in iteration task? or sent appart?
        foreach(AgentTasksInfo a in agentTasks)
        {
            List<Task> tasks = new List<Task>(a.GetIterationTasks());
            allTasks.Add(tasks);
        }

        FormSets(allTasks, 0, new List<Task>());







        return null;
    }

    private void FormSets(List<List<Task>> allTasks, int index, List<Task> currentSet)
    {
        foreach(Task t in allTasks[index])
        {
            List<Task> cloneSet = new List<Task>(currentSet);
            cloneSet.Add(t);

            if(index != allTasks.Count - 1)
            {
                index++;
                FormSets(allTasks, index, cloneSet);
                index--;
            }
            else
            {
                TaskSet set = new TaskSet(cloneSet);
                this.iterationSets.Add(set);
            }

        }
    }

    private class TaskSet
    {
        private List<Task> set;

        public TaskSet(List<Task> set)
        {

            this.set = set;
        }

        public List<Task> GetSet()
        {

            return this.set;
        }
    }
}
