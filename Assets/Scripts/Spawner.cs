using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : Counter
{
    public Food spawn;

    public override bool use(GameObject player)
    {
        GameObject g = Instantiate(spawn.gameObject, this.transform.position, Quaternion.identity);
        return base.pickUp(player) && addOnTop(g.GetComponent<Food>() as Item);
    }

    public new bool pickUp(GameObject player)
    {
        return use(player);
    }

}
