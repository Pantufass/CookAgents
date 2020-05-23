using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour
{

    private PlayerController controller;

    private PlayerMap map;
    // Start is called before the first frame update
    void Start()
    {
        controller = new PlayerController(this.gameObject);
        map = new PlayerMap(); 
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
        foreach(Action a in actions)
        {
            List<Vector3> goal = a.GetGoal();

            List<List<Vector3>> allPossiblePaths = new List<List<Vector3>>();

            foreach(Vector3 v in goal)
            {
                List<List<Vector3>> possiblePaths = map.FindPossiblePaths(this.transform.position, v);

                foreach(List<Vector3> path in possiblePaths)
                {
                    allPossiblePaths.Add(path);
                }
            }

            Task possibleTask = new Task(a, allPossiblePaths);

            possibleTasks.Add(possibleTask);
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
        return null; 
    }
}
