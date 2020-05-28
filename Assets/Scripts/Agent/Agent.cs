using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour
{
    public int id;

    private List<GameObject> otherPlayers = new List<GameObject>();

    private List<List<Task>> iterationTasks = new List<List<Task>>();

    private PlayerController controller;

    private List<Requirement> requestList;

    private int layeredReq;
    private Objective currentObjective;
    private Requirement currentReq;
    private Task currentTask;
    
    private PlayerMap map = new PlayerMap();

    private GameObject delivery;
    // Start is called before the first frame update
    void Start()
    {
        requestList = new List<Requirement>();
        controller = gameObject.GetComponent<PlayerController>();
        controller.addAgent(this);

        GameEvents.current.OnRecipeEnter += newRecipe;
        GameEvents.current.OnRequestEnter += newRequest;

        delivery = GameObject.FindGameObjectWithTag("Delivery");
        layeredReq = 10;
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

    GameObject getClosest(Requirement r, int layer, List<GameObject> other)
    {
        if (currentReq != null && !(layer < layeredReq)) return null;
        GameObject closest = gameObject;
        float minDist = 1000;
        if (r.pos.Count > 0)
        {
            minDist = Vector3.Distance(gameObject.transform.position, r.pos[0]);
        }
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
                foreach (GameObject agent in other)
                {
                    auxDist = Vector3.Distance(agent.transform.position, p);
                    if (auxDist < minDist || (auxDist == minDist && id < agent.GetComponent<Agent>().id))
                    {
                        minDist = auxDist;
                        closest = agent;
                    }
                }
            }
        }
        return closest;
    }

    void recursive(Requirement r, int layer, List<GameObject> other)
    {
        if (r.requested) return;
        GameObject closest = getClosest(r, layer, other);
        if (closest == null) return;
        if (closest == gameObject)
        {
            currentReq = r;
            layeredReq = layer;
            currentReq.requested = true;
        }
        else
        {
            //ask closest
            Agent a = closest.GetComponent<Agent>();
            if (a.youbusy(r, layer))
            {
                //its busy
                List<GameObject> objs = new List<GameObject>();
                foreach (GameObject o in other)
                {
                    if (o != closest)
                    {
                        objs.Add(o);
                    }
                }
                //again without the busy closest
                recursive(r, layer, objs);
            }
            
        }
    }

    void newRequest(Requirement r,int layer)
    {
        if (!r.requested)
        {
            List<GameObject> objs = new List<GameObject>();
            foreach (GameObject o in otherPlayers)
            {
                objs.Add(o);
            }
            recursive(r, layer, objs);
        }

    }

    //know if other agent is closer and available to requirement
    //true if busy or if more distant
    public bool youbusy(Requirement r, int layer)
    {
        return r != currentReq && currentReq != null && !(layer < layeredReq);
    }

    // Update is called once per frame
    void Update()
    {
        Perceive();
        Decide();
        Act();
        Debug.Log(currentReq + " " +id);

        foreach(GameObject a in otherPlayers)
        {
            if (a.GetComponent<Agent>().currentReq == this.currentReq) Debug.Log("FUCK");
        }
    }

    //just throw requests (it perceives what to do when it receives a request)
    private void Perceive()
    {
        if(currentObjective == null && requestList.Count > 0)
        {
            //update new objective
            currentObjective = requestList[0] as Objective;

            //throw request
            Requirement r = currentObjective.nextReq();
            r.target = this.transform.position;
            GameEvents.current.RequestEnter(r,layeredReq);
        }
        else
        {
            if(currentObjective != null)
            {
                Requirement r = currentObjective.nextReq();
                    r.target = this.transform.position;
                    GameEvents.current.RequestEnter(r, 10);
            }
             if(currentReq != null)
            {
                if (!currentReq.sucess())
                {
                    if (currentReq.canDivide())
                    {
                        List<Requirement> rq = currentReq.divide();
                        if(rq != null)
                        {
                            foreach (Requirement r in rq)
                            {
                                    r.target = this.transform.position;
                                    GameEvents.current.RequestEnter(r, layeredReq - 1);
                                
                            }
                        }
                        
                    }
                }
            }
        }


    }

    //translate objective into task
    private void Decide()
    {
        if(currentReq != null)
        {
            if(currentTask == null)
            {
                FoodRequirement fr = currentReq as FoodRequirement;
                if(fr != null)
                {
                    currentTask = new FoodTask(fr,this);
                    currentTask.findClosestTarget(this);
                }
                else
                {
                    DishRequirement dr = currentReq as DishRequirement;
                    if(dr != null)
                    {
                        currentTask = new DishTask(dr,this);
                        currentTask.findClosestTarget(this);
                    }
                    else
                    {
                        currentTask = new Task(currentReq,this);
                        currentTask.findClosestTarget(this);
                    }
                }
            }
            else
            {
                    FoodTask ft = currentTask as FoodTask;
                    if (ft != null)
                    {
                        if (ft.gotThere(this))
                        {
                            if (ft.part1)
                            {
                                if (ft.cutting())
                                {

                                }
                            }
                            if (ft.picking())
                            {
                                if (ft.pickedUp())
                                {
                                    
                                    ft.part1 = true;
                                    ft.goCut(this);
                                }
                            }
                        }
                        
                        
                    }
                

                
            }
        }
    }

    //transform task into commands 
    private void Act()
    {
        List<bool> commands = new List<bool>();
        for(int i = 0; i < 8; i++)
        {
            commands.Add(false);
        }

        if(currentTask != null)
        {
            bool[] t = currentTask.turnTo(this, controller.front);
            for (int i = 0; i < t.Length; i++)
            {
                commands[i+4] = t[i];
            }
            switch (currentTask.a)
            {
                case Task.action.move:
                    bool[] b = currentTask.getDirection(this);
                    for (int i = 0; i < b.Length; i++)
                    {
                        commands[i] = b[i];
                    }
                    break;
                case Task.action.pickUp:
                    commands[7] = true;
                    break;
                case Task.action.drop:
                    commands[7] = true;
                    break;
                default:
                    commands[6] = true;
                    break;
            }
        }
       

        controller.cycle(commands);
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
