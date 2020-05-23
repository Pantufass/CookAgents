using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : Counter
{
    public Food spawn;

    public void spawnFood()
    {
        GameObject g = Instantiate(spawn.gameObject, this.transform.position, Quaternion.identity);
        addOnTop(g.GetComponent<Food>() as Item);

    }
}
