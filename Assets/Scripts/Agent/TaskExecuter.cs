using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskExecuter
{

    private int pathSize = 0;

    private bool stoped = false;

    private PlayerController controller;

    private PlayerMap map;

    private GameObject me;

    private List<GameObject> otherPlayers;

    private int id = 0;

    private Vector3 blacklist = new Vector3();

    private bool dropCarrying = false;

    private Task currentTask = null;

    public TaskExecuter(PlayerController controller, PlayerMap map, GameObject me, List<GameObject> otherPlayers, int id)
    {
        this.controller = controller;
        this.map = map;
        this.me = me;
        this.otherPlayers = otherPlayers;
        this.id = id;
    }

    public void SetCurrentTask(Task currentTask)
    {
        this.currentTask = currentTask;
    }

    public Task GetCurrentTask()
    {
        return this.currentTask;
    }

    public bool GetDropCarrying()
    {
        try
        {
            return this.dropCarrying;
        }
        finally
        {
            this.dropCarrying = false;
        }
    }


    public void Act(Task task)
    {
        Debug.Log("Agent: " + id + "   Doing: " + task.GetAction().GetActionType());
        if (currentTask == null)
        {
            currentTask = task;
            stoped = false;
            pathSize = 0;
        }
        Vector3 position = me.transform.position;

        List<Vector3> path = currentTask.GetPath();

        if (currentTask.GetAction().GetGoal2() != null && (currentTask.GetAction().GetGoal1().x != currentTask.GetAction().GetGoal2().position.x || currentTask.GetAction().GetGoal1().y != currentTask.GetAction().GetGoal2().position.y) && (currentTask.GetAction().GetGoal2().position.x != me.transform.position.x || currentTask.GetAction().GetGoal2().position.y != me.transform.position.y))
        {
            currentTask = null;
            return;
        }
        if (currentTask.GetAction().GetActionType().Equals("empty"))
        {
            currentTask = null;
            return;
        }

        if (path.Count > 1)
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
                com.Add(false);
                com.Add(false);
                com.Add(false);
                com.Add(false);
                controller.Act(com);
            }
            else
            {
                if (this.pathSize == path.Count)
                {
                    if (stoped)
                    {
                        Debug.Log("Agent: " + this.id + " cancelling task");
                        blacklist = currentTask.GetAction().GetGoal1();
                        currentTask = null;
                    }
                    else
                    {
                        stoped = true;
                    }
                }
            }
            pathSize = path.Count;

        }
        else
        {
            controller.RotateTowards(currentTask.GetAction().GetGoal1());
            if (currentTask.GetAction().GetActionType().Equals("drop"))
            {
                Debug.Log("Agent: " + this.id + " droppp");
                PickUp();
                if (me.transform.childCount == 0)
                {
                    this.currentTask = null;
                }
            }
            else if (currentTask.GetAction().GetActionType().Equals("empty"))
            {
                this.currentTask = null;
            }
            else if (currentTask.GetAction().GetActionType().IndexOf("get") > -1)
            {
                GetActions();
            }
            else if (currentTask.GetAction().GetActionType().IndexOf("deliver") > -1)
            {
                DeliverActions();
            }
            else if (currentTask.GetAction().GetActionType().IndexOf("use") > -1)
            {
                Use();
                this.currentTask = null;
            }
            else if (currentTask.GetAction().GetActionType().IndexOf("boilSoup") > -1)
            {
                Use();
                this.currentTask = null;
            }
            else if (currentTask.GetAction().GetActionType().Equals("replacePanInStove"))
            {
                if(this.currentTask.GetAction().GetGoal2().gameObject.GetComponent<Stove>().onTop != null) { this.currentTask = null; return; }
                PickUp();
                if (me.transform.childCount == 0)
                {
                    this.currentTask = null;
                }
            }


        }

    }

    private void GetActions()
    {
        if (currentTask.GetAction().GetActionType().IndexOf("getNew") > -1)
        {

            Use();
            PickUp();

            foreach (Transform t in this.me.transform)
            {
                if(t.gameObject.name.IndexOf("Pan") > -1)
                {
                    PickUp();
                    this.currentTask = null;
                    return;
                }
                UpdateMaps(t);
                this.currentTask = null;
                return;
            }
            this.currentTask = null;
        }
        else if (currentTask.GetAction().GetActionType().IndexOf("getCuted") > -1)
        {

            if (!this.currentTask.GetAction().GetGoal2().gameObject.GetComponent<CuttingBoard>().hasItem) { this.currentTask = null; return; }
            PickUp();
            if (me.transform.childCount > 0)
            {
                this.currentTask = null;
            }

        }
        else if(currentTask.GetAction().GetActionType().IndexOf("getSoupFromPan") > -1 || currentTask.GetAction().GetActionType().IndexOf("getPlate") > -1){
            PickUp();
            if (me.transform.childCount > 0)
            {
                this.currentTask = null;
            }
        }
        else        //get
        {
            PickUp();
            if (me.transform.childCount > 0)
            {
                this.currentTask = null;
            }
        }

    }

    private void DeliverActions()
    {
        if (currentTask.GetAction().GetActionType().IndexOf("deliverUncuted") > -1)
        {
            if (currentTask.GetAction().GetGoal2().gameObject.GetComponent<CuttingBoard>().hasItem)
            {
                currentTask = null;
            }
            else
            {
                PickUp();
                if(me.transform.childCount == 0)
                {
                    this.currentTask = null;
                }
            }
        }
        else if (currentTask.GetAction().GetActionType().IndexOf("deliverCutedInSoup") > -1)
        {
            if (StillPossibleToDeliver())
            {
                if (this.me.transform.childCount == 0)
                {
                    this.currentTask = null;
                    return;
                }
                foreach (Transform t in this.me.transform)
                {
                    DeleteFromMaps(t);
                }
                PickUp();

            }
            else
            {
                this.currentTask = null;
            }
        }
        else if (currentTask.GetAction().GetActionType().IndexOf("deliverCutedInPlate") > -1){


            if (this.me.transform.childCount == 0)
            {
                this.currentTask = null;
                return;
            }
            foreach (Transform t in this.me.transform)
            {
                DeleteFromMaps(t);
            }
            PickUp();

        }
        else if (currentTask.GetAction().GetActionType().IndexOf("deliverSoupInPlate") > -1)
        {
            PickUp();
            this.currentTask = null;
        }
        else        //deliver
        {
            PickUp();
            if (me.transform.childCount == 0)
            {
                this.currentTask = null;
            }

        }

    }


    private void FinishRequest()
    {
        int taskId = currentTask.GetId();
        foreach (GameObject g in otherPlayers)
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
        if (currentTask.GetId() == taskId)
        {
            currentTask = null;
            if (this.me.transform.childCount > 0)
            {

                dropCarrying = true;
            }
        }
    }

    private bool PlayerInThatPosition(Vector3 v)
    {
        foreach (GameObject g in otherPlayers)
        {
            if (g.transform.position.Equals(v))
            {
                return true;
            }
        }
        return false;
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
        com.Add(true);
        com.Add(false);
        controller.Act(com);
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
        com.Add(false);
        com.Add(true);
        controller.Act(com);
    }


    private void UpdateMaps(Transform t)
    {
        this.map.UpdateMap(t);
        foreach (GameObject g in otherPlayers)
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

    private bool StillPossibleToDeliver()
    {
        foreach(Transform food in me.transform)
        {
            return (food.gameObject.GetComponent<Food>().GetType() == currentTask.GetAction().GetGoal2().gameObject.GetComponent<Pan>().GetType() || currentTask.GetAction().GetGoal2().gameObject.GetComponent<Pan>().GetType() == Item.type.none);

        }
        return false;
    }
}
