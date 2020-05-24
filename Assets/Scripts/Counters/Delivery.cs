using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Delivery : Counter
{
    public Return r;
    public FoodRequests fr;

    private void Start()
    {
        r = GameObject.FindGameObjectWithTag("Return").GetComponent<Return>();
        fr = GetComponent<FoodRequests>();
    }
    public override bool addOnTop(Item i)
    {
        Plate p = i as Plate;
        if (p != null)
        {
            fr.clearRequest(p.s);
            r.sendPlate(p);
            return true;
        }
        return false;
    }
}
