using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ActionTypeToActionConverter
{
    static readonly int actionTypeImportance = 2;
    static readonly int requestImportance = 5;

    public static void Get(List<Action> possibleActions, int typeIndex, int requestIndex, string foodType, List<GameObject> otherPlayers, List<Transform> foods, List<Vector3> foodDisp, List<Transform> cuttingBoards)
    {
        foreach (Transform food in foods)
        {
            bool free = true;
            foreach (Transform c in cuttingBoards)
            {
                if (c.gameObject.GetComponent<CuttingBoard>().onTop != null && c.gameObject.GetComponent<CuttingBoard>().onTop.gameObject.Equals(food.gameObject))
                {
                    free = false;
                }

            }
            if (free)
            {
                if (!food.gameObject.GetComponent<Food>().IsCut() && !InPlayer(otherPlayers, food))     //exists but not cut and not in player possession
                {
                    ActionTypeToActionConverter.AddAction(possibleActions, "get" + foodType, new Vector3(food.position.x, food.position.y, -1), food, typeIndex, requestIndex);
                }
            }
        }
        foreach (Vector3 v in foodDisp)
        {
            ActionTypeToActionConverter.AddAction(possibleActions, "getNew" + foodType, new Vector3(v.x, v.y, -1), null, typeIndex, requestIndex);
        }
    }


    public static void DeliverUncuted(List<Action> possibleActions, int typeIndex, int requestIndex, string foodType, List<GameObject> otherPlayers, GameObject me, List<Transform> foods, List<Transform> cuttingBoards)
    {
        foreach (Transform food in foods)
        {
            if (InMyPossession(me, food) && !food.gameObject.GetComponent<Food>().IsCut())
            {
                foreach (Transform c in cuttingBoards)
                {
                    if (c.gameObject.GetComponent<CuttingBoard>().onTop == null)
                    {
                        ActionTypeToActionConverter.AddAction(possibleActions, "deliverUncuted" + foodType, new Vector3(c.position.x, c.position.y, -1), c, typeIndex, requestIndex);
                    }
                }
            }
        }
    }

    public static void Cut(List<Action> possibleActions, int typeIndex, int requestIndex, string foodType, List<Transform> cuttingBoards)
    {
        foreach (Transform c in cuttingBoards)
        {
            GameObject food = null;
            if (c.gameObject.GetComponent<CuttingBoard>().onTop != null)
            {
                food = c.gameObject.GetComponent<CuttingBoard>().onTop.gameObject;
                if (food.name == (foodType + "(Clone)") && !food.GetComponent<Food>().IsCut())
                {
                    ActionTypeToActionConverter.AddAction(possibleActions, "useCuttingBoard", new Vector3(c.position.x, c.position.y, -1), c, typeIndex, requestIndex);
                }
            }

        }
    }

    public static void GetCuted(List<Action> possibleActions, int typeIndex, int requestIndex, string foodType, List<GameObject> otherPlayers, List<Transform> cuttingBoards, List<Transform> foods)
    {
        foreach (Transform board in cuttingBoards)
        {
            GameObject a = null;
            if (board.gameObject.GetComponent<CuttingBoard>().onTop != null)
            {
                a = board.gameObject.GetComponent<CuttingBoard>().onTop.gameObject;
                if (a.name == foodType + "(Clone)" && a.GetComponent<Food>().IsCut())
                {
                    ActionTypeToActionConverter.AddAction(possibleActions, "getCuted" + foodType, new Vector3(board.position.x, board.position.y, -1), board, typeIndex, requestIndex);
                }
            }

        }
        foreach (Transform food in foods)
        {
            bool onBoard = false;
            foreach (Transform board in cuttingBoards)
            {
                Item onTop = board.gameObject.GetComponent<CuttingBoard>().onTop;
                if (onTop != null && onTop.Equals(food))
                {
                    onBoard = true;
                }
            }
            if (!onBoard)
            {
                if (food.gameObject.GetComponent<Food>().IsCut() && !InPlayer(otherPlayers, food))
                {
                    ActionTypeToActionConverter.AddAction(possibleActions, "getCuted" + foodType, new Vector3(food.position.x, food.position.y, -1), food, typeIndex, requestIndex);
                }
            }
        }
    }

    public static void DeliverCutedInPlate(List<Action> possibleActions, int typeIndex, int requestIndex, string foodType, List<GameObject> otherPlayers, GameObject me, List<Transform> foods, List<Transform> plates)
    {
        foreach (Transform food in foods)
        {
            if (food.gameObject.GetComponent<Food>().IsCut() && InMyPossession(me,food))
            {
                foreach (Transform plate in plates)
                {
                    if (!InPlayer(otherPlayers, plate) && plate.gameObject.GetComponent<Plate>().GetState() == Plate.State.empty)
                    {
                        ActionTypeToActionConverter.AddAction(possibleActions, "deliverCutedInPlate" + foodType, new Vector3(plate.position.x, plate.position.y, -1), plate, typeIndex, requestIndex);
                    }
                }
            }
        }
    }

    public static void DeliverCutedInSoup(List<Action> possibleActions, int typeIndex, int requestIndex, string foodType, List<GameObject> otherPlayers, GameObject me, List<Transform> foods, List<Transform> pans, Item.type type)
    {
        foreach (Transform food in foods)
        {
            if (food.gameObject.GetComponent<Food>().IsCut() && InMyPossession(me, food))
            {
                foreach (Transform pan in pans)
                {
                    Soup soup = pan.GetComponent<Pan>().soup;
                    if ((soup.type() == type || soup.type() == Item.type.none) && !soup.isDone())
                    {
                        ActionTypeToActionConverter.AddAction(possibleActions, "deliverCutedInSoup" + foodType, new Vector3(pan.position.x, pan.position.y, -1), pan, typeIndex, requestIndex);
                    }
                }
            }
        }
    }

    public static void BoilSoup(List<Action> possibleActions, int typeIndex, int requestIndex, string foodType, List<GameObject> otherPlayers, List<Transform> pans, Item.type type)
    {
        foreach (Transform pan in pans)
        {
            Soup soup = pan.GetComponent<Pan>().soup;
            if (!InPlayer(otherPlayers, pan) && soup.gameObject.GetComponent<Soup>().canBoil() && soup.type() == type)
            {
                ActionTypeToActionConverter.AddAction(possibleActions, "boilSoup" + foodType, new Vector3(pan.position.x, pan.position.y, -1), pan, typeIndex, requestIndex);

            }

        }
    }

    public static void GetSoupFromPan(List<Action> possibleActions, int typeIndex, int requestIndex, string foodType, List<GameObject> otherPlayers, List<Transform> pans, Item.type type)
    {
        foreach (Transform pan in pans)
        {
            Soup soup = pan.GetComponent<Pan>().soup;
            if (!InPlayer(otherPlayers, pan) && soup.gameObject.GetComponent<Soup>().isDone() && soup.type() == type)     //free
            {
                ActionTypeToActionConverter.AddAction(possibleActions, "getSoupFromPan" + foodType, new Vector3(pan.position.x, pan.position.y, -1), pan, typeIndex, requestIndex);
            }
        }
    }

    public static void DeliverSoupInPlate(List<Action> possibleActions, int typeIndex, int requestIndex, string foodType, List<GameObject> otherPlayers, GameObject me, List<Transform> pans, List<Transform> plates, Item.type type)
    {
        if (pans.Contains(WhatImCarrying(me))){

            Debug.Log("Agent: " + me.GetComponent<Agent>().GetId() + " carrying soup pan");

            if (WhatImCarrying(me).GetComponent<Pan>().soup != null)
            {
                Debug.Log("Agent: " + me.GetComponent<Agent>().GetId() + " not null");

                if (WhatImCarrying(me).GetComponent<Pan>().soup.isDone())
                {
                    Debug.Log("Agent: " + me.GetComponent<Agent>().GetId() + " soup done");
                    if (WhatImCarrying(me).GetComponent<Pan>().soup.type() == type)
                    {
                        Debug.Log("Agent: " + me.GetComponent<Agent>().GetId() + " my type");
                        foreach (Transform plate in plates)
                        {
                            if (plate.gameObject.GetComponent<Plate>().GetState() == Plate.State.empty && !InPlayer(otherPlayers, plate))
                            {
                                Debug.Log("Agent: " + me.GetComponent<Agent>().GetId() + " free plate");
                                ActionTypeToActionConverter.AddAction(possibleActions, "deliverSoupInPlate" + foodType, new Vector3(plate.position.x, plate.position.y, -1), plate, typeIndex, requestIndex);
                            }
                        }
                    }
                }
            }
        }
    }


    public static void GetPlate(List<Action> possibleActions, int typeIndex, int requestIndex, List<GameObject> otherPlayers, List<Transform> plates, Plate.State request)
    {
        foreach (Transform plate in plates)
        {
            if (plate.gameObject.GetComponent<Plate>().GetState() == request && !InPlayer(otherPlayers, plate))
            {
                ActionTypeToActionConverter.AddAction(possibleActions, "getPlate_" + request, new Vector3(plate.position.x, plate.position.y, -1), plate, typeIndex, requestIndex);
            }
        }
    }


    public static void ReplacePanInStove(List<Action> possibleActions, int typeIndex, int requestIndex, string foodType, GameObject me, List<Transform> pans, List<Transform> stoves)
    {
        if (pans.Contains(WhatImCarrying(me)) && WhatImCarrying(me).GetComponent<Pan>().soup.type() == Item.type.none)
        {
            foreach (Transform stove in stoves)
            {
                if (!stove.GetComponent<Stove>().hasItem)
                {
                    ActionTypeToActionConverter.AddAction(possibleActions, "replacePanInStove", new Vector3(stove.position.x, stove.position.y, -1), stove, typeIndex, requestIndex);
                }
            }
        }
    }


    public static void Deliver(List<Action> possibleActions, int typeIndex, int requestIndex, GameObject me, List<Transform> plates, Transform delivery, Plate.State request)
    {

        if (plates.Contains(WhatImCarrying(me)) && WhatImCarrying(me).gameObject.GetComponent<Plate>().GetState() == request)
        {
            ActionTypeToActionConverter.AddAction(possibleActions, "deliver_" + request, new Vector3(delivery.position.x, delivery.position.y, -1), delivery, typeIndex, requestIndex);
        }
    }





    private static void AddAction(List<Action> possibleActions, string type, Vector3 v, Transform t, int typeIndex, int requestIndex)
    {
        Action newAction = new Action(type, v, t, ActionTypeToActionConverter.actionTypeImportance * +requestIndex * ActionTypeToActionConverter.requestImportance);
        possibleActions.Add(newAction);
    }

    private static bool InPlayer(List<GameObject> otherPlayers, Transform t)
    {
        foreach (GameObject g in otherPlayers)
        {
            foreach (Transform a in g.transform)
            {
                if (a.Equals(t))
                {
                    return true;
                }
            }
        }
        return false;
    }

    private static bool InMyPossession(GameObject me, Transform t)
    {
        foreach (Transform a in me.transform)
        {
            if (a.Equals(t))
            {
                return true;
            }
        }
        return false;
    }

    private static Transform WhatImCarrying(GameObject me)
    {
        foreach (Transform child in me.transform)
        {
            return child;
        }
        return null;
    }
} 
