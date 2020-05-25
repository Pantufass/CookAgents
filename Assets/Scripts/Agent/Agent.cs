﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour
{
    private int id;

    private List<GameObject> otherPlayers = new List<GameObject>();

    private List<AgentTasksInfo> iterationTasks = new List<AgentTasksInfo>();

    private PlayerController controller;

    private RequestToAction requestConverter;

    public GameObject ground;

    private PlayerMap map;

    private Policy policy;

    private Task currentTask = null;

    private bool once = false;

    private int delay = 2;

    // Start is called before the first frame update
    void Start()
    {
        controller = this.gameObject.GetComponent<PlayerController>();
        map = this.gameObject.GetComponent<PlayerMap>();
        requestConverter = new RequestToAction();
        policy = new Policy();
    }

    // Update is called once per frame
    void Update()
    {
        if(delay > 0)
        {
            delay--;
        }
        else
        {
            Perceive();
        }
        
    }

    private void Perceive()
    {   

        List<Task> possibleTasks = new List<Task>();

        if(currentTask == null)
        {
            int taskId = 0;
            Plate.State request = GetMostRecentRequest();
            List<Action> actions = GetPossibleActionsForRequest(request);

            foreach (Action a in actions)
            {

                Vector3 goal = a.GetGoal1();

                List<List<Vector3>> possiblePaths = map.FindPossiblePaths(this.transform.position, goal);

                foreach (List<Vector3> path in possiblePaths)
                {
                    Task possibleTask = new Task(this.gameObject, taskId++, a, path);
                    possibleTasks.Add(possibleTask);
                }

            }
        }
        else
        {
            possibleTasks.Add(currentTask);
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
                Debug.Log("lets gooo");
                Debug.Log(iterationTasks.Count);
                Decide();
            }
        }

    }

    private void Decide()
    {
        List<AgentDecisionTask> bestTasks = policy.RationalDecision(this.iterationTasks);
        this.iterationTasks = new List<AgentTasksInfo>();
        foreach(AgentDecisionTask adt in bestTasks)
        {
            if(adt.GetId() != this.GetId())
            {
                foreach(GameObject g in otherPlayers)
                {
                    Agent a = g.GetComponent<Agent>();
                    if (a.GetId() == adt.GetId())
                    {
                        a.Act(adt.GetTask());
                    }
                }
            }
            else
            {
                this.Act(adt.GetTask());
            }
        }
    }

    private void Act(Task task)
    {
        Debug.Log("Agent: " + id + "   Doing: " + task.GetAction().GetActionType());
        if(currentTask == null)
        {
            currentTask = task;
        }
        Vector3 position = this.gameObject.transform.position;

        List<Vector3> path = currentTask.GetPath();


        if(path.Count > 1)
        {
            //currentPos = path[0]
            Vector3 nextPos = path[1];
            if (!PlayerInThatPosition(nextPos))
            {
                path.RemoveAt(0);
                Vector3 walk = nextPos - position;

                List<bool> com = new List<bool>();
                com.Add(walk.x == -1);
                com.Add(walk.x == 1);
                com.Add(walk.y == 1);
                com.Add(walk.y == -1);
                com.Add(Input.GetKey(KeyCode.Q));
                com.Add(Input.GetKey(KeyCode.E));
                com.Add(Input.GetKey(KeyCode.X));
                com.Add(Input.GetKey(KeyCode.Z));
                controller.Act(com);
            }

        }
        else
        {
            controller.RotateTowards(currentTask.GetAction().GetGoal1());

            if (currentTask.GetAction().GetActionType().Equals("getCutedOnion"))
            {
                Use();
                this.currentTask = null;
            }
            else if(currentTask.GetAction().GetActionType().IndexOf("getNew") > -1)
            {
                PickUp();
                foreach (Transform t in this.gameObject.transform)
                {
                    UpdateMaps(t);
                    this.currentTask = null;
                }
                
            }
            else if(currentTask.GetAction().GetActionType().IndexOf("get") > -1)
            {
                PickUp();

                foreach(Transform t in this.gameObject.transform)
                {
                    this.currentTask = null;
                    return;
                }
                
            }
            else if(currentTask.GetAction().GetActionType().IndexOf("deliverCutedInSoup") > -1)
            {
                Transform carrying = null;
                foreach(Transform t in this.gameObject.transform)
                {
                    carrying = t;
                    break;
                }
                if(carrying != null)
                {
                    DeleteFromMaps(carrying);
                }
                Use();
                if(transform.childCount > 0)
                {
                    Debug.Log("Deleting onion");
                }
                else
                {
                    this.currentTask = null;
                }
            }
            else if(currentTask.GetAction().GetActionType().IndexOf("boilSoup") > -1)
            {
                PickUp();
                PickUp();
                this.currentTask = null;
            }
            else if(currentTask.GetAction().GetActionType().IndexOf("getSoupFromPan") > -1)
            {
                Use();
                this.currentTask = null;
            }
            else if(currentTask.GetAction().GetActionType().IndexOf("deliverSoupInPlateOnion") > -1)
            {
                PickUp();
                this.currentTask = null;
            }
            else if(currentTask.GetAction().GetActionType().IndexOf("deliver") > -1 || currentTask.GetAction().GetActionType().IndexOf("use") > -1)
            {
                Use();
                this.currentTask = null;
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


    private Plate.State GetMostRecentRequest()
    {
        Plate.State test = this.controller.LastRequest();
        return test;

    }

    private List<Action> GetPossibleActionsForRequest(Plate.State request)
    {
        List<string> options = requestConverter.GetConversion(request);

        return map.GetPossibleActions(options);
       
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
        if(iterationTasks.Count == otherPlayers.Count + 1)
        {
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
