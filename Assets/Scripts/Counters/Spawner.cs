using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : Counter
{
    public Food spawn;

    public override bool use(GameObject player)
    {
        GameObject g = Instantiate(spawn.gameObject, this.transform.position, Quaternion.identity);
        bool b = addOnTop(g.GetComponent<Food>() as Item);
        return  b && base.pickUp(player) ;
    }

    public override bool pickUp(GameObject player)
    {
        return use(player);
    }


}
