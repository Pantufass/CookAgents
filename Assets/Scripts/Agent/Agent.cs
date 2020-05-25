using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour
{
    private int id;

    private List<GameObject> otherPlayers = new List<GameObject>();

    private List<List<Task>> iterationTasks = new List<List<Task>>();

    private PlayerController controller;

    private List<Requirement> requestList;

    private Requirement currentObjective;
    private Task currentTask;
    
    private PlayerMap map = new PlayerMap();

    private GameObject delivery;
    // Start is called before the first frame update
    void Start()
    {
        requestList = new List<Requirement>();
        controller = FindObjectOfType<PlayerController>();

        GameEvents.current.OnRecipeEnter += newRecipe;
        GameEvents.current.OnRequestEnter += newRequest;

        delivery = GameObject.FindGameObjectWithTag("Delivery");
    }

    GameObject closest(Vector3 pos)
    {
        //find closest agent to delivery

        //start with self
        GameObject closest = gameObject;
        float minDist = Vector3.Distance(gameObject.transform.position, pos);
        Debug.Log(minDist);

        //if its too close doesnt need to compute others
        if (!false)
        {
            foreach (GameObject agent in otherPlayers)
            {
                float auxDist = Vector3.Distance(agent.transform.position, delivery.transform.position);
                if (auxDist < minDist)
                {
                    minDist = auxDist;
                    closest = agent;
                }
            }
        }
        return closest;
    }
    void newRecipe(Plate.State r)
    {
        //make requests if it is the closest one
        if(closest(delivery.transform.position) == gameObject)
        {
            addObjective(r);
        }
         
    }

    void addObjective(Plate.State r)
    {
        Objective o = new Objective(r);
        o.target = delivery.transform.position;
        requestList.Add(o);
    }

    void createRequest(Plate.State r)
    {
        Objective o = new Objective(r);
        o.target = delivery.transform.position;
        requestList.Add(o);
        foreach(Requirement req in o.requirements)
        {
            GameEvents.current.RequestEnter(req);
        }

    }

    void passRequests(Requirement r)
    {
        FoodRequirement fr = r as FoodRequirement;
        if(fr != null)
        {
            if (fr.cut)
            {
                fr.prev.target = this.transform.position;
                GameEvents.current.RequestEnter(fr.prev);
                requestList.Add(fr);
            }
        }
        else
        {
            requestList.Add(r);
        }

    }

    void newRequest(Requirement r)
    {
        if (currentObjective != null) return;
        GameObject closest = gameObject;
        float minDist = 10000;
        foreach (Vector3 p in r.pos)
        {
            float auxDist = Vector3.Distance(gameObject.transform.position, p);
            if (auxDist < minDist)
            {
                minDist = auxDist;
                closest = gameObject;
            }

            //if its too close doesnt need to compute others
            if (!false)
            {
                foreach (GameObject agent in otherPlayers)
                {
                    auxDist = Vector3.Distance(agent.transform.position, p);
                    if (auxDist < minDist)
                    {
                        minDist = auxDist;
                        closest = agent;
                    }
                }
            }
        }

        if (closest == gameObject)
        {
            currentObjective = r;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Perceive();
        Decide();
        Act();
    }

    //perceive current objective
    private void Perceive()
    {
        if(currentObjective == null && requestList[0] != null)
        {
            //update new objective
            currentObjective = requestList[0];

            //throw request
            Objective o = currentObjective as Objective;
            if(o != null) GameEvents.current.RequestEnter(o.nextReq());
        }
        else
        {

        }


    }

    //decide based on objective
    private void Decide()
    {

    }

    //act based on task
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
