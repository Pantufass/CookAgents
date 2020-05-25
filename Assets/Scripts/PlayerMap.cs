﻿using System.Collections;
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

    private List<Vector3>tomatoDisp = new List<Vector3>();

    private List<Transform> cuttingBoards = new List<Transform>();

    private List<Transform> onions = new List<Transform>();

    private List<Transform> tomatos = new List<Transform>();

    private List<Transform> lettuce = new List<Transform>();

    private List<Transform> pans = new List<Transform>();

    private List<Transform> plates = new List<Transform>();

    private List<Transform> soups = new List<Transform>();

    private Transform delivery;

    private Vector3 plateReturn;


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

                case "CuttingBoard":
                    this.cuttingBoards.Add(t);
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




    public List<Action> GetPossibleActions(List<string> options, int requestPriority)
    {
        Debug.Log("Get possible Actions");
        List<Action> possibleActions = new List<Action>();

        bool carrying = this.gameObject.transform.childCount > 0;

        int i = 0;

        for (i = 0; i < 2 *options.Count; i+=2)
        {
            switch (options[i/2])
            {

                case "deliverSoupOnion":
                    if (carrying)
                    {
                        if(this.plates.Contains(WhatImCarrying()) && WhatImCarrying().gameObject.GetComponent<Plate>().GetState() == Plate.State.onSoup)
                        {
                            Action newAction = new Action("deliverSoupOnion", new Vector3(this.delivery.position.x, this.delivery.position.y, -1), this.delivery, i + requestPriority * 10);
                            possibleActions.Add(newAction);
                        }
                    }
                    break;
                case "getPlateSoupOnion":
                    if (!carrying)
                    {
                        foreach(Transform plate in this.plates)
                        {
                            if(plate.gameObject.GetComponent<Plate>().GetState() == Plate.State.onSoup && !InPlayer(plate))
                            {
                                Action newAction = new Action("getPlateSoupOnion", new Vector3(plate.position.x, plate.position.y, -1), plate, i + requestPriority * 10);
                                possibleActions.Add(newAction);
                            }
                        }
                    }
                    break;
                case "replacePanInStove":
                    if (carrying)
                    {
                        if (this.pans.Contains(WhatImCarrying()) && WhatImCarrying().GetComponent<Pan>().soup.type() == Item.type.none)
                        {
                            foreach (Transform stove in this.stoves)
                            {
                                if (!stove.GetComponent<Stove>().hasItem)
                                {
                                    Action newAction = new Action("replacePanInStove", new Vector3(stove.position.x, stove.position.y, -1), stove, i + requestPriority * 10);
                                    possibleActions.Add(newAction);
                                }
                            }
                        }
                    }
                    break;
                case "deliverSoupInPlateOnion":
                    if (carrying)
                    {
                        if (this.pans.Contains(WhatImCarrying()) && WhatImCarrying().GetComponent<Pan>().soup.isDone() && WhatImCarrying().GetComponent<Pan>().soup.type() == Item.type.onion)
                        {
                            foreach(Transform plate in this.plates)
                            {
                                if (plate.gameObject.GetComponent<Plate>().GetState() == Plate.State.empty && !InPlayer(plate))
                                {
                                    Action newAction = new Action("deliverSoupInPlateOnion", new Vector3(plate.position.x, plate.position.y, -1), plate, i + requestPriority * 10);
                                    possibleActions.Add(newAction);
                                }
                            }
                        }
                    }
                    break;

                case "getSoupFromPanOnion":
                    if (!carrying)
                    {
                        foreach(Transform pan in this.pans)
                        {
                            Soup soup = pan.GetComponent<Pan>().soup;
                            if (!InPlayer(pan) && soup.gameObject.GetComponent<Soup>().isDone() && soup.type() == Item.type.onion)     //free
                            {
                                Action newAction = new Action("getSoupFromPanOnion", new Vector3(pan.position.x, pan.position.y, -1), pan, i + requestPriority * 10);
                                possibleActions.Add(newAction);
                            }
                        }
                    }
                    break;
                case "boilSoupOnion":
                    if (!carrying)
                    {
                        foreach(Transform pan in this.pans)
                        {
                            Soup soup = pan.GetComponent<Pan>().soup;
                            if(!InPlayer(pan) && soup.gameObject.GetComponent<Soup>().canBoil() && soup.type() == Item.type.onion)
                            {
                                Action newAction = new Action("boilSoupOnion", new Vector3(pan.position.x, pan.position.y, -1), pan, i + requestPriority * 10);
                                possibleActions.Add(newAction);
                            }

                        }
                    }
                    break;
                case "deliverCutedInSoupOnion":
                    if (carrying)
                    {
                        foreach(Transform onion in this.onions)
                        {
                            if(onion.gameObject.GetComponent<Food>().IsCut() && InMyPossession(onion)){
                                foreach(Transform pan in this.pans)
                                {
                                    Soup soup = pan.GetComponent<Pan>().soup;
                                    if ((soup.type() == Item.type.onion || soup.type() == Item.type.none) && !soup.isDone())
                                    {
                                        Action newAction = new Action("deliverCutedInSoupOnion", new Vector3(pan.position.x, pan.position.y, -1), pan, i + requestPriority * 10);
                                        possibleActions.Add(newAction);
                                    }
                                }
                            }
                        }
                    }
                    break;

                case "getCutedOnion":
                    if (!carrying)
                    {
                        foreach(Transform board in this.cuttingBoards)
                        {
                            GameObject a = null;
                            if (board.gameObject.GetComponent<CuttingBoard>().onTop != null)
                            {
                                a = board.gameObject.GetComponent<CuttingBoard>().onTop.gameObject;
                                if (a.name == "Onion(Clone)" && a.GetComponent<Food>().IsCut())
                                {
                                    Action newAction = new Action("getCutedOnion", new Vector3(board.position.x, board.position.y, -1), board, i + requestPriority * 10);
                                    possibleActions.Add(newAction);
                                }
                            }
                        }
                    }
                    break;
                case "cutOnion":
                    if (!carrying)
                    {
                        foreach(Transform c in this.cuttingBoards)
                        {
                            foreach(Transform food in c)
                            {
                                if(food.gameObject.name == "Onion(Clone)" && food.gameObject.GetComponent<Food>().IsCut())
                                {
                                    Action newAction = new Action("useCuttingBoard", new Vector3(c.position.x, c.position.y, -1), c, i + requestPriority * 10);
                                    possibleActions.Add(newAction);
                                }
                            }
                        }
                    }
                    break;

                case "deliverUncutedOnion":
                    //TODO
                    foreach(Transform onion in this.onions)
                    {
                        if (carrying && InMyPossession(onion) && !onion.gameObject.GetComponent<Food>().IsCut())
                        {
                            foreach(Transform c in this.cuttingBoards)
                            {
                                if (true)
                                {
                                    Debug.Log("Agent: " + this.gameObject.GetComponent<Agent>().GetId() + "  trying to deliver uncuted onion");
                                    Action newAction = new Action("deliverUncutedOnion", new Vector3(c.position.x, c.position.y, -1), c, i + requestPriority * 10);
                                    possibleActions.Add(newAction);
                                }
                            }
                            return possibleActions;
                        }
                    }
                    break;

                case "getOnion":
                    if (!carrying)
                    {
                        foreach (Transform onion in this.onions)
                        {
                            if (!onion.gameObject.GetComponent<Food>().IsCut() && !InPlayer(onion))     //exists but not cut and not in player possession
                            {
                                Action newAction = new Action("getOnion", new Vector3(onion.position.x, onion.position.y, -1), onion, i + requestPriority * 10);
                                possibleActions.Add(newAction);
                            }
                        }
                        foreach (Vector3 v in this.onionDisp)
                        {
                            Action newAction = new Action("getNewOnion", new Vector3(v.x, v.y, -1), null, i + requestPriority * 10);
                            possibleActions.Add(newAction);
                        }
                    }
                    break;

                //Tomato Section
                //Tomato Section
                //Tomato Section
                //Tomato Section
                //Tomato Section
                //Tomato Section
                case "deliverSoupTomato":
                    if (carrying)
                    {
                        if (this.plates.Contains(WhatImCarrying()) && WhatImCarrying().gameObject.GetComponent<Plate>().GetState() == Plate.State.tomSoup)
                        {
                            Action newAction = new Action("deliverSoupTomato", new Vector3(this.delivery.position.x, this.delivery.position.y, -1), this.delivery, i + requestPriority * 10);
                            possibleActions.Add(newAction);
                        }
                    }
                    break;

                case "deliverPlateTomato":
                    if (carrying)
                    {
                        if (this.plates.Contains(WhatImCarrying()) && WhatImCarrying().gameObject.GetComponent<Plate>().GetState() == Plate.State.tomato)
                        {
                            Action newAction = new Action("deliverPlateTomato", new Vector3(this.delivery.position.x, this.delivery.position.y, -1), this.delivery, i + requestPriority * 10);
                            possibleActions.Add(newAction);
                        }
                    }
                    break;
                case "getPlateTomato":
                    if (!carrying)
                    {
                        foreach(Transform plate in this.plates)
                        {
                            if (plate.gameObject.GetComponent<Plate>().GetState() == Plate.State.tomato && !InPlayer(plate))
                            {
                                Action newAction = new Action("getPlateTomato", new Vector3(plate.position.x, plate.position.y, -1), plate, i + requestPriority * 10);
                                possibleActions.Add(newAction);
                            }
                        }
                    }
                    break;


                case "getPlateSoupTomato":
                    if (!carrying)
                    {
                        foreach (Transform plate in this.plates)
                        {
                            if (plate.gameObject.GetComponent<Plate>().GetState() == Plate.State.tomSoup && !InPlayer(plate))
                            {
                                Action newAction = new Action("getPlateSoupTomato", new Vector3(plate.position.x, plate.position.y, -1), plate, i + requestPriority * 10);
                                possibleActions.Add(newAction);
                            }
                        }
                    }
                    break;

                case "deliverSoupInPlateTomato":
                    if (carrying)
                    {
                        if (this.pans.Contains(WhatImCarrying()) && WhatImCarrying().GetComponent<Pan>().soup.isDone() && WhatImCarrying().GetComponent<Pan>().soup.type() == Item.type.tomato)
                        {
                            foreach (Transform plate in this.plates)
                            {
                                if (plate.gameObject.GetComponent<Plate>().GetState() == Plate.State.empty && !InPlayer(plate))
                                {
                                    Action newAction = new Action("deliverSoupInPlateTomato", new Vector3(plate.position.x, plate.position.y, -1), plate, i + requestPriority * 10);
                                    possibleActions.Add(newAction);
                                }
                            }
                        }
                    }
                    break;

                case "getSoupFromPanTomato":
                    if (!carrying)
                    {
                        foreach (Transform pan in this.pans)
                        {
                            Soup soup = pan.GetComponent<Pan>().soup;
                            if (!InPlayer(pan) && soup.gameObject.GetComponent<Soup>().isDone() && soup.type() == Item.type.tomato)     //free
                            {
                                Action newAction = new Action("getSoupFromPanTomato", new Vector3(pan.position.x, pan.position.y, -1), pan, i + requestPriority * 10);
                                possibleActions.Add(newAction);
                            }
                        }
                    }
                    break;

                case "boilSoupTomato":
                    if (!carrying)
                    {
                        foreach (Transform pan in this.pans)
                        {
                            Soup soup = pan.GetComponent<Pan>().soup;
                            if (!InPlayer(pan) && soup.gameObject.GetComponent<Soup>().canBoil() && soup.type() == Item.type.tomato)
                            {
                                Action newAction = new Action("boilSoupTomato", new Vector3(pan.position.x, pan.position.y, -1), pan, i + requestPriority * 10);
                                possibleActions.Add(newAction);
                            }

                        }
                    }
                    break;


                case "deliverCutedInSoupTomato":
                    if (carrying)
                    {
                        foreach (Transform tomato in this.tomatos)
                        {
                            if (tomato.gameObject.GetComponent<Food>().IsCut() && InMyPossession(tomato))
                            {
                                foreach (Transform pan in this.pans)
                                {
                                    Soup soup = pan.GetComponent<Pan>().soup;
                                    if ((soup.type() == Item.type.tomato || soup.type() == Item.type.none) && !soup.isDone())
                                    {
                                        Action newAction = new Action("deliverCutedInSoupOnion", new Vector3(pan.position.x, pan.position.y, -1), pan, i + requestPriority * 10);
                                        possibleActions.Add(newAction);
                                    }
                                }
                            }
                        }
                    }
                    break;

                case "deliverCutedInPlateTomato":
                    if (carrying)
                    {
                        foreach (Transform tomato in this.tomatos)
                        {
                            if (tomato.gameObject.GetComponent<Food>().IsCut() && InMyPossession(tomato))
                            {
                                foreach (Transform plate in this.plates)
                                {
                                    if(!InPlayer(plate) && plate.gameObject.GetComponent<Plate>().GetState() == Plate.State.empty)
                                    {
                                        Action newAction = new Action("deliverCutedInPlateTomato", new Vector3(plate.position.x, plate.position.y, -1), plate, i + requestPriority * 10);
                                        possibleActions.Add(newAction);
                                    }
                                }
                            }
                        }
                    }
                    break;


                case "getCutedTomato":
                    if (!carrying)
                    {
                        foreach (Transform board in this.cuttingBoards)
                        {
                            GameObject a = null;
                            if (board.gameObject.GetComponent<CuttingBoard>().onTop != null)
                            {
                                a = board.gameObject.GetComponent<CuttingBoard>().onTop.gameObject;
                                if (a.name == "Tomato(Clone)" && a.GetComponent<Food>().IsCut())
                                {
                                    Action newAction = new Action("getCutedTomato", new Vector3(board.position.x, board.position.y, -1), board, i + requestPriority * 10);
                                    possibleActions.Add(newAction);
                                }
                            }
                        }
                    }
                    break;

                case "cutTomato":
                    if (!carrying)
                    {
                        foreach (Transform c in this.cuttingBoards)
                        {
                            foreach (Transform food in c)
                            {
                                if (food.gameObject.name == "Tomato(Clone)" && food.gameObject.GetComponent<Food>().IsCut())
                                {
                                    Action newAction = new Action("useCuttingBoard", new Vector3(c.position.x, c.position.y, -1), c, i + requestPriority * 10);
                                    possibleActions.Add(newAction);
                                }
                            }
                        }
                    }
                    break;


                case "deliverUncutedTomato":
                    //TODO
                    foreach (Transform tomato in this.tomatos)
                    {
                        if (carrying && InMyPossession(tomato) && !tomato.gameObject.GetComponent<Food>().IsCut())
                        {
                            foreach (Transform c in this.cuttingBoards)
                            {
                                if (true)
                                {
                                    Debug.Log("Agent: " + this.gameObject.GetComponent<Agent>().GetId() + "  trying to deliver uncuted tomato");
                                    Action newAction = new Action("deliverUncutedTomato", new Vector3(c.position.x, c.position.y, -1), c, i + requestPriority * 10);
                                    possibleActions.Add(newAction);
                                }
                            }
                            return possibleActions;
                        }
                    }
                    break;


                case "getTomato":
                    if (!carrying)
                    {
                        foreach (Transform tomato in this.tomatos)
                        {
                            if (!tomato.gameObject.GetComponent<Food>().IsCut() && !InPlayer(tomato))     //exists but not cut and not in player possession
                            {
                                Action newAction = new Action("getTomato", new Vector3(tomato.position.x, tomato.position.y, -1), tomato, i + requestPriority * 10);
                                possibleActions.Add(newAction);
                            }
                        }
                        foreach (Vector3 v in this.tomatoDisp)
                        {
                            Action newAction = new Action("getNewTomato", new Vector3(v.x, v.y, -1), null, i + requestPriority * 10);
                            possibleActions.Add(newAction);
                        }
                    }
                    break;

                default:
                    break;

            }
        }
        if(possibleActions.Count == 0)
        {
            Debug.Log("Agent: " + this.gameObject.GetComponent<Agent>().GetId() + "   Its empty mate");
            Action newAction = new Action("empty", new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, -1), this.gameObject.transform, i);
            possibleActions.Add(newAction);
        }
        return possibleActions;
    }

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

    private bool InPlayer(Transform t)
    {
        foreach (GameObject g in otherPlayers)
        {
            foreach(Transform a in g.transform)
            {
                if (a.Equals(t))
                {
                    return true;
                }
            }
        }
        return false;
    }

    private bool InMyPossession(Transform t)
    {
        foreach(Transform a in this.gameObject.transform)
        {
            if (a.Equals(t))
            {
                return true;
            }
        }
        return false;
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

    

}   