using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stove : Counter
{
    
    public override bool use(GameObject player)
    {
        Pan p = onTop as Pan;
        if(p != null)
        {
            if (p.soup.canBoil())
            {
                p.Boiled();
                return true;
            }
        }
        return false;
    }

}
