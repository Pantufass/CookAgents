using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Return : Counter
{
    
    void Start()
    {
        
    }

    public void sendPlate(Plate p)
    {
        p.Empty();
        p.gameObject.transform.position = this.transform.position + new Vector3(0, 0, -1);
        p.transform.parent = null;
        onTop = p;
        hasItem = true;
        hasItem = true;
    }
}
