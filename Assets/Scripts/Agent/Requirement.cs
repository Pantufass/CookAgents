using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Requirement
{
    public enum type {boil, cut, pan, plate, food, objective, deliver, dish}

    public type t;

    private bool finished = false;

    public List<Vector3> pos;

    public Vector3 target;

    public Requirement()
    {
    }
    public Requirement(type tipo)
    {
        pos = new List<Vector3>();
        t = tipo;
        switch (t){
            case type.boil:
                getPos("Stove");
                break;
            case type.cut:
                getPos("CuttingBoard");
                break;
            case type.pan:
                getPos("Pan");
                break;
            case type.plate:
                getPos("Plate");
                break;
            case type.deliver:
                getPos("Delivery");
                break;
        }
    }

    protected void getPos(string tag)
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag(tag);
        foreach(GameObject o in objs)
        {
            pos.Add(o.transform.position);
        }
    }

    public virtual bool sucess()
    {
        return finished;
    }

    public void finishReq()
    {
        finished = true;
    }

    
}
