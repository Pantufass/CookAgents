using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour
{
    private int id;

    private List<GameObject> otherPlayers = new List<GameObject>();

    private List<List<Task>> iterationTasks = new List<List<Task>>();

    private PlayerController controller;

    private PlayerMap map = new PlayerMap();
    // Start is called before the first frame update
    void Start()
    {
        controller = new PlayerController(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        Perceive();
        Decide();
        Act();
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
                    a.ReceiveAgentTasks(possibleTasks);
                }
            }
        }
        else
        {
            iterationTasks.Add(possibleTasks);
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




    private void ReceiveAgentTasks(List<Task> tasks)
    {
        iterationTasks.Add(tasks);
        if(iterationTasks.Count == otherPlayers.Count + 1)
        {
            Debug.Log("lets gooo");
            Decide();
        }
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
