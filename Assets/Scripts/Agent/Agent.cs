using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour
{
    private int id;

    private List<GameObject> otherPlayers = new List<GameObject>();

    private List<AgentTasksInfo> iterationTasks = new List<AgentTasksInfo>();

    private PlayerController controller;

    public GameObject ground;

    private PlayerMap map;

    private Task currentTask = null;

    // Start is called before the first frame update
    void Start()
    {
        controller = this.gameObject.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        Perceive();
        Decide();
        Act();
    }

    private void FixedUpdate()
    {
        List<bool> com = new List<bool>();
        com.Add(Input.GetKey(KeyCode.A));
        com.Add(Input.GetKey(KeyCode.D));
        com.Add(Input.GetKey(KeyCode.W));
        com.Add(Input.GetKey(KeyCode.S));
        com.Add(Input.GetKey(KeyCode.Q));
        com.Add(Input.GetKey(KeyCode.E));
        com.Add(Input.GetKey(KeyCode.X));
        com.Add(Input.GetKey(KeyCode.Z));
        controller.Act(com);
    }

    private void Perceive()
    {
        Request request = GetMostRecentRequest();
        List<Action> actions = GetPossibleActionsForRequest(request);
        List<Task> possibleTasks = new List<Task>();
        foreach (Action a in actions)
        {
            List<Vector3> goal = a.GetGoal();

            List<List<Vector3>> allPossiblePaths = new List<List<Vector3>>();

            foreach (Vector3 v in goal)
            {
                List<List<Vector3>> possiblePaths = map.FindPossiblePaths(this.transform.position, v);

                foreach (List<Vector3> path in possiblePaths)
                {
                    allPossiblePaths.Add(path);
                }
            }

            Task possibleTask = new Task(a, allPossiblePaths);
            possibleTasks.Add(possibleTask);



        }

        int highestId = otherPlayers.Count + 1;

        if (id != highestId)
        {
            foreach(GameObject p in otherPlayers)
            {
                Agent a = p.GetComponent<Agent>();
                if(a.GetId() == highestId)
                {
                    a.ReceiveAgentTasks(new AgentTasksInfo(this, possibleTasks, currentTask));
                }
            }
        }
        else
        {
            iterationTasks.Add(new AgentTasksInfo(this, possibleTasks, currentTask));
        }

    }

    private void Decide()
    {

    }

    private void Act()
    {

    }


    private Request GetMostRecentRequest()
    {
        //TODO
        return null;
    }

    private List<Action> GetPossibleActionsForRequest(Request request)
    {
        //TODO
        return new List<Action>(); 
    }




    private void ReceiveAgentTasks(AgentTasksInfo tasks)
    {
        iterationTasks.Add(tasks);
        if(iterationTasks.Count == otherPlayers.Count + 1)
        {
            Debug.Log("lets gooo");
            Decide();
        }
    }
    public void StartMap()
    {
        ground = GameObject.FindGameObjectWithTag("Ground");
        map = new PlayerMap(ground);
    }
    public void AddOtherPlayer(GameObject player)
    {
        otherPlayers.Add(player);
        map.AddOtherPlayer(player);
    }

    public void SetId(int id)
    {
        this.id = id;
    }

    public int GetId()
    {
        return this.id;
    }
}
