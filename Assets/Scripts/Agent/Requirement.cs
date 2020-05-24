using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Requirement
{
    public enum type {boil, cut, pan, plate, food, objective}

    public type t;

    public bool finished = false;

    public Requirement()
    {

    }

    public virtual bool sucess()
    {
        return finished;
    }

}
