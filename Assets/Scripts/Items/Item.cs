using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public enum type {tomato,lettuce,onion,plate,pan, none }

    public type t;


    public void pickUp()
    {

    }

    public void drop()
    {

    }
    public type GetType()
    {
        return t;
    }
}
