using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour
{
    private int id;

    private List<GameObject> otherPlayers = new List<GameObject>();

    private List<AgentTasksInfo> iterationTasks = new List<AgentTasksInfo>();

    private PlayerController controller;

    private RequestToAction requestConverter;

    private TaskExecuter taskExecuter;

    public GameObject ground;

    private PlayerMap map;

    private Policy policy;

    private Task currentTask = null;

    private int delay = 1;

    private int pathSize = 0;

    private bool stoped = false;

    private bool dropCarrying = false;

    private Vector3 blacklist = new Vector3();

    // Start is called before the first frame update
    void Start()
    {
        controller = this.gameObject.GetComponent<PlayerController>();
        map = this.gameObject.GetComponent<PlayerMap>();
        requestConverter = new RequestToAction();
        policy = new Policy();
        taskExecuter = new TaskExecuter(this.controller, this.map, this.gameObject, this.otherPlayers, this.id);

    }

    // Update is called once per frame
    void Update()
    {
        if (delay > 0) delay--;
        else
        {
            Perceive();
            delay = 1;
        }
        
    }

    private void Perceive()
    {   

        List<Task> possibleTasks = new List<Task>();

        if(taskExecuter.GetCurrentTask() == null)
        {
            Debug.Log("Agent: " + id + " currentTask = null");
            List<Plate.State> requests = GetMostRecentRequest();
            for(int i = 0; i < requests.Count; i++)
            {
                Plate.State request = requests[i];
                List<Action> actions = GetPossibleActionsForRequest(request, i);

                foreach (Action a in actions)
                {

                    Vector3 goal = a.GetGoal1();
                    if(goal == blacklist)
                    {
                        blacklist = new Vector3();
                        continue;
                    }

                    List<List<Vector3>> possiblePaths = map.FindPossiblePaths(this.transform.position, goal);

                    foreach (List<Vector3> path in possiblePaths)
                    {
                        Task possibleTask = new Task(this.gameObject, i, a, path);
                        possibleTasks.Add(possibleTask);
                    }
                }
            }
            if(possibleTasks.Count == 0)
            {
                Action newAction = new Action("empty", new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, -1), this.gameObject.transform, 0);
                Task possibleTask = new Task(this.gameObject, -1, newAction, map.FindPossiblePaths(this.transform.position,this.transform.position)[0]);
                possibleTasks.Add(possibleTask);
            }
        }
        else if (taskExecuter.GetDropCarrying())      //task he was performing is over and he has something related in hands 
        {
            Debug.Log("Agent: " + id + " DROPING SOMETHING");
            dropCarrying = false;
            List<Action> dropActions = map.GetPossibleDropActions();

            foreach (Action a in dropActions)
            {

                Vector3 goal = a.GetGoal1();

                List<List<Vector3>> possiblePaths = map.FindPossiblePaths(this.transform.position, goal);

                foreach (List<Vector3> path in possiblePaths)
                {
                    Task possibleTask = new Task(this.gameObject, -1, a, path);             //task id == -1 because it is not the same as everyone elses tasks
                    possibleTasks.Add(possibleTask);
                }

            }
        }
        else
        {
            possibleTasks.Add(taskExecuter.GetCurrentTask());
        }
       
        int lowestId = 1;

        if (id != lowestId)
        {
            foreach(GameObject p in otherPlayers)
            {
                Agent a = p.GetComponent<Agent>();
                if(a.GetId() == lowestId)
                {
                    a.ReceiveAgentTasks(new AgentTasksInfo(this, possibleTasks, currentTask));
                }
            }
        }
        else
        {
            iterationTasks.Add(new AgentTasksInfo(this, possibleTasks, currentTask));
            if (iterationTasks.Count == otherPlayers.Count + 1)
            {

                Decide();
            }
        }

    }

    private void Decide()
    {
        List<AgentDecisionTask> bestTasks = policy.RationalDecision(this.iterationTasks);
        this.iterationTasks = new List<AgentTasksInfo>();

        foreach(GameObject p in otherPlayers)
        {
            p.GetComponent<Agent>().InformTasks(bestTasks);
        }
        this.InformTasks(bestTasks);
        
    }

    public void InformTasks(List<AgentDecisionTask> bestTasks)
    {
        map.InformTasks(bestTasks);
        foreach(AgentDecisionTask adt in bestTasks)
        {
            if(adt.GetId() == this.id)
            {
                this.Act(adt.GetTask());
            }
        }
    }

    private void Act(Task task)
    {
        taskExecuter.Act(task);

    }

    private void  FinishRequest()
    {
        int taskId = currentTask.GetId();
        foreach(GameObject g in otherPlayers)
        {
            g.GetComponent<Agent>().StopRequest(taskId);
        }
        currentTask = null;
    }

    public void StopRequest(int taskId)
    {
        if (currentTask == null)
        {
            return;
        }
        Debug.Log("Agent: " + this.id + " They are trying to stop me");
        Debug.Log("Agent: " + this.id + " My id: " + currentTask.GetId());

        Debug.Log("Agent: " + this.id + " Their id: " + taskId);
        if (currentTask.GetId() == taskId)
        {
            currentTask = null;
            if(this.transform.childCount > 0)
            {
                Debug.Log("Agent: " + this.id + " Have child to drop");
                dropCarrying = true;
            }
        }
    }

    private bool PlayerInThatPosition(Vector3 v)
    {
        foreach(GameObject g in otherPlayers)
        {
            if (g.transform.position.Equals(v))
            {
                return true;
            }
        }
        return false;
    }

    private void PickUp()
    {
        List<bool> com = new List<bool>();
        com.Add(false);
        com.Add(false);
        com.Add(false);
        com.Add(false);
        com.Add(false);
        com.Add(false);
        com.Add(true);
        com.Add(false);
        controller.Act(com);
    }

    private void Use()
    {
        List<bool> com = new List<bool>();
        com.Add(false);
        com.Add(false);
        com.Add(false);
        com.Add(false);
        com.Add(false);
        com.Add(false);
        com.Add(false);
        com.Add(true);
        controller.Act(com);
    }


    private List<Plate.State> GetMostRecentRequest()
    {
        List<Plate.State> test = this.controller.LastRequest();
        return test;

    }

    private List<Action> GetPossibleActionsForRequest(Plate.State request, int requestPriority)
    {
        List<string> options = requestConverter.GetConversion(request);

        return map.GetPossibleActions(options, request, requestPriority);
       
    }


    private void UpdateMaps(Transform t)
    {
        this.map.UpdateMap(t);
        foreach(GameObject g in otherPlayers)
        {
            g.GetComponent<PlayerMap>().UpdateMap(t);
        }
    }

    private void DeleteFromMaps(Transform t)
    {
        this.map.DeleteFromMap(t);
        foreach (GameObject g in otherPlayers)
        {
            g.GetComponent<PlayerMap>().DeleteFromMap(t);
        }
    }

    private void ReceiveAgentTasks(AgentTasksInfo tasks)
    {
        iterationTasks.Add(tasks);
        if (iterationTasks.Count == otherPlayers.Count + 1)
        {
            Debug.Log("lets gooo");
            Decide();
        }
    }
    public void AddOtherPlayer(GameObject player)
    {
        otherPlayers.Add(player);
    }

    public void SetId(int id)
    {
        this.id = id;
    }

    public int GetId()
    {
        return this.id;
    }

    private void PrintPath(List<Vector3> path)
    {
        string print = "";
        foreach(Vector3 v in path)
        {
            print += v + " -> " ;
        }
        Debug.Log(print);
    }
}
