using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutttingBoard : Counter
{
    public void Cut()
    {
        Food f = onTop as Food;
        if(f != null)
        {
            if (!f.cut)
            {
                f.Cut();
            }
        }

    }
}
