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
        base.addOnTop(p);
    }
}
