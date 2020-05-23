using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutttingBoard : Counter
{

    public override bool use(GameObject player)
    {
        Food f = onTop as Food;
        if (f != null)
        {
            if (!f.cut)
            {
                f.Cut();
            }
        }
        return false;
    }
}
