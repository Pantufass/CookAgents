using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Policy
{

    private readonly int colisionPenalty = 200;
    private readonly int concurrencyPenalty = 50;
    private readonly int bonusForProximity = 50;
    private readonly int priorityPenalty = 2;

    private TaskSet bestSet;


    public List<AgentDecisionTask> RationalDecision(List<AgentTasksInfo> agentTasks)
    {
        bestSet = null;

        List<List<Task>> allTasks = new List<List<Task>>();

        //Handle currenTask, only task in iteration task? or sent appart?
        foreach(AgentTasksInfo a in agentTasks)
        {
            List<Task> tasks = new List<Task>(a.GetIterationTasks());
            allTasks.Add(tasks);
        }

        FormSets(allTasks, 0, new List<Task>());

        if(bestSet == null)
        {
            foreach(AgentTasksInfo a in agentTasks)
            {
                Debug.Log(a.GetIterationTasks().Count);
            }
            return null;
        }

        List<AgentDecisionTask> agentsDecisions = new List<AgentDecisionTask>();
        List<Task> bestTasks = bestSet.GetSet();
        for (int i = 0; i < bestTasks.Count; i++)
        {
            AgentDecisionTask adt = new AgentDecisionTask(bestTasks[i], agentTasks[i].GetId());
            agentsDecisions.Add(adt);
        }
        return agentsDecisions;

    }

    private void FormSets(List<List<Task>> allTasks, int index, List<Task> currentSet)
    {

        foreach(Task t in allTasks[index])
        {
            List<Task> cloneSet = new List<Task>(currentSet);
            cloneSet.Add(t);

            if(index < allTasks.Count - 1)
            {
                index++;
                FormSets(allTasks, index, cloneSet);
                index--;
            }
            else
            {
                TaskSet set = new TaskSet(cloneSet);
                CalculateSetUtility(set);
                if(this.bestSet == null)
                {
                    bestSet = set;
                }
                else
                {
                    if(set.GetCost() < bestSet.GetCost())
                    {
                        bestSet = set;
                    }
                }
            }

        }
    }

    private class TaskSet
    {
        private List<Task> set;
        private int cost;

        public TaskSet(List<Task> set)
        {

            this.set = set;
        }

        public List<Task> GetSet()
        {

            return this.set;
        }

        public int GetCost()
        {
            return this.cost;
        }

        public void SetCost(int cost)
        {
            this.cost = cost;
        }

    }

    private void CalculateSetUtility(TaskSet set)
    {
        int cost = 0;
        List<Task> tasks = set.GetSet();
        for(int i = 0; i < tasks.Count - 1; i++)
        {
            int size = tasks[i].GetPath().Count;
            cost += size;

            cost += (tasks[i].GetAction().GetPriority() * this.priorityPenalty);

            if(size == 1)
            {
                cost -= this.bonusForProximity;
            }

            for(int j = i + 1; j < tasks.Count; j++)
            {
                
                if(ExistsCollision(tasks[i].GetPath(), tasks[j].GetPath())){
                    cost += this.colisionPenalty;
                }
                if (tasks[i].GetAction().GetActionType().Equals(tasks[j].GetAction().GetActionType()))
                {
                    cost += this.concurrencyPenalty;
                }
            }
        }
        set.SetCost(cost);

    }


    private bool ExistsCollision(List<Vector3> path1, List<Vector3> path2)
    {
        if(path1.Count == 0 || path2.Count == 0)
        {
            return false;
        }
        int index1 = 0;
        int index2 = 0;
        int size1 = path1.Count;
        int size2 = path2.Count;

        while(index1 != size1 - 1 || index2 != size2 - 1)
        {
            if (path1[index1].Equals(path2[index2]))
            {
                return true;
            }
            else if (index1 > 0 && index2 > 0)
            {
                if (path1[index1].Equals(path2[index2 - 1]) && path2[index2].Equals(path1[index1 - 1]))
                {
                    return true;
                }
            }
            if(index1 != size1 - 1) { index1++; }
            if (index2 != size2 - 1) { index2++; }

        }

        return false;
    }



    private void PrintSet(TaskSet set)
    {
        string print = "Set = (";
        foreach(Task t in set.GetSet())
        {
            print += t.GetId() + ", ";
        }
        print += ")";
        Debug.Log(print);
    }
}
