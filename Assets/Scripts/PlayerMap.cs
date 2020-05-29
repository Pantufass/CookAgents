using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMap : MonoBehaviour
{

    private bool DEBUG = true;

    private readonly int maxWaitTime = 5;

    private List<GameObject> otherPlayers = new List<GameObject>();

    public GameObject ground;

    public GameObject specials;

    public GameObject balcony;

    public List<Transform> usableBalcony = new List<Transform>();

    private List<List<Vector3>> crossing = new List<List<Vector3>>();

    private List<Vector3> center = new List<Vector3>();

    private List<Vector3> positions = new List<Vector3>();

    private List<Vector3> intersections = new List<Vector3>();

    private List<Transform> stoves = new List<Transform>();

    private List<Vector3> onionDisp = new List<Vector3>();

    private List<Vector3> tomatoDisp = new List<Vector3>();

    private List<Vector3> lettuceDisp = new List<Vector3>();

    private List<Transform> cuttingBoards = new List<Transform>();

    private List<Transform> onions = new List<Transform>();

    private List<Transform> tomatos = new List<Transform>();

    private List<Transform> lettuces = new List<Transform>();

    private List<Transform> pans = new List<Transform>();

    private List<Transform> plates = new List<Transform>();

    private List<Transform> soups = new List<Transform>();

    private Transform delivery;

    private Vector3 plateReturn;


    private readonly int actionTypeImportance = 2;

    private readonly int requestImportance = 10;


    private List<AgentDecisionTask> bestTasks;

    void Start()
    {
        this.ground = GameObject.FindGameObjectWithTag("Ground");
        this.specials = GameObject.Find("Specials");
        this.balcony = GameObject.Find("Balcony");

        RunSpecials();

        foreach (Transform t in ground.transform)
        {
            positions.Add(new Vector3(t.position.x, t.position.y, -1));
        }
        foreach (Transform t in balcony.transform)
        {
            if(!t.gameObject.name.Equals("Corner") && !t.gameObject.name.Equals("Center"))
            {
                this.usableBalcony.Add(t);
            }
        }

        foreach (Vector3 v in positions)
        {
            List<Vector3> temp = new List<Vector3>();
            foreach(Vector3 t in positions)
            {
                if (!v.Equals(t))
                {
                    if(AdjacentPositions(v, t))
                    {
                        temp.Add(t);
                    }
                }
            }
            if(temp.Count == 3)
            {
                center.Add(v);
                foreach(Vector3 h in temp)
                {
                    if (!intersections.Contains(h))
                    {
                        intersections.Add(h);
                    }
                } 
            }
        }

        for(int i = 1; i < 4; i++)
        {
            GameObject pan = GameObject.Find("Pan" + i);
            this.pans.Add(pan.transform);
            GameObject cuttingBoard = GameObject.Find("CuttingBoard" + i);
            this.cuttingBoards.Add(cuttingBoard.transform);
        }
        for (int i = 1; i < 4; i++)
        {
            GameObject plate = GameObject.Find("Plate" + i);
            this.plates.Add(plate.transform);
        }
        for(int i = 1; i < 7; i++)
        {
            List<Vector3> specificCrossing = new List<Vector3>();
            foreach(Transform a in ground.transform)
            {
                if( a.gameObject.name.Equals("Crossing" + i))
                {
                    specificCrossing.Add(a.position);
                }
            }
            crossing.Add(specificCrossing);
        }

    }

    private void RunSpecials()
    {
        foreach(Transform t in specials.transform)
        {
            switch (t.gameObject.name)
            {
                case "Stove":
                    this.stoves.Add(t);
                    break;

                case "OnionDisp":
                    this.onionDisp.Add(new Vector3(t.position.x, t.position.y, -1));
                    break;

                case "TomatoDisp":
                    this.tomatoDisp.Add(new Vector3(t.position.x, t.position.y, -1));
                    break;

                case "LettuceDisp":
                    this.lettuceDisp.Add(new Vector3(t.position.x, t.position.y, -1));
                    break;
                    
                case "Delivery":
                    this.delivery = t;
                    break;

                case "Return":
                    this.plateReturn = new Vector3(t.position.x, t.position.y, -1);
                    break;

                default:
                    break;
            }
        }
    }


    public List<List<Vector3>> FindPossiblePaths(Vector3 start, Vector3 goal)
    {
        List<Vector3> walkablePositions = this.FindClosestWalkablePosition(goal);
       
        List<List<Vector3>> allPossiblePaths = new List<List<Vector3>>();

        foreach(Vector3 w in walkablePositions)
        {
            List<List<Vector3>> lp = this.FindPaths(start, w, new List<Vector3>(), 5);

            foreach(List<Vector3> p in lp)
            {
                allPossiblePaths.Add(p);
            }
        }
        return allPossiblePaths;
        
    }



    private List<List<Vector3>> FindPaths(Vector3 start, Vector3 goal, List<Vector3> path, int wait)
    {
        //Debug.Log(start + "  -->  " + goal);
        if (start.Equals(goal))
        {
            //Debug.Log("Path found");
            path.Add(start);
            List<List<Vector3>> paths = new List<List<Vector3>>();
            paths.Add(path);
            return paths;
        }
        List<List<Vector3>> possiblePaths = new List<List<Vector3>>();

        foreach(Vector3 v in positions)
        {
            if (start.Equals(v))
            {
                continue;
            }
            if(AdjacentPositions(start, v)) //Adjacent square he can walk
            {
                int ocurrences = OcurrencesInPath(path, v);
                if(ocurrences == 0)        // path where it is coming from
                {
                    int newWaitTime = 5;
                    if (!intersections.Contains(v)) { newWaitTime = 0; }
                    List<Vector3> pathCopy = new List<Vector3>(path);
                    pathCopy.Add(start);
                    List<List<Vector3>> pathsReturned = FindPaths(v, goal, pathCopy, newWaitTime);
                    foreach(List<Vector3> p in pathsReturned)
                    {
                        possiblePaths.Add(p);
                    }
                }/*
                //take this out if dont want them to wait in intersections
                else if(ocurrences == 1 && intersections.Contains(start) && !intersections.Contains(v))   //means you just left an intersection and only passed in the intersection once
                {
                    List<Vector3> pathCopy = new List<Vector3>(path);
                    pathCopy.Add(start);
                    List<List<Vector3>> pathsReturned = FindPaths(v, goal, pathCopy, 0);
                    foreach (List<Vector3> p in pathsReturned)
                    {
                        possiblePaths.Add(p);
                    }/*
                    if(wait > 0)
                    {
                        pathCopy = new List<Vector3>(path);
                        pathCopy.Add(start);
                        pathsReturned = FindPaths(start, goal, pathCopy, wait - 1);
                        foreach (List<Vector3> p in pathsReturned)
                        {
                            possiblePaths.Add(p);
                        }
                    }
                }  */
            }
        }
        return possiblePaths;

    }


    private List<Vector3> FindClosestWalkablePosition(Vector3 goal)
    {
        List<Vector3> walkablePositions = new List<Vector3>();

        if (positions.Contains(goal))
        {
            walkablePositions.Add(goal);
            return walkablePositions;
        }
        else
        {
            foreach(Vector3 p in positions)
            {
                float xDiference = Mathf.Abs(p.x - goal.x);
                float yDiference = Mathf.Abs(p.y - goal.y);

                if((xDiference + yDiference) <= 1)
                {
                    walkablePositions.Add(p);
                }
            }
            return walkablePositions;
        }
    }

    private int OcurrencesInPath(List<Vector3> path, Vector3 v)
    {
        int ocurrences = 0;
        foreach(Vector3 position in path)
        {
            if (position.Equals(v))
            {
                ocurrences++;
            }
        }
        return ocurrences;
    }

    private bool AdjacentPositions(Vector3 pos1, Vector3 pos2)
    {
        float xDiference = Mathf.Abs(pos2.x - pos1.x);
        float yDiference = Mathf.Abs(pos2.y - pos1.y);

        return (xDiference + yDiference) == 1;
    }


    public Vector3 GetPositionOf(GameObject desiredObject)   //FIXME Gameobject ??
    {
        //TODO
        return new Vector3();

    }

    public void AddOtherPlayer(GameObject player)
    {
        otherPlayers.Add(player);
    }




    public List<Action> GetPossibleActions(List<string> options, Plate.State request,  int requestIndex)
    {
        List<Action> possibleActions = new List<Action>();

        bool carrying = this.gameObject.transform.childCount > 0;

        int i = 0;

        for (i = 0; i < options.Count; i++)
        {
            string[] part = options[i].Split('_');
            List<Transform> foods = new List<Transform>();
            List<Vector3> foodDisp = new List<Vector3>();
            string foodType = "";
            Item.type itemType = Item.type.none;
            if(part.Length == 2)
            {
                foodType = part[1];
                switch (part[1])
                {
                    case "Onion":
                        foods = this.onions;
                        foodDisp = this.onionDisp;
                        itemType = Item.type.onion;
                        break;
                    case "Tomato":
                        foods = this.tomatos;
                        foodDisp = this.tomatoDisp;
                        itemType = Item.type.tomato;
                        break;
                    case "Lettuce":
                        foods = this.lettuces;
                        foodDisp = this.lettuceDisp;
                        itemType = Item.type.lettuce;
                        break;
                }
            }

            switch (part[0])
            {

                case "replacePanInStove":
                    if (carrying)
                    {
                        ActionTypeToActionConverter.ReplacePanInStove(possibleActions, i, requestIndex, foodType, this.gameObject, this.pans, this.stoves);
                    }
                    break;

                case "deliver":
                    if (carrying)
                    {
                        ActionTypeToActionConverter.Deliver(possibleActions, i, requestIndex, this.gameObject, this.plates, this.delivery, request);
                    }
                    break;
                case "getPlate":
                    if (!carrying)
                    {
                        ActionTypeToActionConverter.GetPlate(possibleActions, i, requestIndex, this.otherPlayers, this.plates, request);
                    }
                    break;

                case "deliverSoupInPlate":
                    if (carrying)
                    {
                        ActionTypeToActionConverter.DeliverSoupInPlate(possibleActions, i, requestIndex, foodType, this.otherPlayers, this.gameObject, this.pans, this.plates, itemType);
                    }
                    break;

                case "getSoupFromPan":
                    if (!carrying)
                    {
                        ActionTypeToActionConverter.GetSoupFromPan(possibleActions, i, requestIndex, foodType, this.otherPlayers, this.pans, itemType);
                    }
                    break;

                case "boilSoup":
                    if (!carrying)
                    {
                        ActionTypeToActionConverter.BoilSoup(possibleActions, i, requestIndex, foodType, this.otherPlayers, this.pans, itemType);
                    }
                    break;


                case "deliverCutedInSoup":
                    if (carrying)
                    {
                        ActionTypeToActionConverter.DeliverCutedInSoup(possibleActions, i, requestIndex, foodType, this.otherPlayers, this.gameObject, foods, this.pans, itemType);

                    }
                    break;

                case "deliverCutedInPlate":
                    if (carrying)
                    {
                        ActionTypeToActionConverter.DeliverCutedInPlate(possibleActions, i, requestIndex, foodType, this.otherPlayers, this.gameObject, foods, this.plates);
                    }
                    break;


                case "getCuted":
                    if (!carrying)
                    {
                        ActionTypeToActionConverter.GetCuted(possibleActions, i, requestIndex, foodType, this.otherPlayers, this.cuttingBoards, foods);
                    }
                    break;

                case "cut":
                    if (!carrying)
                    {
                        ActionTypeToActionConverter.Cut(possibleActions, i, requestIndex, foodType, this.cuttingBoards);
                    }
                    break;


                case "deliverUncuted":
                    if (carrying)
                    {
                        ActionTypeToActionConverter.DeliverUncuted(possibleActions, i, requestIndex, foodType, otherPlayers, this.gameObject, foods, this.cuttingBoards);
                    }
                    break;


                case "get":
                    if (!carrying)
                    {
                        ActionTypeToActionConverter.Get(possibleActions, i, requestIndex, foodType, otherPlayers, foods, foodDisp, this.cuttingBoards);
                    }
                    break;

                default:
                    break;

            }
        }
        return possibleActions;
    }

    //TODO put this in ActionTypeToActionConverter
    public List<Action> GetPossibleDropActions()
    {
        List<Action> possibleActions = new List<Action>();

        if (pans.Contains(WhatImCarrying()))
        {
            foreach(Transform stove in this.stoves)
            {
                if (!stove.gameObject.GetComponent<Stove>().hasItem)
                {
                    possibleActions.Add(new Action("drop", new Vector3(stove.position.x, stove.position.y, -1), stove, 0));
                    
                }
            }
            return possibleActions;
        }

        foreach(Transform v in usableBalcony)
        {
            if (!v.gameObject.GetComponent<Counter>().hasItem && (v.position - this.gameObject.transform.position).magnitude <10 )
            {
                possibleActions.Add(new Action("drop", new Vector3(v.position.x, v.position.y, -1), v, 0));
                if(possibleActions.Count == 3) { return possibleActions; } //Already has 3 possible places to drop item
            }
        }
        return possibleActions;

    }

    private Transform WhatImCarrying()
    {
        foreach(Transform child in this.gameObject.transform)
        {
            return child;
        }
        return null;
    }


    public void UpdateMap(Transform t)
    {
        switch (t.gameObject.name)
        {
            case "Onion(Clone)":
                Debug.Log("Added an onion");
                this.onions.Add(t);
                break;
            //TODO
            case "Tomato(Clone)":
                Debug.Log("Added an onion");
                this.tomatos.Add(t);
                break;
            default:
                break;
        }
    }

    public void DeleteFromMap(Transform t)
    {
        switch (t.gameObject.name)
        {
            case "Onion(Clone)":
                Debug.Log("Removed an onion");
                this.onions.Remove(t);
                break;
            case "Tomato(Clone)":
                Debug.Log("Removed a tomato");
                this.tomatos.Remove(t);
                break;
            //TODO
            default:
                break;
        }
    }


    public void InformTasks(List<AgentDecisionTask> bestTasks)
    {
        this.bestTasks = bestTasks;
    }

    public void improveMyPath()
    {
        Task mytask = null;

        foreach(AgentDecisionTask adt in this.bestTasks)
        {
            if (adt.GetId() == this.gameObject.GetComponent<Agent>().GetId())
            {
                mytask = adt.GetTask();
            }
        }


    }

    private bool ExistsCollision(List<Vector3> path1, List<Vector3> path2)
    {
        if (path1.Count == 0 || path2.Count == 0)
        {
            return false;
        }
        int index1 = 0;
        int index2 = 0;
        int size1 = path1.Count;
        int size2 = path2.Count;

        while (index1 != size1 - 1 || index2 != size2 - 1)
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
            if (index1 != size1 - 1) { index1++; }
            if (index2 != size2 - 1) { index2++; }

        }

        return false;
    }



}   
