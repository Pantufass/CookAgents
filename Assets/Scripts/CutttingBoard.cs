using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutttingBoard : Counter
{

    public override void use(GameObject player)
    {
        Food f = onTop as Food;
        if (f != null)
        {
            if (!f.cut)
            {
                f.Cut();
            }
        }
    }
}
